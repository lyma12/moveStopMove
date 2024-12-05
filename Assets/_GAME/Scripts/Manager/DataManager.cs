using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    MAIN_MENU,
    ON_PLAY,
    SHOP,
    VICTORY,
    LOSE,
}
public class DataGame
{
    private int coinPlayer;
    public int CoinPlayer
    {
        get { return coinPlayer; }
        set
        {
            coinPlayer = value;
            isChange = true;
        }
    }
    public bool CanBuy(int cost)
    {
        if (coinPlayer >= cost)
        {
            return true;
        }
        return false;
    }
    public DataGame(int coinPlayer)
    {
        this.coinPlayer = coinPlayer;
        level = 1;
        playerWeapon = new bool[Contanst.NumberOfTypeWeapon];
        playerSkin = new bool[Contanst.NumberOfTypeSkin];
        playerPant = new bool[Contanst.NumberOfTypePant];
        playerWeapon[0] = true;
        playerSkin[0] = true;
        playerPant[0] = true;
        isChange = true;
    }
    private int level;
    private bool[] playerWeapon = new bool[Contanst.NumberOfTypeWeapon];
    private int currentWeapon;
    private bool[] playerSkin = new bool[Contanst.NumberOfTypeSkin];
    private int currentSkin;
    private bool[] playerHat;
    private int currentHat;
    private bool[] playerPant = new bool[Contanst.NumberOfTypePant];
    private int currentPant;
    private State state;
    public State State
    {
        get { return state; }
        set { state = value; }
    }
    private int currentLevel;
    public int CurrentLevel
    {
        get { return currentLevel; }
        set
        {
            if (currentLevel <= level)
            {
                currentLevel = value;
            }
            else
            {
                currentLevel = level;
            }
            isChange = true;
        }
    }
    public int Level
    {
        get { return level; }
        set
        {
            if (level <= 0) level = 1;
            else level = value;
            isChange = true;
        }
    }
    private bool isChange = false;
    public bool IsChange
    {
        get { return isChange; }
    }
    public int CurrentWeapon
    {
        get { return currentWeapon; }
        set { currentWeapon = value; }
    }
    public bool[] PlayerWeapon
    {
        get { return playerWeapon; }
        set
        {
            playerWeapon = value;
        }
    }
    public void BuyWeapon(int i)
    {
        if (!playerWeapon[i])
        {
            coinPlayer -= DataManager.Instance.GetPriceItem(i, TypeItem.WEAPON);
            playerWeapon[i] = true;
            isChange = true;
        }
    }
    public bool[] PlayerSkin
    {
        get { return playerSkin; }
        set { playerSkin = value; }
    }
    public int CurrentSkin
    {
        get { return currentSkin; }
        set { currentSkin = value; }
    }
    public void BuySkin(int i)
    {
        if (!playerSkin[i])
        {
            coinPlayer -= DataManager.Instance.GetPriceItem(i, TypeItem.SKIN);
            playerSkin[i] = true;
            isChange = true;
        }
    }
    public bool[] PlayerHat
    {
        get { return playerHat; }
    }
    public int CurrentHat
    {
        get { return currentHat; }
        set { currentHat = value; }
    }
    public void BuyHat(int i)
    {
        if (!playerHat[i])
        {
            playerHat[i] = true;
            isChange = true;
        }
    }
    public bool[] PlayerPant
    {
        get { return playerPant; }
        set { playerPant = value; }
    }
    public int CurrentPant
    {
        get { return currentPant; }
        set { currentPant = value; }
    }
    public void BuyPant(int i)
    {
        if (!playerPant[i])
        {
            coinPlayer -= DataManager.Instance.GetPriceItem(i, TypeItem.PANT);
            playerPant[i] = true;
            isChange = true;
        }
    }
}
class DataManager : Singleton<DataManager>, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    [SerializeField]
    private WeaponData weaponData;
    [SerializeField]
    private PantData paints;
    [SerializeField]
    private SkinData skins;
    private DataGame data;
    public int GetPriceItem(int type, TypeItem typeItem)
    {
        switch (typeItem)
        {
            case TypeItem.WEAPON:
                return weaponData.GetPriceItem(type);
            case TypeItem.SKIN:
                return skins.GetPriceItem(type);
            case TypeItem.PANT:
                return paints.GetPriceItem(type);
        }
        return 0;
    }
    public DataGame Data
    {
        get { return data; }
    }
    public State State
    {
        get { return data.State; }
        set
        {
            data.State = value;
        }
    }

    public Action OnLoadedData;
    private void Awake()
    {
        UIManager.Instance.OnUIInitialized += OnInit;
    }
    private void OnInit()
    {
        data = LoadPlayerData();
        OnMainMenu();
        OnLoadedData?.Invoke();
    }
    public WeaponItemData GetWeapon(WeaponType wpType)
    {
        return weaponData.GetWeapon(wpType);
    }

    public Material GetPant(PantType type)
    {
        return paints.GetPant(type).Pant;
    }
    public Material GetSkin(SkinType type)
    {
        return skins.GetSkin(type).Skin;
    }
    private void SaveData(DataGame data)
    {
        if (data.IsChange)
        {
            string s = JsonConvert.SerializeObject(data);
            PlayerPrefs.SetString(Contanst.KEY_DATA, s);
        }
    }
    private DataGame LoadPlayerData()
    {
        string s = PlayerPrefs.GetString(Contanst.KEY_DATA);
        if (string.IsNullOrEmpty(s))
        {
            return new DataGame(20);
        }
        return JsonConvert.DeserializeObject<DataGame>(s);
    }
    private bool IsState(State otherState)
    {
        return otherState == State;
    }
    public void SetCurrentLevel(int newLevel)
    {
        Data.CurrentLevel = newLevel;
    }
    public void ChangeState(State newState)
    {
        if (newState == State) return;
        switch (newState)
        {
            case State.MAIN_MENU:
                OnMainMenu();
                break;
            case State.VICTORY:
                OnVictory();
                break;
            case State.LOSE:
                OnLose();
                break;
            case State.SHOP:
                OnShop();
                break;
            case State.ON_PLAY:
                OnPlay();
                break;
            default:
                OnMainMenu();
                break;

        }
    }
    private void OnPlay()
    {
        PlayCanva canva = UIManager.Instance.CloseAndOpenUI<PlayCanva>();
        LevelManager.Instance.LoadLevel(Data.CurrentLevel);
        LevelManager.Instance.Player.Joystick = canva.SetJoytick();
        State = State.ON_PLAY;

    }
    private void OnMainMenu()
    {
        State = State.MAIN_MENU;
        UIManager.Instance.CloseAndOpenUI<MainMenu>();
    }
    private void OnVictory()
    {
        State = State.ON_PLAY;
        if (data.CurrentLevel == data.Level) data.Level++;
        data.CoinPlayer += 50;
        UIManager.Instance.GetUI<EndGameCanva>().SetUIVictory();
        UIManager.Instance.CloseAndOpenUI<EndGameCanva>();
    }
    private void OnLose()
    {
        UIManager.Instance.GetUI<EndGameCanva>().SetUpLose();
        UIManager.Instance.CloseAndOpenUI<EndGameCanva>();
    }
    private void OnShop()
    {
        State = State.SHOP;
        UIManager.Instance.CloseAndOpenUI<ShopCanvas>();
    }
    private void OnApplicationQuit()
    {
        SaveData(data);
    }

    public void Attach(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        _observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateState(this);
        }
    }
}
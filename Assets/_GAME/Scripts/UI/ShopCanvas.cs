using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopCanvas : UICanvas
{
    [SerializeField]
    private Button btnReturn;
    [SerializeField]
    private Button btnWeaponUI;
    [SerializeField]
    private Button btnSkinUI;
    [SerializeField]
    private Button btnPantUI;
    private TypeItem type = TypeItem.WEAPON;

    [SerializeField]
    private Button btnNext;
    [SerializeField]
    private Button btnPrev;
    [SerializeField]
    private Player player;
    [SerializeField]
    private SkinData skinData;
    [SerializeField]
    private WeaponData weaponData;
    [SerializeField]
    private PantData pantData;
    [SerializeField]
    private ItemView weaponItemView;
    [SerializeField]
    private Button btnBuy;
    [SerializeField]
    private Button btnTake;
    private bool[] weaponPlayer;
    private bool[] skinPlayer;
    private bool[] pantPlayer;
    private Vector3 originalPosition;
    private WeaponType currentWeapon;
    private SkinType currentSkin;
    private PantType currentPant;

    protected override void OnInit()
    {
        base.OnInit();
        btnPrev.onClick.AddListener(OnClickButtonPrev);
        btnNext.onClick.AddListener(OnClickButtonNext);
        btnTake.onClick.AddListener(Take);
        btnBuy.onClick.AddListener(Buy);
        btnWeaponUI.onClick.AddListener(() => ChangeType(TypeItem.WEAPON));
        btnSkinUI.onClick.AddListener(() => ChangeType(TypeItem.SKIN));
        btnPantUI.onClick.AddListener(() => ChangeType(TypeItem.PANT));
        btnReturn.onClick.AddListener(ReturnHome);
    }
    public override void Setup()
    {
        base.Open();
        weaponPlayer = DataManager.Instance.Data.PlayerWeapon;
        skinPlayer = DataManager.Instance.Data.PlayerSkin;
        pantPlayer = DataManager.Instance.Data.PlayerPant;
        currentPant = (PantType)DataManager.Instance.Data.CurrentPant;
        currentSkin = (SkinType)DataManager.Instance.Data.CurrentSkin;
        currentWeapon = (WeaponType)DataManager.Instance.Data.CurrentWeapon;
        //
        if (originalPosition == null) originalPosition = weaponItemView.transform.position;
        ChangeType(type);
        UpdateItemView();
    }
    private void ReturnHome()
    {
        DataManager.Instance.ChangeState(State.MAIN_MENU);
    }
    public void ChangeType(TypeItem typeItem)
    {
        type = typeItem;
        var currentWeaponType = (WeaponType)DataManager.Instance.Data.CurrentWeapon;
        var currentPantType = (PantType)DataManager.Instance.Data.CurrentPant;
        var currentSkinType = (SkinType)DataManager.Instance.Data.CurrentSkin;
        player.ChangeWeapon(currentWeaponType);
        player.ChangeSkin(currentSkinType);
        player.ChangePant(currentPantType);
        switch (type)
        {
            case TypeItem.WEAPON:
                weaponItemView.OnInit<WeaponItemData>(weaponData.GetWeapon(currentWeapon), player, weaponPlayer[(int)currentWeapon]);
                weaponItemView.TakeItem();
                break;
            case TypeItem.PANT:
                weaponItemView.OnInit<PantItemData>(pantData.GetPant(currentPant), player, weaponPlayer[(int)currentPant]);
                weaponItemView.TakeItem();
                break;
            case TypeItem.SKIN:
                weaponItemView.OnInit<SkinItemData>(skinData.GetSkin(currentSkin), player, weaponPlayer[(int)currentSkin]);
                weaponItemView.TakeItem();
                break;
        }
    }

    private void Buy()
    {
        SoundManager.Instance.PlaySoundOnBuy();
        int cost;
        switch (type)
        {
            case TypeItem.WEAPON:
                cost = weaponData.Listdata[(int)currentWeapon].Price;
                if (DataManager.Instance.Data.CanBuy(cost))
                {
                    DataManager.Instance.Data.BuyWeapon((int)currentWeapon);
                    UpdateItemView();
                }
                break;
            case TypeItem.SKIN:
                cost = skinData.ListData[(int)currentSkin].Price;
                if (DataManager.Instance.Data.CanBuy(cost))
                {
                    DataManager.Instance.Data.BuySkin((int)currentSkin);
                    UpdateItemView();
                }
                break;
            case TypeItem.PANT:
                cost = pantData.ListData[(int)currentPant].Price;
                if (DataManager.Instance.Data.CanBuy(cost))
                {
                    DataManager.Instance.Data.BuyPant((int)currentPant);
                    UpdateItemView();
                }
                break;
        }
    }
    private void Take()
    {
        SoundManager.Instance.PlaySoundOnClick();
        int indexItem;
        bool isHave;
        switch (type)
        {
            case TypeItem.WEAPON:
                indexItem = (int)currentWeapon;
                isHave = weaponPlayer[indexItem];
                if (isHave)
                {
                    DataManager.Instance.Data.CurrentWeapon = (int)currentWeapon;
                }
                break;
            case TypeItem.PANT:
                indexItem = (int)currentPant;
                isHave = pantPlayer[indexItem];
                if (isHave)
                {
                    DataManager.Instance.Data.CurrentPant = (int)currentPant;
                }
                break;
            case TypeItem.SKIN:
                indexItem = (int)currentSkin;
                isHave = skinPlayer[indexItem];
                if (isHave)
                {
                    DataManager.Instance.Data.CurrentSkin = (int)currentSkin;
                }
                break;
        }
    }
    private IEnumerator AnimateFrameChangeNext()
    {
        Vector3 targetPositionOut = originalPosition + new Vector3(-800, 0, 0);
        yield return MoveToPosition(weaponItemView.transform, targetPositionOut, 0.25f, new Vector3(0.5f, 0.5f, 1f));

        UpdateItemView();

        weaponItemView.transform.localPosition = new Vector3(800, 0, 0);
        yield return MoveToPosition(weaponItemView.transform, originalPosition, 0.25f, Vector3.one);
    }

    private IEnumerator AnimateFrameChangePrev()
    {
        Vector3 targetPositionOut = originalPosition + new Vector3(800, 0, 0);
        yield return MoveToPosition(weaponItemView.transform, targetPositionOut, 0.25f, new Vector3(0.5f, 0.5f, 1f));

        UpdateItemView();

        weaponItemView.transform.localPosition = new Vector3(-800, 0, 0);
        yield return MoveToPosition(weaponItemView.transform, originalPosition, 0.25f, Vector3.one);
    }
    private void UpdateItemView()
    {
        bool isHave;
        switch (type)
        {
            case TypeItem.WEAPON:
                weaponItemView.TakeItem();
                isHave = weaponPlayer[(int)currentWeapon];
                btnBuy.gameObject.SetActive(!isHave);
                btnTake.gameObject.SetActive(isHave);
                break;
            case TypeItem.PANT:
                weaponItemView.TakeItem();
                isHave = pantPlayer[(int)currentPant];
                btnBuy.gameObject.SetActive(!isHave);
                btnTake.gameObject.SetActive(isHave);
                break;
            case TypeItem.SKIN:
                weaponItemView.TakeItem();
                isHave = skinPlayer[(int)currentSkin];
                btnBuy.gameObject.SetActive(!isHave);
                btnTake.gameObject.SetActive(isHave);
                break;
        }
    }

    private void OnClickButtonPrev()
    {
        SoundManager.Instance.PlaySoundOnClick();
        switch (type)
        {
            case TypeItem.WEAPON:
                if ((int)currentWeapon > 0)
                {
                    currentWeapon = (WeaponType)((int)currentWeapon - 1);
                    weaponItemView.OnInit<WeaponItemData>(weaponData.GetWeapon(currentWeapon), player, weaponPlayer[(int)currentWeapon]);
                    StartCoroutine(AnimateFrameChangePrev());
                }
                break;
            case TypeItem.PANT:
                if ((int)currentPant > 0)
                {
                    currentPant = (PantType)((int)currentPant - 1);
                    weaponItemView.OnInit<PantItemData>(pantData.GetPant(currentPant), player, pantPlayer[(int)currentPant]);
                    StartCoroutine(AnimateFrameChangePrev());
                }
                break;
            case TypeItem.SKIN:
                if ((int)currentSkin > 0)
                {
                    currentSkin = (SkinType)((int)currentSkin - 1);
                    weaponItemView.OnInit<SkinItemData>(skinData.GetSkin(currentSkin), player, skinPlayer[(int)currentSkin]);
                    StartCoroutine(AnimateFrameChangePrev());
                }
                break;
        }
    }

    private void OnClickButtonNext()
    {
        SoundManager.Instance.PlaySoundOnClick();
        switch (type)
        {
            case TypeItem.WEAPON:
                if ((int)currentWeapon < weaponData.Listdata.Count - 1)
                {
                    currentWeapon = (WeaponType)((int)currentWeapon + 1);
                    weaponItemView.OnInit<WeaponItemData>(weaponData.GetWeapon(currentWeapon), player, weaponPlayer[(int)currentWeapon]);
                    StartCoroutine(AnimateFrameChangeNext());
                }
                break;
            case TypeItem.PANT:
                if ((int)currentPant < pantData.ListData.Count - 1)
                {
                    currentPant = (PantType)((int)currentPant + 1);
                    weaponItemView.OnInit<PantItemData>(pantData.GetPant(currentPant), player, pantPlayer[(int)currentPant]);
                    StartCoroutine(AnimateFrameChangeNext());
                }
                break;
            case TypeItem.SKIN:
                if ((int)currentSkin < skinData.ListData.Count - 1)
                {
                    currentSkin = (SkinType)((int)currentSkin + 1);
                    weaponItemView.OnInit<SkinItemData>(skinData.GetSkin(currentSkin), player, skinPlayer[(int)currentSkin]);
                    StartCoroutine(AnimateFrameChangeNext());
                }
                break;
        }
    }

    private IEnumerator MoveToPosition(Transform target, Vector3 targetPosition, float duration, Vector3 targetScale)
    {
        Vector3 startPosition = target.localPosition;
        Vector3 startScale = target.localScale;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            target.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            target.localScale = Vector3.Lerp(startScale, targetScale, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        target.localPosition = targetPosition;
        target.localScale = targetScale;
    }
}

public enum TypeItem
{
    WEAPON = 0,
    SKIN = 1,
    PANT = 2,
}
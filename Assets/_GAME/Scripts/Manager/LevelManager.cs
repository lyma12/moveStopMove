using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>, ISubject
{
    private List<IObserver> _observers = new List<IObserver>();
    [SerializeField]
    private Transform playerTransform;
    private Player player;
    public Player Player
    {
        get
        {
            return player;
        }
    }
    [SerializeField]
    private CameraFollower cam;
    [SerializeField]
    private Level[] levelPrefab;
    private Level currentLevel;
    public int MaxLevel
    {
        get { return levelPrefab.Length; }
    }
    private LevelState state;
    public LevelState LevelState
    {
        get { return state; }
        set
        {
            if (state == value) return;
            switch (value)
            {
                case LevelState.ON_ENDGAME:
                    EndGame();
                    break;
                case LevelState.ON_PAUSE:
                    OnPause();
                    break;
                case LevelState.ON_CONTINUE:
                    OnPlay();
                    break;
                case LevelState.ON_RESTART:
                    OnRestart();
                    break;
                case LevelState.PLAY:
                    OnPlay();
                    break;
                case LevelState.ON_VICTORY:
                    OnVictory();
                    break;
            }
            Notify();
        }
    }
    private void Awake()
    {
        state = LevelState.ON_HOME;
    }
    private void OnVictory()
    {
        Time.timeScale = 0;
        state = LevelState.ON_ENDGAME;
        //ObjectPoolManager.Instance.ClearObjectActiveInPool();
        SimplePool.CollectAll();
        DataManager.Instance.ChangeState(State.VICTORY);
    }
    public void ReturnHome()
    {
        state = LevelState.ON_ENDGAME;
        if (currentLevel != null)
        {
            SimplePool.CollectAll();
            SimplePool.Despawn(currentLevel);
            // ObjectPoolManager.Instance.ReturnObjectToPool(currentLevel);
            // ObjectPoolManager.Instance.ClearObjectActiveInPool();
            currentLevel = null;
        }
        Time.timeScale = 1;
        DataManager.Instance.ChangeState(State.MAIN_MENU);
    }
    private void EndGame()
    {
        state = LevelState.ON_ENDGAME;
        Time.timeScale = 0;
        SimplePool.CollectAll();
        //ObjectPoolManager.Instance.ClearObjectActiveInPool();
        DataManager.Instance.ChangeState(State.LOSE);
    }
    public void NextLevel()
    {
        int level = DataManager.Instance.Data.CurrentLevel + 1;
        DataManager.Instance.Data.CurrentLevel = level;
        if (currentLevel != null)
        {
            SimplePool.CollectAll();
            SimplePool.Despawn(currentLevel);
            //ObjectPoolManager.Instance.ReturnObjectToPool(currentLevel);
            // ObjectPoolManager.Instance.ClearObjectActiveInPool();
            currentLevel = null;
        }
        LoadLevel(level);
    }
    public void LoadLevel(int level)
    {
        if (levelPrefab != null && levelPrefab.Length >= level)
        {
            currentLevel = SimplePool.Spawn(levelPrefab[level - 1], transform.position, Quaternion.identity, transform);
            //currentLevel = ObjectPoolManager.Instance.SpawnObject(levelPrefab[level - 1].gameObject, transform.position, Quaternion.identity, transform);
            InitPlayer();
            player.gameObject.SetActive(true);
        }
        LevelState = LevelState.PLAY;
    }
    private void OnPlay()
    {
        Time.timeScale = 1;
        state = LevelState.PLAY;
    }
    private void OnRestart()
    {
        if (currentLevel != null)
        {
            ResetLevel();
        }
        LevelState = LevelState.PLAY;
    }
    private void OnPause()
    {
        state = LevelState.ON_PAUSE;
        Time.timeScale = 0;
    }
    public void InitPlayer()
    {
        if (player == null)
        {
            player = SimplePool.Spawn<Player>(PoolType.Player, playerTransform.position, Quaternion.identity);
            PlayCanva playCanva = UIManager.Instance.OpenUI<PlayCanva>();
            player.Joystick = playCanva.SetJoytick();
            cam.Player = player;
        }
        player.Reset();
        player.transform.position = playerTransform.position;
        var currentWeaponType = (WeaponType)DataManager.Instance.Data.CurrentWeapon;
        var currentPantType = (PantType)DataManager.Instance.Data.CurrentPant;
        var currentSkinType = (SkinType)DataManager.Instance.Data.CurrentSkin;
        player.ChangeWeapon(currentWeaponType);
        player.ChangeSkin(currentSkinType);
        player.ChangePant(currentPantType);
    }
    public void ResetLevel()
    {
        if (currentLevel != null)
        {
            currentLevel.Reset();
            SimplePool.CollectAll();
            SimplePool.Despawn(player);
            currentLevel.gameObject.SetActive(true);
            InitPlayer();
            player.gameObject.SetActive(true);
        }
    }

    public void Attach(IObserver observer)
    {
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this._observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.UpdateState(this);
        }
    }
}

public enum LevelState
{
    PLAY = 0,
    ON_RESTART = 1,
    ON_CONTINUE = 2,
    ON_HOME = 3,
    ON_ENDGAME = 4,
    ON_RETRY = 5,
    ON_VICTORY = 6,
    ON_PAUSE = 7,
}
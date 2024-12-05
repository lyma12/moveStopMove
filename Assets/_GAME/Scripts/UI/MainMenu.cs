using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class MainMenu : UICanvas
{
    [Header("Attribute UI")]
    [SerializeField]
    private Transform frameCradLevel;
    [SerializeField]
    private Button btnPrev;
    [SerializeField]
    private Button btnNext;
    [SerializeField]
    private TMP_Text titleLevel;
    [SerializeField]
    private GameObject frameEnable;
    [SerializeField]
    private GameObject frameAble;
    [SerializeField]
    private Button btnSetting;
    [SerializeField]
    private Button btnPlay;
    [SerializeField]
    private Button btnShop;
    [SerializeField]
    private Button btnProfile;

    [Header("Attribute value")]
    private int currentLevel = 1;
    private Vector3 originalPosition;
    private int maxLevel = 5;
    private int levelProcessing = 1;
    protected override void OnInit()
    {
        base.OnInit();
        btnPrev.onClick.AddListener(OnClickButtonPrev);
        btnNext.onClick.AddListener(OnClickButtonNext);
        btnSetting.onClick.AddListener(OnOpenSetting);
        btnPlay.onClick.AddListener(OnPlay);
        btnShop.onClick.AddListener(OnShop);
        btnProfile.onClick.AddListener(OnProfile);
    }
    private void OnProfile()
    {
        SoundManager.Instance.PlaySoundOnClick();
        UIManager.Instance.OpenUI<ProfileCanva>();
    }
    public override void Setup()
    {
        base.Setup();
        originalPosition = frameCradLevel.localPosition;
        maxLevel = LevelManager.Instance.MaxLevel;
    }

    private void OnShop()
    {
        SoundManager.Instance.PlaySoundOnClick();
        DataManager.Instance.ChangeState(State.SHOP);
    }

    public override void Open()
    {
        base.Open();
        currentLevel = DataManager.Instance.Data.Level;
        levelProcessing = DataManager.Instance.Data.CurrentLevel;
        UpdateLevelTitle();
    }

    private void OnPlay()
    {
        SoundManager.Instance.PlaySoundOnClick();
        DataManager.Instance.SetCurrentLevel(currentLevel);
        DataManager.Instance.ChangeState(State.ON_PLAY);
    }

    private void OnOpenSetting()
    {
        SoundManager.Instance.PlaySoundOnClick();
        UIManager.Instance.OpenUI<SettingCanva>();
    }

    private void OnClickButtonPrev()
    {
        if (currentLevel > 1)
        {
            SoundManager.Instance.PlaySoundOnClick();
            currentLevel--;
            StartCoroutine(AnimateFrameChangePrev());
        }
    }

    private void OnClickButtonNext()
    {
        if (currentLevel < maxLevel)
        {
            SoundManager.Instance.PlaySoundOnClick();
            currentLevel++;
            StartCoroutine(AnimateFrameChangeNext());
        }
    }

    private void UpdateLevelTitle()
    {
        if (currentLevel == 0) currentLevel = 1;
        titleLevel.text = "Level " + currentLevel;
        if (currentLevel <= levelProcessing)
        {
            frameAble.SetActive(true);
            frameEnable.SetActive(false);
            btnPlay.gameObject.SetActive(true);
        }
        else
        {
            frameAble.SetActive(false);
            frameEnable.SetActive(true);
            btnPlay.gameObject.SetActive(false);
        }
    }

    private IEnumerator AnimateFrameChangeNext()
    {
        Vector3 targetPositionOut = originalPosition + new Vector3(-800, 0, 0);
        yield return MoveToPosition(frameCradLevel, targetPositionOut, 0.25f, new Vector3(0.5f, 0.5f, 1f));

        UpdateLevelTitle();

        frameCradLevel.localPosition = new Vector3(800, 0, 0);
        yield return MoveToPosition(frameCradLevel, originalPosition, 0.25f, Vector3.one);
    }

    private IEnumerator AnimateFrameChangePrev()
    {
        Vector3 targetPositionOut = originalPosition + new Vector3(800, 0, 0);
        yield return MoveToPosition(frameCradLevel, targetPositionOut, 0.25f, new Vector3(0.5f, 0.5f, 1f));

        UpdateLevelTitle();

        frameCradLevel.localPosition = new Vector3(-800, 0, 0);
        yield return MoveToPosition(frameCradLevel, originalPosition, 0.25f, Vector3.one);
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
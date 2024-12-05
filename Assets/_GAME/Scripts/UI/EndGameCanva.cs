using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class EndGameCanva : UICanvas
{
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnHome;
    [SerializeField] private Button btnRetry;
    [SerializeField] private TMP_Text titlePopup;
    [SerializeField] private TMP_Text titleInfo;
    protected override void OnInit()
    {
        base.OnInit();
        btnNext.onClick.AddListener(PlayNextLevel);
        btnRetry.onClick.AddListener(RetryCurrentLevel);
        btnHome.onClick.AddListener(ReturnHome);
    }
    private void PlayNextLevel()
    {
        SoundManager.Instance.PlaySoundOnClick();
        LevelManager.Instance.NextLevel();
        UIManager.Instance.CloseAndOpenUI<PlayCanva>();
    }
    private void RetryCurrentLevel()
    {
        SoundManager.Instance.PlaySoundOnClick();
        LevelManager.Instance.LevelState = LevelState.ON_RESTART;
        UIManager.Instance.CloseAndOpenUI<PlayCanva>();
    }
    private void ReturnHome()
    {
        SoundManager.Instance.PlaySoundOnClick();
        LevelManager.Instance.ReturnHome();
    }
    public void SetUpLose()
    {
        titlePopup.text = Contanst.LOSE_TEXT;
        int level = DataManager.Instance.Data.CurrentLevel;
        titleInfo.text = Contanst.LEVEL_TEXT + level.ToString();
    }
    public void SetUIVictory()
    {
        titlePopup.text = Contanst.WIN_TEXT;
        int level = DataManager.Instance.Data.CurrentLevel;
        titleInfo.text = Contanst.LEVEL_TEXT + level.ToString();
    }
}
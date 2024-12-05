using UnityEngine;
using UnityEngine.UI;

public class PlayCanva : UICanvas
{
    [SerializeField]
    private Joystick joystick;
    [SerializeField]
    private Button btnPopupMenu;
    [SerializeField]
    private GameObject popupMenu;
    [SerializeField]
    private Button btnReturn;
    [SerializeField]
    private Button btnHome;
    [SerializeField]
    private Button btnClose;

    public override void CloseDirectly()
    {
        popupMenu.SetActive(false);
        base.CloseDirectly();
    }

    public Joystick SetJoytick()
    {
        return joystick;
    }
    protected override void OnInit()
    {
        base.OnInit();
        btnClose.onClick.AddListener(OnClose);
        btnHome.onClick.AddListener(OnHome);
        btnReturn.onClick.AddListener(OnReturn);
        btnPopupMenu.onClick.AddListener(OpenPopup);
    }
    private void OnHome()
    {
        LevelManager.Instance.ReturnHome();
    }
    private void OnClose()
    {
        popupMenu.SetActive(false);
        LevelManager.Instance.LevelState = LevelState.ON_CONTINUE;
    }
    private void OpenPopup()
    {
        popupMenu.SetActive(true);
        LevelManager.Instance.LevelState = LevelState.ON_PAUSE;
    }
    private void OnReturn()
    {
        popupMenu.SetActive(false);
        LevelManager.Instance.LevelState = LevelState.ON_RESTART;
    }
}
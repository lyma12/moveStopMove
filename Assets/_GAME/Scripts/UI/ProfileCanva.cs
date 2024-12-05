using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileCanva : UICanvas
{
    [SerializeField]
    private Button btnClose;
    [SerializeField]
    private TMP_Text currentLevel;
    [SerializeField]
    private TMP_Text currentCoin;
    protected override void OnInit()
    {
        base.OnInit();
        btnClose.onClick.AddListener(CloseDirectly);
    }
    public override void Setup()
    {
        base.Setup();
        int coin = DataManager.Instance.Data.CoinPlayer;
        int levelProcessing = DataManager.Instance.Data.Level;
        currentCoin.text = Contanst.CoinString(coin);
        currentLevel.text = Contanst.LevelString(levelProcessing);
    }
}
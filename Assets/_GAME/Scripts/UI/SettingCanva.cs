using UnityEngine;
using UnityEngine.UI;

public class SettingCanva : UICanvas
{
    [SerializeField]
    private Button btnClose;
    [SerializeField]
    private Toggle toggleSound;
    [SerializeField]
    private Slider sliderVolume;
    protected override void OnInit()
    {
        base.OnInit();
        btnClose.onClick.AddListener(CloseDirectly);
        toggleSound.onValueChanged.AddListener(OnAndOffSound);
        sliderVolume.onValueChanged.AddListener(SetVolume);
    }
    public override void Setup()
    {
        base.Setup();
        toggleSound.isOn = SoundManager.Instance.isOnSound;
        sliderVolume.value = SoundManager.Instance.volume;
    }
    private void SetVolume(float value)
    {
        SoundManager.Instance.SetVolume(value);
    }
    private void OnAndOffSound(bool isSetUp)
    {
        SoundManager.Instance.PlaySoundOnClick();
        SoundManager.Instance.SetSoundActive(isSetUp);
    }
}
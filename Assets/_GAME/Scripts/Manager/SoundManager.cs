using UnityEngine;
using Newtonsoft.Json;
using System;
public class SoundManager : Singleton<SoundManager>, IObserver
{
    [Header("-----------------------Audio Source----------------------")]
    [SerializeField]
    private AudioSource musicSound;
    [SerializeField]
    private AudioSource SFXSOund;
    [Header("-----------------------Audio Clip----------------------")]
    [SerializeField]
    private AudioClip soundBackGround;
    [SerializeField]
    private AudioClip soundMenuSound;
    [SerializeField]
    private AudioClip soundOnClick;
    [SerializeField]
    private AudioClip soundOnClose;
    [SerializeField]
    private AudioClip soundOnVictory;
    [SerializeField]
    private AudioClip soundOnLoser;
    [SerializeField] private AudioClip soundOnBuy;
    [SerializeField] private AudioClip soundOnLevelUp;
    private SettingSoundGameData data;
    public bool isOnSound => data.isSoundOn;
    public float volume => data.volume;
    private void Awake()
    {
        data = LoadDataSettingSound();
        musicSound.clip = soundMenuSound;
        musicSound.Play();
        musicSound.volume = data.volume;
        LevelManager.Instance.Attach(this);
        DataManager.Instance.Attach(this);
    }
    public void SetVolume(float volume)
    {
        data.volume = volume;
        musicSound.volume = volume;
    }
    private SettingSoundGameData LoadDataSettingSound()
    {
        string s = PlayerPrefs.GetString(Contanst.KEY_DATA_SETTING_SOUND);
        if (string.IsNullOrEmpty(s))
        {
            return new SettingSoundGameData();
        }
        return JsonConvert.DeserializeObject<SettingSoundGameData>(s);
    }
    private void SaveData()
    {
        if (data == null)
        {
            data = new SettingSoundGameData();
        }
        string s = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(Contanst.KEY_DATA_SETTING_SOUND, s);
    }
    public void UpdateState(ISubject subject)
    {
        if (subject is LevelManager)
        {
            LevelState levelState = (subject as LevelManager).LevelState;
            switch (levelState)
            {
                case LevelState.PLAY:
                    OnGamePlay();
                    break;
                case LevelState.ON_PAUSE:
                    OnGamePause();
                    break;
                case LevelState.ON_VICTORY:
                    PlaySFX(soundOnVictory);
                    OnGamePause();
                    break;
                case LevelState.ON_ENDGAME:
                    PlaySFX(soundOnLoser);
                    OnGamePause();
                    break;
            }
        }
    }
    public void PlaySFX(AudioClip audioClip)
    {
        SFXSOund.PlayOneShot(audioClip);
    }
    private void OnGamePlay()
    {
        musicSound.clip = soundBackGround;
        musicSound.Play();
    }
    public void OnGamePause()
    {
        musicSound.clip = soundMenuSound;
        musicSound.Play();
    }
    public void PlaySoundOnClick()
    {
        PlaySFX(soundOnClick);
    }
    public void PlaySoundOnBuy()
    {
        PlaySFX(soundOnBuy);
    }
    public void PlaySoundOnLevelUp()
    {
        PlaySFX(soundOnLevelUp);
    }
    public void PlaySoundOnClose()
    {
        PlaySFX(soundOnClose);
    }
    public void SetSoundActive(bool isSoundOn)
    {
        SFXSOund.mute = isSoundOn;
    }
    private void OnApplicationQuit()
    {
        SaveData();
    }
}

class SettingSoundGameData
{
    public SettingSoundGameData()
    {
        volume = 1f;
        isSoundOn = true;
    }
    public float volume;
    public bool isSoundOn;
}
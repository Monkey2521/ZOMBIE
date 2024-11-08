using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;
using ZombieSurvival.UI;

public class SettingsMenu : UIMenu 
{
    [Header("Settings")]
    [SerializeField] private Sprite _settingHandleOn;
    [SerializeField] private Sprite _settingHandleOff;
    [SerializeField] private Sprite _settingBackgroundOn;
    [SerializeField] private Sprite _settingBackgroundOff;

    [Header("Sounds settings")]
    [SerializeField] private Image _soundsImage;
    [SerializeField] private Sprite _soundsOnIcon;
    [SerializeField] private Sprite _soundsOffIcon;

    [Space(5)]
    [SerializeField] private Image _soundsOffImage;
    [SerializeField] private Image _soundsOnImage;
    [SerializeField] private Image _soundsBackgroundImage;

    [Header("Music setting")]
    [SerializeField] private Image _musicImage;
    [SerializeField] private Sprite _musicOnIcon;
    [SerializeField] private Sprite _musicOffIcon;

    [Space(5)]
    [SerializeField] private Image _musicOffImage;
    [SerializeField] private Image _musicOnImage;
    [SerializeField] private Image _musicBackgroundImage;
    
    [Header("Vibration setting")]
    [SerializeField] private Image _vibrationImage;
    [SerializeField] private Sprite _vibrationOnIcon;
    [SerializeField] private Sprite _vibrationOffIcon;

    [Space(5)]
    [SerializeField] private Image _vibrationOffImage;
    [SerializeField] private Image _vibrationOnImage;
    [SerializeField] private Image _vibrationBackgroundImage;

    [Header("Mixers settings")]
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private List<AudioMixerGroup> _soundGroups;

    private SettingStates _musicState;
    private SettingStates _soundState;
    private SettingStates _vibrationState;

    private const float MIN_BOUNDS = -80f;
    private const float MAX_MUSIC_BOUNDS = -5f;
    private const float MAX_SOUNDS_BOUNDS = -10f;

    private const string MIXER_NAME = "Master";
    private const string MUSIC_VOLUME_VAR = "MusicVolume";
    private const string SOUNDS_VOLUME_VAR = "SoundsVolume";


    private const float VIBRATION_OFF_BOUNDS = 0f;
    private const float VIBRATION_ON_BOUNDS = 1f;
    private const string VIBRATION_VAR = "Vibration";

    public void Initialize()
    {
        #region Music
        float bounds = Load(MUSIC_VOLUME_VAR);

        _musicGroup.audioMixer.SetFloat(MIXER_NAME, bounds);
        _musicState = bounds == MIN_BOUNDS ? SettingStates.Disabled : SettingStates.Enabled;

        _musicOnImage.sprite = _settingHandleOn;
        _musicOffImage.sprite = _settingHandleOff;

        if (_musicState.Equals(SettingStates.Enabled))
        {
            _musicImage.sprite = _musicOnIcon;
            _musicState = SettingStates.Enabled;

            _musicOffImage.enabled = false;
            _musicOnImage.enabled = true;

            _musicBackgroundImage.sprite = _settingBackgroundOn;
        }
        else
        {
            _musicImage.sprite = _musicOffIcon;
            _musicState = SettingStates.Disabled;

            _musicOffImage.enabled = true;
            _musicOnImage.enabled = false;

            _musicBackgroundImage.sprite = _settingBackgroundOff;
        }
        #endregion

        #region Sounds
        bounds = Load(SOUNDS_VOLUME_VAR);

        _soundState = bounds == MIN_BOUNDS ? SettingStates.Disabled : SettingStates.Enabled;
        
        foreach (var mixer in _soundGroups)
        {
            mixer.audioMixer.SetFloat(MIXER_NAME, bounds);
        }
       
        _soundsOnImage.sprite = _settingHandleOn;
        _soundsOffImage.sprite = _settingHandleOff;

        if (_soundState.Equals(SettingStates.Enabled))
        {
            _soundsImage.sprite = _soundsOnIcon;
            _soundState = SettingStates.Enabled;

            _soundsOffImage.enabled = false;
            _soundsOnImage.enabled = true;

            _soundsBackgroundImage.sprite = _settingBackgroundOn;
        }
        else
        {
            _soundsImage.sprite = _soundsOffIcon;
            _soundState = SettingStates.Disabled;

            _soundsOffImage.enabled = true;
            _soundsOnImage.enabled = false;

            _soundsBackgroundImage.sprite = _settingBackgroundOff;
        }
        #endregion

        #region Vibration
        bounds = Load(VIBRATION_VAR);

        _vibrationState = bounds == VIBRATION_OFF_BOUNDS ? SettingStates.Disabled : SettingStates.Enabled;

        _vibrationOnImage.sprite = _settingHandleOn;
        _vibrationOffImage.sprite = _settingHandleOff;

        if (_vibrationState.Equals(SettingStates.Enabled))
        {
            _vibrationImage.sprite = _vibrationOnIcon;
            _vibrationState = SettingStates.Enabled;

            _vibrationOffImage.enabled = false;
            _vibrationOnImage.enabled = true;

            _vibrationBackgroundImage.sprite = _settingBackgroundOn;
        }
        else
        {
            _vibrationImage.sprite = _vibrationOffIcon;
            _vibrationState = SettingStates.Disabled;

            _vibrationOffImage.enabled = true;
            _vibrationOnImage.enabled = false;

            _vibrationBackgroundImage.sprite = _settingBackgroundOff;
        }
        #endregion
    }

    public void OnSoundsClick()
    {
        float bounds;

        if (_soundState.Equals(SettingStates.Enabled))
        {
            foreach (var mixer in _soundGroups)
            {
                mixer.audioMixer.SetFloat(MIXER_NAME, MIN_BOUNDS);
            }

            _soundState = SettingStates.Disabled;
            _soundsImage.sprite = _soundsOffIcon;

            bounds = MIN_BOUNDS;
        }
        else
        {
            foreach (var mixer in _soundGroups)
            {
                mixer.audioMixer.SetFloat(MIXER_NAME, MAX_SOUNDS_BOUNDS);
            }

            _soundState = SettingStates.Enabled;
            _soundsImage.sprite = _soundsOnIcon;

            bounds = MAX_SOUNDS_BOUNDS;
        }

        if (_soundState.Equals(SettingStates.Enabled))
        {
            _soundsImage.sprite = _soundsOnIcon;
            _soundState = SettingStates.Enabled;

            _soundsOffImage.enabled = false;
            _soundsOnImage.enabled = true;

            _soundsBackgroundImage.sprite = _settingBackgroundOn;
        }
        else
        {
            _soundsImage.sprite = _soundsOffIcon;
            _soundState = SettingStates.Disabled;

            _soundsOffImage.enabled = true;
            _soundsOnImage.enabled = false;

            _soundsBackgroundImage.sprite = _settingBackgroundOff;
        }

        Save(SOUNDS_VOLUME_VAR, bounds);
    }

    public void OnMusicClick()
    {
        float bounds;

        if (_musicState.Equals(SettingStates.Enabled))
        {
            _musicGroup.audioMixer.SetFloat(MIXER_NAME, MIN_BOUNDS);

            _musicState = SettingStates.Disabled;
            _musicImage.sprite = _musicOffIcon;

            bounds = MIN_BOUNDS;
        }
        else
        {
            _musicGroup.audioMixer.SetFloat(MIXER_NAME, MAX_MUSIC_BOUNDS);

            _musicState = SettingStates.Enabled;
            _musicImage.sprite = _musicOnIcon;

            bounds = MAX_MUSIC_BOUNDS;
        }

        if (_musicState.Equals(SettingStates.Enabled))
        {
            _musicImage.sprite = _musicOnIcon;
            _musicState = SettingStates.Enabled;

            _musicOffImage.enabled = false;
            _musicOnImage.enabled = true;

            _musicBackgroundImage.sprite = _settingBackgroundOn;
        }
        else
        {
            _musicImage.sprite = _musicOffIcon;
            _musicState = SettingStates.Disabled;

            _musicOffImage.enabled = true;
            _musicOnImage.enabled = false;

            _musicBackgroundImage.sprite = _settingBackgroundOff;
        }

        Save(MUSIC_VOLUME_VAR, bounds);
    }

    public void OnVibrationClick()
    {
        float bounds;

        if (_vibrationState.Equals(SettingStates.Enabled))
        {
            _vibrationState = SettingStates.Disabled;
            _vibrationImage.sprite = _vibrationOffIcon;

            bounds = VIBRATION_OFF_BOUNDS;
        }
        else
        {
            _vibrationState = SettingStates.Enabled;
            _vibrationImage.sprite = _musicOnIcon;

            bounds = VIBRATION_ON_BOUNDS;
        }

        if (_vibrationState.Equals(SettingStates.Enabled))
        {
            _vibrationImage.sprite = _vibrationOnIcon;
            _vibrationState = SettingStates.Enabled;

            _vibrationOffImage.enabled = false;
            _vibrationOnImage.enabled = true;

            _vibrationBackgroundImage.sprite = _settingBackgroundOn;
        }
        else
        {
            _vibrationImage.sprite = _vibrationOffIcon;
            _vibrationState = SettingStates.Disabled;

            _vibrationOffImage.enabled = true;
            _vibrationOnImage.enabled = false;

            _vibrationBackgroundImage.sprite = _settingBackgroundOff;
        }

        Save(VIBRATION_VAR, bounds);
    }

    private void Save(string varName, float value)
    {
        PlayerPrefs.SetFloat(varName, value);
        PlayerPrefs.Save();
    }

    private float Load(string varName)
    {
        if (PlayerPrefs.HasKey(varName))
        {
            return PlayerPrefs.GetFloat(varName);
        }
        else
        {
            switch (varName)
            {
                case MUSIC_VOLUME_VAR: return MAX_MUSIC_BOUNDS;
                case SOUNDS_VOLUME_VAR: return MAX_SOUNDS_BOUNDS;
                case VIBRATION_VAR: return VIBRATION_ON_BOUNDS;
                default:
                    Debug.Log("Missing key!");
                    return 0;
            }
        }
    }

    [ContextMenu("ResetData")]
    private void ResetData()
    {
        PlayerPrefs.DeleteKey(SOUNDS_VOLUME_VAR);
        PlayerPrefs.DeleteKey(MUSIC_VOLUME_VAR);
        PlayerPrefs.DeleteKey(VIBRATION_VAR);
    }

    private enum SettingStates
    {
        Enabled,
        Disabled
    }
}

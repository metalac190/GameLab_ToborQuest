using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public AudioMixer _mixer;
    
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _dialogueSlider;
    [SerializeField] private Slider _ambienceSlider;

    private const string MasterMixer = "MasterVol";
    private const string MusicMixer = "MusicVol";
    private const string SfxMixer = "SFXVol";
    private const string DialogueMixer = "dialogueVol";
    private const string AmbienceMixer = "AmbientVol";

    private void Start()
    {
        LoadSliderValues();
    }

    private void OnEnable()
    {
        ExtrasSettings.OnDataChanged += LoadSliderValues;
    }

    private void OnDisable()
    {
        ExtrasSettings.OnDataChanged -= LoadSliderValues;
    }
    
    public void LoadSliderValues()
    {
        SetValue(_masterSlider, MasterMixer, SettingsSaver.MasterVolume);
        SetValue(_musicSlider, MusicMixer, SettingsSaver.MusicVolume);
        SetValue(_sfxSlider, SfxMixer, SettingsSaver.SfxVolume);
        SetValue(_dialogueSlider, DialogueMixer, SettingsSaver.DialogueVolume);
        SetValue(_ambienceSlider, AmbienceMixer, SettingsSaver.AmbientVolume);
    }

    private void SetValue(Slider slider, string mixer, float value)
    {
        slider.value = value;
        _mixer.SetFloat(mixer, Convert(value));
    }
    
    public void SetMasterVol(float value)
    {
        _mixer.SetFloat(MasterMixer, Convert(value));
        SettingsSaver.MasterVolume = value;
    }
    
    public void SetMusicLvl(float value)
    {
        _mixer.SetFloat(MusicMixer, Convert(value));
        SettingsSaver.MusicVolume = value;
    }

    public void SetSfxLvl(float value)
    {
        _mixer.SetFloat(SfxMixer, Convert(value));
        SettingsSaver.SfxVolume = value;
    }
    
    public void SetDialogueVol(float value)
    {
        _mixer.SetFloat(DialogueMixer, Convert(value));
        SettingsSaver.DialogueScale = value;
    }
    
    public void SetAmbientLvl(float value)
    {
        _mixer.SetFloat(AmbienceMixer, Convert(value));
        SettingsSaver.AmbientVolume = value;
    }

    private static float Convert(float value) => value == 0 ? -80 : Mathf.Log10(value) * 20;
}

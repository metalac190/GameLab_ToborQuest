using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
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
    private const string ToborMixer = "ToborVol";
    private const string DialogueMixer = "dialogueVol";
    private const string AmbienceMixer = "AmbientVol";

    private void Start()
    {
        LoadSliderValues();
    }

    private void OnEnable()
    {
        ExtrasSettings.OnResetData += LoadSliderValues;
    }

    private void OnDisable()
    {
        ExtrasSettings.OnResetData -= LoadSliderValues;
    }
    
    public void LoadSliderValues()
    {
        SetValue(_masterSlider, MasterMixer, SavingManager.MasterVolume);
        SetValue(_musicSlider, MusicMixer, SavingManager.MusicVolume);
        SetValue(_sfxSlider, ToborMixer, SavingManager.SfxVolume);
        SetValue(_sfxSlider, SfxMixer, SavingManager.SfxVolume);
        SetValue(_dialogueSlider, DialogueMixer, SavingManager.DialogueVolume);
        SetValue(_ambienceSlider, AmbienceMixer, SavingManager.AmbientVolume);
    }

    private void SetValue(Slider slider, string mixer, float value)
    {
        slider.value = value;
        _mixer.SetFloat(mixer, Convert(value));
    }
    
    public void SetMasterVol(float value)
    {
        _mixer.SetFloat(MasterMixer, Convert(value));
        SavingManager.MasterVolume = value;
    }
    
    public void SetMusicLvl(float value)
    {
        _mixer.SetFloat(MusicMixer, Convert(value));
        SavingManager.MusicVolume = value;
    }

    public void SetSfxLvl(float value)
    {
        _mixer.SetFloat(SfxMixer, Convert(value));
        _mixer.SetFloat(ToborMixer, Convert(value));
        SavingManager.SfxVolume = value;
    }
    
    public void SetDialogueVol(float value)
    {
        _mixer.SetFloat(DialogueMixer, Convert(value));
        SavingManager.DialogueScale = value;
    }
    
    public void SetAmbientLvl(float value)
    {
        _mixer.SetFloat(AmbienceMixer, Convert(value));
        SavingManager.AmbientVolume = value;
    }

    private static float Convert(float value) => value == 0 ? -80 : Mathf.Log10(value) * 20;
}

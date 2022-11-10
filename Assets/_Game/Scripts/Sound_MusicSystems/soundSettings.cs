using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class soundSettings : MonoBehaviour
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
        // TODO: Store values using save system and set slider values on start instead
        
        Debug.Log("Resetting all volume mixers to slider defaults in settings menu", gameObject);
        _mixer.SetFloat(MasterMixer, Convert(_masterSlider.value));
        _mixer.SetFloat(MusicMixer, Convert(_musicSlider.value));
        _mixer.SetFloat(SfxMixer, Convert(_sfxSlider.value));
        _mixer.SetFloat(ToborMixer, Convert(_sfxSlider.value));
        _mixer.SetFloat(DialogueMixer, Convert(_dialogueSlider.value));
        _mixer.SetFloat(AmbienceMixer, Convert(_ambienceSlider.value));
    }
    
    public void setMasterVol(float _masterLvl)
    {
        _mixer.SetFloat(MasterMixer, Convert(_masterLvl));
    }
    
    public void setMusicLvl(float _musicLvl)
    {
        _mixer.SetFloat(MusicMixer, Convert(_musicLvl));
    }

    public void setSFXLvl(float _sfxLvl)
    {
        _mixer.SetFloat(SfxMixer, Convert(_sfxLvl));
        _mixer.SetFloat(ToborMixer, Convert(_sfxLvl));
    }
    
    public void setDialogueVol(float _dialogueVol)
    {
        _mixer.SetFloat(DialogueMixer, Convert(_dialogueVol));
    }
    
    public void setAmbientLvl(float _ambientVol)
    {
        _mixer.SetFloat(AmbienceMixer, Convert(_ambientVol));
    }

    private static float Convert(float value) => value == 0 ? -80 : Mathf.Log10(value) * 20;
}

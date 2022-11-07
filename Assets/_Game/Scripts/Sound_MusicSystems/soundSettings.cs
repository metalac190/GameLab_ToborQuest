using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundSettings : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void setSFXLvl(float _sfxLvl)
    {
        masterMixer.SetFloat("SFXVol", Convert(_sfxLvl));
        masterMixer.SetFloat("ToborVol", Convert(_sfxLvl));
    }
    public void setAmbientLvl(float _ambientVol)
    {
        masterMixer.SetFloat("AmbientVol", Convert(_ambientVol));

    }
    public void setMusicLvl(float _musicLvl)
    {
        masterMixer.SetFloat("MusicVol", Convert(_musicLvl));
    }
    public void setDialogueVol(float _dialogueVol)
    {
        masterMixer.SetFloat("dialogueVol", Convert(_dialogueVol));
    }
    public void setMasterVol(float _masterLvl)
    {
        masterMixer.SetFloat("MasterVol", Convert(_masterLvl));
    }

    public void ClearVolume()
    {
        masterMixer.ClearFloat("musicVol");
    }

    private static float Convert(float value) => value == 0 ? -80 : Mathf.Log10(value) * 20;
}

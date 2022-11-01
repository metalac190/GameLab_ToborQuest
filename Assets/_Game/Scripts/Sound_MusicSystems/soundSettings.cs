using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundSettings : MonoBehaviour
{
    public AudioMixer masterMixer;

    public void setSFXLvl(float _sfxLvl)
    {
        masterMixer.SetFloat("SFXVol", _sfxLvl);
        masterMixer.SetFloat("ToborVol", _sfxLvl);
    }
    public void setAmbientLvl(float _ambientVol)
    {
        masterMixer.SetFloat("AmbientVol", _ambientVol);

    }
    public void setMusicLvl(float _musicLvl)
    {
        masterMixer.SetFloat("MusicVol", _musicLvl);
    }
    public void setMasterVol(float _masterLvl)
    {
        masterMixer.SetFloat("MasterVol", _masterLvl);
    }

    public void ClearVolume()
    {
        masterMixer.ClearFloat("musicVol");
    }
}

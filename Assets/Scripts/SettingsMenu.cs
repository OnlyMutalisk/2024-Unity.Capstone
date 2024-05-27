using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{
    public AudioMixer BGM;
    public AudioMixer SFX;
    public static GameObject Popup;

    public void setBGM(float Volume)
    {
        BGM.SetFloat("BGM", Volume);
    }

    public void MuteBGM(bool muted)
    {
        if (muted) {
            BGM.SetFloat("BGM", -80);
        }
    }
    public void setSFX(float Volume)
    {
        SFX.SetFloat("SFX", Volume);
    }

    public void MuteSFX(bool muted)
    {
        if (muted)
        {
            SFX.SetFloat("SFX", -80);
        }
    }

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void CloseMenu()
    {
        Popup = GameObject.Find("SettingsPage");
        Popup.SetActive(false);

    }
}

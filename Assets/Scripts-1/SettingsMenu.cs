using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void setvolume(float Volume)
    {
        audioMixer.SetFloat("Volume", Volume);
    }

    public void MuteButton(bool muted)
    {
        if (muted) {
            audioMixer.SetFloat("Volume", -80);
        }
    }

}

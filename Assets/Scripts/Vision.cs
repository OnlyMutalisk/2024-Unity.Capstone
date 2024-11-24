using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vision : MonoBehaviour
{
    public static Vision instance;
    public void Awake() { instance = this; }
    bool isActive = false;

    public void VisionOnOff()
    {
        isActive = !isActive;

        foreach (var mob in Mob.Mobs)
        {
            mob.vision.SetActive(isActive);
        }
    }
    public void VisionOnOff(bool value)
    {
        isActive = !isActive;

        foreach (var mob in Mob.Mobs)
        {
            mob.vision.SetActive(value);
        }
    }
}

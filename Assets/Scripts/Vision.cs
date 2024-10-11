using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vision : MonoBehaviour
{
    bool isActive = false;

    // Start is called before the first frame update
    public void VisionOnOff()
    {
        isActive = !isActive;

        foreach (var mob in Mob.Mobs)
        {
            mob.vision.SetActive(isActive);
        }
    }
}

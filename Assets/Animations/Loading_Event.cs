using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading_Event : MonoBehaviour
{
    public void End()
    {
        gameObject.SetActive(false);
    }

    public void StopAnimation()
    {
        transform.Find("Animation").gameObject.SetActive(false);
    }
}
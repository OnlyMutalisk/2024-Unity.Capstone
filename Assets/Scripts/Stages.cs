using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stages : MonoBehaviour
{
    // 기본값 False
    public static bool[] isOn = new bool[99];
    public int index;

    // Map 0 은 Open
    private void Awake()
    {
        isOn[0] = true;
    }

    private void Start()
    {
        if (isOn[index] == false) gameObject.SetActive(false);
    }
}

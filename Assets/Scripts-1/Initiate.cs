using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initiate : MonoBehaviour
{
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}

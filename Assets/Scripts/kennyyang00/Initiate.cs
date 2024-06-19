using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Initiate : MonoBehaviour
{
    public static GameObject Popup;
    void Awake()
    {
        Invoke("LoadScene", 0); // 바로 다음 씬으로 넘어가기
    }
    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //다음씬 넘어가기
    }
}



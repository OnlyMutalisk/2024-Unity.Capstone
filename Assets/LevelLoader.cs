using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public GameObject Cover;
    public void start()
    {
        transition.SetTrigger("Fadein");
        Cover.SetActive(false);   //덮는 object 비활성화
    }

}

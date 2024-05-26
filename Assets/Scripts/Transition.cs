using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator transition;
    public GameObject Cover;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Time.timeSinceLevelLoad >= 5.0f) // 애니메이션 진행중일 동안은 터치해도 안넘어가게 (현재는 5초)
            {
                LoadNextLevel();
            }
        }
    }
    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));  //다음씬 넘어가는 coroutine
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Cover.SetActive(true); //덮는 object 활성화
        transition.SetTrigger("Fadeout");   //화면 어둡게
        yield return new WaitForSeconds(1);  //애니메이션 진행시간
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

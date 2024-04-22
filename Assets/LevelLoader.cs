using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LoadNextLevel();
        }
    }

        public void LoadNextLevel()
        {
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex+1));
        }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Fadein");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelIndex);
    }
}

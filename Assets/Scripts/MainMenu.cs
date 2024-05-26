using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public Animator transition;
    public GameObject Cover;

    void start()
    {
        GameObject Popup;
        Popup = GameObject.FindWithTag("Settings");
        Popup.SetActive(false);
    }


    public void StartGame(int scenenumber) {
        LoadNextLevel();   //무조건 다음 scene으로 넘기기
    }

    public void ContinueGame(int scenenumber)
    {
        LoadNextLevel();
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        Cover.SetActive(true);
        transition.SetTrigger("Fadeout");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        //Popup.SetActive(true);
    }
}

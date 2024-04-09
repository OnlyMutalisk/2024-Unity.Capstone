using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour{

    public void StartGame(string scenenumber) {
        SceneManager.LoadScene(scenenumber);
    }

    public void QuitGame() {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}

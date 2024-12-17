using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    //public GameObject buttonPlay;
    public GameObject buttonQuit;
    public GameObject GameManagerGO;
    private bool isPaused = false; 

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

   
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f; 
        buttonQuit.SetActive(true);
        GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.Pause);

    }

    // Oyunu devam ettirir
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f; 
        buttonQuit.SetActive(false);
        GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.Gameplay);
    }

    // Oyundan çýkar
    public void QuitGame()
    {
        Time.timeScale = 1f; 
        Application.Quit(); 
    }
}

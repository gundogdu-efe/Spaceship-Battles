using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    //Reference to the game objects
    public GameObject quitButton;
    public GameObject playButton;
    public GameObject playerShip;
    public GameObject enemySpawner;
    public GameObject GameOverGO;
    public GameObject PowerUpSpawner;
    public GameObject scoreUITextGO;
    public GameObject MeteorSpawner;
    public GameObject PauseMenuGO;

    public enum GameManagerState
    {

        Opening,
        Gameplay,
        Pause,
        GameOver, 
    }
    GameManagerState GMState;
     
    //use this for initalization
    void Start()
    {
        GMState = GameManagerState.Opening;
    }

    void UpdateGameManagerState()
    {
        switch(GMState)
        {
            case GameManagerState.Opening:
                //hide game over
                GameOverGO.SetActive(false);
                //set play button visible (active)
                playButton.SetActive(true);
                quitButton.SetActive(true);
                break;
            case GameManagerState.Gameplay:             
                //hide the play button on game play state
                playButton.SetActive(false);
                quitButton.SetActive(false);

                //set the player visible (active) and init the player lives
                playerShip.GetComponent<PlayerControl>().Init();
                //start enemy spawner
                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
                //start power up spawner
                PowerUpSpawner.GetComponent<PowerUpSpawner>().SchedulePowerUpSpawner();
                //start meteor spawner
                MeteorSpawner.GetComponent<MeteorGenerator>().ScheduleMeteorSpawner();

                break;
            case GameManagerState.Pause:
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                PowerUpSpawner.GetComponent<PowerUpSpawner>().UnschedulePowerUpSpawner();
                MeteorSpawner.GetComponent<MeteorGenerator>().UnscheduleMeteorSpawner();
                scoreUITextGO.GetComponent<GameScore>();
                playButton.SetActive(false);
                quitButton.SetActive(true);
                break;
            case GameManagerState.GameOver:
                //stop enemy spawner
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                scoreUITextGO.GetComponent<GameScore>().Score = 0;

                              
                //stop power up spawner
                PowerUpSpawner.GetComponent<PowerUpSpawner>().UnschedulePowerUpSpawner();
                //stop meteor spawner
                MeteorSpawner.GetComponent<MeteorGenerator>().UnscheduleMeteorSpawner();
                //display game over
                GameOverGO.SetActive(true);
                //change game manager state to opening state after 3 seconds
                Invoke("ChangeToOpeningState", 3f);
                break;
        }
    }

    //Function to set the game manager state
    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }
    
    //play button will call this function when the user clicks the button
    public void StartGamePlay()
    {
        GMState = GameManagerState.Gameplay;
        UpdateGameManagerState();
    }

    //function to change game manager state to opening state
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }

}

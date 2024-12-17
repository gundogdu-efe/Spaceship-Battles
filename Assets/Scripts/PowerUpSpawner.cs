using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject PowerUp;
    public GameObject PowerUp2;

    float maxSpawnRateInSeconds = 15f;
   
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPowerUp()
    {
        //this is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //this is the top-right point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //instantiate a powerup
        GameObject powerUp = (GameObject)Instantiate(PowerUp);
        powerUp.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        GameObject powerUp2 = (GameObject)Instantiate(PowerUp2);
        powerUp2.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        ScheduleNextPowerUpSpawn();

    }

    void ScheduleNextPowerUpSpawn()
    {
        float spawnInSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            //pick a number between 1 and maxSpawnRateInSeconds
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);

        }
        else

            spawnInSeconds = 10f;

        Invoke("SpawnPowerUp", spawnInSeconds);
    }

    public void SchedulePowerUpSpawner()
    {
        Invoke("SpawnPowerUp", maxSpawnRateInSeconds);

       
    }
    //function to stop powerup spawner
    public void UnschedulePowerUpSpawner()
    {
        CancelInvoke("SpawnPowerUp");
       

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorGenerator : MonoBehaviour
{
    public GameObject MeteorGO; //this is the meteor prefab

    float maxSpawnRateInSeconds = 6f;
    void Start()
    {
        ScheduleMeteorSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnMeteor()
    {
        //this is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //this is the top-right point of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //instantiate a meteor
        GameObject aMeteor = (GameObject)Instantiate(MeteorGO);
        aMeteor.transform.position = new Vector2(Random.Range(min.x, max.x), max.y);

        //schedule when to spawn next meteor
        ScheduleNextMeteorSpawn();

    }

    void ScheduleNextMeteorSpawn()
    {
        float spawnInSeconds;

        if (maxSpawnRateInSeconds > 1f)
        {
            //pick a number between 1 and maxSpawnRateInSeconds
            spawnInSeconds = Random.Range(1f, maxSpawnRateInSeconds);

        }
        else

            spawnInSeconds = 10f;

        Invoke("SpawnMeteor", spawnInSeconds);
    }

   
    //function to start meteor spawner
    public void ScheduleMeteorSpawner()
    {
       

        Invoke("SpawnMeteor", maxSpawnRateInSeconds);

      
    }
    //function to stop meteor spawner
    public void UnscheduleMeteorSpawner()
    {
        CancelInvoke("SpawnMeteor");


    }
}

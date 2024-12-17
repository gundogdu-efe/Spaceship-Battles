using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUp : MonoBehaviour
{
    public bool activateShield;
    public bool addGuns;
    float speed;
   

    void Start()
    {
        DeactivateShield(); 
        
        speed = 2f; 
    }

    // Update is called once per frame
    void Update()
    {
        //get the enemy current position
        Vector2 position = transform.position;

        //compute the enemy new position
        position = new Vector2(position.x, position.y - speed * Time.deltaTime);

        //update the enemy position
        transform.position = position;

        //this is the bottom-left point of the screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        //if the enemy went outside the screen on the bottom, then destroy the enemy
        if (transform.position.y < min.y)
        {
            Destroy(gameObject);
        }
    }

    void ActivateShield()
    {
        activateShield= true;   
    }
    void DeactivateShield()
    {
        activateShield = false; 
    }

    void ActiveGuns()
    {
        addGuns = true;

    }

    void DeactiveGuns()
    {
        addGuns = false;
    }
 

    void OnTriggerEnter2D(Collider2D col)
    {
     
        if (col.tag == "PlayerShipTag")
        {
            ActiveGuns();
            ActivateShield();
            Destroy(gameObject);
        }

    }
}


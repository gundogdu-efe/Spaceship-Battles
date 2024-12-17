using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using TMPro;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    public GameObject GameManagerGO;
    public GameObject PlayerBulletGO; //this is the player's bullet prefab
    public GameObject bulletPosition01;
    public GameObject bulletPosition02;
    public GameObject bulletUp01;
    public GameObject bulletUp02;
    public GameObject ExplosionGO;
    public GameObject PowerUpAnim;
    public GameObject Shield;
    public GameObject PowerUp;
    public GameObject PowerUp02;

    public TextMeshProUGUI LivesUIText;
    const int MaxLives = 3;//maximum player lives
    int lives;//current player lives

    public float speed;
    private bool isMultiShotActive = false;
    private AudioSource audioSource;
    private bool isShieldActive = false;

    public void Init()
    {
        isShieldActive = false;
        lives = MaxLives;
        //update the lives UI text
        LivesUIText.text = lives.ToString();
        //reset the player position to the center of the screen
        transform.position = new Vector2(0, 0);

        //set this player game object to active
        gameObject.SetActive(true);
    }


    void Start()
    {
        DeactiveGuns();
        DeactivateShield();
        isShieldActive = false;
        //Shield = transform.Find("Shield").gameObject;
        bulletUp01 = transform.Find("BulletUp01").gameObject;
        bulletUp02 = transform.Find("BulletUp02").gameObject;

        //AudioSource
        audioSource = GetComponent<AudioSource>();



    }

    // Update is called once per frame
    void Update()
    {
        //fire bullets when the spacebar is pressed
        if (Input.GetKeyDown("space"))
        {
            //play the laser sound effect
            audioSource.Play();
            //instantiate the firs bullet
            GameObject bullet01 = (GameObject)Instantiate(PlayerBulletGO);
            bullet01.transform.position = bulletPosition01.transform.position; //set the bullet initial position

            //instantiate the second bullet
            GameObject bullet02 = (GameObject)Instantiate(PlayerBulletGO);
            bullet02.transform.position = bulletPosition02.transform.position; //set the bullet initial position
            if (isMultiShotActive)
            {
                GameObject bullet03 = (GameObject)Instantiate(PlayerBulletGO);             
                bullet03.transform.position = bulletUp01.transform.position;

                PlayerBullet bulletScript03 = bullet03.GetComponent<PlayerBullet>();
                bulletScript03.direction = new Vector2(-1, 1).normalized;


                GameObject bullet04 = (GameObject)Instantiate(PlayerBulletGO);            
                bullet04.transform.position = bulletUp02.transform.position;

                PlayerBullet bulletScript04 = bullet04.GetComponent<PlayerBullet>();
                bulletScript04.direction = new Vector2(1, 1).normalized;
            }

        }
        float x = Input.GetAxisRaw("Horizontal"); // -1, 0, 1 (left, no input, right)
        float y = Input.GetAxisRaw("Vertical"); // -1, 0, 1 (down, no input, up)

        // now based on the input we compute a direction vector, and we normalize it to get a unit vector
        Vector2 direction = new Vector2(x, y).normalized;

        // we call the function that computes and sets the player's position
        Move(direction);
    }

    void ActiveGuns()
    {
        bulletUp01.SetActive(true);
        bulletUp02.SetActive(true);
        isMultiShotActive = true;

    }

    void DeactiveGuns()
    {
        bulletUp01.SetActive(false);
        bulletUp02.SetActive(false);
        isMultiShotActive = false;
    }
    void ActivateShield()
    {
       
        Shield.SetActive(true); // Kalkaný görünür yap
        isShieldActive = true;  // Kalkanýn aktif olduðunu iþaretle

    }
    void DeactivateShield()
    {
       
       Shield.SetActive(false);
       isShieldActive = false;


    }

    bool HasGuns()
    {
        return bulletUp01.activeSelf || bulletUp02.activeSelf;
        
    }
    bool HasShield()
    {
        return Shield.activeSelf;
    }
    

    void Move(Vector2 direction)
    {
     // Find the screen limits to the player's movement (left, right, top, and bottom edges of the screen)
    
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2 (0,0)); // this is the bottom-left point (corner) of the screen
        Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1,1)); // this is the top-right point (corner) of the screen

        max.x = max.x - 0.225f; //subtract the player sprite half width
        min.x = min.x + 0.225f; //add the player sprite half width

        max.y = max.y - 0.285f; //subtract the player sprite half height
        min.y = min.y + 0.285f; //add the player sprite half height

        //Get the player's current position

        Vector2 pos = transform.position;

        //Calculate the new position

        pos += direction * speed * Time.deltaTime;

        //Making sure the new position is not outsite the screen

        pos.x = Mathf.Clamp (pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        //Update the player's position
        transform.position = pos;   
    }

    IEnumerator TemporaryShield(float duration)
    {
        if (!isShieldActive) 
        {
            
            ActivateShield();
            yield return new WaitForSeconds(duration);
            DeactivateShield();
            
        }
    }

    IEnumerator TemporaryMultiShot(float duration)
    {
        ActiveGuns();
        yield return new WaitForSeconds(duration);
        DeactiveGuns();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
     
        if (HasShield())
        {
            DeactivateShield();

        }
       
        else if ((col.tag == "EnemyShipTag") || (col.tag == "EnemyBulletTag") || (col.tag =="MeteorTag"))
        {
            PlayExplosion();

            lives--; //subtract one live
            LivesUIText.text = lives.ToString();//update lives UI text
            if(lives== 0)
            {
                //change game manager state to game over state
                GameManagerGO.GetComponent<GameManager>().SetGameManagerState(GameManager.GameManagerState.GameOver);


                //hide the player's ship
                gameObject.SetActive(false);
            }
            
        }
        if (col.tag == "SheildUpTag")
        {
            PowerUpAnimation();
            StartCoroutine(TemporaryShield(8f));
            Destroy(col.gameObject);
           
        }
        if (col.tag == "BulletUpTag")
        {
            PowerUpAnimation();
            StartCoroutine(TemporaryMultiShot(8f));
            Destroy(col.gameObject);
        }
       



    }

    //function to instantiate an explosion
    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionGO);

        //set the position of the explosion
        explosion.transform.position = transform.position;
    }

    void PowerUpAnimation()
    {
        GameObject powerUpAnim = (GameObject)Instantiate(PowerUpAnim);

        powerUpAnim.transform.position = transform.position;

        Destroy(powerUpAnim, 0.2f);
    }

    
}

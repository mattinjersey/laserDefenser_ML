using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class xPlayer : MonoBehaviour {
   [SerializeField] float aSpeed=10f;
    [SerializeField] float StartHealth = 100f;
    float health;
    keepScor aKeepScore;
    [SerializeField] float padding = 1f;
    [SerializeField] GameObject aLaser;
    [SerializeField] public float projectileSpeed=10f;
    [SerializeField] float projectileFiringPeriod = 1f;
    [SerializeField] AudioClip aClip;
    [SerializeField] AudioClip bClip;
    [SerializeField] float aVolume = 10.0f;
    LaserAgent agent;
    float xMin,xMax,yMin,yMax;
    bool HasFired = false;
    Coroutine aFire;
	// Use this for initialization
	void Start () {
        SetUpMoveBoundaries();
       // Debug.Log("player laser" + aLaser);
        health = StartHealth;
        agent = FindObjectOfType<LaserAgent>();
        aKeepScore = FindObjectOfType<keepScor>();

    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera=Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
    }

    // Update is called once per frame
    void Update () {
        //Move();
        //  Fire();
        if (agent == null)
        {
            agent = FindObjectOfType<LaserAgent>();
        }
    }
    public void RestoreHealth()
    {
        health = StartHealth;
    }
    public void Fire(int inFire)
    {


        if (inFire == 1)
        {
            sqPrint();
                }
    }
    private void sqPrint()
    {
        
            AudioSource.PlayClipAtPoint(aClip, Camera.main.transform.position, aVolume);
            GameObject laser = Instantiate(aLaser, transform.position,
                   Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity =
                new Vector2(0, projectileSpeed);
        
    }
        private void OnTriggerEnter2D(Collider2D other)
        {
           // Debug.Log("Collision:" + other);
        damageenemyLaser damageDealer = other.gameObject.GetComponent<damageenemyLaser>();
        if (damageDealer!=null)
            ProcessHit(damageDealer);
        }
    public float GetHealth()
    {
        return health;
    }

    private void ProcessHit(damageenemyLaser damageDealer)
    {
        float aDamage;

        AudioSource.PlayClipAtPoint(bClip, Camera.main.transform.position, aVolume);
        aDamage = damageDealer.GetDamage();
        health -= aDamage;
       
            damageDealer.Hit();
            if (health <= 00)
            {
            Debug.Log(1.0f / Time.deltaTime);
            // Destroy(gameObject);
            // Debug.Log("destroy player!");

            //GameObject.Find("gameScore").GetComponent<keepScor>().ResetGame();
            //FindObjectOfType<gameManage>().playerShot();
            health = StartHealth;
            aKeepScore.Resetcore();
            /* this will reset score and cause us to wait a couple seconds*/
            if (agent != null)
            {
                agent.IncrementReward(-0.2f);
            }
           

    
            }
            else
            {
            if (agent != null)
            {
                // agent.IncrementReward(-aDamage / StartHealth);
                agent.IncrementReward(-0.2f);
            }

            }
        
    }






    public void Move(int Xdir,int Ydir)
    {
        //var deltaX = Input.GetAxis("Horizontal")*Time.deltaTime*aSpeed;
        //var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * aSpeed;
        float deltaX = ( (float)Xdir)*Time.deltaTime*aSpeed;
        float deltaY = ( (float)Ydir ) * Time.deltaTime * aSpeed;
        var newXPos = transform.position.x + deltaX;
        var newYPos = transform.position.y + deltaY;
        newXPos = Mathf.Clamp(newXPos, xMin+padding, xMax-padding);
        newYPos = Mathf.Clamp(newYPos, yMin+padding, yMax-padding);
        if (Mathf.Abs(Xdir)>0)
        {
            //Debug.Log("deltaX:" + deltaX);
        }
        transform.position = new Vector2(newXPos, newYPos);

    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class enemy : MonoBehaviour {
    [SerializeField] GameObject explosion;
    [SerializeField] float StartHealth = 100;
    [SerializeField] float threshVal;
    float health;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] AudioClip aClip;
    [SerializeField] AudioClip bClip;
    [SerializeField] int enemyPoints = 5;
    [SerializeField] float LaserVolume = 10.0f;
    [SerializeField] float maxTimeBetweenShots = 1f;
    LaserAgent agent;
    GameObject Player;
    bool aStart = true;
   
    [SerializeField] public int thisEnemyIndex=-1;
   public GameObject xProjectile;
    keepScor aKeepScore;
    [SerializeField] float projectileSpeed=1f;
    private bool bombDropped=false;
    private bool goodHit = false;
    // Use this for initialization
    void Start () {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        health = StartHealth;
        //Debug.Log("enemy laser from Start:"+xProjectile);
        aKeepScore = FindObjectOfType<keepScor>();
        agent = FindObjectOfType<LaserAgent>();
        Player = GameObject.Find("Player");
        if (agent != null)
        {
            thisEnemyIndex = agent.getFirstAvailableEnemyIndex(1);
            if (thisEnemyIndex >= 0)
            {
                agent.setEnemyActive(1, thisEnemyIndex, true); /* turn enemy to active*/
            }
            else
                Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        bool success;
        //Debug.Log("enemy laser from Update:" + xProjectile);
       
        CountDownAndShoot();
		if (Player==null || !Player.activeSelf )
        {
            agent.setEnemyActive(1, thisEnemyIndex, false); /* turn enemy to active*/
            Destroy(gameObject);
           
            // I'm going to destroy all enemies if the player dies disappears!
        }
       
        if (thisEnemyIndex>=0)
        {
            Vector3 thisPos = transform.position;
            Vector2 TwoPos ;
            TwoPos.x = thisPos.x;
            TwoPos.y = thisPos.y;
            success=agent.setEnemyPosition(1,thisEnemyIndex, TwoPos);
            if (!success)
            {
                Destroy(gameObject);
            }
        }
	}
    private void CountDownAndShoot()
    {
        bool startFire = false ;
        float aVal;
        shotCounter -= Time.deltaTime;
        // makes sure he gets 1 shot off.
        aVal = Random.Range(0.0f, 1.0f);
        if (aVal<threshVal && aStart)
        {
            startFire = true;
        }
        aStart = false;
       if (shotCounter<=0f || startFire || !bombDropped)
        {
            fire();
            aStart = false;
            bombDropped = true;
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }
    private void fire()
    {
        Vector2 aDelta =  Vector2.zero;
        float mDelta = 0.9f;

        //Debug.Log();
        AudioSource.PlayClipAtPoint(aClip, Camera.main.transform.position, LaserVolume);
       // Debug.Log("enemy firing!");
        aDelta.x = transform.position.x;
        aDelta.y = transform.position.y -mDelta;
        GameObject plaser = Instantiate(xProjectile, aDelta,
                       Quaternion.identity) as GameObject;
        plaser.GetComponent<Rigidbody2D>().velocity =
                    new Vector2(0, -1f*projectileSpeed);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        int HitByLaser = 0;
        //Debug.Log("Collision:" + other);
        damageenemyLaser damageDealer = other.gameObject.GetComponent<damageenemyLaser>();
        if (other.name.Contains("playerLaser") )
            {
            HitByLaser = 1;
        }
        if (HitByLaser == 1 && damageDealer!=null)
        {
            ProcessHit(damageDealer, HitByLaser);
        }
    }

    private void ProcessHit(damageenemyLaser damageDealer,int HitByLaser)
    {
        float aDamage;
       // Debug.Log("damageDealter:" + damageDealer);
        aDamage= damageDealer.GetDamage();
        //aDamage = 10f;
        health -= aDamage;
        damageDealer.Hit();
        if (health <= 00)
        {
            AudioSource.PlayClipAtPoint(bClip, Camera.main.transform.position);
            Destroy(gameObject);
            if (thisEnemyIndex >= 0)
            {
                agent.setEnemyActive(1, thisEnemyIndex, false); /* turn enemy to inactive*/
            }
            GameObject xExplode = Instantiate(explosion, transform.position,
                       Quaternion.identity) as GameObject;
            Destroy(xExplode,1f);
            if (HitByLaser == 1)
            {
                aKeepScore.addScore(enemyPoints); //also increment basic agent reward here.
            }
        }
        else /* lets give the player some points just for a hit.*/
        {
         if (HitByLaser == 1)
            {
                FindObjectOfType<keepScor>().addScore((int) ((float)enemyPoints*aDamage/StartHealth));
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyLaserScript : MonoBehaviour {
    LaserAgent agent;
    GameObject Player;
    [SerializeField] public int thisEnemyLaserIndex = -1;
    [SerializeField] public int EnemyOrPlayer=3;
    [SerializeField] float laserSpeed=1;
    // Use this for initialization
    void Start()
    {
        agent = FindObjectOfType<LaserAgent>();
        Player = GameObject.Find("Player");
      
        if (agent != null)
        {
            thisEnemyLaserIndex = agent.getFirstAvailableEnemyIndex(EnemyOrPlayer); //use 3 for enemy laser index 2 for player.
            if (thisEnemyLaserIndex >= 0)
            {
                agent.setEnemyActive(EnemyOrPlayer, thisEnemyLaserIndex, true); /* turn enemy to active*/
            }
            else
                Destroy(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Player == null || !Player.activeSelf || agent == null)
        {
            agent.setEnemyActive(EnemyOrPlayer, thisEnemyLaserIndex, false); /* turn enemy to active*/
            Destroy(gameObject);
         
            // I'm going to destroy all enemies if the player dies disappears!
        }
     
        if (thisEnemyLaserIndex >= 0)
        {
            Vector3 thisPos = transform.position;
            Vector3 copyPos = transform.position;
            bool success;
            Vector2 TwoPos;
            TwoPos.x = thisPos.x;
            TwoPos.y = thisPos.y;
         
            TwoPos.y -= Time.deltaTime * laserSpeed;
            copyPos.x = TwoPos.x;
            copyPos.y = TwoPos.y;
            transform.position = copyPos;
            success=agent.setEnemyPosition(EnemyOrPlayer, thisEnemyLaserIndex, TwoPos);
            if (!success)
            {
                Destroy(gameObject);
            }
        //    Debug.Log("laserindex:"+ thisEnemyLaserIndex+"     enemylaser positon:" + TwoPos);
        }
    }
}

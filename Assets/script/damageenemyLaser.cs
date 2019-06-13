using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageenemyLaser : MonoBehaviour {
    [SerializeField] int damage = 100;
    [SerializeField] int EnemyorPlayer;
    LaserAgent agent;
    // Use this for initialization
    void Start()
    {
        agent = FindObjectOfType<LaserAgent>();
       
    }
    public int GetDamage()
    {
        return damage;
    }
    public void Hit()
    {
        int enemyLaserIndex;
        Destroy(gameObject);
        enemyLaserIndex= GetComponent<enemyLaserScript>().thisEnemyLaserIndex   ;
        if (enemyLaserIndex != null && enemyLaserIndex>=0)
        {
            agent.setEnemyActive(EnemyorPlayer, enemyLaserIndex, false); /* turn enemy to active*/
        }
    }
}

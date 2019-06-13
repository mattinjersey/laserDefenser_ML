using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shredScript : MonoBehaviour {
    LaserAgent agent;
    [SerializeField] float FrameRate;
    void Start()
    {

        agent = FindObjectOfType<LaserAgent>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject xObject;
       FrameRate=(1.0f / Time.deltaTime);
        // Debug.Log("Collision");
        xObject = collision.gameObject;
        if (xObject.tag=="EnemyLaser")
        {
            if (xObject != null)
            {
                int enemyLaserIndex = 0;
                enemyLaserIndex=xObject.GetComponent<enemyLaserScript>().thisEnemyLaserIndex;
                if (enemyLaserIndex != null && enemyLaserIndex >= 0)
                {
                    agent.setEnemyActive(3, enemyLaserIndex, false); /* turn laser to inactive*/
                }
            }
        }
        else if (xObject.tag == "playerLaser")
        {
            if (xObject != null)
            {
                int enemyLaserIndex = 0;
                enemyLaserIndex = xObject.GetComponent<enemyLaserScript>().thisEnemyLaserIndex;
                if (enemyLaserIndex != null && enemyLaserIndex >= 0)
                {
                    agent.setEnemyActive(2, enemyLaserIndex, false); /* turn laser to inactive*/
                }
            }
        }
        Destroy(xObject);
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour {
     [SerializeField] waveConfig waveConfig;
    LaserAgent agent;
    List<Transform>  waypoints;
     float moveSpeed;
    int wayPointIndex = 0;
    int thisEnemyIndex;
	// Use this for initialization
	void Start () {
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[0].transform.position;
        moveSpeed = waveConfig.GetmoveSpeed();
        thisEnemyIndex = GetComponent<enemy>().thisEnemyIndex;
        agent = FindObjectOfType<LaserAgent>();
    }
    public void SetWaveConfig(waveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }
    // Update is called once per frame
    void Update() {
        if (thisEnemyIndex==-1)
        {
            thisEnemyIndex = GetComponent<enemy>().thisEnemyIndex;
        }
        if (agent == null)
        {
            agent = FindObjectOfType<LaserAgent>();
        }
        Move();
    }
    void Move()
    {
        
		if (wayPointIndex<=waypoints.Count-1)
        {
            var targetPosition = waypoints[wayPointIndex].transform.position;
            //Debug.Log("current pos:" + transform.position + "   targ:" + targetPosition);
            var movementThisFrame = moveSpeed * Time.deltaTime;
            transform.position=Vector3.MoveTowards(transform.position, targetPosition, movementThisFrame);
            if (transform.position==targetPosition)
            {
                wayPointIndex++;
              //  Debug.Log("move:" + wayPointIndex);
            }
        }
        else
        {
            Destroy(gameObject);
            if (agent!=null)
                {
                agent.setEnemyActive(1,thisEnemyIndex, false); /* turn enemy to inactive*/
            }
        }
	}
}

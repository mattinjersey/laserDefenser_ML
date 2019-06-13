using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathVert : MonoBehaviour {
     [SerializeField] waveConfig waveConfig;
    LaserAgent agent;
    List<Transform>  Xwaypoints;
    Vector3[] waypoints;
    int NumPoints=2;
    float moveSpeed;
    int wayPointIndex = 0;
    int thisEnemyIndex;
    GameObject player;
    // Use this for initialization
    void Start () {
        Vector3 aVector;
        Vector3 bVector;
        player = GameObject.Find("Player");
        Xwaypoints = (waveConfig.GetWaypoints());
        waypoints = new Vector3[2];
        waypoints[0].x = Xwaypoints[0].transform.position.x;
        waypoints[0].y = Xwaypoints[0].transform.position.y;
        waypoints[1].x = Xwaypoints[1].transform.position.x;
        waypoints[1].y = Xwaypoints[1].transform.position.y;
        //  Debug.Log(waypoints[0])

        // aVector.x = Random.Range(-8.5f, 8.5f);
        aVector.x = player.transform.position.x + Random.Range(-0.1f, 0.1f);
        waypoints[0].x=aVector.x;
        waypoints[1].x = aVector.x;

        //  waypoints[1].transform.position = bVector;
        transform.position = waypoints[0];
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
        
		if (wayPointIndex<= NumPoints - 1)
        {
          
            var targetPosition = waypoints[wayPointIndex];
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

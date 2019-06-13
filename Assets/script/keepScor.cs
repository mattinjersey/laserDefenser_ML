using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class keepScor : MonoBehaviour {
   
    [SerializeField] int gameScore = 0;
    GameObject player;
    LaserAgent agent;
    private void Awake()
    {
        SetUpSingleton();

    }

    private void SetUpSingleton()
    {
        int numGameSessions = FindObjectsOfType<keepScor>().Length;
        if (numGameSessions>1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start () {
       // Debug.Log(gameScore);
        player = GameObject.Find("Player");
        agent =FindObjectOfType<LaserAgent>();
    }
	
	// Update is called once per frame
	void Update () {
       if (player==null)
        {
            player = GameObject.Find("Player");
        }
        if (agent == null)
        {
            agent = FindObjectOfType<LaserAgent>();
        }
    }
    public void ResetGame()
    {
        Destroy(gameObject);
    }
    public void addScore(int aScore)
    {
        if (player != null && player.activeSelf)
        {
            {
                float xIncrement = 0.09f;
                gameScore += aScore;
                if (agent != null)
                {
                    agent.IncrementReward(xIncrement);
                }

                //Debug.Log(gameScore);
            }
        }
    }
    public int getScore()
    {
        return gameScore;
    }
    public void Resetcore()
    {
        gameScore = 0;
    }
}

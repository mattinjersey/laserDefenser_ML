using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAgents;

public class LaserAgent : Agent
{
    [Header("Specific to Basic")]
    private Academy academy;
    public float timeBetweenDecisionsAtInference;
    private float timeSinceDecision;
    bool LaserDelay = false;
    xPlayer PlayerRoutine;
    bool mStart;
    int position;
    int bCount;
    float divideFactor = 10.0f;
    int NumStep = 0;
    int MaxNumSteps = 9 * 30 * 50; // make this about 30 seconds.
    [SerializeField] int MaxNumPlayerLasers = 4;
    [SerializeField] float FrameRate;
    int smallGoalPosition;
    int largeGoalPosition;
    float xReward = 0;
    System.IO.StreamWriter logFile;
    [SerializeField] float LastReward = 0f;
    int ChooseObservationType = 2;  // send vector
    bool HasFired = false;
    [SerializeField] int NumEnemies; // this to be used for observations.
    [SerializeField] int movement;
    [SerializeField] int mFire;
    [SerializeField] float aPlayerX;
    [SerializeField] float aPlayerY;
    [SerializeField] float aPlayerXold;
    [SerializeField] Vector2[] CurrentEnemyPositions;
    [SerializeField] Vector2[] PriorEnemyPositions;
    [SerializeField] bool[] isEnemyActive;
    [SerializeField] int NumPlayerLasers; // playr lasers.
    [SerializeField] Vector2[] CurrentPlayerLaserPositions;
    float PlayerLaserProjectileSpeed;
    [SerializeField] bool[] isPlayerLaserActive;
    [SerializeField] int NumEnemyLasers; // enemy lasers.
    [SerializeField] Vector2[] CurrentEnemyLaserPositions;
    [SerializeField] Vector2[] PriorEnemyLaserPositions;
    [SerializeField] bool[] isEnemyLaserActive;
    // [SerializeField] int[,] occArray = new int[21, 21];
    Text endGameText;
    GameObject player;
    // public GameObject largeGoal;
    //public GameObject smallGoal;
    keepScor aKeepScore;
    [SerializeField] float projectileFiringPeriod;
    [SerializeField] float aConvertX;
    [SerializeField] float aConvertY;
    float priorPlayerPosition = 0f;
    public override void InitializeAgent()
    {


        // logFile = new System.IO.StreamWriter(@"C:\Users\matt\Haskell\xUnity\LD_ML\mLogFile.txt");
        // Time.timeScale = 10;
        Debug.Log(1.0f / Time.deltaTime);
        academy = FindObjectOfType(typeof(Academy)) as Academy;
        PlayerRoutine = GetComponent<xPlayer>();
        player = GameObject.Find("Player");
        CurrentEnemyPositions = new Vector2[NumEnemies];
        CurrentEnemyPositions.Initialize();
        PriorEnemyPositions = new Vector2[NumEnemies];
        aKeepScore = FindObjectOfType<keepScor>();
        isEnemyActive = new bool[NumEnemies];
        CurrentPlayerLaserPositions = new Vector2[NumPlayerLasers];
        CurrentPlayerLaserPositions.Initialize();
        isPlayerLaserActive = new bool[NumPlayerLasers];
        isPlayerLaserActive.Initialize();
        PlayerLaserProjectileSpeed = GetComponent<xPlayer>().projectileSpeed;
        CurrentEnemyLaserPositions = new Vector2[NumEnemyLasers];
        CurrentEnemyLaserPositions.Initialize();
        PriorEnemyLaserPositions = new Vector2[NumEnemyLasers];
        PriorEnemyLaserPositions.Initialize();
        isEnemyLaserActive = new bool[NumEnemyLasers];
        isPlayerLaserActive.Initialize();
        xInitializeArrays();
    }
    public void xInitializeArrays()
    {
        for (int count = 0; count < NumEnemies; count++)
            CurrentEnemyPositions[count] = new Vector2(-1.0f, -1.0f);
        for (int count = 0; count < NumEnemies; count++)
            PriorEnemyPositions[count] = new Vector2(-1.0f, -1.0f);
        for (int count = 0; count < NumEnemyLasers; count++)
            CurrentEnemyLaserPositions[count] = new Vector2(-1.0f, -1.0f);
        for (int count = 0; count < NumEnemyLasers; count++)
            PriorEnemyLaserPositions[count] = new Vector2(-1.0f, -1.0f);
        for (int count = 0; count < NumPlayerLasers; count++)
            CurrentPlayerLaserPositions[count] = new Vector2(-1.0f, -1.0f);

    }
    public void IncrementReward(float inReward)
    {
        xReward += inReward;
    }
    public override void CollectObservations()
    {
        int aCount = 0;
        //float aPlayerY;
        //float aConvertX, aConvertY;
        //  int[,] occArray = new int[21, 21];
        if (player.activeSelf)
        {
            /*float aMax_x = -100f;
            float aMin_x = 100f;
            float aMin_y = 100f;
            float aMax_y = -100f;*/

            //float aConvert = 0f;
            mStart = true;
            // add player observations

            if (ChooseObservationType == 1)
            {
                aPlayerX = 10f + Mathf.Round(10.0f * player.transform.position.x / divideFactor);
                aPlayerY = 10f + Mathf.Round(10.0f * player.transform.position.y / divideFactor);
            }
            else
            {
                aPlayerX = (player.transform.position.x / divideFactor);
                aPlayerY = (player.transform.position.y / divideFactor);
            }
            // occArray[(int)aPlayerX, (int)aPlayerY] = 1;
            //  xObs(aPlayerX, "thisPlayerX", 0);
            mStart = false;
            //  Debug.Log(aCount + "PlayerX   " + aPlayerX);
            aCount += 1;
            //xObs(aPlayerXold,"thisPlayerPriorX",0);
            // if (occArray[(int)aPlayerXold, (int)aPlayerY] == 0)
            //   occArray[(int)aPlayerXold, (int)aPlayerY] = 2;
            //  Debug.Log(aCount + "PLayerXold   " + aPlayerXold);
            aCount += 1;

            //Debug.Log(priorPlayerPosition/8.5f);
            // add enemy observations
            for (int count = 0; count < NumEnemies; count++)
            {
                bool thisEnemyActive = isEnemyActive[count];
                Vector2 thisEnemyCurrentPosition = CurrentEnemyPositions[count];

                Vector2 thisEnemyPriorPosition = PriorEnemyPositions[count];
                if (thisEnemyActive)
                {

                    aCount += 1;
                    aConvertX = thisEnemyCurrentPosition.x;

                    // xObs(aConvert,"thisEnemy.CurrentPosition.x",count);

                    //    Debug.Log(aCount +"enemyIndex: "+count+"   enemyX   " + 0.5f * thisEnemyCurrentPosition.x);
                    aCount += 1;
                    aConvertY = thisEnemyCurrentPosition.y;

                    // Debug.Log("aConvertX:" + aConvertX + "   aConvertY:" + aConvertY);
                    //  occArray[(int)aConvertX, (int)aConvertY] = 3;
                    //xObs(aConvert, "thisEnemy.CurrentPosition.y",count);
                    //    Debug.Log(aCount + "enemyIndex: " + count  + "enemyY   " + 0.5f * thisEnemyCurrentPosition.y);
                    aCount += 1;

                    aConvertX = thisEnemyPriorPosition.x;

                    //xObs(aConvert, "thisEnemyPriorPosition.x",count);
                    //   Debug.Log(aCount  +"enemyIndex: " + count + "enemyXprior   " + 0.5f * thisEnemyPriorPosition.x);
                    aCount += 1;
                    aConvertY = thisEnemyPriorPosition.y;

                    //xObs(aConvert, "thisEnemyPriorPosition.y",count);
                    //  if (occArray[(int)aConvertX, (int)aConvertY] == 0)
                    //    occArray[(int)aConvertX, (int)aConvertY] = 4;
                    //PriorEnemyPositions[count] = CurrentEnemyPositions[count];
                }
                else
                    PriorEnemyPositions[count] = new Vector2(-1.0f, -1.0f);

            }
            // add player lasers and player laser projectile speed.

            // xObs(PlayerLaserProjectileSpeed / 20f, "PlayerLaserProjectileSpeed", 0);
            for (int count = 0; count < NumPlayerLasers; count++)
            {
                bool thisProjectileActive = isPlayerLaserActive[count];
                Vector2 thisProjectileCurrentPosition = CurrentPlayerLaserPositions[count];
                aConvertX = thisProjectileCurrentPosition.x;
                if (thisProjectileActive)
                {
                    //xObs(aConvert, "thisProjectileCurrentPosition.x",count);

                    // Debug.Log(aCount + "playerLaserIndex: " + count + "LaserX  " + 0.5f * thisProjectileCurrentPosition.x);

                    aConvertY = thisProjectileCurrentPosition.y;

                    //  xObs(aConvert, "thisProjectileCurrentPosition.y",count);
                    //if (occArray[(int)aConvertX, (int)aConvertY] == 0)
                    //   occArray[(int)aConvertX, (int)aConvertY] = 5;
                }
            }
            // add enemy observations
            for (int count = 0; count < NumEnemyLasers; count++)
            {
                bool thisEnemyLaserActive = isEnemyLaserActive[count];


                Vector2 thisEnemyLasrCurrentPosition = CurrentEnemyLaserPositions[count];
                Vector2 thisEnemyLasrPriorPosition = PriorEnemyLaserPositions[count];
                if (thisEnemyLaserActive)
                {

                    aConvertX = thisEnemyLasrCurrentPosition.x;

                    //xObs(aConvert, "thisEnemyLasrCurrentPosition.x",count);
                    // Debug.Log(aCount + "enemyLaserIndex: " + count + "LaserX  " + 0.5f * thisEnemyLasrCurrentPosition.x);
                    aCount += 1;
                    aConvertY = thisEnemyLasrCurrentPosition.y;

                    //xObs(aConvert, "thisEnemyLasrCurrentPosition.y",count);
                    // occArray[(int)aConvertX, (int)aConvertY] = 6;

                    aConvertY = thisEnemyLasrPriorPosition.y;

                    //if (occArray[(int)aConvertX, (int)aConvertY] == 0)
                    //  occArray[(int)aConvertX, (int)aConvertY] = 7;
                    //xObs(aConvert, "thisEnemyLasrPriorPosition.y",count);

                    // AddVectorObs(xToFloat(thisEnemyLaserActive));
                    //   Debug.Log(aCount + "enemyLaserIndex: " + count + "ENEYLASERACTIVE  " + xToFloat(thisEnemyLaserActive));
                    //  aCount += 1;
                }
                else
                    PriorEnemyLaserPositions[count] = new Vector2(-1.0f, -1.0f);
            }

        }
        //write array.
        if (ChooseObservationType == 1)
        {
            for (int countX = 0; countX < 21; countX++)
            {
                for (int countY = 0; countY < 21; countY++)
                {
                    // AddVectorObs(occArray[countX, countY], 8);
                }
            }
        }
        else if (ChooseObservationType == 2)
        {
            AddVectorObs(0.5f * aPlayerX);
            AddVectorObs(0.5f * aPlayerY);
            AddVectorObs(0.5f * aPlayerXold);
            for (int count = 0; count < NumPlayerLasers; count++)
            {
                AddVectorObs(0.5f * CurrentPlayerLaserPositions[count].x);
                AddVectorObs(0.5f * CurrentPlayerLaserPositions[count].y);
                //AddVectorObs(isPlayerLaserActive[count]);
            }
            for (int count = 0; count < NumEnemyLasers; count++)
            {
                AddVectorObs(0.5f * CurrentEnemyLaserPositions[count].x);
                AddVectorObs(0.5f * CurrentEnemyLaserPositions[count].y);
                //AddVectorObs(PriorEnemyLaserPositions[count]);
                // AddVectorObs(isEnemyLaserActive[count]);
            }
            for (int count = 0; count < NumEnemies; count++)
            {
                AddVectorObs(0.5f * CurrentEnemyPositions[count].x);
                AddVectorObs(0.5f * CurrentEnemyPositions[count].y);
                AddVectorObs(0.5f * PriorEnemyPositions[count].x);
                AddVectorObs(0.5f * PriorEnemyPositions[count].y);
                //AddVectorObs(isEnemyActive[count]);
            }

        }
        /* update priors */
        aPlayerXold = aPlayerX;
        for (int count = 0; count < NumEnemyLasers; count++)
        {
            PriorEnemyLaserPositions[count] = CurrentEnemyLaserPositions[count];
        }
        for (int count = 0; count < NumEnemies; count++)
        {
            PriorEnemyPositions[count] = CurrentEnemyPositions[count];
        }
    }
    public void xObs(float inVal, string inString, int count)
    {
        string aMessage;
        if (mStart)
        { bCount = 0; }
        bCount++;
        aMessage = "obs number:" + bCount + ". " + "index number:" + count + "..." + inString + ": " + inVal;
        //logFile.WriteLine(aMessage);
        // AddVectorObs(inVal);
    }
    public float xToFloat(bool inB)
    {
        float outVal = 0.0f;
        if (inB)
        { outVal = 1.0f; }

        return outVal;
    }
    /* function getFirstEnemyIndex
     * this function will return the first open slot in our list of enemies
     * denoted by isActivEnemy is false
     * THis is also used for playerLasers, enemy lasers
     use xObjectIndex 1 for enemy, 2 for player laser, 3 for enemy laser.*/
    public int getFirstAvailableEnemyIndex(int xObjectIndex)
    {
        bool thisBool = true;
        int returnIndex = -1;
        int maxVal = 1;
        if (xObjectIndex == 1)
            maxVal = NumEnemies;
        else if (xObjectIndex == 2)
            maxVal = NumPlayerLasers;
        else if (xObjectIndex == 3)
            maxVal = NumEnemyLasers;
        for (int count = 0; count < maxVal; count++)
        {
            if (xObjectIndex == 1)
                thisBool = isEnemyActive[count];
            else if (xObjectIndex == 2)
                thisBool = isPlayerLaserActive[count];
            else if (xObjectIndex == 3)
                thisBool = isEnemyLaserActive[count];
            if (!thisBool)
            {
                //Debug.Log("thisCount:" + count);
                returnIndex = count;
                break;
            }

        }
        //Debug.Log("setting new enemy:" + returnIndex);
        return returnIndex;
    }
    /* this method will set enemy active or inactive
     * it is accessed by enemies.
     It can also be used by playerLaswers, enemy lasers, */
    public void setEnemyActive(int xObjectIndex, int count, bool xActive)
    {
        if (xObjectIndex == 1)
        {
            isEnemyActive[count] = xActive;
            if (!xActive)
            {
                CurrentEnemyPositions[count] = new Vector2(-1.0f, -1.0f);
                PriorEnemyPositions[count] = new Vector2(-1.0f, -1.0f);
            }
        }
        else if (xObjectIndex == 2)
        {
            isPlayerLaserActive[count] = xActive;
            if (!xActive)
            {
                CurrentPlayerLaserPositions[count] = new Vector2(-1.0f, -1.0f);
            }
        }
        else if (xObjectIndex == 3)
        {
            isEnemyLaserActive[count] = xActive;
            if (!xActive)
            {
                CurrentEnemyLaserPositions[count] = new Vector2(-1.0f, -1.0f);
                PriorEnemyLaserPositions[count] = new Vector2(-1.0f, -1.0f);
            }
        }
        // Debug.Log("setting enemy:" + count + " active state:" + xActive+"   showingVal:"+isEnemyActive[count]);
    }
    /* this method will set enemy position
   * it is accessed by enemies. (also playerlaser, enemy laser.*/
    public bool setEnemyPosition(int xObjectIndex, int count, Vector2 thiscurrentPosition)
    {
        Vector2 starPosition;
        bool success = true; ;
        thiscurrentPosition /= divideFactor;
        if (ChooseObservationType == 1)
        {
            starPosition.x = (float)(10f + (int)Mathf.Round(10.0f * Mathf.Clamp(thiscurrentPosition.x, -1.0f, 1.0f)));
            starPosition.y = (float)(10f + (int)Mathf.Round(10.0f * Mathf.Clamp(thiscurrentPosition.y, -1.0f, 1.0f)));
        }
        else
        {
            starPosition = thiscurrentPosition;
        }

        if (xObjectIndex == 1)
        {

            CurrentEnemyPositions[count] = starPosition;
            if (!isEnemyActive[count])
                success = false;
        }
        else if (xObjectIndex == 2)
        {
            CurrentPlayerLaserPositions[count] = starPosition;
            if (!isPlayerLaserActive[count])
                success = false;
        }
        else if (xObjectIndex == 3)
        {

            CurrentEnemyLaserPositions[count] = starPosition;
            if (!isEnemyLaserActive[count])
                success = false;
        }
        return success;
    }
    IEnumerator sqPrint()
    {
        while (true)
        {
            PlayerRoutine.Fire(1);
            //Debug.Log("fire!");   
            yield return new WaitForSeconds(projectileFiringPeriod);
            LaserDelay = false;
        }

    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        movement = Mathf.FloorToInt(vectorAction[0]);
        mFire = Mathf.FloorToInt(vectorAction[1]);
        int aFire = 0;
        int direction = 0;
        if (movement == 1)
        {
            direction = -1;
        }
        else if (movement == 2)
        {
            direction = 1;

        }
        if (mFire == 1)
        {
            aFire = 1;
        }

        PlayerRoutine.Move(direction, 0);
        // only start firing if LaserDelay is false.
        if (aFire == 1 & !HasFired & LaserDelay == false)
        {
            StartCoroutine("sqPrint");
            // Debug.Log("Starting Firing");
            HasFired = true;
            LaserDelay = true;

        }
        else if (aFire == 0 & HasFired & LaserDelay == false)
        {
            //Debug.Log("ending firing");
            // only end firing if LaserDelay is false.

            StopCoroutine("sqPrint");
            HasFired = false;

        }


        // Debug.Log("reward:" + GetCumulativeReward());
        FrameRate = (1.0f / Time.deltaTime);
        AddReward(xReward);

        if (Mathf.Abs(xReward) > 0.01)
        {
            LastReward = xReward;
            LaserDelay = false;
        }
        if (xReward < -0.01) /*died*/
        {
            xReward = 0f;

            endGameText = GameObject.Find("endGameText").GetComponent<Text>();
            endGameText.text = "GAME OVER...RESTARTING..." + (1.0f / Time.deltaTime); ;
            //player.SetActive(false);
            Invoke("youDead", 0.5f);
            aKeepScore.Resetcore();
            Debug.Log("reward:" + GetCumulativeReward());
            xInitializeArrays();
            Done();

        }


        xReward = 0f; //reset reward every cycle.
        NumStep += 1;
        if (NumStep > MaxNumSteps)
        {
            NumStep = 0;
            Debug.Log("reward:" + GetCumulativeReward());
            xInitializeArrays();
            Done();

        }
    }
    public void youDead()
    {
        float a1, a2, a3;
        Vector3 temp;
        a1 = Random.Range(-divideFactor, divideFactor);
        a2 = player.transform.position.y;
        a3 = player.transform.position.z;
        temp = new Vector3(a1, a2, a3);
        player.transform.position = temp;

        // Debug.Log("player got hit!");
        endGameText.text = "";
        player.SetActive(true);
        PlayerRoutine.RestoreHealth();
        LaserDelay = false;
        xInitializeArrays();

    }
    public override void AgentReset()
    {
        GameObject.FindObjectOfType<keepScor>().Resetcore();
        // reset all enemies, all lasers
        for (int count = 0; count < NumEnemies; count++)
        {
            isEnemyActive[count] = false;

        }
        isPlayerLaserActive = new bool[NumPlayerLasers];
        isEnemyLaserActive = new bool[NumEnemyLasers];
    }

    public override void AgentOnDone()
    {

    }

    /* public void FixedUpdate()
     {
         WaitTimeInference();
     }
 */
    //  private void WaitTimeInference()
    //{
    /*  if (!academy.GetIsInference())
      {
          RequestDecision();
      }
      else
      {
          if (timeSinceDecision >= timeBetweenDecisionsAtInference)
          {
              timeSinceDecision = 0f;
              RequestDecision();
          }
          else
          {
              timeSinceDecision += Time.fixedDeltaTime;
          }
      }*/
    //}

}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallsHandler : MonoBehaviour
{
    public static BallsHandler Instance;

    [Header("Unity Events")]
    public UnityEvent OnAllBallsInactiveOrAtBottom;
    public UnityEvent OnBallCheckTriggered;

    // Nouveaux events :
    public UnityEvent OnSpeedIncreaseShouldStop;
    public UnityEvent OnGameShouldResume;
    public UnityEvent OnMachineTurnShouldStart;


    public GameObject ballPrefab;
    public int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    //public UnityEvent OnAllBallsInactiveOrAtBottom;


    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(ballPrefab, this.transform);
            //obj.SetActive(false);
            pool.Add(obj);
        }

    }

    public void CheckAllBallsState()
    {
        OnBallCheckTriggered.Invoke();

        if (AllBallsInactiveOrAtBottom())
        {
            OnSpeedIncreaseShouldStop?.Invoke();
            OnGameShouldResume?.Invoke();
            OnMachineTurnShouldStart?.Invoke();
            OnAllBallsInactiveOrAtBottom?.Invoke();
        }
    }


    public GameObject GetBall()
    {
        foreach (GameObject ball in pool)
        {
            BallController bc = ball.GetComponent<BallController>();
            if (!bc.isInUse)
            {
                return ball;
            }
        }

        GameObject newBall = Instantiate(ballPrefab, this.transform);
        pool.Add(newBall);
        return newBall;
    }

    public List<GameObject> GetAllBalls()
    {
        return pool;
    }


    public bool AllBallsInactiveOrAtBottom()
    {
        foreach (GameObject ball in pool)
        {
            BallController bc = ball.GetComponent<BallController>();
            if (bc.isInUse && !bc.hasTouchedBottom)
            {
                return false;
            }
        }
        return true;
    }
}


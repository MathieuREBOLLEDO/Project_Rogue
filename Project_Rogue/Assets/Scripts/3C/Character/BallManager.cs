using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallManager : MonoBehaviour
{
    public static BallManager Instance;

    public GameObject ballPrefab;
    public int poolSize = 20;

    private List<GameObject> pool = new List<GameObject>();

    public UnityEvent OnAllBallsInactiveOrAtBottom;
    

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
        if (AllBallsInactiveOrAtBottom() && GameManager.Instance.CurrentState == GameState.WaitingForBalls)
        {

            // Arrêter la vitesse croissante
            BallSettingsManager.Instance.StopSpeedIncreaseLoop();

            GameManager.Instance.SetState(GameState.Playing);
            GameManager.Instance.StartMachineTurn();
            OnAllBallsInactiveOrAtBottom.Invoke();
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


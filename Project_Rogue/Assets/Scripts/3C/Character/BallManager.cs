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
    private bool eventInvoked = false;

    void Awake()
    {
        Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(ballPrefab, this.transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public void StartBallTurn()
    {
        GameManager.Instance.SetState(GameState.WaitingForBalls);
        eventInvoked = false;

        foreach (GameObject ball in pool)
        {
            ball.SetActive(true);
            ball.GetComponent<BallController>().ResetBall();
        }
    }


    void Update()
    {
        if (GameManager.Instance.IsWaiting() && !eventInvoked && AllBallsInactiveOrAtBottom())
        {
            eventInvoked = true;
            GameManager.Instance.SetState(GameState.Playing);
            OnAllBallsInactiveOrAtBottom.Invoke();
        }
    }
    public GameObject GetBall()
    {
        foreach (GameObject ball in pool)
        {
            if (!ball.activeInHierarchy)
            {
                return ball;
            }
        }

        // Optionnel : étendre dynamiquement si aucun disponible
        GameObject newBall = Instantiate(ballPrefab, this.transform);
        newBall.SetActive(false);
        pool.Add(newBall);
        return newBall;
    }
    public bool AllBallsInactiveOrAtBottom()
    {
        foreach (GameObject ball in pool)
        {
            if (ball.activeInHierarchy && !ball.GetComponent<BallController>().hasTouchedBottom)
            {
                return false;
            }
        }
        return true;
    }
}


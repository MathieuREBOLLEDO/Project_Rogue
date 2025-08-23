using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallsHandler : MonoBehaviour
{
    public static BallsHandler Instance;

    [Header("Ball Datas")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private int poolSize = 20;
    private List<GameObject> allBalls = new List<GameObject>();

    [Header("Spawner")]
    [SerializeField] private GameObject AverrageBallsLocations;
    [SerializeField] private float lerpDuration = 0.75f;


    void Awake()
    {
        GlobalBallVariables.SetBallSize(ballPrefab.transform.localScale.x / 2);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {        
        InitLaunchPosition();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(ballPrefab, AverrageBallsLocations.transform.position, Quaternion.identity, this.transform);
            allBalls.Add(obj);
        }
    }
    public void InitLaunchPosition() =>
        AverrageBallsLocations.transform.position = new Vector3(
            0f,
            ScreenUtils.ScreenMin.y + GlobalBallVariables.ballSize,
            0);
    

    public void CheckAllBallsState()
    {
        if (AllBallsInactiveOrAtBottom())
        {
            MoveBallToAverragePosition();
            GameManager.Instance.NotifyPlayerTurnEnd();
        }
    }

    public GameObject GetBall()
    {
        foreach (GameObject ball in allBalls)
        {
            BallController bc = ball.GetComponent<BallController>();
            if (!bc.isInUse)
            {
                return ball;
            }
        }

        GameObject newBall = Instantiate(ballPrefab, this.transform);
        allBalls.Add(newBall);
        return newBall;
    }

    public List<GameObject> GetAllBalls()
    {
        return allBalls;
    }

    public bool AllBallsInactiveOrAtBottom()
    {
        foreach (GameObject ball in allBalls)
        {
            BallController bc = ball.GetComponent<BallController>();
            if (bc.isInUse && !bc.hasTouchedBottom)
                return false;
        }
        return true;
    }

    /// <summary>
    ///  TO DO move all datas link to average position to another scripts -> maybe (Move ball)
    /// </summary>

    public void MoveBallToAverragePosition()
    {
        Vector3 avgPosition = GetAverageBallPosition(allBalls);
        foreach (var ball in allBalls)
            StartCoroutine(LerpToPosition(ball.transform, avgPosition, lerpDuration));
        AverrageBallsLocations.transform.position = avgPosition;
    }

    Vector3 GetAverageBallPosition(List<GameObject> balls)
    {
        Vector3 total = Vector3.zero;
        foreach (var ball in balls)
            total += ball.transform.position;
        return total / balls.Count;
    }

    IEnumerator LerpToPosition(Transform ballTransform, Vector3 targetPos, float duration)
    {
        Vector3 start = ballTransform.position;
        float time = 0f;

        while (time < duration)
        {
            float t = time / duration;
            ballTransform.position = Vector3.Lerp(start, targetPos, t);
            time += Time.deltaTime;
            yield return null;
        }

        ballTransform.position = targetPos;
    }

    
}


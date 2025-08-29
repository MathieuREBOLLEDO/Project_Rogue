using UnityEngine;

public class BallSettingsManager : MonoBehaviour
{
    public static BallSettingsManager Instance;

    [Header("Ball Speed Settings")]
    public float currentSpeed = 5f;
    public float initSpeed = 5f;

    public float speedIncrease = 1f;
    public float increaseInterval = 10f;

    private bool isSpeedIncreasing = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        ResetBallSpeed();
    }

    private void OnEnable()
    {
        GameEventManager.Instance.Subscribe(GameEventType.PlayerTurnEnd, StopSpeedIncreaseLoop);
    }

    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(GameEventType.PlayerTurnEnd, StopSpeedIncreaseLoop);
    }

    public float GetBallSpeed() 
    {
        return currentSpeed;
    }

    private void ResetBallSpeed()
    {
        currentSpeed = initSpeed;
    }

    public void StartSpeedIncreaseLoop()
    {
        if (!isSpeedIncreasing)
            StartCoroutine(SpeedIncreaseRoutine());
    }

    public void StopSpeedIncreaseLoop(GameEvent gameEvent)
    {
        isSpeedIncreasing = false;
        StopAllCoroutines();
        ResetBallSpeed();
    }

    private System.Collections.IEnumerator SpeedIncreaseRoutine()
    {
        isSpeedIncreasing = true;

        while (!BallsHandler.Instance.AllBallsInactiveOrAtBottom())
        {
            yield return new WaitForSeconds(increaseInterval);
            currentSpeed += speedIncrease;
        }

        isSpeedIncreasing = false;
    }
}

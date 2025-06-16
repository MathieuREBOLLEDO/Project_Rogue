using UnityEngine;

public class BallSettingsManager : MonoBehaviour
{
    public static BallSettingsManager Instance;

    [Header("Ball Speed Settings")]
    public float baseSpeed = 5f;
    public float speedIncrease = 1f;
    public float increaseInterval = 10f;

    private bool isSpeedIncreasing = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public float GetBallSpeed()
    {
        return baseSpeed;
    }

    public void StartSpeedIncreaseLoop()
    {
        if (!isSpeedIncreasing)
            StartCoroutine(SpeedIncreaseRoutine());
    }

    public void StopSpeedIncreaseLoop()
    {
        isSpeedIncreasing = false;
        StopAllCoroutines();
    }

    private System.Collections.IEnumerator SpeedIncreaseRoutine()
    {
        isSpeedIncreasing = true;

        while (!BallManager.Instance.AllBallsInactiveOrAtBottom())
        {
            yield return new WaitForSeconds(increaseInterval);
            baseSpeed += speedIncrease;
        }

        isSpeedIncreasing = false;
    }
}

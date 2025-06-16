using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
    SingleShot,
    Burst // Toutes les balles à la chaîne
}

public class BallSpawner : MonoBehaviour
{
    public FireMode fireMode = FireMode.SingleShot;
    public int burstCount = 10; // nombre de balles en rafale
    public float burstDelay = 0.1f;


    public void TryShootBall(Vector2 pos)
    {
        if (BallManager.Instance.AllBallsInactiveOrAtBottom())
        {
            if (fireMode == FireMode.SingleShot)
            {
                ShootOneBall(pos);
            }
            else if (fireMode == FireMode.Burst)
            {
                StartCoroutine(FireBurst(pos));
            }
            BallSettingsManager.Instance.StartSpeedIncreaseLoop();
        }
    }

    void ShootOneBall(Vector2 pos)
    {
        GameObject ball = BallManager.Instance.GetBall();
        if (ball != null)
        {
            //ball.SetActive(true);
            ball.GetComponent<BallController>().LaunchBall(pos);
        }
    }

    IEnumerator FireBurst(Vector2 pos)
    {
        for (int i = 0; i < burstCount; i++)
        {
            ShootOneBall(pos);
            //Debug.Log(i);
            yield return new WaitForSeconds(burstDelay);
        }
    }
}

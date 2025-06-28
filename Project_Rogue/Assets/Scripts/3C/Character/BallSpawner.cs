using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        if (BallsHandler.Instance.AllBallsInactiveOrAtBottom())
        {
            EventBus.PublishGameStateChange(GameState.WaitingForBalls);
           // GameManager.Instance.SetState(GameState.WaitingForBalls);

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
        GameObject ball = BallsHandler.Instance.GetBall();
        if (ball != null)
        {
            //ball.SetActive(true);
            ball.GetComponent<BallController>().Launch(pos);
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

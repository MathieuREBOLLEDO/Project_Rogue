using System.Collections;
using UnityEngine;

public enum FireMode
{
    SingleShot,
    Burst // Toutes les balles à la chaîne
}

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject averrageBallLocation;
    public FireMode fireMode = FireMode.SingleShot;
    public int burstCount = 10; // nombre de balles en rafale
    public float burstDelay = 0.1f;

    private float minAllowedAngle = -GlobalBallVariables.angleOfShooting; 
    private float maxAllowedAngle = GlobalBallVariables.angleOfShooting;  

    public void TryShootBall(Vector2 pos)
    {
        Vector2 shootDirection = (pos - (Vector2)averrageBallLocation.transform.position).normalized;
        float angle = Vector2.Angle(Vector2.up, shootDirection); // Angle par rapport à vertical (ou change en Vector2.right si horizontal)


        // Vérifie si l'angle est dans les limites autorisées
        if (angle < minAllowedAngle || angle > maxAllowedAngle)
        {
            Debug.LogWarning($"Angle de tir invalide : {angle}°. Autorisé entre {minAllowedAngle}° et {maxAllowedAngle}°.");
            return; // On bloque le tir
        }
        

        if (BallsHandler.Instance.AllBallsInactiveOrAtBottom())
        {
            EventBus.PublishGameStateChange(GameState.WaitingForBalls);

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


            /*

            if (BallsHandler.Instance.vnosnInactiveOrAtBottom())
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
            }*/
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

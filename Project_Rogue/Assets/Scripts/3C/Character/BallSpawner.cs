using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [SerializeField] private GameObject averrageBallLocation;
    public FireMode fireMode = FireMode.SingleShot;
    public int burstCount = 10; // nombre de balles en rafale
    public float burstDelay = 0.1f;

    private float minAllowedAngle = -GlobalBallVariables.angleOfShooting; 
    private float maxAllowedAngle = GlobalBallVariables.angleOfShooting;

    private bool canShoot = true;


    private void Start()
    {
        GameEventManager.Instance.Subscribe(GameEventType.PlayerTurnStart, SetCanShoot);
        GameEventManager.Instance.Subscribe(GameEventType.PlayerTurnEnd, SetCanShoot);
    }
    private void OnDisable()
    {
        GameEventManager.Instance.Unsubscribe(GameEventType.PlayerTurnStart, SetCanShoot);
        GameEventManager.Instance.Unsubscribe(GameEventType.PlayerTurnEnd, SetCanShoot);
    }

    private void SetCanShoot(GameEvent gameEvent)
    {
        canShoot = (gameEvent.eventType == GameEventType.PlayerTurnStart) ? true : false;
    }

    public void TryShootBall(Vector2 pos)
    {
        Vector2 shootDirection = (pos - (Vector2)averrageBallLocation.transform.position).normalized;
        float angle = Vector2.Angle(Vector2.up, shootDirection); // Angle par rapport à vertical (ou change en Vector2.right si horizontal)

        
        if (angle < minAllowedAngle || angle > maxAllowedAngle)  // Vérifie si l'angle est dans les limites autorisées
        {
            Debug.LogWarning($"Angle de tir invalide : {angle}°. Autorisé entre {minAllowedAngle}° et {maxAllowedAngle}°.");
            return;
        }

        if (ShootHandler.CheckNumberIsLessThan(0))
        {
            Debug.LogWarning("Not more shoot available");
            GameManager.Instance.NotifyEndGame();
            return;
        }

        if (BallsHandler.Instance.AllBallsInactiveOrAtBottom() && canShoot)
        {
            Debug.LogWarning("Can shoot ball");
            GameManager.Instance.NotifyPlayerTurnEnd();
            //EventBus.PublishGameStateChange(GameState.WaitingForBalls);

            if (fireMode == FireMode.SingleShot)
            {
                ShootOneBall(pos);
            }
            else if (fireMode == FireMode.Burst)
            {
                StartCoroutine(FireBurst(pos));
            }

            ShootHandler.Instance.RemoveShootNumber(1);
            BallSettingsManager.Instance.StartSpeedIncreaseLoop();
        }
        else
            Debug.Log("Couldn'tShoot");

    }

    void ShootOneBall(Vector2 pos)
    {
        GameObject ball = BallsHandler.Instance.GetBall();
        if (ball != null)
        {
            ball.GetComponent<BallController>().Launch(pos);
        }
    }

    IEnumerator FireBurst(Vector2 pos)
    {
        for (int i = 0; i < burstCount; i++)
        {
            ShootOneBall(pos);
            yield return new WaitForSeconds(burstDelay);
        }
    }
}

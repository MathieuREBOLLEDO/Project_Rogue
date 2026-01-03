using MoreMountains.Feedbacks;
using System;
using UnityEditor.UIElements;
using UnityEngine;

public class BallController : MonoBehaviour, IBallLauncher
{
    private IBallMovement ballMovement;
    private Vector2 direction;
    private Rigidbody2D rb;
    private bool isLaunched = false;

    public bool isInUse { get; private set; } = false;
    public bool hasTouchedBottom { get; private set; } = false;

    [SerializeField] private float raycastDistance = 0.05f; // Distance du Raycast pour anticiper la collision

    public MMFeedbacks HitFeedbck;

    void Awake()
    {
        ballMovement = GetComponent<IBallMovement>();
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }
    }

    void Start()
    {
        ballMovement.InitPosition();
    }

    void Update()
    {
        if (isLaunched)
        {
            CheckUpcomingCollision();  // Ajout de vérification avant le mouvement
            ballMovement.Move();
        }
    }

    public void Launch(Vector2 targetPosition)
    {
        isInUse = true;
        isLaunched = true;
        hasTouchedBottom = false;
        direction = (targetPosition - (Vector2)transform.position).normalized;
        ballMovement.SetDirection(direction);
    }

    /// <summary>
    /// Vérifie avec un Raycast si un obstacle est proche dans la direction actuelle,
    /// et ajuste la direction en conséquence avant le mouvement.
    /// </summary>
    private void CheckUpcomingCollision()
    {
        LayerMask ball = 3;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance,ball);
        if (hit.collider != null)
        {
            // On réfléchit la direction avant d'entrer dans le collider
            direction = Vector2.Reflect(direction, hit.normal);
            ballMovement.SetDirection(direction);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Calcul de la normale moyenne pour les collisions multiples (coins)
        Vector2 averageNormal = Vector2.zero;
        foreach (var contact in collision.contacts)
        {
            averageNormal += contact.normal;
        }
        averageNormal.Normalize();

        direction = Vector2.Reflect(direction, averageNormal);
        ballMovement.SetDirection(direction);

        var triggerable = collision.gameObject.GetComponent<ITriggerable>();
        triggerable?.OnTriggered();

        HitFeedbck?.PlayFeedbacks();
    }

    public void ResetBall()
    {
        isLaunched = false;
        hasTouchedBottom = true;
        isInUse = false;
        ballMovement.ResetPosition();
        BallsHandler.Instance.CheckAllBallsState(); // à injecter pour plus de solidité
    }
}

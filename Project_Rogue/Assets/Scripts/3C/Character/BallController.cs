using System;
using UnityEditor;
using UnityEngine;

public class BallController : MonoBehaviour, IBallLauncher
{
    private IBallMovement ballMovement;
    // private IRendererController rendererController;
    private Vector2 direction;
    private bool isLaunched = false;
    public bool isInUse { get; private set; } = false;
    public bool hasTouchedBottom { get; private set; } = false;

    void Awake()
    {
        ballMovement = GetComponent<IBallMovement>();
        // rendererController = GetComponent<IRendererController>();
    }

    void Start()
    {
        ballMovement.InitPosition();
    }

    void Update()
    {
        if (isLaunched)
        {
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

    private void OnCollisionEnter2D(Collision2D collision)
    { 
        Vector2 normal = collision.contacts[0].normal;
        direction = Vector2.Reflect(direction, normal);
        ballMovement.SetDirection(direction);

        var triggerable = collision.gameObject.GetComponent<ITriggerable>();
        triggerable?.OnTriggered();
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

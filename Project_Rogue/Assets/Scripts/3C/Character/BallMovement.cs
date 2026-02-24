using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour, IBallMovement
{
    private float radius;
    private Vector2 currentDirection;
    public LayerMask collisionMask;

    void Awake()
    {
        radius = transform.localScale.x / 2f;
    }

    public void SetDirection(Vector2 direction)
    {
        currentDirection = direction.normalized;
    }

    public void InitPosition()
    {

    }

   // public void Move()
   // {
   //     float speed = BallSettingsManager.Instance.GetBallSpeed();
   //
   //     Vector2 position = transform.position;
   //     Vector2 remainingMove = currentDirection * speed * Time.deltaTime;
   //
   //     int safety = 0;
   //
   //     while (remainingMove.magnitude > 0.0001f && safety < 3)
   //     {
   //         safety++;
   //
   //         RaycastHit2D hit = Physics2D.CircleCast(
   //             position,
   //             radius,
   //             remainingMove.normalized,
   //             remainingMove.magnitude,
   //             LayerMask.GetMask(collisionMask.ToString())
   //         );
   //
   //         if (hit.collider == null)
   //         {
   //             position += remainingMove;
   //             break;
   //         }
   //
   //         // Position au point de collision
   //         position = hit.point + hit.normal * radius;
   //
   //         // Calcul rebond
   //         currentDirection = Vector2.Reflect(
   //             remainingMove.normalized,
   //             hit.normal
   //         ).normalized;
   //
   //         // Distance restante après collision
   //         float traveled = hit.distance;
   //
   //         float remainingDistance =
   //             remainingMove.magnitude - traveled;
   //
   //         remainingMove =
   //             currentDirection * remainingDistance * 0.98f;
   //     }
   //
   //
   //     // --- collisions écran (ton code OK) ---
   //     List<ScreenBounds> touchedEdges =
   //         ScreenUtils.GetTouchedEdges(position, radius);
   //
   //     if (touchedEdges.Count > 0)
   //     {
   //         currentDirection =
   //             ScreenUtils.ReflectDirection(
   //                 currentDirection,
   //                 touchedEdges
   //             );
   //
   //         position =
   //             ScreenUtils.ClampToScreen(
   //                 position,
   //                 radius
   //             );
   //     }
   //
   //     if (ScreenUtils.TouchesBottom(position, radius))
   //     {
   //         GetComponent<BallController>().ResetBall();
   //         return;
   //     }
   //
   //     transform.position =
   //         new Vector3(position.x, position.y, 0f);
   // }
    public void Move()
    {
        float speed = BallSettingsManager.Instance.GetBallSpeed();
    
        Vector2 startPos = transform.position;
        Vector2 movement = currentDirection * speed * Time.deltaTime;
    
       RaycastHit2D hit = Physics2D.CircleCast(
       startPos,
       radius,
       currentDirection,
       movement.magnitude,
       collisionMask
       );
    
        Vector2 pos;
    
        if (hit.collider != null)
        {
            pos = hit.point + hit.normal * radius;
    
            currentDirection = Vector2.Reflect(currentDirection, hit.normal).normalized;
        }
        else
        {
            pos = startPos + movement;
        }
    
        // --- collisions avec la PlayArea ---
        List<ScreenBounds> touchedEdges = ScreenUtils.GetTouchedEdges(pos, radius);
    
        if (touchedEdges.Count > 0)
        {
            SetDirection(ScreenUtils.ReflectDirection(currentDirection, touchedEdges));
            pos = ScreenUtils.ClampToScreen(pos, radius);
        }
    
        // --- perte de balle ---
        if (ScreenUtils.TouchesBottom(pos, radius))
        {
            GetComponent<BallController>().ResetBall();
            return;
        }
    
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }

    public void ResetPosition()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin; // TOUJOURS à jour
        transform.position = new Vector3(transform.position.x, screenMin.y + radius, 0f);
    }
}

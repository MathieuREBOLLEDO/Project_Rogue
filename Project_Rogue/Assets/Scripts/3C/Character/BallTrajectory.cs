using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BallTrajectory : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform startPos;

    [Header("Trajectory")]
    public int maxBounces = 10;
    public float maxDistance = 50f;
    public float ballRadius = 0.2f;

    [Header("Physics")]
    public LayerMask collisionMask; // Borders, Bricks, World

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
    }

    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
    }

    public void ShowTrajectory(Vector2 targetPos)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 0;

        Vector2 currentPos = startPos.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        List<Vector3> points = new List<Vector3>();
        points.Add(currentPos);

        float remainingDistance = maxDistance;

        for (int i = 0; i < maxBounces; i++)
        {
            if (remainingDistance <= 0f)
                break;

            RaycastHit2D hit = Physics2D.CircleCast(
                currentPos,
                ballRadius,
                direction,
                remainingDistance,
                collisionMask
            );

            if (hit.collider != null)
            {
                // point touché
                points.Add(hit.point);

                // calcul du rebond
                direction = Vector2.Reflect(direction, hit.normal);

                // avancer légèrement pour éviter les collisions infinies
                currentPos = hit.point + hit.normal * 0.01f;

                remainingDistance -= hit.distance;
            }
            else
            {
                // rien touché -> ligne droite
                points.Add(currentPos + direction * remainingDistance);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPositions(points.ToArray());
    }
}

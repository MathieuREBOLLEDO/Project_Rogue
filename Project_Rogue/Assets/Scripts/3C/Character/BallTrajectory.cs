using UnityEngine;
using System.Collections.Generic;

public class BallTrajectoryGhost : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform startPos;

    [Header("Ghost settings")]
    public GameObject ghostBallPrefab;
    public float spacing = 0.3f;          // distance entre chaque ghost
    public int maxGhosts = 50;

    [Header("Physics")]
    public float ballRadius = 0.1f;
    public int maxBounces = 10;
    public float maxDistance = 50f;
    public LayerMask collisionMask;

    private readonly List<GameObject> ghosts = new();
    private int ghostIndex;

    private void Awake()
    {
        // Pool
        for (int i = 0; i < maxGhosts; i++)
        {
            GameObject g = Instantiate(ghostBallPrefab, transform);
            g.SetActive(false);
            ghosts.Add(g);
        }
    }

    public void HideTrajectory()
    {
        foreach (var g in ghosts)
            g.SetActive(false);
    }

    public void ShowTrajectory(Vector2 targetPos)
    {
        HideTrajectory();
        ghostIndex = 0;

        Vector2 currentPos = startPos.position;
        Vector2 direction = (targetPos - currentPos).normalized;

        float remainingDistance = maxDistance;

        while (remainingDistance > 0f && ghostIndex < maxGhosts)
        {
            RaycastHit2D hit = Physics2D.CircleCast(
                currentPos,
                ballRadius,
                direction,
                remainingDistance,
                collisionMask
            );

            float segmentLength = hit.collider != null ? hit.distance : remainingDistance;
            int steps = Mathf.FloorToInt(segmentLength / spacing);

            for (int i = 0; i < steps && ghostIndex < maxGhosts; i++)
            {
                Vector2 pos = currentPos + direction * spacing * i;
                PlaceGhost(pos);
            }

            if (hit.collider != null)
            {
                currentPos = hit.point + hit.normal * 0.01f;
                direction = Vector2.Reflect(direction, hit.normal);
                remainingDistance -= hit.distance;
            }
            else
            {
                break;
            }
        }
    }

    void PlaceGhost(Vector2 pos)
    {
        GameObject g = ghosts[ghostIndex++];
        g.transform.position = pos;
        g.SetActive(true);
    }
}

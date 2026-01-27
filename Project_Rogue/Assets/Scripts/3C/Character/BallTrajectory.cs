using UnityEngine;
using System.Collections.Generic;

public class BallTrajectoryGhost : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform startPos;

    [Header("Ghost Prefabs")]
    public GameObject ghostBallPrefab;
    public GameObject impactBallPrefab;

    [Header("Ghost Settings")]
    public float spacing = 0.3f;
    public int maxGhosts = 80;
    public int maxImpacts = 10;

    [Header("Physics")]
    public float ballRadius = 0.1f;
    public float maxDistance = 50f;
    public int maxBounces = 10;
    public LayerMask collisionMask;

    private readonly List<GameObject> ghostPool = new();
    private readonly List<GameObject> impactPool = new();

    private int ghostIndex;
    private int impactIndex;

    private void Awake()
    {
        // Pool des ghosts normaux
        for (int i = 0; i < maxGhosts; i++)
        {
            GameObject g = Instantiate(ghostBallPrefab, transform);
            g.SetActive(false);
            ghostPool.Add(g);
        }

        // Pool des impacts
        for (int i = 0; i < maxImpacts; i++)
        {
            GameObject g = Instantiate(impactBallPrefab, transform);
            g.SetActive(false);
            impactPool.Add(g);
        }
    }

    public void HideTrajectory()
    {
        foreach (var g in ghostPool) g.SetActive(false);
        foreach (var g in impactPool) g.SetActive(false);
    }

    public void ShowTrajectory(Vector2 targetPos)
    {
        HideTrajectory();
        ghostIndex = 0;
        impactIndex = 0;

        Vector2 currentPos = startPos.position;
        Vector2 direction = (targetPos - currentPos).normalized;
        float remainingDistance = maxDistance;

        for (int bounce = 0; bounce < maxBounces; bounce++)
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

            float travelDistance = hit.collider != null
                ? hit.distance
                : remainingDistance;

            float traveled = 0f;

            // Avancer progressivement jusqu’au point de collision
            while (traveled + spacing < travelDistance && ghostIndex < maxGhosts)
            {
                currentPos += direction * spacing;
                traveled += spacing;
                PlaceGhost(currentPos);
            }

            if (hit.collider != null)
            {
                //  impact visuel EXACT
                PlaceImpact(hit.point);

                //  position réelle du centre de la balle au contact
                currentPos = hit.point + hit.normal * ballRadius;

                //  rebond physique correct
                direction = Vector2.Reflect(direction, hit.normal).normalized;

                remainingDistance -= hit.distance;
            }
            else
            {
                // pas de collision, fin de trajectoire
                break;
            }
        }
    }



    void PlaceGhost(Vector2 pos)
    {
        if (ghostIndex >= ghostPool.Count) return;

        GameObject g = ghostPool[ghostIndex++];
        g.transform.position = pos;
        g.SetActive(true);
    }

    void PlaceImpact(Vector2 pos)
    {
        if (impactIndex >= impactPool.Count) return;

        GameObject g = impactPool[impactIndex++];
        g.transform.position = pos;
        g.SetActive(true);
    }
}

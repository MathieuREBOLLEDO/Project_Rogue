using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallTrajectory : MonoBehaviour
{
    [SerializeField] Transform startPos;
    public float simulationStep = 0.01f; // temps par step (en secondes)
    public float simulationDuration = 3f; // durée totale de simulation (en secondes)
    public float ballSpeed = 5f; // vitesse de la balle (unités/seconde)
    public float ballRadius = 0.2f; // rayon de la balle

    private LineRenderer lineRenderer;

    private float maxAllowedAngle = GlobalBallVariables.angleOfShooting;
    private float minAllowedAngle = -GlobalBallVariables.angleOfShooting;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        HideTrajectory();
    }
      
    public void HideTrajectory()
    {
        lineRenderer.enabled = false;
    }

    public void ShowTrajectory( Vector2 pos)
    {
        lineRenderer.enabled = true;
        List<Vector3> points = new List<Vector3>();
        Vector2 currentPos = (Vector2)startPos.position;
        Vector2 currentDir = (pos - (Vector2)startPos.position).normalized; //direction.normalized;

        float timeElapsed = 0f;
        float stepDistance = ballSpeed * simulationStep;

        float angle = Vector2.Angle(Vector2.up, currentDir); // Angle par rapport à vertical (ou change en Vector2.right si horizontal)


        if (angle < minAllowedAngle || angle > maxAllowedAngle)  // Vérifie si l'angle est dans les limites autorisées
        {
            Debug.LogWarning($"Angle de tir invalide : {angle}°. Autorisé entre {minAllowedAngle}° et {maxAllowedAngle}°.");
            return;
        }

        else
        {
            while (timeElapsed < simulationDuration)
            {
                points.Add(currentPos);

                Vector2 nextPos = currentPos + currentDir * stepDistance;

                var edges = ScreenUtils.GetTouchedEdges(nextPos, ballRadius);
                if (edges.Count > 0)
                {
                    currentDir = ScreenUtils.ReflectDirection(currentDir, edges);
                    nextPos = currentPos + currentDir * stepDistance;
                }

                currentPos = nextPos;
                timeElapsed += simulationStep;
            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ConvertAll(p => (Vector3)p).ToArray());
        }
    }

}

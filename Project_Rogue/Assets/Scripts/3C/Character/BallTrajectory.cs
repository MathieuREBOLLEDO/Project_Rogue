using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BallTrajectory : MonoBehaviour
{

    public Vector2 positionInitiale = Vector2.zero; // Position de départ
    public Vector2 direction = new Vector2(1f, 0f); // Direction initiale
    public float vitesse = 5f; // Vitesse de la balle
    public float dureeSimulation = 2f; // Durée de la simulation en secondes
    public int nombreDePoints = 50; // Nombre de points pour la ligne

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = nombreDePoints;

        DessinerTrajectoire();
    }

    void DessinerTrajectoire()
    {
        Vector3[] points = new Vector3[nombreDePoints];
        Vector2 position = positionInitiale;
        Vector2 vitesseVecteur = direction.normalized * vitesse;

        for (int i = 0; i < nombreDePoints; i++)
        {
            float t = (dureeSimulation / nombreDePoints) * i;
            Vector2 nouvellePosition = position + vitesseVecteur * t;
            points[i] = new Vector3(nouvellePosition.x, nouvellePosition.y, 0f);
        }

        lineRenderer.SetPositions(points);
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//
//[RequireComponent(typeof(LineRenderer))]
//public class BallTrajectory : MonoBehaviour
//{
//
//    [SerializeField] private GameObject ballSpawner;
//    [SerializeField] private GameObject goDebug;
//    private Vector2 initPos;
//    private Vector2 targetPosition;
//    private Vector2 direction;
//    public float vitesse = 5f; // Vitesse de la balle
//    public float dureeSimulation = 2f; // Durée de la simulation en secondes
//    public int nombreDePoints = 50; // Nombre de points pour la ligne
//
//    private LineRenderer lineRenderer;
//
//    void Start()
//    {
//        lineRenderer = GetComponent<LineRenderer>();
//        lineRenderer.positionCount = nombreDePoints;
//        initPos = ballSpawner.transform.position;
//
//        //DessinerTrajectoire();
//    }
//    private void SetInitPosition() => initPos = ballSpawner.transform.position;
//    private void SetDirection(Vector2 dir) =>  direction = dir.normalized;
//
//    /*public void DrawTrajectory(Vector2 target)
//    {
//        float radius = 0.5f;
//        SetInitPosition();
//        targetPosition = target;
//
//        lineRenderer.positionCount = nombreDePoints;
//
//        Vector2 position = initPos;
//        SetDirection(targetPosition - (Vector2)initPos); // On définit la direction une seule fois au début
//        Vector2 velocity = direction.normalized * vitesse; // vitesse = float, déjà définie ?
//
//        float deltaT = dureeSimulation / nombreDePoints;
//
//        for (int i = 0; i < nombreDePoints; i++)
//        {
//            // Calcul de la nouvelle position avec la vitesse actuelle
//            Vector2 newPos = position + velocity * deltaT;
//
//            // Vérifier les collisions avec les bords
//            List<ScreenBounds> touchedEdges = ScreenUtils.GetTouchedEdges(newPos, radius);
//
//            if (touchedEdges.Count > 0)
//            {
//                // Réfléchir la vitesse
//                velocity = ScreenUtils.ReflectDirection(velocity, touchedEdges);
//                // Recalculer la position après réflexion
//                newPos = position + velocity * deltaT;
//            }
//
//            // Mise à jour de la position
//            position = newPos;
//
//            // Envoi au LineRenderer
//            lineRenderer.SetPosition(i, new Vector3(position.x, position.y, 0f));
//
//            // Debug : placer un petit objet pour visualiser le point
//            GameObject.Instantiate(goDebug, position, Quaternion.identity);
//        }
//    }*/
//
//    public LayerMask collisionMask;   // calque(s) des objets à rebondir
//    //public GameObject goDebug;
//    //
//    //Vector2 initPos;
//    //Vector2 targetPosition;
//    //Vector2 direction;
//
//  //  void SetInitPosition() { /* ta logique existante */ }
//  //  void SetDirection(Vector2 dir) => direction = dir.normalized;
//  //
//    public void DrawTrajectory(Vector2 target)
//    {
//        float radius = 0.5f;
//
//        SetInitPosition();
//        targetPosition = target;
//
//        Vector2 position = initPos;
//        SetDirection(targetPosition - (Vector2)initPos);
//        Vector2 velocity = direction.normalized * vitesse;
//
//        float dt = dureeSimulation / Mathf.Max(1, nombreDePoints - 1);
//
//        var points = new List<Vector3>(nombreDePoints + 16);
//        points.Add(new Vector3(position.x, position.y, 0f)); // point de départ
//
//        for (int i = 0; i < nombreDePoints - 1 && points.Count < nombreDePoints; i++)
//        {
//            float remaining = dt;
//            int safety = 0;
//
//            while (remaining > 0f && safety++ < 6 && points.Count < nombreDePoints)
//            {
//                Vector2 step = velocity * remaining;
//
//                // 1) collision avec le décor (bords d'écran)
//                float uEdge;
//                Vector2 edgeNormal;
//                bool hitEdge = TryEdgeHit(position, step, radius, out uEdge, out edgeNormal);
//
//                // 2) collision avec objets 2D (raycast circulaire)
//                RaycastHit2D hit = Physics2D.CircleCast(position, radius, step.normalized, step.magnitude, collisionMask);
//                bool hitObj = hit.collider != null;
//                float uObj = hitObj ? (hit.distance / step.magnitude) : float.PositiveInfinity;
//
//                bool hasCollision = hitEdge || hitObj;
//
//                if (hasCollision)
//                {
//                    // prendre la collision la plus proche dans [0,1]
//                    float u = 1f;
//                    Vector2 normal = Vector2.zero;
//                    Vector2 impact;
//
//                    if (uObj < uEdge)
//                    {
//                        u = Mathf.Clamp01(uObj);
//                        impact = position + step * u;
//                        normal = hit.normal; // normal du collider
//                    }
//                    else
//                    {
//                        u = Mathf.Clamp01(uEdge);
//                        impact = position + step * u;
//                        normal = edgeNormal; // normal du mur (écran)
//                    }
//
//                    // placer un point EXACTEMENT à l'impact
//                    points.Add(new Vector3(impact.x, impact.y, 0f));
//                    if (goDebug) Instantiate(goDebug, (Vector3)impact, Quaternion.identity);
//                    if (points.Count >= nombreDePoints) break;
//
//                    // réfléchir et continuer avec le temps restant
//                    velocity = Vector2.Reflect(velocity, normal).normalized * vitesse;
//                    position = impact;
//
//                    // petit "décollage" pour éviter le collage numérique au mur
//                    position += velocity.normalized * 0.001f;
//
//                    remaining *= (1f - u);
//                }
//                else
//                {
//                    // pas de collision : on va au bout du pas
//                    Vector2 newPos = position + step;
//                    points.Add(new Vector3(newPos.x, newPos.y, 0f));
//                    if (goDebug) Instantiate(goDebug, (Vector3)newPos, Quaternion.identity);
//
//                    position = newPos;
//                    remaining = 0f;
//                }
//            }
//        }
//
//        lineRenderer.positionCount = points.Count;
//        lineRenderer.SetPositions(points.ToArray());
//    }
//
//    // --- Utilitaire : détection de la 1ère collision avec les bords de l'écran ---
//    // suppose Camera.main orthographique
//    bool TryEdgeHit(Vector2 pos, Vector2 step, float radius, out float u, out Vector2 normal)
//    {
//        var cam = Camera.main;
//        float halfH = cam.orthographicSize;
//        float halfW = halfH * cam.aspect;
//        Vector2 c = cam.transform.position;
//
//        float minX = c.x - halfW + radius;
//        float maxX = c.x + halfW - radius;
//        float minY = c.y - halfH + radius;
//        float maxY = c.y + halfH - radius;
//
//        u = float.PositiveInfinity;
//        normal = Vector2.zero;
//
//        // murs verticaux
//        if (Mathf.Abs(step.x) > 1e-6f)
//        {
//            if (step.x > 0f)
//            {
//                float tx = (maxX - pos.x) / step.x;
//                if (tx >= 0f && tx <= 1f && tx < u) { u = tx; normal = Vector2.left; }
//            }
//            else
//            {
//                float tx = (minX - pos.x) / step.x;
//                if (tx >= 0f && tx <= 1f && tx < u) { u = tx; normal = Vector2.right; }
//            }
//        }
//
//        // murs horizontaux
//        if (Mathf.Abs(step.y) > 1e-6f)
//        {
//            if (step.y > 0f)
//            {
//                float ty = (maxY - pos.y) / step.y;
//                if (ty >= 0f && ty <= 1f)
//                {
//                    if (Mathf.Abs(ty - u) < 1e-4f) // coin : double contact
//                        normal = (normal + Vector2.down).normalized;
//                    else if (ty < u) { u = ty; normal = Vector2.down; }
//                }
//            }
//            else
//            {
//                float ty = (minY - pos.y) / step.y;
//                if (ty >= 0f && ty <= 1f)
//                {
//                    if (Mathf.Abs(ty - u) < 1e-4f)
//                        normal = (normal + Vector2.up).normalized;
//                    else if (ty < u) { u = ty; normal = Vector2.up; }
//                }
//            }
//        }
//
//        return !float.IsInfinity(u);
//    }
//}
//
//
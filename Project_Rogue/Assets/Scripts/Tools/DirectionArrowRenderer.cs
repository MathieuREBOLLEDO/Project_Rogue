using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class DirectionArrowRenderer : MonoBehaviour
{
    public Transform target;
    private LineRenderer line;

    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 3;
    }

    void Update()
    {
        if (target == null) return;

        Vector3 from = transform.position;
        Vector3 to = target.position;
        Vector3 dir = (to - from).normalized;
        float headLength = 0.5f;

        // Ligne de base
        line.SetPosition(0, from);
        line.SetPosition(1, to);

        // Tête de flèche
        Vector3 right = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 150, 0) * Vector3.forward;
        Vector3 left = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -150, 0) * Vector3.forward;

        // Tête : un seul point, tu peux en ajouter plus selon le style que tu veux
        line.positionCount = 4;
        line.SetPosition(2, to - right * headLength);
        line.SetPosition(3, to - left * headLength);
    }
}

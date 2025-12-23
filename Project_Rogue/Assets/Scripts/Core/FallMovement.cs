using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallMovement : MonoBehaviour
{
    public float fallSpeed = 2f;

    void Update()
    {
        // Déplacement vers le bas à vitesse constante
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }
}

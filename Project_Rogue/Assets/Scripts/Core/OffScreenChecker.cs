using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class OffScreenChecker : MonoBehaviour
{
    private Camera mainCam;
    private CircleCollider2D circle;

    void Start()
    {
        mainCam = Camera.main;
        circle = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if(ScreenUtils.IsBelowScreen(transform.position,circle.radius))
        {
            Debug.Log(gameObject.name + " est sorti de l'écran !");
            Destroy(gameObject);
        }
    }
}

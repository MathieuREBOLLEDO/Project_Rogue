using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BallController: MonoBehaviour 
{  
    //public float speed = 5f;

    private Vector2 direction;
    private bool isLaunched = false;
    public bool isInUse = false;

    Vector2 screenMin;
    public bool hasTouchedBottom { get; private set; } = false; 

    private float radius;

    void Start()
    {
        radius = transform.localScale.x / 2f;
        screenMin = ScreenUtils.ScreenMin;
        InitPosition(); 
    }

    public void SetRendererActive(bool state)
    {
        GetComponent<SpriteRenderer>().enabled = state;
    }


    void Update()
    {
        if (isLaunched)
            MoveBall();
    }

    public void LaunchBall(Vector2 mousePos)
    {
        // Direction initiale vers le haut avec un angle aléatoire

        isInUse = true;
        direction = (mousePos - (Vector2)transform.position).normalized;
        isLaunched = true;
        hasTouchedBottom = false;

    }

    void MoveBall()
    {

        float speed = BallSettingsManager.Instance.GetBallSpeed();
        Vector3 pos = transform.position;
        pos += (Vector3)(direction * speed * Time.deltaTime);

        // Vérifie les collisions avec les bords
        List<ScreenBounds> touchedEdges = ScreenUtils.GetTouchedEdges(pos, radius);

        if (touchedEdges.Count > 0)
        {
            direction = ScreenUtils.ReflectDirection(direction, touchedEdges);
            pos = ScreenUtils.ClampToScreen(pos, radius);
        }

        if (ScreenUtils.TouchesBottom(pos, radius))
        {
            ResetBall();
            return;
        }

        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Calcule du vecteur normal moyen pour rebondir proprement
        Vector2 normal = collision.contacts[0].normal;
        direction = Vector2.Reflect(direction, normal);
        Destroy(collision.gameObject);        
    }

    public void InitPosition()
    {
        
        Vector3 startPos = new Vector3(0f, screenMin.y + radius + 0.2f, 0f);
        transform.position = startPos;
    }

    public void ResetBall()
    {
        isLaunched = false;
        hasTouchedBottom = true;
        isInUse = false;
        transform.position = new Vector3(transform.position.x, screenMin.y + radius, 0f);
        //gameObject.SetActive(false);
        BallManager.Instance.CheckAllBallsState();
    }
}


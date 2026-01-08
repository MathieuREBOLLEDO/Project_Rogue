using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour, IBallMovement
{
    private float radius;
    private Vector2 currentDirection;

    void Awake()
    {
        radius = transform.localScale.x / 2f;
    }

    public void SetDirection(Vector2 direction)
    {
        currentDirection = direction.normalized;
    }

    public void InitPosition()
    {

    }
    public void Move()
    {
        float speed = BallSettingsManager.Instance.GetBallSpeed();

        Vector3 pos = transform.position + (Vector3)(currentDirection * speed * Time.deltaTime);

        // --- collisions avec la PlayArea ---
        List<ScreenBounds> touchedEdges = ScreenUtils.GetTouchedEdges(pos, radius);

        if (touchedEdges.Count > 0)
        {
            SetDirection(ScreenUtils.ReflectDirection(currentDirection, touchedEdges));
            pos = ScreenUtils.ClampToScreen(pos, radius);
        }

        // --- perte de balle ---
        if (ScreenUtils.TouchesBottom(pos, radius))
        {
            GetComponent<BallController>().ResetBall();
            return;
        }

        transform.position = pos;
    }

    public void ResetPosition()
    {
        Vector2 screenMin = ScreenUtils.ScreenMin; // TOUJOURS à jour
        transform.position = new Vector3(transform.position.x, screenMin.y + radius, 0f);
    }
}

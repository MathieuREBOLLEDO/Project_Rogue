using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour, IBallMovement
{
    private Vector2 screenMin;
    private float radius;

    private Vector2 currentDirection;


    void Awake()
    {
        screenMin = ScreenUtils.ScreenMin;
        radius = transform.localScale.x / 2f;
    }

    public void SetDirection(Vector2 direction)
    {
        currentDirection = direction.normalized;
    }

    public void InitPosition()
    {
        transform.position = new Vector3(0f, screenMin.y + radius + 0.2f, 0f);
    }

    public void Move()
    {
        float speed = BallSettingsManager.Instance.GetBallSpeed();
        Vector3 pos = transform.position + (Vector3)(currentDirection * speed * Time.deltaTime);


        List<ScreenBounds> touchedEdges = ScreenUtils.GetTouchedEdges(pos, radius);

        if (touchedEdges.Count > 0)
        {
            SetDirection(ScreenUtils.ReflectDirection(currentDirection, touchedEdges));
            pos = ScreenUtils.ClampToScreen(pos, radius);
        }



        if (ScreenUtils.TouchesBottom(pos, radius))
        {
            GetComponent<BallController>().ResetBall();
            return;
        }

        transform.position = pos;
    }

    public void ResetPosition()
    {
        transform.position = new Vector3(transform.position.x, screenMin.y + radius, 0f);
    }
}

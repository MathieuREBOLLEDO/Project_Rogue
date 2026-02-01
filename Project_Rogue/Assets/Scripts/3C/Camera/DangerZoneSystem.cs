using UnityEngine;
using System;

public class DangerZoneSystem : MonoBehaviour
{
    public PlayAreaLayoutSO layout;
    public Transform bricksParent;

    public event Action OnDangerWarning;
    public event Action OnDangerCollision;

    private ZoneDef dangerZone;
    private float dangerLineY;
    private float warningDistance = 1;

    private bool warningTriggered;

    void Start()
    {
        if (!layout.TryGetZone(ZoneType.Danger, out dangerZone))
        {
            Debug.LogError("Zone Danger non définie dans le layout");
            enabled = false;
            return;
        }

        ComputeDangerLinePosition();
        CreateLineRenderer();
    }

    void Update()
    {
        CheckBricks();
    }

    void ComputeDangerLinePosition()
    {
        float cellHeight = GridService.Instance.CellSize.y;
        dangerLineY = dangerZone.rows * cellHeight;
    }

    void CheckBricks()
    {
        foreach (Transform brick in bricksParent)
        {
            float brickY = brick.position.y;
            float distance = Mathf.Abs(brickY - dangerLineY);

            // ?? Warning
            if (distance <= warningDistance && !warningTriggered)
            {
                warningTriggered = true;
                Debug.Log("DANGER");
                OnDangerWarning?.Invoke();
            }

            // ?? Collision
            if (brickY <= dangerLineY)
            {
                Debug.Log("on perd un lancer");
                OnDangerCollision?.Invoke();
            }
        }
    }

    void OnDrawGizmos()
    {
        if (dangerZone == null) return;

        Gizmos.color = Color.red;

        float width = layout.globalGrid.columns * GridService.Instance.CellSize.x;
        Vector3 start = new Vector3(0, dangerLineY, 0);
        Vector3 end = new Vector3(width, dangerLineY, 0);

        Gizmos.DrawLine(start, end);
    }

    void CreateLineRenderer()
    {
        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.red;
        lr.endColor = Color.red;

        float width = layout.globalGrid.columns * GridService.Instance.CellSize.x;

        lr.SetPosition(0, new Vector3(0, dangerLineY, 0));
        lr.SetPosition(1, new Vector3(width, dangerLineY, 0));
    }
}

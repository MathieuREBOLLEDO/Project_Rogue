using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangerZoneRuntimeLine : MonoBehaviour
{
    [Header("Bricks")]
    [SerializeField] private Transform bricksRoot;

    [Header("Danger Settings")]
    [SerializeField] private float warningDistance = 1f;

    public PlayAreaLayoutBuilder builder;
    public LineRenderer line;

    private Coroutine initRoutine;

    private float dangerLineY;
    private bool dangerLogged;
    private HashSet<Transform> bricksAlreadyCollided = new HashSet<Transform>();

    void OnEnable()
    {
//        GridEvents.OnRequestNewBrickLine += CheckBricks;
        GameEventManager.Instance.Subscribe(GameEventType.MachineTurnStart, _ => CheckBricks());
    
        initRoutine = StartCoroutine(InitWhenPlayAreaReady());
    }

    void OnDisable()
    {
  //      GridEvents.OnRequestNewBrickLine -= CheckBricks;
        GameEventManager.Instance.Unsubscribe(GameEventType.MachineTurnStart, _ => CheckBricks());
        if (initRoutine != null)
            StopCoroutine(initRoutine);
    }

    IEnumerator InitWhenPlayAreaReady()
    {
        // Attente que le PlayAreaProvider soit prêt
        yield return new WaitUntil(() =>
            PlayAreaProvider.Instance != null &&
            PlayAreaProvider.Instance.TryGetPlayArea(out _)
        );

        // Construction du layout
        if (!builder.Build())
        {
            Debug.LogError("Impossible de construire le PlayAreaLayout");
            yield break;
        }
        SetupLineRenderer();

        DrawDangerLine();
    }

    void SetupLineRenderer()
    {
        line.positionCount = 2;
        line.useWorldSpace = true;

       // line.startWidth = 2f;
       // line.endWidth = 2f;

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.red;
        line.endColor = Color.red;

        line.sortingOrder = 1000;
        line.alignment = LineAlignment.View;
    }

    void DrawDangerLine()
    {
        if (builder.Zones == null) return;

        if (!builder.Zones.TryGetValue(ZoneType.Bricks, out var bricks)) return;
        if (!builder.Zones.TryGetValue(ZoneType.Bumpers, out var bumpers)) return;

        // Ligne EXACTE entre Bricks et Bumpers
        float y = bricks.rect.yMin;

        Vector3 left = new Vector3(bricks.rect.xMin, y, 0f);
        Vector3 right = new Vector3(bricks.rect.xMax, y, 0f);

        line.SetPosition(0, left);
        line.SetPosition(1, right);
    }

    void CheckBricks()
    {
        foreach (Transform brick in bricksRoot)
        {
            float brickY = brick.position.y;
            float distance = Mathf.Abs(brickY - dangerLineY);
            

            //  PROXIMITÉ
            if (distance <= warningDistance && !dangerLogged)
            {
                dangerLogged = true;
                Debug.LogError("!!!!!Danger !!!!!");
            }

            //  CONTACT / FRANCHISSEMENT
            if (brickY <= dangerLineY && !bricksAlreadyCollided.Contains(brick))
            {
                bricksAlreadyCollided.Add(brick);
                Debug.LogError("!!!!! Loose 1 Throw !!!!! ");
            }
        }
    }
}
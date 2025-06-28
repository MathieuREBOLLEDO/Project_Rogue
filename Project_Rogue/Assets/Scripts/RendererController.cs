using System.Collections;
using UnityEngine;

public class RendererController : MonoBehaviour, IRendererController
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetRendererActive(bool state)
    {
        spriteRenderer.enabled = state;
    }
}


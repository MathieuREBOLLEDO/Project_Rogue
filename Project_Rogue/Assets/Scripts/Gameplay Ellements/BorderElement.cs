using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class BorderElement : MonoBehaviour
{
    [Header("Score")]
    public int points = 10;

    [Header("UI")]
    public Text displayText;

    [Header("Destruction")]
    public bool destroyOnHit = true;

    private bool isActive = true;

    private void Start()
    {
        if (displayText != null)
            displayText.text = points.ToString();
    }

    public void SetActive(bool value)
    {
        isActive = value;
        gameObject.SetActive(value);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        if (collision.gameObject.CompareTag("Ball"))
        {
            //ScoreManager.Instance.AddScore(points);

            if (destroyOnHit)
                Destroy(gameObject);
        }
    }
}


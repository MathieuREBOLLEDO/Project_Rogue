using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoint : MonoBehaviour
{
    public int scoreValue = 10;  // Points pour cette caisse

    public void AddToScore()
    {
     // On ajoute le score
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.AddScoreTemp(scoreValue);
    }
}

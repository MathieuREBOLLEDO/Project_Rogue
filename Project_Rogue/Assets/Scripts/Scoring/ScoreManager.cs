using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentRoundScore = 0;   // Score temporaire du tour
    private int totalScore = 0;          // Score final cumulé

    public IntEvent OnUpdateScore;

    private void Awake()
    {
        // Singleton pour y accéder de partout
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Appelé quand un objet destructible est détruit.
    /// </summary>
    /// <param name="scoreValue">Valeur en points de l'objet détruit</param>
    public void AddScoreTemp(int scoreValue)
    {
        currentRoundScore += scoreValue;
        Debug.Log($"Score temporaire : {currentRoundScore}");
        OnUpdateScore?.Invoke(scoreValue);
    }

    /// <summary>
    /// Transfère le score temporaire au score final (fin de tour).
    /// </summary>
    public void ValidateRoundScore()
    {
        totalScore += currentRoundScore;
        currentRoundScore = 0;
        Debug.Log($"Score total : {totalScore}");
    }

    /// <summary>
    /// Récupérer le score total actuel.
    /// </summary>
    public int GetTotalScore()
    {
        return totalScore;
    }


}

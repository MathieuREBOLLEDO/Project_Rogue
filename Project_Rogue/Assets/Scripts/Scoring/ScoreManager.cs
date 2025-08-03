using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentRoundScore = 0;   // Score temporaire du tour
    private int totalScore = 0;          // Score final cumul�

    public IntEvent OnUpdateScore;

    private void Awake()
    {
        // Singleton pour y acc�der de partout
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Appel� quand un objet destructible est d�truit.
    /// </summary>
    /// <param name="scoreValue">Valeur en points de l'objet d�truit</param>
    public void AddScoreTemp(int scoreValue)
    {
        currentRoundScore += scoreValue;
        Debug.Log($"Score temporaire : {currentRoundScore}");
        OnUpdateScore?.Invoke(scoreValue);
    }

    /// <summary>
    /// Transf�re le score temporaire au score final (fin de tour).
    /// </summary>
    public void ValidateRoundScore()
    {
        totalScore += currentRoundScore;
        currentRoundScore = 0;
        Debug.Log($"Score total : {totalScore}");
    }

    /// <summary>
    /// R�cup�rer le score total actuel.
    /// </summary>
    public int GetTotalScore()
    {
        return totalScore;
    }


}

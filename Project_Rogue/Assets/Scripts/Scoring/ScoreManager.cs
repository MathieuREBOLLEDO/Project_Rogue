using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int currentRoundScore = 0;   // Score temporaire du tour
    private int totalScore = 0;          // Score final cumulé

    
    public IntEvent OnInitScore;
    public IntEvent OnUpdateRoundScore;
    public IntEvent OnUpdateTotalScore;

    private void Awake()
    {
        // Singleton pour y accéder de partout
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        OnInitScore?.Invoke(0);
    }

    public void AddScoreTemp(int scoreValue)
    {
        currentRoundScore += scoreValue;
        Debug.Log($"Score temporaire : {currentRoundScore}");
        OnUpdateRoundScore?.Invoke(currentRoundScore);
    }

    public void ValidateRoundScore()
    {
        totalScore += currentRoundScore;
        currentRoundScore = 0;
        Debug.Log($"Score total : {totalScore}");
        OnUpdateTotalScore?.Invoke(totalScore);
    }

    public int GetTotalScore()
    {
        return totalScore;
    }


}

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
        //Debug.Log($"Score temporaire : {currentRoundScore}");
        OnUpdateRoundScore?.Invoke(currentRoundScore);
    }

    public void ValidateRoundScore()
    {
        StartCoroutine(UpdateTotalNumber());


        //totalScore += currentRoundScore;
        //currentRoundScore = 0;
        //Debug.Log($"Score total : {totalScore}");
        //OnUpdateTotalScore?.Invoke(totalScore);
    }

    private IEnumerator UpdateTotalNumber()
    {
        int sourceScore = totalScore;
        int destinationScore = totalScore + currentRoundScore;
        while (sourceScore < destinationScore)
        {
            sourceScore++;
            currentRoundScore --;
            totalScore++;
            OnUpdateRoundScore?.Invoke(currentRoundScore);
            OnUpdateTotalScore?.Invoke(totalScore);
            //Debug.LogWarning("Total Score  : " + totalScore + " | CurrentTmpScore : " + currentRoundScore);
            yield return new WaitForFixedUpdate(); 
        }
        //yield return new WaitForSeconds(0.2f);
    }


    public int GetTotalScore()
    {
        return totalScore;
    }


}

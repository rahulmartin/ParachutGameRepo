using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionsProvider : MonoBehaviour
{
    //This is only for this test prototype because in real scenario 
    //we should use scriptable object on fetch question from web

    [SerializeField]
    List<string> correctStatements;
    [SerializeField]
    List<string> incorrectStatements;

    public int randomizationSeed;
    public bool IsDebugMode;
    private void Start()
    {
        Random.InitState(randomizationSeed);
    }
    public string GetCorrectStatement()
    {
        if(IsDebugMode)
        {
            return "Correct";
        }
        return correctStatements[Random.Range(0, correctStatements.Count)];
    }

    public string GetIncorrectStatement()
    {
        if (IsDebugMode)
            return "InCorrect";
        return incorrectStatements[Random.Range(0, incorrectStatements.Count)];
    }
}

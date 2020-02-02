using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Patient : MonoBehaviour, ITaskProgress
{
    public Action<Patient> OnPatientTimeOut;
    public Action<Patient> OnPatientCured;

    public AntidoteRecipeData requiredAntidote;

    public bool movetime;

    public float timeLimit;

    public float timeLeft;

    [Header("UI")]
    public Image ingredient1Sprite;
    public Image ingredient2Sprite;
    public Image antidoteSprite;

    public float Progress
    {
        get
        {
            return timeLeft / timeLimit;
        }
    }

    private void Awake()
    {
        timeLeft = timeLimit;
    }

    private void Update()
    {
        if (movetime && GameManager.Instance.currentGameState == EGameState.Playing)
        {
            timeLeft -= Time.deltaTime;

            if (timeLeft <= 0)
            {
                TimeLimitReached();
            }
        }
    }

    public void SetRequiredAntidote(AntidoteRecipeData antidoteRecipeData)
    {
        if (antidoteRecipeData == null) return;

        requiredAntidote = antidoteRecipeData;

        ingredient1Sprite.sprite = requiredAntidote.antidoteRecipe.ingredients[0].IngredientDisplaySprite;
        ingredient2Sprite.sprite = requiredAntidote.antidoteRecipe.ingredients[1].IngredientDisplaySprite;
        antidoteSprite.sprite = requiredAntidote.antidoteRecipe.antidoteSprite;
    }

    private void TimeLimitReached()
    {
        OnPatientTimeOut?.Invoke(this);

        Destroy(this.gameObject);
    }

    public void GiveAntidoteToPatient(AntidoteRecipe antidote)
    {
        if (requiredAntidote.antidoteRecipe != antidote)
        {
            OnPatientTimeOut?.Invoke(this);
        }
        else
        {
            Debug.Log("Fed correct antidote");
            OnPatientCured?.Invoke(this);
        }

        Destroy(this.gameObject);
    }
}

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

    public Animator patientOverlayAnimator;

    public bool movetime;

    public float timeLimit;

    public float timeLeft;

    private bool isDisabled;

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

            if (timeLeft <= 0 && !isDisabled)
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
        
        if (patientOverlayAnimator != null)
            patientOverlayAnimator.SetTrigger("OpenRed");

        var index = UnityEngine.Random.Range(0, 2);
        if (index == 0)
        {
            SoundManager.Instance.PlaySFX("SFX_FemaleDeath");
        }
        else
        {
            SoundManager.Instance.PlaySFX("SFX_MaleDeath");
        }

        StartCoroutine(DelayedDestroy(1f));
    }

    public void GiveAntidoteToPatient(AntidoteRecipe antidote)
    {
        if (requiredAntidote.antidoteRecipe == antidote)
        {
            SoundManager.Instance.PlaySFX("SFX_PatientCured");

            if (patientOverlayAnimator != null)
                patientOverlayAnimator.SetTrigger("OpenYellow");

            OnPatientCured?.Invoke(this);

            StartCoroutine(DelayedDestroy(1f));
        }
    }

    private IEnumerator DelayedDestroy(float time)
    {
        isDisabled = true;

        yield return new WaitForSeconds(time);

        Destroy(this.gameObject);
    }
}

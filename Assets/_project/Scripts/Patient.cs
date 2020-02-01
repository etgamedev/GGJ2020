using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Patient : MonoBehaviour, ITaskProgress
{
    public Action<Patient> OnPatientTimeOut;

    public AntidoteRecipeData requiredAntidote;

    public bool movetime;

    public float timeLimit;

    public float timeElapsed;

    public float Progress
    {
        get
        {
            return timeElapsed / timeLimit;
        }
    }

    private void Update()
    {
        if (movetime)
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed >= timeLimit)
            {
                TimeLimitReached();
            }
        }
    }

    private void TimeLimitReached()
    {
        OnPatientTimeOut?.Invoke(this);
    }
}

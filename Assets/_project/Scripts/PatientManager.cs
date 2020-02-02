using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PatientManager : MonoBehaviour
{
    public Patient patientPrefab;
    public AntidoteRecipeBook antidoteRecipeBook;
    public Transform patientWaitPanel;

    public TextMeshProUGUI tmPatientDeadLimit;
    public TextMeshProUGUI tmDeadPatient;
    public TextMeshProUGUI tmPatientLeft;
    public TextMeshProUGUI tmTotalPatient;

    public float patientSpawningInterval;
    public int maxPatients;
    public int maxDeadPatients;
    public int patientsLeft;
    public int totalPatients;

    public List<Patient> activePatients = new List<Patient>();

    private int deadPatientCount;
    private float patientSpawnTimeElapsed;
    private Coroutine createPatientCoroutine;
    private int patientsSpawned;

    private void Start()
    {
        StartCoroutine(WaitToStartSpawning());
    }

    private void Update()
    {
        if (tmPatientDeadLimit != null)
        {
            tmPatientDeadLimit.text = maxDeadPatients.ToString();
        }

        if (tmDeadPatient != null)
        {
            tmDeadPatient.text = deadPatientCount.ToString();

            if (deadPatientCount >= maxDeadPatients - 1)
            {
                tmDeadPatient.color = Color.red;
            }
        }

        if (tmPatientLeft != null)
        {
            tmPatientLeft.text = patientsLeft.ToString();
        }

        if (tmTotalPatient != null)
        {
            tmTotalPatient.text = totalPatients.ToString();
        }
    }

    private void OnDestroy()
    {
        for(int i =0; i<activePatients.Count; ++i)
        {
            activePatients[i].OnPatientCured -= OnPatientCured;
            activePatients[i].OnPatientTimeOut -= OnPatientTimeOut;
        }
    }

    private IEnumerator WaitToStartSpawning()
    {
        yield return new WaitUntil(() => GameManager.Instance.currentGameState == EGameState.Playing);
        patientSpawnTimeElapsed = patientSpawningInterval;
        patientsLeft = totalPatients;
        StartSpawningPatients();
    }

    private void AttemptToCreatePatient()
    {
        if (activePatients.Count >= maxPatients)
            return;

        if (patientsSpawned >= totalPatients)
            return;

        var newPatient = Instantiate(patientPrefab, patientWaitPanel);

        newPatient.OnPatientTimeOut += OnPatientTimeOut;
        newPatient.OnPatientCured += OnPatientCured;
        newPatient.SetRequiredAntidote(antidoteRecipeBook.GetRandomAntidote());

        patientsSpawned++;

        activePatients.Add(newPatient);

        StartCoroutine(WaitBeforeActivatingPatient(0.5f, newPatient));
    }

    private IEnumerator WaitBeforeActivatingPatient(float delay, Patient patientToActivate)
    {
        yield return new WaitForSeconds(delay);

        patientToActivate.movetime = true;
    }

    private void OnPatientTimeOut(Patient patient)
    {
        deadPatientCount++;
        patientsLeft--;
        activePatients.Remove(patient);

        if (deadPatientCount == maxDeadPatients)
        {
            MaxDeathReached();
        }
    }

    private void OnPatientCured(Patient patient)
    {
        patientsLeft--;
        if (deadPatientCount < maxDeadPatients && patientsLeft <= 0)
        {
            FinishedAllVisits();
        }
    }

    private void MaxDeathReached()
    {
        StopSpawningPatients();
        GameManager.Instance.FailGame();
    }

    private void FinishedAllVisits()
    {
        StopSpawningPatients();
        GameManager.Instance.WinGame();
    }

    public void StartSpawningPatients()
    {
        if (createPatientCoroutine != null)
            StopCoroutine(createPatientCoroutine);

        createPatientCoroutine = StartCoroutine(PeriodicallySpawnPatient());
    }

    public void StopSpawningPatients()
    {
        if (createPatientCoroutine != null)
            StopCoroutine(createPatientCoroutine);
    }

    private IEnumerator PeriodicallySpawnPatient()
    {
        while(GameManager.Instance.currentGameState == EGameState.Playing)
        {
            patientSpawnTimeElapsed += Time.deltaTime;

            if (patientSpawnTimeElapsed >= patientSpawningInterval)
            {
                patientSpawnTimeElapsed = 0;
                AttemptToCreatePatient();
            }

            yield return null;
        }
    }

    public void FeedPatient(AntidoteRecipe antidote)
    {
        if (activePatients.Count < 1) return;

        var targetPatient = activePatients[0];

        if (targetPatient != null)
        {
            targetPatient.GiveAntidoteToPatient(antidote);
            activePatients.Remove(targetPatient);
        }
    }
}

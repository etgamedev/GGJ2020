using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientManager : MonoBehaviour
{
    public Patient patientPrefab;
    public AntidoteRecipeBook antidoteRecipeBook;
    public Transform patientWaitPanel;

    public float patientSpawningInterval;
    public int maxPatients;
    public int maxDeadPatients;
    public int totalPatients;

    public List<Patient> activePatients = new List<Patient>();

    private int deadPatientCount;
    private float patientSpawnTimeElapsed;
    private Coroutine createPatientCoroutine;

    private void Start()
    {
        StartCoroutine(WaitToStartSpawning());
    }

    private void OnDestroy()
    {
        for(int i =0; i<activePatients.Count; ++i)
        {
            activePatients[i].OnPatientTimeOut -= OnPatientTimeOut;
        }
    }

    private IEnumerator WaitToStartSpawning()
    {
        yield return new WaitUntil(() => GameManager.Instance.currentGameState == EGameState.Playing);
        patientSpawnTimeElapsed = patientSpawningInterval;
        StartSpawningPatients();
    }

    private void AttemptToCreatePatient()
    {
        if (activePatients.Count >= maxPatients)
            return;

        var newPatient = Instantiate(patientPrefab, patientWaitPanel);

        newPatient.OnPatientTimeOut += OnPatientTimeOut;
        newPatient.SetRequiredAntidote(antidoteRecipeBook.GetRandomAntidote());

        StartCoroutine(WaitBeforeActivatingPatient(0.5f, newPatient));
    }

    private IEnumerator WaitBeforeActivatingPatient(float delay, Patient patientToActivate)
    {
        yield return new WaitForSeconds(delay);

        patientToActivate.movetime = true;
    }

    private void OnPatientTimeOut(Patient patient)
    {
        Debug.Log("Patient dieded");
        deadPatientCount++;

        activePatients.Remove(patient);

        if (deadPatientCount == maxDeadPatients)
        {
            MaxDeathReached();
        }
    }

    private void MaxDeathReached()
    {
        Debug.Log("Max death reached");
        StopSpawningPatients();
        GameManager.Instance.FailGame();
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
}

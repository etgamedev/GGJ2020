using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatientManager : MonoBehaviour
{
    public Patient patientPrefab;
    public AntidoteRecipeBook antidoteRecipeBook;
    public float patientSpawningInterval;
    public int maxPatients;
    public List<Patient> activePatients = new List<Patient>();
    private float patientSpawnTimeElapsed;

    private void OnDestroy()
    {
        for(int i =0; i<activePatients.Count; ++i)
        {
            activePatients[i].OnPatientTimeOut -= OnPatientTimeOut;
        }
    }

    private void AttemptToCreatePatient()
    {
        if (activePatients.Count >= maxPatients)
            return;


    }

    private void OnPatientTimeOut(Patient patient)
    {

    }
}

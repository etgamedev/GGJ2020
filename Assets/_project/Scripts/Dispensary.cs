using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispensary : Countertop
{
    public PatientManager patientManager;

    private void Awake()
    {
        if (patientManager == null)
        {
            patientManager = FindObjectOfType<PatientManager>();
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        var antidote = collision.gameObject.GetComponent<AntidoteObject>();

        if (antidote != null)
        {
            DispenseAntidote(antidote);
            return;
        }

        var genericItem = collision.gameObject.GetComponent<GenericItem>();

        if (genericItem != null)
        {
            Destroy(genericItem.gameObject);
        }
    }

    protected override void OnTriggerExit(Collider collision)
    {
        
    }

    public override void Interact(GameObject instigator)
    {
        var player = instigator.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (player.heldItem != null)
            {
                var antidote = player.heldItem as AntidoteObject;

                if (antidote != null)
                {
                    Debug.Log("Holding is antidote");
                    DispenseAntidote(antidote);
                }
                else
                {
                    Debug.Log("Holding is not antidote");
                }
            }
        }
    }

    private void DispenseAntidote(AntidoteObject antidoteObject)
    {
        if (patientManager != null)
            patientManager.FeedPatient(antidoteObject.antidoteRecipe);

        Destroy(antidoteObject.gameObject);
    }
}

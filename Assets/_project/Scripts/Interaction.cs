using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
}

public class Interaction : MonoBehaviour
{
    public GameObject targetInteractionObj;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collided with " + other.name);
        var getObject = other.gameObject.GetComponent<IInteractable>();

        Debug.Log("Is interactable object?" + getObject != null);

        if (getObject != null)
        {
            Debug.Log("HAAAAAAAAAAAAAAAAAAAAAAA");
            targetInteractionObj = other.gameObject;

            Debug.Log(string.Format("TargetInteractionObject not null? {0}", targetInteractionObj != null));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetInteractionObj)
        {
            Debug.Log("Exitusss");
            targetInteractionObj = null;
        }
    }

    public void InteractWithObject()
    {
        Debug.Log("Try to interact with targeted object if applicable");
        if (targetInteractionObj != null)
        {
            Debug.Log("Targeted object is not null, call interact!");
            var getObject = targetInteractionObj.GetComponent<IInteractable>();
            getObject.Interact();
        }
    }
}

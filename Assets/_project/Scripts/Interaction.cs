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
        var getObject = other.gameObject.GetComponent<IInteractable>();

        if (getObject != null)
        {
            targetInteractionObj = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetInteractionObj)
        {
            targetInteractionObj = null;
        }
    }

    public IInteractable InteractWithObject()
    {
        if (targetInteractionObj != null)
        {
            var getObject = targetInteractionObj.GetComponent<IInteractable>();
            getObject.Interact();
			return getObject;
        }
		
		return null;
    }
}

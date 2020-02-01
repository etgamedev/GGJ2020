using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    int InteractPriority { get; }
    void Interact(GameObject instigator);
}

public class Interaction : MonoBehaviour
{
    public List<GameObject> interactableObjectsInFrame;

    private void LateUpdate()
    {
        interactableObjectsInFrame.Clear();
    }

    private void OnTriggerStay(Collider other)
    {
        var getObject = other.gameObject.GetComponent<IInteractable>();

        if (getObject != null)
        {
            if (!interactableObjectsInFrame.Contains(other.gameObject))
            {
                interactableObjectsInFrame.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    public void TriggerInteraction()
    {
        for (int i = 0; i < interactableObjectsInFrame.Count; ++i)
        {
            var getObject = interactableObjectsInFrame[i].GetComponent<IInteractable>();
            getObject.Interact(this.gameObject);
        }
    }
}

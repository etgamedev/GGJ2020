using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : MonoBehaviour, IInteractable
{
    public virtual void Interact()
    {
        Debug.Log("CB");
        Destroy(this.gameObject);
    }
}

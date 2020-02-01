using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countertop : MonoBehaviour, IInteractable
{
    public Transform ingredientPlacementlPosition;
    public GenericItem placedItem;

    public int interactPriority = 1;

    public int InteractPriority { get { return interactPriority; } }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        var genericItem = collision.gameObject.GetComponent<GenericItem>();

        PlaceItemOnCounter(genericItem);
    }

    protected virtual void OnCollisionExit(Collision collision)
    {
        var genericItem = collision.gameObject.GetComponent<GenericItem>();

        if (genericItem !=null)
        {
            if (genericItem == placedItem)
            {
                placedItem = null;
            }
        }
    }

    public virtual void Interact(GameObject instigator)
    {
        var player = instigator.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (player.heldItem != null && placedItem == null)
            {
                PlaceItemOnCounter(player.heldItem);
                player.heldItem = null;
            }
        }
    }

    protected virtual void PlaceItemOnCounter(GenericItem item)
    {
        if (item == null) return;

        if (placedItem != null)
            return;

        item.transform.parent = ingredientPlacementlPosition;
        item.transform.localPosition = Vector3.zero;
        item.ToggleColliderAndGravity(true);
        placedItem = item;
    }
}

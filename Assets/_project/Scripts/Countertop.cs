using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Countertop : MonoBehaviour, IInteractable
{
    public Transform ingredientPlacementlPosition;
    public GenericItem placedItem;

    public void Interact(GameObject instigator)
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

    protected void PlaceItemOnCounter(GenericItem item)
    {
        item.transform.parent = ingredientPlacementlPosition;
        item.transform.localPosition = Vector3.zero;
        placedItem = item;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDispenser : MonoBehaviour, IInteractable
{
    public GenericItem dispensingItemPrefab;
    public SpriteRenderer dispensingItemSpriteDisplay;

    private void OnValidate()
    {
        if (dispensingItemPrefab != null && dispensingItemSpriteDisplay != null)
        {
            dispensingItemSpriteDisplay.sprite = ((IngredientObject)dispensingItemPrefab).ingredientData.IngredientDisplaySprite;
        }
    }

    public void Interact(GameObject instigator)
    {
        Debug.Log("Interacted with item dispenser");
        var player = instigator.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (player.heldItem == null)
            {
                Debug.Log("Give player item boi");
                var dispensedItem = Instantiate(dispensingItemPrefab);
                dispensedItem.ToggleColliderAndGravity(false);
                player.HoldItem(dispensedItem);
            }
        }
    }
}

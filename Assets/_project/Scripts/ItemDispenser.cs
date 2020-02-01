using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDispenser : MonoBehaviour, IInteractable
{
    public GenericItem dispensingItemPrefab;
    public SpriteRenderer dispensingItemSpriteDisplay; 

    public int interactPriority = 1;

    public int InteractPriority { get { return interactPriority; } }

    private void OnValidate()
    {
        if (dispensingItemPrefab != null && dispensingItemSpriteDisplay != null)
        {
            dispensingItemSpriteDisplay.sprite = ((IngredientObject)dispensingItemPrefab).ingredientData.IngredientDisplaySprite;
        }
    }

    public void Interact(GameObject instigator)
    {
        var player = instigator.GetComponentInParent<PlayerController>();

        if (player != null)
        {
            if (player.heldItem == null)
            {
                var dispensedItem = Instantiate(dispensingItemPrefab);

                StartCoroutine(LetPlayerHoldItemNextframe(player, dispensedItem));
            }
        }
    }

    private IEnumerator LetPlayerHoldItemNextframe(PlayerController player, GenericItem item)
    {
        yield return null;

        if (player != null)
        {
            player.HoldItem(item);
        }
    }
}

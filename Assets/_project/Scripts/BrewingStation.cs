using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrewingStation : Countertop
{
    public List<IngredientData> ingredientDatas = new List<IngredientData>();
    public int maxIngredient;
    public float brewTime;
    public float brewElapsedTime;

    public GameObject uiBrewingTimeBar;

    public AntidoteRecipe currentRecipe;
    public AntidoteObject antidotePrefab;

    public bool brewing;

    private Coroutine brewingCoroutine;

    public float BrewingProgress
    {
        get
        {
            return brewElapsedTime / brewTime;
        }
    }

    public override void Interact(GameObject instigator)
    {
        var player = instigator.GetComponentInParent<PlayerController>();
        if (player == null) return;
        if (player.heldItem == null)
        {
            if (placedItem != null)
            {
                base.Interact(instigator);
            }
            else
            {
                ClearAllIngredients();
            }
        }
        else
        {
            AddIngredient(player.heldItem);
        }
    }

    private void ClearAllIngredients()
    {
        if (brewing)
            brewing = false;

        ingredientDatas.Clear();
    }

    private void AddIngredient(GenericItem item)
    {
        if (item == null) return;

        if (ingredientDatas.Count != maxIngredient)
        {
            var ingredient = item as IngredientObject;

            if (ingredient != null)
            {
                ingredientDatas.Add(ingredient.ingredientData);

                Destroy(item.gameObject);

                if (ingredientDatas.Count == maxIngredient)
                {
                    StartBrewing();
                }
            }
        }
    }

    private void StartBrewing()
    {
        if (CheckIfIngredientsMatchesRecipe() && !brewing)
        {
            brewingCoroutine = StartCoroutine(BrewAntidote());

            if (uiBrewingTimeBar != null)
            {
                uiBrewingTimeBar.SetActive(true);
            }
        }
    }

    private IEnumerator BrewAntidote()
    {
        brewing = true;
        brewElapsedTime = 0;
        while(brewElapsedTime < brewTime)
        {
            brewElapsedTime += Time.deltaTime;
            yield return null;
        }

        StopBrewing();
        CreateAntidote();
    }

    private bool CheckIfIngredientsMatchesRecipe()
    {
        if (currentRecipe != null)
        {
            var match = currentRecipe.CheckIfMatchesIngredients(ingredientDatas);
            Debug.Log("Ingredient matches? " + match);
            return match;
        }

        Debug.Log("Ingredient does not match");
        return false;
    }

    private void StopBrewing()
    {
        if (brewingCoroutine != null)
        {
            StopCoroutine(brewingCoroutine);
        }

        brewing = false;

        if (uiBrewingTimeBar != null)
            uiBrewingTimeBar.SetActive(false);
    }

    private void CreateAntidote()
    {
        var antidote = Instantiate(antidotePrefab);

        antidote.ToggleColliderAndGravity(true);
        PlaceItemOnCounter(antidote);
    }
}

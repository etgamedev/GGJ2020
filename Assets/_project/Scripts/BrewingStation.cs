using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrewingStation : Countertop, ITaskProgress
{
    public List<IngredientData> ingredientDatas = new List<IngredientData>();
    public int maxIngredient;
    public float brewTime;
    [HideInInspector] public float brewElapsedTime;

    public GameObject uiBrewingTimeBar;
    public Image ingredient1;
    public Image ingredient2;
    public Sprite defaultIngredientSprite;
    public GameObject ingredientDisplayPanel;

    public AntidoteRecipeBook antidoteRecipeBook;

    [HideInInspector] public bool brewing;

    private AntidoteRecipeData selectedRecipeData;
    private Coroutine brewingCoroutine;

    public float Progress
    {
        get
        {
            return brewElapsedTime / brewTime;
        }
    }

    private void Update()
    {
        uiBrewingTimeBar.SetActive(brewing);

        if (ingredientDatas.Count > 0)
        {
            ingredient1.sprite = ingredientDatas[0].IngredientDisplaySprite;
            ingredient1.gameObject.SetActive(true);
            ingredientDisplayPanel.gameObject.SetActive(true);

            if (ingredientDatas.Count > 1)
            {
                ingredient2.sprite = ingredientDatas[1].IngredientDisplaySprite;

                ingredient2.gameObject.SetActive(true);
            }
            else
            {
                ingredient2.sprite = defaultIngredientSprite;
                ingredient2.gameObject.SetActive(false);
            }
        }
        else
        {
            ingredient1.sprite = defaultIngredientSprite;
            ingredient2.sprite = defaultIngredientSprite;
            ingredientDisplayPanel.SetActive(false);
        }
    }

    protected override void OnTriggerEnter(Collider collision)
    {
        var genericItem = collision.gameObject.GetComponent<GenericItem>();

        if (genericItem != null)
        {
            if (placedItem == null)
            {
                AddIngredient(genericItem);
            }
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
        Debug.Log("Clearing ingredients in brewing station");
        ingredientDatas.Clear();
    }

    private void AddIngredient(GenericItem item)
    {
        if (item == null)
        {
            Debug.Log("cannot add null item to brewing station as ingredient");
            return;
        }

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
    AudioSource brewingAudioSource;

    private void StartBrewing()
    {
        if (CheckIfIngredientsMatchesRecipe() && !brewing)
        {
            Debug.Log("START BREWING");
            brewingCoroutine = StartCoroutine(BrewAntidote());

            if (uiBrewingTimeBar != null)
            {
                uiBrewingTimeBar.SetActive(true);
            }

            brewingAudioSource = SoundManager.Instance.PlaySFXLoop("SFX_Brewing");
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

        SoundManager.Instance.PlaySFX("SFX_BrewingDone");

        CreateAntidote();
    }

    private bool CheckIfIngredientsMatchesRecipe()
    {
        if (antidoteRecipeBook != null)
        {
            selectedRecipeData = antidoteRecipeBook.GetMatchingRecipe(ingredientDatas);           
            
            return selectedRecipeData != null;
        }

        return false;
    }

    private void StopBrewing()
    {
        if (brewingCoroutine != null)
        {
            StopCoroutine(brewingCoroutine);
        }

        brewing = false;

        if (brewingAudioSource != null)
            brewingAudioSource.Stop();

        if (uiBrewingTimeBar != null)
            uiBrewingTimeBar.SetActive(false);
    }

    private void CreateAntidote()
    {
        if (selectedRecipeData == null) return;

        var antidote = Instantiate(selectedRecipeData.antidotePrefab);
        antidote.ToggleColliderAndGravity(true);
        PlaceItemOnCounter(antidote);

        ingredientDatas.Clear();
    }
}

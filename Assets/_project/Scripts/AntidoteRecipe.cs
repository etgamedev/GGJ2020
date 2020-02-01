using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AntidoteRecipe : ScriptableObject
{
    public string antidoteName;
    public List<IngredientData> ingredients;
    private Dictionary<IngredientData, int> ingredientDict = new Dictionary<IngredientData, int>();

    private void Awake()
    {
        
    }

    private void InitRecipe()
    {
        //!Build ingredient dictionary
        ParseIngredientListIntoDict(this.ingredients, out this.ingredientDict);
    }

    public bool CheckIfMatchesIngredients(List<IngredientData> ingredients)
    {
        if (this.ingredients.Count > ingredients.Count)
        {
            return false;
        }

        InitRecipe();

        ParseIngredientListIntoDict(ingredients, out var checkedIngredientDict);

        foreach(var item in checkedIngredientDict)
        {
            if (!ingredientDict.ContainsKey(item.Key))
            {
                return false;
            }

            if (ingredientDict[item.Key] != item.Value)
            {
                return false;
            }
        }
        
        return true;
    }

    private void ParseIngredientListIntoDict(List<IngredientData> ingredients, out Dictionary<IngredientData, int> ingredientDict)
    {
        ingredientDict = new Dictionary<IngredientData, int>();
        if (ingredients == null)
        {
            return;
        }

        for (int i = 0; i < ingredients.Count; ++i)
        {
            if (ingredientDict.ContainsKey(ingredients[i]))
            {
                ingredientDict[ingredients[i]]++;
            }
            else
            {
                ingredientDict.Add(ingredients[i], 1);
            }
        }
    }
}

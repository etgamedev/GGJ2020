using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AntidoteRecipeData
{
    public string antidoteName;
    public AntidoteRecipe antidoteRecipe;
    public AntidoteObject antidotePrefab;
}

[CreateAssetMenu]
public class AntidoteRecipeBook : ScriptableObject
{
    public List<AntidoteRecipeData> antidoteRecipeDatas = new List<AntidoteRecipeData>();

    public AntidoteRecipeData GetMatchingRecipe(List<IngredientData> ingredients)
    {
        for (int i = 0; i < antidoteRecipeDatas.Count; ++i)
        {
            if (antidoteRecipeDatas[i].antidoteRecipe.CheckIfMatchesIngredients(ingredients))
                return antidoteRecipeDatas[i];
        }

        return null;
    }

    public AntidoteRecipeData GetRandomAntidote()
    {
        var index = Random.Range(0, antidoteRecipeDatas.Count);

        return antidoteRecipeDatas[index];
    }
}

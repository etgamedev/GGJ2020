using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EIngredientTypes
{
	Alpha,
	Beta,
	Charlie
}

[CreateAssetMenu]
public class IngredientData : ScriptableObject
{
	[SerializeField] private Sprite ingredientDisplaySprite;
	[SerializeField] private string ingredientDisplayName;
	[SerializeField] private EIngredientTypes ingredientType;

	public Sprite IngredientDisplaySprite { get { return ingredientDisplaySprite; } }

	public string IngredientDisplayName { get { return ingredientDisplayName; } }

	public EIngredientTypes IngredientType { get { return ingredientType; } }
}

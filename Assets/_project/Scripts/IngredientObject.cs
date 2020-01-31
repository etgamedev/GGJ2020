using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientObject : GenericItem
{
	public Collider ingredientCollider;
	public Rigidbody rb;
	
	private void Awake()
	{
		if (ingredientCollider == null)
		{
			ingredientCollider = GetComponent<Collider>();
		}
		if (rb == null)
		{
			rb = GetComponent<Rigidbody>();
		}
	}
	
	public override void Throw(Vector3 direction)
	{
		ingredientCollider.enabled = true;
		rb.useGravity = true;
		rb.AddForce(direction, ForceMode.Impulse);
	}
	
    public override void Interact()
    {
		ingredientCollider.enabled = false;
		rb.useGravity = false;
        //Destroy(this.gameObject);
    }
}

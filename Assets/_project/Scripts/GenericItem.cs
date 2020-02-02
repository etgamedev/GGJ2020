using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
	void Throw(Vector3 direction);
}

public class GenericItem : MonoBehaviour, IInteractable, IThrowable
{
	public Collider ingredientCollider;
	public Rigidbody rb;
	public int interactPriority = 100000;

	public int InteractPriority { get { return interactPriority; } }

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

	public virtual void Interact(GameObject instigator)
	{
		var player = instigator.GetComponentInParent<PlayerController>();

		LetPlayerHoldItem(player);
	}

	protected virtual bool LetPlayerHoldItem(PlayerController player)
	{
		if (player != null)
		{
			if (player.heldItem == null)
			{
				ToggleColliderAndGravity(false);
				player.HoldItem(this);

				return true;
			}

			return false;
		}

		return false;
	}

	public virtual void Throw(Vector3 throwDirection)
	{
		ToggleColliderAndGravity(true);
		SoundManager.Instance.PlaySFX("SFX_Throwing");
		rb.AddForce(throwDirection, ForceMode.Impulse);
	}

	public void ToggleColliderAndGravity(bool enable)
	{
		ingredientCollider.enabled = enable;
		rb.useGravity = enable;
	}
}

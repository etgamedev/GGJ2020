using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
	public float throwItemForce;
	public Transform holdItemPosition;
    public Movement playerMovement;
    public Interaction playerInteraction;
	public GenericItem heldItem;

    private void Awake()
    {
        if (playerMovement == null)
            playerMovement = GetComponent<Movement>();

        if (playerInteraction == null)
            playerInteraction = GetComponent<Interaction>();
    }

    private void Update()
    {
        if (GameManager.Instance.currentGameState != EGameState.Playing) return;

        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            if (playerMovement.CanMove)
            {
                playerMovement.Move();
                //m_animator.SetBool("IsMoving", true);
            }
        }
        else if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
        {
            //m_animator.SetBool("IsMoving", false);
        }

        if (Input.GetButtonDown("Interact"))
        {
            playerInteraction.TriggerInteraction();
        }
		else if(Input.GetButtonDown("Throw"))
		{
			ThrowHeldItem();
		}
    }

    private void OnDestroy()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.PlayerDestroyed();
        }
    }

    public void HoldItem(GenericItem itemToHold)
	{
        heldItem = itemToHold;

        if (heldItem != null)
		{
            itemToHold.ToggleColliderAndGravity(false);
            heldItem.transform.parent = holdItemPosition;
			heldItem.transform.localPosition = Vector3.zero;

            SoundManager.Instance.PlaySFX("SFX_Interact3");
		}
	}
	
	private void ThrowHeldItem()
	{
		if (heldItem != null)
		{
			heldItem.transform.parent = null;
			heldItem.Throw(transform.forward * throwItemForce);
			heldItem = null;
		}
	}
}

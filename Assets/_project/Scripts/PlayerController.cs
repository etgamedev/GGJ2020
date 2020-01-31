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
            var interacted = playerInteraction.InteractWithObject();
			
			if (interacted != null)
			{
				Debug.Log("Bleep");
				heldItem = interacted as GenericItem;
				
				HoldItem();
			}
        }
		else if(Input.GetButtonDown("Throw"))
		{
			ThrowHeldItem();
		}
    }
	
	private void HoldItem()
	{
		if (heldItem != null)
		{
			Debug.Log("BleepBleep");
			heldItem.transform.parent = holdItemPosition;
			heldItem.transform.localPosition = Vector3.zero;
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

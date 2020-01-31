using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    public Movement playerMovement;
    public Interaction playerInteraction;

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
            playerInteraction.InteractWithObject();
        }
    }
}

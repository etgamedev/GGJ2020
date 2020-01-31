using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MoveSpeed;
    public float JumpHeight;
    public float SecondJumpHeight;
    public LayerMask movementBlockingMask;
    public CharacterController characterController;
    public bool CanMove;
    public bool IsInAir;
    public bool IsSecondJump;
    public bool MoveBasedOnCamera;
    public AudioSource JumpAudioSource;
    public AudioSource DoubleJumpAudioSource;
    
    private Rigidbody rb;

    private void Start()
    {
        if (rb == null)
        {
            rb = this.GetComponent<Rigidbody>();
        }
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    public void Update()
    {
    }

    public void Move()
    {
        //8 Direction Movement
        float xAxis = Input.GetAxisRaw("Horizontal");
        float zAxis = Input.GetAxisRaw("Vertical");
        //Debug.Log("xAxis : " + xAxis + "zAxis : " + zAxis);
        Vector3 targetLocation = new Vector3(xAxis, 0.0f, zAxis);

        if (MoveBasedOnCamera)
        {
            targetLocation = Camera.main.transform.TransformDirection(targetLocation);
        }

        targetLocation.y = 0.0f;
        Vector3 finalLocation = targetLocation.normalized * Time.deltaTime * MoveSpeed;
        characterController.Move(finalLocation);
        //RaycastHit hit;
        //if (!Physics.Raycast(this.transform.position + this.transform.up / 2, transform.forward, out hit, 1.0f, movementBlockingMask))
        //{
        //    if (hit.transform == null)
        //    {
        //        this.transform.Translate(finalLocation, Space.World);
        //    }
        //    else
        //    {
        //        if (hit.transform.CompareTag("Wall") || hit.transform.CompareTag("Block"))
        //        {
        //            this.transform.Translate(finalLocation, Space.World);
        //        }
        //    }
        //}

        if (finalLocation.magnitude > 0.001f)
        {
            this.transform.rotation = Quaternion.LookRotation(targetLocation.normalized);
        }
    }

    public void Jump()
    {
        if (IsInAir)
        {
            if (!IsSecondJump)
            {
                if (DoubleJumpAudioSource != null)
                {
                    DoubleJumpAudioSource.Play();
                }
                rb.AddForce(Vector3.up * SecondJumpHeight, ForceMode.VelocityChange);
                IsSecondJump = true;
            }
            else
                return;
        }
        else
        {
            if (JumpAudioSource != null)
            {
                JumpAudioSource.Play();
            }
            rb.AddForce(Vector3.up * JumpHeight, ForceMode.VelocityChange);
            IsInAir = true;
        }
    }

    //Called by child object when it collides with a ground
    public void OnLandingTrigger()
    {
        IsInAir = false;
        IsSecondJump = false;
    }

    public void OnLeavingTrigger()
    {
        IsInAir = true;
    }
}

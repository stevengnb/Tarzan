using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("Climbing")]
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private LayerMask climbable;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float maxTime;
    private float climbTimer;
    private bool climbing;
    [SerializeField] private float detectLength;
    [SerializeField] private float castRadius;
    [SerializeField] private float maxLookAngle;
    private float lookAngle;
    private RaycastHit wallHit;
    private bool frontWall;

    private void Update()
    {
        CheckClimb();
        ClimbState();
        if(climbing)
        {
            DoClimb();
        }
    }

    private void ClimbState()
    {
        if(frontWall && Input.GetKey(KeyCode.Space) && lookAngle < maxLookAngle)
        {
            if (!climbing && climbTimer > 0)
            {
                ClimbStarts();
            }

            if(climbTimer > 0)
            {
                climbTimer -= Time.deltaTime;
            }

            if(climbTimer < 0)
            {
                ClimbEnds();
            }
        } else
        {
            if(climbing)
            {
                ClimbEnds();
            }
        }
    }

    private void CheckClimb()
    {
        frontWall = Physics.SphereCast(transform.position, castRadius, orientation.forward, out wallHit, detectLength, climbable);
        lookAngle = Vector3.Angle(orientation.forward, -wallHit.normal);

        if(movement.isGrounded)
        {
            climbTimer = maxTime;
        }
    }

    private void ClimbStarts()
    {
        climbing = true;
    }

    private void DoClimb()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void ClimbEnds()
    {
        climbing = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyMovement : MonoBehaviour
{
    public PlayerControl playerControl;
    // public Collider2D collider2d;

    public Rigidbody2D rigidbody2d;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        BabyFollow();
        UpdateConstraints();
    }

    /// <summary>
    /// Follows player while being attached to GrabPoint
    /// </summary>
    private void BabyFollow()
    {
        // collider2d.isTrigger = false;

        if (playerControl.isKidGrabbed)
        {
            transform.position = playerControl.grabPoint.position;
            transform.rotation = playerControl.grabPoint.rotation;

            SetAnimations();
        }
    }

    private void SetAnimations()
    {
        animator.SetFloat(AnimationParameters.SPEED, playerControl.playerRB.velocity.magnitude);

        string triggerName = playerControl.GetAnimationTrigger();

        if (triggerName != null)
            animator.SetTrigger(triggerName);
    }

    /// <summary>
    /// Updates kidRB constraints
    /// </summary>
    private void UpdateConstraints()
    {
        if (playerControl.isKidGrabbed)
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        else
            rigidbody2d.constraints = RigidbodyConstraints2D.FreezeAll;
    }
}

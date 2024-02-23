using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Jump(bool isJumping)
    {
        animator.SetBool("Jump", isJumping);
    }

    public void Crouch(bool isCrouching)
    {
        //animator.SetBool("Crouch", isCrouching);
    }

    public void SetMovement(float speed)
    {
        animator.SetFloat("Speed", speed);
    }
}
using UnityEngine;

[RequireComponent (typeof(VirtualController))]
[RequireComponent(typeof(AnimationController))]
public class BodyController : MonoBehaviour
{
    VirtualController virtualController;
    AnimationController animationController;

    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
        virtualController = GetComponent<VirtualController>();
    }

    private void Update()
    {

        if (virtualController.MoveAxisForward != 0 || virtualController.MoveAxisRight != 0)
        {
            animationController.SetMovement(Mathf.Abs(virtualController.MoveAxisRight + virtualController.MoveAxisForward));
        }
        else
        {
            animationController.SetMovement(0);
        }

        // Jumping input
        if (virtualController.Jump)
        {
            animationController.Jump(true);
        }

        animationController.Crouch(virtualController.Crouch);
    }
}
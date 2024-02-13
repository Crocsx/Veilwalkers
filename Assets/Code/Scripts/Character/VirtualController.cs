
using UnityEngine;

public class VirtualController : MonoBehaviour
{
    public float MoveAxisForward { get; private  set; }
    public float MoveAxisRight { get; private set; }
    public float LookAxisUp { get; private set; }
    public float LookAxisRight { get; private set; }
    public bool Jump { get; private  set; }
    public bool Crouch { get; private  set; }

    public void UpdateInputs(PlayerCharacterInputs playerInputs)
    {
        MoveAxisForward = playerInputs.MoveAxisForward;
        MoveAxisRight = playerInputs.MoveAxisRight;
        LookAxisUp = playerInputs.LookAxisUp;
        LookAxisRight = playerInputs.LookAxisRight;
        Jump = playerInputs.Jump;
        Crouch = playerInputs.Crouch;
    }
}
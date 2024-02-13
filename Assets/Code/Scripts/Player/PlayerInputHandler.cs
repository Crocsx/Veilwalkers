using Unity.Netcode;
using UnityEngine;

public struct PlayerCharacterInputs
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public float LookAxisUp;
    public float LookAxisRight;
    public bool Jump;
    public bool Crouch;
}

[RequireComponent(typeof(VirtualController))]
public class PlayerInputHandler : NetworkBehaviour
{
    private InputManager inputManager;
    private VirtualController virtualController;

    private void Awake()
    {
        inputManager = InputManager.Instance;
        virtualController = GetComponent<VirtualController>();
        GetComponent<Transform>().name = "Myobject" + NetworkManager.Singleton.ConnectedClients.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("isOwner : " + GetComponent<Transform>().name + " " + IsOwner);
        if (!IsOwner) return;
        virtualController.UpdateInputs(GetInputs());
    }

    public PlayerCharacterInputs GetInputs()
    {
        return new PlayerCharacterInputs
        {
            MoveAxisForward = inputManager.GetMovement().x,
            MoveAxisRight = inputManager.GetMovement().y,
            LookAxisRight = inputManager.GetLook().x,
            LookAxisUp = inputManager.GetLook().y,
            Jump = inputManager.IsJumpingThisFrame(),
            Crouch = inputManager.IsCrouching()
        };
    }
}

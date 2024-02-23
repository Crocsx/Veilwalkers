using Unity.Netcode;
using UnityEngine;

public struct PlayerCharacterInputs : INetworkSerializable
{
    public float MoveAxisForward;
    public float MoveAxisRight;
    public float LookAxisUp;
    public float LookAxisRight;
    public bool Jump;
    public bool Crouch;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref MoveAxisForward);
        serializer.SerializeValue(ref MoveAxisRight);
        serializer.SerializeValue(ref LookAxisUp);
        serializer.SerializeValue(ref LookAxisRight);
        serializer.SerializeValue(ref Jump);
        serializer.SerializeValue(ref Crouch);
    }
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        PlayerCharacterInputs inputs = GetInputs();
        virtualController.UpdateInputs(inputs);
        SendInputToServerServerRpc(inputs);
    }

    [ServerRpc]
    public void SendInputToServerServerRpc(PlayerCharacterInputs inputs)
    {
        virtualController.UpdateInputs(inputs);
    }
    
    public void ProcessInputsOnServer(PlayerCharacterInputs inputs)
    {
        virtualController.UpdateInputs(inputs);
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

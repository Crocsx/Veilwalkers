using Unity.Netcode;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : NetworkBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            GameManager.Instance.RegisterPlayerCamera(this);
            ActivateCamera();
        }
        else
        {
            DeactivateCamera();
        }
    }

    public void ActivateCamera()
    {
        virtualCamera.Priority = 10; // Ensure higher priority than default
    }

    public void DeactivateCamera()
    {
        virtualCamera.Priority = 0; // Lower priority to deactivate
    }
}
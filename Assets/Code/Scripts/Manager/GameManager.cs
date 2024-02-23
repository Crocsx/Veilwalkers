
using UnityEngine;
using System.Collections.Generic;
using KinematicCharacterController;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<PlayerCameraController> playerCameras = new List<PlayerCameraController>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void RegisterPlayerCamera(PlayerCameraController camera)
    {
        playerCameras.Add(camera);

        if (playerCameras.Count == 1)
        {
            camera.ActivateCamera();
        }
    }

    public void UnregisterPlayerCamera(PlayerCameraController camera)
    {
        playerCameras.Remove(camera);

        if (playerCameras.Count > 0)
        {
            playerCameras[0].ActivateCamera();
        }
    }

    private void FixedUpdate()
    {
        KinematicCharacterSystem.Simulate(Time.fixedDeltaTime, KinematicCharacterSystem.CharacterMotors, KinematicCharacterSystem.PhysicsMovers);
    }
}
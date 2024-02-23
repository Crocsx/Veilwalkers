using UnityEngine;
using Unity.Netcode;
using KinematicCharacterController;

public class NetworkKinematicCharacterMotor : NetworkBehaviour
{
    private KinematicCharacterMotor motor;

    public NetworkVariable<Vector3> NetworkPosition = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> NetworkRotation = new NetworkVariable<Quaternion>();

    public float rotationTreshold = 5f;
    private float rotationLerpRate = 10f;
    private Quaternion currentTargetRotation;


    private Vector3 currentTargetPosition;
    public float positionTreshold = 0.1f; 
    private float positionLerpRate = 10f;

    private void Awake()
    {
        motor = GetComponent<KinematicCharacterMotor>();
    }

    private void Update()
    {
        if (IsClient)
        {
            KinematicCharacterMotorState motorState = motor.GetState();
            if (!IsOwner || (IsOwner && Vector3.Distance(motorState.Position, currentTargetPosition) > positionTreshold))
            {
                motor.SetPosition(Vector3.Lerp(motorState.Position, currentTargetPosition, Time.deltaTime * positionLerpRate));
            }
            if (!IsOwner || (IsOwner && Quaternion.Angle(motorState.Rotation, currentTargetRotation) > rotationTreshold))
            {
                motor.SetRotation(Quaternion.Lerp(motorState.Rotation, currentTargetRotation, Time.deltaTime * rotationLerpRate));
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            UpdateNetworkState();
        }
    }

    public override void OnNetworkSpawn()
    {
        NetworkPosition.OnValueChanged += OnPositionChanged;
        NetworkRotation.OnValueChanged += OnRotationChanged;
    }

    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        currentTargetPosition = newPosition;
        KinematicCharacterMotorState motorState = motor.GetState();
        // Directly set the position if the discrepancy is large to avoid visual snapping
        if (Vector3.Distance(motorState.Position, newPosition) > 1f) // This threshold can be adjusted
        {
            motor.SetPosition(newPosition);
        }
    }

    private void OnRotationChanged(Quaternion oldRotation, Quaternion newRotation)
    {
        currentTargetRotation = newRotation;
        KinematicCharacterMotorState motorState = motor.GetState();
        // Directly set the rotation if the discrepancy is large to avoid visual snapping
        if (Quaternion.Angle(motorState.Rotation, newRotation) > 10f) // This threshold can be adjusted
        {
            motor.SetRotation(newRotation);
        }
    }

    private void UpdateNetworkState()
    {
        KinematicCharacterMotorState motorState = motor.GetState();
        NetworkPosition.Value = motorState.Position;
        NetworkRotation.Value = motorState.Rotation;
    }

    public override void OnDestroy()
    {
        NetworkPosition.OnValueChanged -= OnPositionChanged;
        NetworkRotation.OnValueChanged -= OnRotationChanged;
        base.OnDestroy();
    }
}
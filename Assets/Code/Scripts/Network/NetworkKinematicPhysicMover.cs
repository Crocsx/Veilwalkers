using KinematicCharacterController;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkKinematicPhysicMover : NetworkBehaviour
{
    private PhysicsMover mover;

    public NetworkVariable<PhysicsMoverState> NetworkState = new NetworkVariable<PhysicsMoverState>();

    public float rotationTreshold = 5f;
    private float rotationLerpRate = 10f;
    public float positionTreshold = 0.1f;
    private float positionLerpRate = 10f;

    private PhysicsMoverState currentState;

    private void Awake()
    {
        mover = GetComponent<PhysicsMover>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkState.Value = mover.GetState();
        }
        NetworkState.OnValueChanged += OnStateChanged;
    }

    private void Update()
    {
        if (IsClient)
        {
            InterpolateStateToCurrentNetworkState();
        }
    }

    private void FixedUpdate()
    {
        if (IsServer)
        {
            NetworkState.Value = mover.GetState();
        }
    }

    private void InterpolateStateToCurrentNetworkState()
    {
        if (IsClient)
        {
            PhysicsMoverState moverState = mover.GetState();
            if (!IsOwner || (IsOwner && Vector3.Distance(moverState.Position, currentState.Position) > positionTreshold))
            {
                mover.SetPosition(Vector3.Lerp(moverState.Position, currentState.Position, Time.deltaTime * positionLerpRate));
            }
            if (!IsOwner || (IsOwner && Quaternion.Angle(moverState.Rotation, currentState.Rotation) > rotationTreshold))
            {
                mover.SetRotation(Quaternion.Lerp(moverState.Rotation, currentState.Rotation, Time.deltaTime * rotationLerpRate));
            }
        }
    }

    private void OnStateChanged(PhysicsMoverState oldState, PhysicsMoverState newState)
    {
        float positionDifference = Vector3.Distance(mover.GetState().Position, newState.Position);
        float rotationDifference = Quaternion.Angle(mover.GetState().Rotation, newState.Rotation);

        if (positionDifference > positionTreshold || rotationDifference > rotationTreshold)
        {
            mover.ApplyState(newState);
        }
        currentState = newState;
    }



    public override void OnDestroy()
    {
        NetworkState.OnValueChanged -= OnStateChanged;
        base.OnDestroy();
    }
}

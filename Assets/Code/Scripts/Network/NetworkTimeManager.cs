using Unity.Netcode;
using UnityEngine;

public class NetworkTimeManager : NetworkBehaviour
{
    public static NetworkTimeManager Instance { get; private set; }

    private NetworkVariable<float> serverTime = new NetworkVariable<float>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetSynchronizedGameTime()
    {
        return serverTime.Value;
    }

    [ServerRpc]
    public void UpdateServerTimeServerRpc(float time)
    {
        serverTime.Value = time;
    }

    private void Update()
    {
        if (IsServer)
        {
            UpdateServerTimeServerRpc(Time.time);
        }
    }
}
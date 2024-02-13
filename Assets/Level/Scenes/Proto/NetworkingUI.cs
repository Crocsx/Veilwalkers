using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkingUI : MonoBehaviour
{
    [SerializeField] private Button host;
    [SerializeField] private Button join;
    // Start is called before the first frame update
    void Start()
    {
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });

        join.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }

}

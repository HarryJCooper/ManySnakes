using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn, clientBtn, hostBtn;
    
    void Awake()
    {
        serverBtn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        clientBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        hostBtn.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
    }
}

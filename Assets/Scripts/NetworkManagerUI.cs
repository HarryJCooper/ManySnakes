using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private Button serverBtn, clientBtn, hostBtn;
    [SerializeField] public TMP_Text joinCodeDisplay;
    [SerializeField] public TMP_Text joinCodeInput;
    
    void Awake()
    {
        serverBtn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        clientBtn.onClick.AddListener(() => JoinRelay(joinCodeInput.text));
        hostBtn.onClick.AddListener(() => CreateRelay());
    }

    private async void Start(){
        await UnityServices.InitializeAsync();

        AuthenticationService.Instance.SignedIn += () => {
            Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
        };
        // if (!AuthenticationService.Instance.IsSignedIn) await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    public async void CreateRelay(){
        try {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(3);

            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            Debug.Log("Join code: " + joinCode);

            RelayServerData relayServerData = new RelayServerData(allocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartHost();

            joinCodeDisplay.text = joinCode;
        } catch (RelayServiceException e) {
            Debug.Log("Error creating relay: " + e.Message);
        }
    }

    public async void JoinRelay(string joinCode){
        try {
            joinCode = joinCodeInput.text.Substring(0,6);
            Debug.Log("Joining relay with code: " + joinCode);
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

            RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

            NetworkManager.Singleton.StartClient();
        } catch (RelayServiceException e) {
            Debug.Log("Error joining relay: " + e.Message);
        }
    }
}

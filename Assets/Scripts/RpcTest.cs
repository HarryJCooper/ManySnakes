using UnityEngine;
using Unity.Netcode;

public class RpcTest : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        if (!IsServer && IsOwner)
        {
            TestServerRpc(0, NetworkObjectId);
        }
    }

    [ClientRpc]
    void TestClientRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"ClientRpc: {value} from {sourceNetworkObjectId}");
        if (IsOwner) //Only send an RPC to the server on the client that owns the NetworkObject that owns this NetworkBehaviour instance
        {
            TestServerRpc(value + 1, sourceNetworkObjectId);
        }
    }

    [ServerRpc]
    void TestServerRpc(int value, ulong sourceNetworkObjectId)
    {
        Debug.Log($"ServerRpc: {value} from {sourceNetworkObjectId}");
        TestClientRpc(value + 1, sourceNetworkObjectId);
    }
}

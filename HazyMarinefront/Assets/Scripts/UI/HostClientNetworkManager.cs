using UnityEngine;
using MLAPI;
using TMPro;
using System.Text;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using MLAPI.Connection;

public class HostClientNetworkManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField roomcodeInputField;
    [SerializeField] private GameObject roomcodeEntryUI;
    [SerializeField] private GameObject leaveButton;
    [SerializeField] private GameObject attackBtn;
    [SerializeField] private GameObject spawnButton;

    [SerializeField] private NetworkVariable<int> ClientsNum = new NetworkVariable<int>(0);


    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;

        GameObject.Find("EventSystem").GetComponent<MoveBtnEventListener>().MoveControllUICanvas.SetActive(false);

        leaveButton.SetActive(false);
        attackBtn.SetActive(false);
        spawnButton.SetActive(false);

        ClientsNum.OnValueChanged += ClientsNumValueChanged;
    }

    private void OnDestroy()
    {
        // Prevent error in the editor
        if (NetworkManager.Singleton == null) { return; }

        NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;
        ClientsNum.OnValueChanged -= ClientsNumValueChanged;
    }

    public void Host()
    {
        // Hook up password approval check
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.StartHost(new Vector3(-2f, 0f, 0f), Quaternion.Euler(0f, 135f, 0f));

        GetTurnManager().SetGameState(0);
    }

    public void Client()
    {
        // Set password ready to send to the server to validate
        NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(roomcodeInputField.text);
        NetworkManager.Singleton.StartClient();
    }

    public void Leave()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            NetworkManager.Singleton.StopHost();
            NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            NetworkManager.Singleton.StopClient();
        }

        roomcodeEntryUI.SetActive(true);
        leaveButton.SetActive(false);
        spawnButton.SetActive(false);
    }

    private void HandleServerStarted()
    {
        // Temporary workaround to treat host as client
        if (NetworkManager.Singleton.IsHost)
        {
            ClientsNum.Value = 0;

            Debug.Log("server start");
            HandleClientConnected(NetworkManager.Singleton.ServerClientId);
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        // Are we the client that is connecting?
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            roomcodeEntryUI.SetActive(false);
            leaveButton.SetActive(true);

            attackBtn.SetActive(true);
        }
        ClientsNum.Value++;
    }

    private void HandleClientDisconnect(ulong clientId)
    {
        // Are we the client that is disconnecting?
        if (clientId == NetworkManager.Singleton.LocalClientId)
        {
            roomcodeEntryUI.SetActive(true);
            leaveButton.SetActive(false);
        }
    }

    private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
    {
        string password = Encoding.ASCII.GetString(connectionData);

        bool approveConnection = password == roomcodeInputField.text;

        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;

        switch (NetworkManager.Singleton.ConnectedClients.Count)
        {
            case 1:
                spawnPos = new Vector3(6, 3, -2);
                spawnRot = Quaternion.Euler(0f, 180f, 0f);
                break;
            case 2:
                spawnPos = new Vector3(6, 3, 0);
                spawnRot = Quaternion.Euler(0f, 225, 0f);
                break;
        }
        callback(true, null, approveConnection, spawnPos, spawnRot);
    }

    [ServerRpc]
    private void CheckClientsNumServerRpc(int num)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            if (num >= 2)
            {
                spawnButton.SetActive(true);

                // 일단 자동으로 상대 들어오면 ready 상태로
                GetTurnManager().SetGameState(1);

            }
            else
            {
                spawnButton.SetActive(false);
                GetTurnManager().SetGameState(0);
            }
        }
    }

    private void ClientsNumValueChanged(int previousValue, int newValue)
    {
        CheckClientsNumServerRpc(newValue);
    }

    public void SpawnShip()
    {
        if ((GetTurnManager()?.GameState.Value ?? -1)!= 1)
        {
            // ready 상태가 아닐 경우
            return;
        }

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        PlayManager.SpawnShipRandomCoordServerRpc();

        spawnButton.SetActive(false);

        // 임시로 host 먼저 턴 시작
        GetTurnManager().SetGameState(2);
    }

    private TurnManager GetTurnManager()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return null;
        }

        if (!networkClient.PlayerObject.TryGetComponent<TurnManager>(out var TurnManager))
        {
            return null;
        }

        return TurnManager;
    }
}

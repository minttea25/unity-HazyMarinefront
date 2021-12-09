    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class EndTurnBtnEventListener : MonoBehaviour
{
    public GameObject EndTurnBtn;

    private void Awake()
    {
    }

    public void EndTurn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GetTurnManager()?.SetGameState(3);
            GameObject.Find("EventSystem").GetComponent<MoveBtnEventListener>().SetActiveMoveCanvas(false);
        }
        else
        {
            GetTurnManager()?.SetGameState(2);
            GameObject.Find("EventSystem").GetComponent<MoveBtnEventListener>().SetActiveMoveCanvas(false);
        }
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

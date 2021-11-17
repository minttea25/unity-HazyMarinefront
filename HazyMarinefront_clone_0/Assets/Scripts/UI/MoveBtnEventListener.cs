using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class MoveBtnEventListener : MonoBehaviour
{
    public Button moveBtn;

    public DropDownEventListener ddel;
    private const int amount = 1; // 움직임 1칸 고정


    public void MoveShip()
    {
        ShipSymbol s = MapLayout.GetSymbolByShiptypeTeam(ddel.shipType, ddel.team);

        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        PlayManager.SetMoveShipServerRpc(s, ddel.dirType, amount);
    }
}

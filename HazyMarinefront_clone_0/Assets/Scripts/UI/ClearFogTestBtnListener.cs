using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.UI;
using MLAPI.Connection;

public class ClearFogTestBtnListener : MonoBehaviour
{
    public Button clearFogButton;
    public FixedFogManager ffm;

    private void Start()
    {
        // 테스트 기능 비활성화
        clearFogButton.gameObject.SetActive(false);
    }

    public void ClearRandomFog()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            return;
        }

        PlayManager.ClearFogTestServerRpc();
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Connection;

public class AbilityBtnEventListner : MonoBehaviour
{
    public Text abilityText;
    public bool CrossAttackMode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCrossAttackMode(bool atk)
    {
        if (!atk)
        {
            abilityText.text = "CANCEL";
            CrossAttackMode = true;
        }
        else
        {
            abilityText.text = "ABILITY";
            CrossAttackMode = false;
        }
    }

    public void ActivateAbility()
    {
        ulong localClientId = NetworkManager.Singleton.LocalClientId;

        if (!NetworkManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkClient networkClient))
        {
            Debug.Log("Cannot find NetworkClient");
            return;
        }

        if (!networkClient.PlayerObject.TryGetComponent<PlayManager>(out var PlayManager))
        {
            Debug.Log("Cannot find PlayerManager");
            return;
        }

        PlayManager.MapInstance.GetComponent<Map>().GetSelectedShip().ActivateAbility();
    }

}

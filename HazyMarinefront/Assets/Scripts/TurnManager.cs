using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using MLAPI.Messaging;
using System;

public class TurnManager : NetworkBehaviour
{
    // 0: waiting client
    // 1: ready to start
    // 2: host turn
    // 3: client turn
    // 4: game over
    // 5: end game
    public NetworkVariableInt GameState;
    public NetworkVariableInt WinLose;

    public bool UIAvailable;



    private void Awake()
    {
        GameState.Value = 0;
        UIAvailable = false;
    }

    private void OnEnable()
    {
        GameState.OnValueChanged += OnGameStateChanged;
        WinLose.OnValueChanged += OnWinLoseChanged;
    }

    private void OnDisable()
    {
        GameState.OnValueChanged -= OnGameStateChanged;
        WinLose.OnValueChanged -= OnWinLoseChanged;
        Debug.Log("disabeld");
    }

    private void OnWinLoseChanged(int previousValue, int newValue)
    {
        if (newValue > 0)
        {
            ShowResult();
        }
    }

    private void OnGameStateChanged(int previousValue, int newValue)
    {
        switch (newValue)
        {
            case 0:
                Debug.Log("Waiting for a client...");
                break;
            case 1:
                Debug.Log("Ready to start game...");
                break;
            case 2:
                Debug.Log("It's Host Turn.");
                StartHostTurn();
                break;
            case 3:
                Debug.Log("It's Client Turn");
                StartClientTurn();
                break;
            case 4:
                Debug.Log("Game Over");
                GameOver();
                break;
            case 5:
                Debug.Log("Leaving from server...");
                break;
            default:
                break;
        }
        Debug.Log(previousValue + " -> " + newValue + " - " + System.DateTime.Now);
    }

    private void GameOver()
    {
        // UI ºñÈ°¼º (both)
        SetUIShow(false);
        ShowResult();

    }

    public void ShowResult()
    {
        GameObject.Find("EventSystem").GetComponent<WinLoseBtnEventListener>().SetActiveWinLoseCanvas(true);

        // ´©°¡ ÀÌ°å´ÂÁö È®ÀÎ ÈÄ °¢°¢ ´Ù¸¥ dialog ¶ç¿öÁÖ±â
        if (WinLose.Value == 0)
        {
            Debug.Log("DRAW");
            // ¹«½ÂºÎ
            GameObject.Find("EventSystem").GetComponent<WinLoseBtnEventListener>().ChangeWinLoseText("DRAW");
        }
        else if (WinLose.Value == 1)
        {
            Debug.Log("A TEAM WIN");
            // A ÆÀ ½Â
            if (NetworkManager.Singleton.IsServer)
            {
                GameObject.Find("EventSystem").GetComponent<WinLoseBtnEventListener>().ChangeWinLoseText("WIN");
            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<WinLoseBtnEventListener>().ChangeWinLoseText("LOSE");
            }
        }
        else
        {
            Debug.Log("B TEAM WIN");
            // B ÆÀ ½Â
            if (NetworkManager.Singleton.IsServer)
            {
                GameObject.Find("EventSystem").GetComponent<WinLoseBtnEventListener>().ChangeWinLoseText("Lose");
            }
            else
            {
                GameObject.Find("EventSystem").GetComponent<WinLoseBtnEventListener>().ChangeWinLoseText("Win");
            }
        }
    }

    private void StartHostTurn()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            //GameObject.Find("EventSystem").GetComponent<EndTurnBtnEventListener>().SetActiveTurnEndBtn(true);
            SetUIShow(true);
        }
        else
        {
            //Debug.Log("It is not host");
        }
    }

    private void StartClientTurn()
    {
        if (!NetworkManager.Singleton.IsServer)
        {
            //GameObject.Find("EventSystem").GetComponent<EndTurnBtnEventListener>().SetActiveTurnEndBtn(true);
            SetUIShow(true);
        }
        else
        {
            //Debug.Log("It is not client");
        }
    }

    private void SetUIShow(bool show)
    {
        GameObject.Find("EventSystem").GetComponent<MoveBtnEventListener>().SetActiveMoveCanvas(show);
    }

    public void SetGameState(int gameState)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameState.Value = -1;
            GameState.Value = gameState;
        }
        else
        {
            SetGameStateServerRpc(gameState);
        }
    }

    public void SetWinLose(int winLose)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            WinLose.Value = winLose;
        }
        else
        {
            SetWinLoseClientRpc(winLose);
        }
    }

    [ClientRpc]
    private void SetWinLoseClientRpc(int winLose)
    {
        WinLose.Value = winLose;
    }

    [ServerRpc]
    private void SetGameStateServerRpc(int gameState)
    {
        GameState.Value = -1;
        GameState.Value = gameState;
    }
}

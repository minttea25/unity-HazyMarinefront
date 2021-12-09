using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostAIManager : MonoBehaviour
{
    [SerializeField] private GameObject roomcodeEntryUI;
    [SerializeField] private GameObject leaveButton;
    [SerializeField] private GameObject attackBtn;
    [SerializeField] private GameObject spawnButton;
    [SerializeField] private GameObject costUI;


    private void Start()
    {
        GameObject.Find("EventSystem").GetComponent<MoveBtnEventListener>().MoveControllUICanvas.SetActive(false);

        costUI.SetActive(false);
        leaveButton.SetActive(false);
        attackBtn.SetActive(false);
        spawnButton.SetActive(false);

    }

    private void OnDestroy()
    {
 
    }

    public void AI()
    {
        

        spawnButton.SetActive(false);
        costUI.SetActive(true);

        //GetTurnManager().SetGameState(0);
        //SpawnShip();
    }

    public void Leave()
    {

        costUI.SetActive(false);
        roomcodeEntryUI.SetActive(true);
        leaveButton.SetActive(false);
        spawnButton.SetActive(false);
    }



    public void SpawnShip()
    {
        //PlayManager.SpawnShipRandomCoordServerRpc();

        spawnButton.SetActive(false);
        costUI.SetActive(true);
    }


    public void SetActiveCostUI(bool show)
    {
        costUI.SetActive(show);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBtnEventListner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateAbility()
    {
        GameObject.Find("NetworkManager").GetComponent<PlayManager>().MapInstance.GetComponent<Map>().GetSelectedShip().ActivateAbility();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rader : MonoBehaviour
{
    private Transform sweepTransform;
    private float rotateSpeed;

    private void Awake()
    {
        sweepTransform = transform.Find("sweep");
        rotateSpeed = 360f;

        
    }

    public void stopRader()
    {
        this.rotateSpeed = 0f;
    }


    private void Update()
    {
        sweepTransform.eulerAngles += new Vector3(rotateSpeed, 0, rotateSpeed) * Time.deltaTime;        
    }
}

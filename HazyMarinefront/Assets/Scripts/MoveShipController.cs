using UnityEngine;

public class MoveShipController : MonoBehaviour
{
    public bool moveFlag;
    public Vector3 desPosition;
    public Transform shipTransform;

    public float speed = 1;

    private void Start()
    {
        moveFlag = false;
    }

    private void Update()
    {
        if (moveFlag)
        {
            //shipTransform.position += (desPosition = shipTransform.position).normalized * 10.0f * Time.deltaTime;
            shipTransform.position = Vector3.MoveTowards(shipTransform.position, desPosition, Time.deltaTime * speed);
        }
    }

    public void MoveTo(Transform transform, Vector3 desPosition)
    {
        transform.position += (desPosition - transform.position).normalized * 10.0f * Time.deltaTime;
        Debug.Log(transform.position);
        
        //transform.position = desPosition;
        //transform.Translate(desPosition);
        //Debug.Log(desPosition);
        //transform.position = Vector3.MoveTowards(transform.position, desPosition, Time.deltaTime * speed);
    }

}

using UnityEngine;

public class MoveShipController : MonoBehaviour
{

    public void MoveTo(Transform transform, Vector3 desPosition)
    {
        transform.position = desPosition;
        //transform.Translate(desPosition);
        //Debug.Log(desPosition);
        //transform.position = Vector3.MoveTowards(transform.position, desPosition, Time.deltaTime * speed);
    }

}

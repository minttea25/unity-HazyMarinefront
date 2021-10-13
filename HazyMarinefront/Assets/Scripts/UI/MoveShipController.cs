using UnityEngine;

public class MoveShipController : MonoBehaviour
{
    public void MoveTo(Transform transform, Vector3 desPosition)
    {
        transform.position = desPosition;
    }

}

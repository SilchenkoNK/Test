using UnityEngine;

public class RotateForever : MonoBehaviour
{
    [SerializeField]
    Vector3 Direction;

    void Update()
    {
        transform.Rotate(Direction * Time.deltaTime);
    }
}

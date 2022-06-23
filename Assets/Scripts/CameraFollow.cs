using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Kuplinov;

    public float T;

    void Update()
    {
        transform.position = Vector3.LerpUnclamped(transform.position, Kuplinov.position, T);
        transform.rotation = Quaternion.LerpUnclamped(transform.rotation, Kuplinov.rotation, T * .1f);
    }
}

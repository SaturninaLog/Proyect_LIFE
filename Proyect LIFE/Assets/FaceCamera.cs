using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera playerCamera;

    void LateUpdate()
    {
        if (playerCamera != null)
        {
            transform.LookAt(playerCamera.transform);
            transform.Rotate(0, 180f, 0);
        }
    }
}

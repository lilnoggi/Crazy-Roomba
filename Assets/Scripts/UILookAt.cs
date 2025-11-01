using UnityEngine;

//_____________________________________\\
//  This script makes a UI element always  \\
//  face the camera in a 3D space.        \\
//_____________________________________\\

public class UILookAt : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
    }
}

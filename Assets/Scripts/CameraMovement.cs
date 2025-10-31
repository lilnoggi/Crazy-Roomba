using UnityEngine;

//_______________________\\_____________________\\
// CAMERA MOVEMENT SCRIPT \\_____________________\\
// This script handles the camera movement logic. \\
// Pressing "R" will rotate the camera 90 degrees. \\
//__________________________________________________\\

public class CameraMovement : MonoBehaviour
{
    // VARIABLES \\
    public float rotationSpeed = 5f; // Speed of camera rotation
    public GameObject player; // Reference to the player object

    private void Update()
    {
        // Check for "R" key press to rotate the camera
        if (Input.GetKeyDown(KeyCode.R))
        {
            RotateCamera();
        }
    }

    // METHOD TO ROTATE CAMERA \\
    void RotateCamera()
    {
        // Rotate the camera 90 degrees around the Y-axis
        transform.RotateAround(player.transform.position, Vector3.up, 90f);
        // Optional: Smooth rotation can be implemented here if needed
    }
}


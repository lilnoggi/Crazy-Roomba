using System.Collections;
using UnityEngine;

//_______________________\\_____________________\\
// CAMERA MOVEMENT SCRIPT \\_____________________\\
// This script handles the camera movement logic. \\
// Pressing "R" will rotate the camera 90 degrees. \\
//__________________________________________________\\

public class CameraMovement : MonoBehaviour
{
    // VARIABLES \\
    public float rotationDuration = 0.3f; // Speed of camera rotation
    public GameObject player; // Reference to the player object

    private bool isRotating = false; // Flag to check if rotation is in progress
    private const float targetAngle = -90f; // Target angle for rotation 

    private void Update()
    {
        // Check for "R" key press to rotate the camera
        if (Input.GetKeyDown(KeyCode.R) && !isRotating)
        {
            StartCoroutine(RotateCameraSmoothly(targetAngle)); 
        }
    }

    // === COROUTINE FOR SMOOTH ROTATION === \\
    IEnumerator RotateCameraSmoothly(float angle)
    {
        isRotating = true; // Set rotation flag to true
        float timeElapsed = 0f; // Time elapsed since the start of rotation

        // 1. Define start and end rotation
        // FIX: Use localRotation to read the rotation relative to the Roomba
        Quaternion startRotation = transform.localRotation;

        // Calculate the target rotation based on the current rotation + the desired angle around Vector3.up (Y-axis)
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, angle, 0);

        // 2. Interpolate over the duration
        while (timeElapsed < rotationDuration)
        {
            // FIX: Use localRotation to apply the rotation relative to the Roomba
            transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / rotationDuration);
            timeElapsed += Time.deltaTime; // Increment elapsed time
            yield return null; // Wait until next frame
        }

        // 3. Snap to the final position to avoid floating point errors
        transform.localRotation = targetRotation; // FIX: Use localRotation again

        isRotating = false; // Reset rotation flag
    }
}

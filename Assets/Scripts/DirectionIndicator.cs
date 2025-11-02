using UnityEngine;

//________________________________\\
//  DIRECTION INDICATOR SCRIPT  \\
// This script is for the direction indicator functionality. \\
// When player capacity reaches 5, the direction indicator will point towards the disposal zone. \\
//________________________________\\

public class DirectionIndicator : MonoBehaviour
{
    [SerializeField] private Transform disposalZone; // Reference to the disposal zone transform

    private void Update()
    {
        var targetPosition = disposalZone.position; // Get the position of the disposal zone
        targetPosition.y = transform.position.y; // Keep the indicator's y position unchanged
        transform.LookAt(targetPosition); // Make the indicator point towards the disposal zone
    }
}

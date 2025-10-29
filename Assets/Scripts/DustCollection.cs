using UnityEngine;

public class DustCollection : MonoBehaviour
{
    private Mover moverScript; // Reference to the Mover script

    private void Start()
    {
        moverScript = GameObject.FindWithTag("Player").GetComponent<Mover>(); // Get the Mover script from the player
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && moverScript.isFull == false) // Check if the collider is the player and capacity is not full
        {
            Destroy(gameObject); // Destroy the dust object upon collision with the player
        }
        else
        {
            Debug.Log("Cannot collect dust, capacity full!"); // Log message if capacity is full
        }
    }
}

using UnityEngine;

public class DustCollection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(gameObject); // Destroy the dust object upon collision with the player
        }
    }
}

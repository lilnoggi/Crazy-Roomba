using UnityEngine;

public class DustSpawner : MonoBehaviour
{
    public GameObject dustPrefab;
    public int numOfDust = 10; // no. of dust to spawn
    public float spawnRadius = 20f;
    public float spawnHeight = 0.5f;

    public float dustRadius = 0.5f; // Adjust based on the size of the dust prefab's collider
    public LayerMask furnitureLayer; // layer for furniture

    void Start()
    {
        SpawnDust();
    }

    void SpawnDust()
    {
        const int maxAttempts = 50;

        for (int i = 0; i < numOfDust; i++)
        {
            Vector3 spawnPos = Vector3.zero;
            bool positionFound = false;

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                spawnPos = GetRandomSpawnPosition();

                // Check if the random position is clear of furniture
                if (IsPositionClear(spawnPos))
                {
                    positionFound = true;
                    break; // Exit the attempt loop as a position is found!
                }
            }

            if (positionFound) 
            {
                Instantiate(dustPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Debug.Log("Couldn't find a clear position for dust to spawn!");
            }
        }
    }

    private bool IsPositionClear(Vector3 position)
    {
        // This checks for colliders within a sphere at the given position,
        // only checking objects on the furnitureLayer
        bool isOverlapping = Physics.CheckSphere(position, dustRadius, furnitureLayer);

        // Return true if it is NOT overlapping
        return !isOverlapping;
    }

    ///<summary>
    ///Generated a random position within a circular area around the spawner's transform.
    /// </summary>
    /// <returns>A random Vector3 position for the new coin.</returns>
    Vector3 GetRandomSpawnPosition()
    {
        // Random.insideUnitCircle returns a random point inside a 2D circle with radius 1.
        // Multiply this by the spawnRadius to stretch it to the desired size.
        Vector2 randomPoint2D = Random.insideUnitCircle * spawnRadius;

        // Take the spawner's world position as the center
        Vector3 center = transform.position;

        // Convert the 2D point (X, Y) into a 3D point (X, Height, Z)
        Vector3 randomPosition = center + new Vector3(randomPoint2D.x, spawnHeight, randomPoint2D.y);

        return randomPosition;
    }
}

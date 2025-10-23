using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mover : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 10f; // How fast the Roomba can move
    [SerializeField] int hits = 0; // Amount of times the player has hit furniture
    [SerializeField] int hitLimit = 0;
    public int dustCollected; // Amount of dust player collected
    public int score; // Player's score

    [Header("UI Components")]
    public TextMeshProUGUI dustCounter; // UI text fore score goes here!
    public TextMeshProUGUI furnitureHitCounter; // UI text for amount of furniture hit
    public TextMeshProUGUI scoreCounter; // UI text for score

    [Header("Audio")]
    public AudioClip pickupSound;
    public AudioClip[] furnitureHitSounds;
    public AudioClip vacuumLoop;
    public AudioClip vacuumOff;
    public AudioClip vacuumOn;
    public AudioClip brokeDown;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // --- LOGIC FOR AUDIO LOOPING --- \\
        if (audioSource != null && vacuumLoop != null)
        {
            audioSource.clip = vacuumLoop; // Set the clip to looping
            audioSource.loop = true;      // Enable looping
            audioSource.Play();          // Start the loop
        }
    }

    void Update()
    {
        MoveRoomba(); // Roomba movement method
        UpdateUI(); // Updates the UI output
    }

    void MoveRoomba()
    {
        float xValue = Input.GetAxis("Horizontal") * Time.deltaTime; // Get the horizontal keys (A, D, Left, Right)
        float zValue = Input.GetAxis("Vertical") * Time.deltaTime; // Get vertical movement keys (W, S, Up, Down)

        transform.Translate(xValue * moveSpeed, 0f, zValue * moveSpeed); // Movement speed calculation
    }

    // --- COLLIDE WITH FURNITURE --- \\
    private void OnCollisionEnter(Collision other) // CHANGED from private int to private void
    {
       PlayRandomSound();
        
        hits++; // If the player collides with an object, the hit counter increases by 1.
        score -= 50;
        hitLimit++;
        
        Debug.Log($"Bad Roomba! You hit: {hits} pieces of furniture!"); // Console output.
        
        GetComponentInChildren<MeshRenderer>().material.color = Color.red; // The roomba changes red.
        
        StartCoroutine(ChangeColour());

        // --- CHECK FOR BREAKAGE --- \\
        if (hitLimit >= 3)
        {
            // Stop the current vacuum sound instantly
            audioSource.Stop();

            // Start the entire break/fix sequence
            StartCoroutine(BreakVacuumSequence(4.5f)); // 5 is the duration of the "break"

            // Reset the hit limit
            hitLimit = 0;
        }
    }

    IEnumerator ChangeColour()
    {
        yield return new WaitForSeconds(1); // Delay before changing back

        GetComponentInChildren<MeshRenderer>().material.color = Color.black; // Change roomba back to black
    }

    // --- COLLIDE WITH DUST --- \\
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dust")
        {
            StartCoroutine(SlowDown());
            PlaySound(pickupSound);
            dustCollected++;
            score += 100;
        }
    }

    void UpdateUI()
    {
        dustCounter.text = $"Dust Collected: {dustCollected}"; // displays on screen // updates ui
        furnitureHitCounter.text = $"Furniture Hit: {hits}"; // Displays
        scoreCounter.text = $"Score: {score}";
    }

    // --- AUDIO --- \\
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void PlayRandomSound()
    {
        // --- SAFETY CHECK --- \\ 
        // If the array is null or empty, exit the method immediately
        if (furnitureHitSounds == null || furnitureHitSounds.Length == 0)
        {
            return;
        }

        // 1. Get a random index from 0 up to the array length.
        int randomIndex = UnityEngine.Random.Range(0, furnitureHitSounds.Length);

        // 2. Get the specific clip using the random index.
        AudioClip randomClip = furnitureHitSounds[randomIndex];

        // 3. Play the clip
        PlaySound(randomClip);
    }

    // Slow player down when dust is collected
    IEnumerator SlowDown()
    {
        moveSpeed = 2f;
        
        yield return new WaitForSeconds(1);

        moveSpeed = 10f;
    }

    // The main sequence for stopping and restarting the vacuum
    IEnumerator BreakVacuumSequence(float delay)
    {
        PlaySound(brokeDown);
        // 1. Enter the "broken" state
        // Temporarily set speed to 0 to stop movement
        moveSpeed = 0;

        // Play the vacuum OFF sound
        PlaySound(vacuumOff);

        GetComponentInChildren<MeshRenderer>().material.color = Color.red;

        // 2. Wait for the break period
        yield return new WaitForSeconds(delay);

        // 3. Exit the "broken" state and repair

        // Play the vacuum on sound
        PlaySound(vacuumOn);

        GetComponentInChildren<MeshRenderer>().material.color = Color.green;

        // Wait for the vacuum ON sound to finish
        yield return new WaitForSeconds(0.5f);

        // 4. Return to the normal game state

        GetComponentInChildren<MeshRenderer>().material.color = Color.black;

        // Resume movement speed
        moveSpeed = 10;

        // Restart the looping vacuum sound
        if (audioSource != null && vacuumLoop != null)
        {
            audioSource.clip = vacuumLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}

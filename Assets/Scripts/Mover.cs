using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mover : MonoBehaviour
{
    // === VARIABLES === \\
    [Header("Settings")]
    private const float baseMoveSpeed = 10f; // Base movement speed

    [SerializeField] private float moveSpeed = baseMoveSpeed; // How fast the Roomba can move
    [SerializeField] int hits = 0;                 // Amount of times the player has hit furniture
    [SerializeField] int hitLimit = 0;            // Limit before the Roomba breaks down
    public int dustCollected;                    // Amount of dust player collected
    public int score;                           // Player's score

    [Header("Capacity Variables")]
    public int maxCapacity = 10;            // Maximum dust capacity before Roomba needs to empty
    public int currentCapacity;            // Current dust capacity

    [Header("UI Components")]
    public TextMeshProUGUI dustCounter;          // UI text fore score goes here!
    public TextMeshProUGUI furnitureHitCounter; // UI text for amount of furniture hit
    public TextMeshProUGUI scoreCounter;       // UI text for score
    public TextMeshProUGUI capacityCounter;   // UI text for dust capacity

    [Header("Audio")]
    public AudioClip pickupSound;
    public AudioClip[] furnitureHitSounds;
    public AudioClip vacuumLoop;
    public AudioClip vacuumOff;
    public AudioClip vacuumOn;
    public AudioClip brokeDown;

    private AudioSource audioSource;

    private bool isBroken =  false; // Prevents speed from being reset while broken
    private bool isSlowed = false; // Prevents the next fix from being overridden
    public bool isFull = false;  // New flag for bag status

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // === LOGIC FOR AUDIO LOOPING === \\
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
        UpdateUI();  // Updates the UI output
    }

    // === ROOMBA MOVEMENT === \\
    void MoveRoomba()
    {
        // --- Only allow movement if not broken --- \\
        if (isBroken)
        {
            moveSpeed = 0f;
            return; // Exit the method early if broken
        }

        float targetSpeed = baseMoveSpeed;

        // --- Apply slowdown capacity penalty --- \\
        if (currentCapacity >= maxCapacity)
        {
            targetSpeed = 5f;
            isFull = true;
        }
        else
        {
            isFull = false;
        }

        // --- Apply slowdown from dust collection --- \\
        if (isSlowed)
        {
            targetSpeed = 2f;
        }

        // --- Final speed is the calculated target speed --- \\
        moveSpeed = targetSpeed;

        float xValue = Input.GetAxis("Horizontal") * Time.deltaTime; // Get the horizontal keys (A, D, Left, Right)
        float zValue = Input.GetAxis("Vertical") * Time.deltaTime;  // Get vertical movement keys (W, S, Up, Down)

        transform.Translate(xValue * moveSpeed, 0f, zValue * moveSpeed); // Movement speed calculation
    }

    // === COLLIDE WITH FURNITURE === \\
    private void OnCollisionEnter(Collision other) // CHANGED from private int to private void
    {
        if (other.gameObject.tag == "Furniture")
        {
            if (isBroken) return;

            PlayRandomSound();

            hits++; // If the player collides with an object, the hit counter increases by 1.
            score -= 50;
            hitLimit++;

            Debug.Log($"Bad Roomba! You hit: {hits} pieces of furniture!"); // Console output.

            GetComponentInChildren<MeshRenderer>().material.color = Color.red; // The roomba changes red.

            StartCoroutine(ChangeColour()); // Start the colour change coroutine.

            // --- CHECK FOR BREAKAGE --- \\
            if (hitLimit >= 3)
            {
                isBroken = true; // Set broken state to true
                // Stop the current vacuum sound instantly
                audioSource.Stop();

                // Start the entire break/fix sequence
                StartCoroutine(BreakVacuumSequence(4.5f)); // 5 is the duration of the "break"

                // Reset the hit limit
                hitLimit = 0;
            }
        }
    }

    // === COLLIDE WITH DUST === \\
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Dust")
        {
            // --- If Broken or Full, do not collect dust --- \\
            if (isBroken || currentCapacity >= maxCapacity)
            {
                Debug.Log(isBroken ? "Cannot collect dust while broken!" : "Dust bag is full! Please empty.");
                return; // Exit the method early
            }

            // --- Capacity Tracking --- \\
            const int dustCapacityCost = 1; // Each dust collected costs 1 capacity
            const int dustScore = 10; // How many points each dust is worth

            currentCapacity += dustCapacityCost; // Increase current capacity
            score += dustScore;                 // Increase score
            dustCollected++;                   // Increase dust collected

            StartCoroutine(SlowDown()); // Slow down the player temporarily
            PlaySound(pickupSound);
        }

        // EVIDENCE HANDLING GOES HERE \\
    }

    // === UPDATE UI === \\
    void UpdateUI()
    {
        dustCounter.text = $"Dust Collected: {dustCollected}";                    // Updates dust collected UI
        furnitureHitCounter.text = $"Furniture Hit: {hits}";                     // Updates furniture hit UI
        scoreCounter.text = $"Score: {score}";                                  // Updates score UI
        capacityCounter.text = $"Capacity: {dustCollected}/{maxCapacity}"; // Updates capacity UI
    }

    // === AUDIO === \\
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null) // Safety check
        {
            audioSource.PlayOneShot(clip); // Play the sound once
        }
    }

    void PlayRandomSound() // Play a random furniture hit sound
    {
        // --- SAFETY CHECK --- \\ 
        // If the array is null or empty, exit the method immediately
        if (furnitureHitSounds == null || furnitureHitSounds.Length == 0) // Safety check
        {
            return; // Exit the method if there are no sounds to play
        }

        // 1. Get a random index from 0 up to the array length.
        int randomIndex = UnityEngine.Random.Range(0, furnitureHitSounds.Length);

        // 2. Get the specific clip using the random index.
        AudioClip randomClip = furnitureHitSounds[randomIndex];

        // 3. Play the clip
        PlaySound(randomClip);
    }

    // === COROUTINES === \\
    IEnumerator ChangeColour()
    {
        yield return new WaitForSeconds(1); // Delay before changing back

        // --- Return to original colour only if not currently in the BreakVaccuumSequence --- \\
        if (!isBroken)
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.aquamarine; // Change roomba back to orginal colour
        }
    }

    // --- Slow player down when dust is collected --- \\
    IEnumerator SlowDown()
    {
        isSlowed = true; // Set the slowed flag to true

        moveSpeed = 2f; // Reduce speed

        yield return new WaitForSeconds(1); // Wait for 1 second

        isSlowed = false; // Reset the slowed flag

        moveSpeed = 10f; // Restore speed
    }

    // --- The main sequence for stopping and restarting the vacuum --- \\
    IEnumerator BreakVacuumSequence(float delay)
    {
        PlaySound(brokeDown);
        // 1. Enter the "broken" state
        // Temporarily set speed to 0 to stop movement
        moveSpeed = 0;

        // Play the vacuum OFF sound
        PlaySound(vacuumOff);

        GetComponentInChildren<MeshRenderer>().material.color = Color.red; // Change roomba to red to indicate broken state

        // 2. Wait for the break period
        yield return new WaitForSeconds(delay);

        // 3. Exit the "broken" state and repair

        // Play the vacuum on sound
        PlaySound(vacuumOn);

        GetComponentInChildren<MeshRenderer>().material.color = Color.blue; // Change roomba to green to indicate repair

        // Wait for the vacuum ON sound to finish
        yield return new WaitForSeconds(0.5f);

        // 4. Return to the normal game state

        GetComponentInChildren<MeshRenderer>().material.color = Color.aquamarine; // Change roomba back to normal colour

        isBroken = false; // Reset broken state

        // Resume movement speed
        moveSpeed = 10;

        // Restart the looping vacuum sound
        if (audioSource != null && vacuumLoop != null) // Safety check
        {
            audioSource.clip = vacuumLoop;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}

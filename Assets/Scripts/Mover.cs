using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//_____________________________________\\
// PLAYER SCRIPT                        \\_______\\
// This script handles the Roomba movement logic. \\
// It also manages dust collection, furniture      \\
// collisions, and disposal mechanics.              \\
//___________________________________________________\\

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

    [Header("Disposal Variables")]
    public bool playerDetection = false; // Whether the player is in the disposal area
    public GameObject disposalArea;  // Reference to the disposal area object
    public GameObject binBagPrefab; // Prefab for the bin bag to instantiate

    [Header("UI Components")]
    public TextMeshProUGUI dustCounter;          // UI text fore score goes here!
    public TextMeshProUGUI furnitureHitCounter; // UI text for amount of furniture hit
    public TextMeshProUGUI scoreCounter;       // UI text for score
    public TextMeshProUGUI capacityCounter;   // UI text for dust capacity
    public GameObject promptCanvas;          // UI canvas for prompts

    [Header("Audio")]
    public AudioClip pickupSound;
    public AudioClip[] furnitureHitSounds;
    public AudioClip vacuumLoop;
    public AudioClip vacuumOff;
    public AudioClip vacuumOn;
    public AudioClip brokeDown;

    private AudioSource audioSource;

    [Header("State Flags")]
    private bool isBroken =  false; // Prevents speed from being reset while broken
    private bool isSlowed = false; // Prevents the next fix from being overridden
    public bool isFull = false;  // New flag for bag status

    private Transform cameraTransform; // Reference to the main camera's transform
    private Vector3 movement;          // Movement vector

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

        // === MAIN CAMERA REFERENCE === \\
        // Find & store the main camera's transform \\
        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform; // gets camera transform
        }
        else
        {
            Debug.LogError("CAMERA NOT FOUND!!!!"); // camera is not placed in the inspector
        }
    }

    void Update()
    {
        UpdateUI();  // Updates the UI output

        HandleDisposalInput(); // Check for disposal input

        // === CAMERA INFLUENCE ON MOVEMENT === \\
        // Get the RAW input values
        Vector3 rawInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // Convert the raw input to be relative to the camera's orientation
        movement = GetCameraRelativeMovement(rawInput);

        MoveRoomba(); // Roomba movement method
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

        //float xValue = Input.GetAxis("Horizontal") * Time.deltaTime; // Get the horizontal keys (A, D, Left, Right) // Commented out for new HandleRotaiton()
        //float zValue = Input.GetAxis("Vertical") * Time.deltaTime;  // Get vertical movement keys (W, S, Up, Down)

        // =-= FIX: APPLY MOVEMENT VECTOR DIRECTLY =-= \\
        // Apply the camera-relative movement vector 'movement' to the position \\
        // The movement vector is already calculated in Update() based on input \\
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World); // Movement speed calculation
    }

    // Camera rotation influences player movement direction \\
    Vector3 GetCameraRelativeMovement(Vector3 rawInput)
    {
        if (cameraTransform == null || rawInput.magnitude < 0.1f)
        {
            return Vector3.zero; // no camera or no input, return no movement
        }

        // Get the camera's Y rotation 
        Quaternion cameraRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // Rotate the input vector (forward/backward/strafe) by the camera's rotation
        Vector3 newMovement = cameraRotation * new Vector3(rawInput.x, 0f, rawInput.z);

        // Keep movement vector normalised
        return newMovement.normalized;
    }

    // === HANDLE DISPOSAL INPUT === \\
    void HandleDisposalInput()
    {
        if (playerDetection && Input.GetKeyDown(KeyCode.E))
        {
            if (currentCapacity > 0)
            {
                moveSpeed = 0f; // Temporarily stop movement
                EmptyBag(); // Call the method to empty the dust bag
                moveSpeed = baseMoveSpeed; // Restore movement speed
            }
            else
            {
                Debug.Log("Dust bag is already empty.");
            }
        }
    }

    // === EMPTY DUST BAG METHOD === \\
    void EmptyBag()
    {
        Debug.Log("Emptying dust bag...");
        GetComponentInChildren<MeshRenderer>().material.color = Color.yellow;
        currentCapacity = 0; // Reset capacity
        UpdateUI();         // Update UI immediately
        
        SpawnBinBag(); // Spawn the bin bag
        StartCoroutine(ChangeColour()); // Change colour back after delay
    }

    // === SPAWN BIN BAG METHOD === \\
    void SpawnBinBag()
    {
        if (binBagPrefab != null && disposalArea != null)
        {
            Vector3 spawnPosition = disposalArea.transform.position + new Vector3(0f, 1.5f, 0f); // Slightly above the disposal area
            Instantiate(binBagPrefab, spawnPosition, Quaternion.identity); // Spawn the bin bag prefab
        }
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

    // === TRIGGER ENTRY || Dust & Disposal === \\
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

        // === COLLIDE WITH DISPOSAL AREA === \\
        if (other.gameObject == disposalArea)
        {
            promptCanvas.SetActive(true); // Show prompt UI
            playerDetection = true;
            Debug.Log("In disposal area. Press 'E' to empty dust bag.");
        }

        // EVIDENCE HANDLING GOES HERE \\
    }

    // === TRIGGER EXIT || Disposal Area === \\
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == disposalArea)
        {
            promptCanvas.SetActive(false); // Hide prompt UI
            playerDetection = false;
            Debug.Log("Left disposal area.");
        }
    }

    // === UPDATE UI === \\
    void UpdateUI()
    {
        dustCounter.text = $"Dust Collected: {dustCollected}";                    // Updates dust collected UI
        furnitureHitCounter.text = $"Furniture Hit: {hits}";                     // Updates furniture hit UI
        scoreCounter.text = $"Score: {score}";                                  // Updates score UI
        capacityCounter.text = $"Capacity: {currentCapacity}/{maxCapacity}"; // Updates capacity UI
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

        GetComponentInChildren<MeshRenderer>().material.color = Color.green; // Change roomba to green to indicate repair

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

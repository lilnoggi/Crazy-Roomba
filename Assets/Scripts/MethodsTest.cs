using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodsTest : MonoBehaviour
{

    private void Awake()
    {
        Debug.Log($"Awake: {name}");
        //Can be used to initialise *before* Start if order and logic are an issue.
        //Called only once!
    }

    private void Start()
    {
        Debug.Log($"Start: {name}");
        //Things in Start happen only once at the start of the game.
    }

    private void OnEnable()
    {
        Debug.Log($"OnEnable: {name}");
        //Happens each time the script is initialised. So, *could* be more than once.
        //Good for enabling UI or HUD.
    }

    private void FixedUpdate()
    {
        Debug.Log($"FixedUpdate: {name}");
        //Synchronised with the physics engine, so doesn't necessarily update every frame!
        //See project settings > time > fixed timestep to adjust speed of the update.
    }
    
    private void Update()    
    {
        Debug.Log($"Update: {name}");
        //Call something every single frame!
        //Anything here will definitely 100% be called every single frame.
    }

    private void LateUpdate()
    {
        Debug.Log($"LateUpdate: {name}");
        //Also definitely 100% called every frame.
        //Happens *after* the normal Update, so can sequnce things (like a follow cam).
    }

    private void OnDisable()
    {
        Debug.Log($"OnDisable: {name}");
        //Used to "unsubscribe to events" such as turning off some UI.
        //Can turn it back on again with OnEnable.
    }
    private void OnDestroy()
    {
        Debug.Log($"OnDestroy: {name}");
        //Either remove the script or Game Object.
        //The opposite of the Awake and Start methods.
    }
}

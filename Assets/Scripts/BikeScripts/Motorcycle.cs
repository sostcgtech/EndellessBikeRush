using UnityEngine;
using System;

/// <summary>
/// Character script for motorcycle. Works the same as cat/raccoon but represents a motorcycle.
/// This script is attached to the motorcycle prefab (should be in Bundles/Characters folder)
/// </summary>
public class Motorcycle : MonoBehaviour
{
    public string characterName = "Motorcycle";
    public int cost = 0; // Set to 0 if you want it free, or any cost
    public int premiumCost = 0;
    public CharacterAccessories[] accessories;
    public Animator animator;
    public Sprite icon;

    [Header("Sound")]
    public AudioClip jumpSound; // Use motorcycle jump/rev sound
    public AudioClip hitSound;  // Use crash/collision sound
    public AudioClip deathSound; // Use motorcycle crash sound

    [Header("Motorcycle Specific")]
    public GameObject motorcycleModel; // Reference to your motorcycle 3D model
    public Transform wheelFront; // Optional: for wheel rotation animation
    public Transform wheelRear;  // Optional: for wheel rotation animation
    public float wheelRotationSpeed = 360f; // Degrees per second

    private void Update()
    {
        // Optional: Rotate wheels while running for visual effect
        if (TrackManager.instance != null && TrackManager.instance.isMoving)
        {
            RotateWheels();
        }
    }

    private void RotateWheels()
    {
        if (wheelFront != null)
        {
            wheelFront.Rotate(Vector3.right, wheelRotationSpeed * Time.deltaTime);
        }

        if (wheelRear != null)
        {
            wheelRear.Rotate(Vector3.right, wheelRotationSpeed * Time.deltaTime);
        }
    }

    // Called by the game when an accessory changes
    // a value of -1 as parameter disables all accessory.
    public void SetupAccesory(int accessory)
    {
        for (int i = 0; i < accessories.Length; ++i)
        {
            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
        }
    }
}
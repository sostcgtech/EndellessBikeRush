//using UnityEngine;

///// <summary>
///// Motorcycle variant of Character script
///// Handles motorcycle-specific properties and animations
///// </summary>
//public class Motorcycle : MonoBehaviour
//{
//    [Header("Motorcycle Info")]
//    public string motorcycleName;
//    public int cost;
//    public int premiumCost;
//    public Sprite icon;

//    [Header("Customization")]
//    public CharacterAccessories[] accessories; // For paint jobs, decals, etc.

//    [Header("Components")]
//    public Animator animator;

//    [Header("Wheel References (Optional - for wheel spinning)")]
//    public Transform frontWheel;
//    public Transform backWheel;
//    public float wheelSpinSpeed = 360f; // Degrees per second

//    [Header("Sounds")]
//    public AudioClip jumpSound;      // Engine rev up
//    public AudioClip hitSound;       // Crash/impact
//    public AudioClip deathSound;     // Big crash
//    public AudioClip engineIdleSound; // Engine running (optional)

//    [Header("Visual Effects")]
//    public ParticleSystem exhaustSmoke; // Optional smoke effect
//    public ParticleSystem slideEffect;  // Sparks when sliding

//    private bool isMoving = false;

//    void Start()
//    {
//        // Initialize animator if not set
//        if (animator == null)
//            animator = GetComponent<Animator>();

//        // Optional: Play idle engine sound
//        if (engineIdleSound != null)
//        {
//            AudioSource audioSource = GetComponent<AudioSource>();
//            if (audioSource != null)
//            {
//                audioSource.clip = engineIdleSound;
//                audioSource.loop = true;
//                audioSource.Play();
//            }
//        }
//    }

//    void Update()
//    {
//        // Spin wheels when moving (if not animated in your animations)
//        if (isMoving && animator != null)
//        {
//            float speed = animator.GetBool("Moving") ? wheelSpinSpeed : 0f;

//            if (frontWheel != null)
//                frontWheel.Rotate(Vector3.right, speed * Time.deltaTime);

//            if (backWheel != null)
//                backWheel.Rotate(Vector3.right, speed * Time.deltaTime);
//        }
//    }

//    // Called by CharacterInputController
//    public void SetMoving(bool moving)
//    {
//        isMoving = moving;
//    }

//    // Enable/disable particle effects
//    public void SetSlideEffect(bool active)
//    {
//        if (slideEffect != null)
//        {
//            if (active)
//                slideEffect.Play();
//            else
//                slideEffect.Stop();
//        }
//    }

//    // Called when accessory changes (paint job, decals, etc.)
//    public void SetupAccesory(int accessory)
//    {
//        for (int i = 0; i < accessories.Length; ++i)
//        {
//            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
//        }
//    }
//}


////using UnityEngine;

////public class Character : MonoBehaviour
////{
////    public string characterName;
////    public int cost;
////    public int premiumCost;

////    public CharacterAccessories[] accessories;

////    [HideInInspector] public Animator animator; // optional now
////    public Sprite icon;

////    [Header("Sound")]
////    public AudioClip jumpSound;
////    public AudioClip hitSound;
////    public AudioClip deathSound;

////    public void SetupAccesory(int accessory)
////    {
////        for (int i = 0; i < accessories.Length; ++i)
////        {
////            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
////        }
////    }
////}

using UnityEngine;
using System;

/// <summary>
/// Mainly used as a data container to define a character. This script is attached to the prefab
/// (found in the Bundles/Characters folder) and is to define all data related to the character.
/// </summary>
public class Character : MonoBehaviour
{
    public string characterName;
    public int cost;
    public int premiumCost;

    public CharacterAccessories[] accessories;

    public Animator animator;
    public Sprite icon;

    [Header("Sound")]
    public AudioClip jumpSound;
    public AudioClip hitSound;
    public AudioClip deathSound;

    // Called by the game when an accessory changes, enable/disable the accessories children objects accordingly
    // a value of -1 as parameter disables all accessory.
    public void SetupAccesory(int accessory)
    {
        for (int i = 0; i < accessories.Length; ++i)
        {
            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
        }
    }
}

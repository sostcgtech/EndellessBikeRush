using UnityEngine;

/// <summary>
/// Smooth camera follow that responds to character lane changes with dynamic movement
/// Attach this to the Main Camera
/// </summary>
public class SmoothCameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target; // The character controller transform
    public Vector3 offset = new Vector3(0, 2, -5); // Camera offset from target

    [Header("Lane Follow Settings")]
    public float laneOffset = 1.0f; // Should match TrackManager.laneOffset
    public float lateralFollowSpeed = 8f; // How fast camera follows left/right
    public float lateralSmoothTime = 0.15f; // Smoothing for lane changes

    [Header("Tilt Settings")]
    public bool enableTilt = true;
    public float tiltAngle = 8f; // Max tilt angle when moving left/right
    public float tiltSpeed = 5f; // How fast camera tilts

    [Header("Dynamic Movement")]
    public bool enableDynamicMovement = true;
    public float dynamicMoveAmount = 0.3f; // Extra movement during lane change
    public float dynamicMoveSpeed = 3f;

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f; // Camera looks slightly ahead
    public float lookAheadSpeed = 3f;

    private Vector3 m_CurrentVelocity;
    private float m_TargetLateralOffset;
    private float m_CurrentLateralOffset;
    private float m_CurrentTilt;
    private float m_DynamicOffset;
    private float m_LastXPosition;
    private float m_PreviousTargetX;

    private CharacterInputController m_CharacterController;

    void Start()
    {
        if (target == null)
        {
            // Try to find the character controller
            m_CharacterController = Object.FindFirstObjectByType<CharacterInputController>();
            if (m_CharacterController != null)
                target = m_CharacterController.transform;
        }
        else
        {
            m_CharacterController = target.GetComponent<CharacterInputController>();
        }

        if (target != null)
        {
            m_LastXPosition = target.position.x;
            m_PreviousTargetX = target.position.x;
        }

        m_CurrentLateralOffset = 0f;
        m_TargetLateralOffset = 0f;
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Calculate target lateral offset based on character's X position
        float targetX = target.position.x;
        m_TargetLateralOffset = targetX;

        // Detect significant lane change for dynamic movement
        if (enableDynamicMovement)
        {
            float deltaX = targetX - m_PreviousTargetX;
            if (Mathf.Abs(deltaX) > laneOffset * 0.3f) // Threshold for lane change detection
            {
                // Add extra movement in the direction of lane change
                float direction = Mathf.Sign(deltaX);
                m_DynamicOffset = direction * dynamicMoveAmount;
            }
            m_PreviousTargetX = targetX;
        }

        // Smoothly interpolate current offset to target
        m_CurrentLateralOffset = Mathf.SmoothDamp(
            m_CurrentLateralOffset,
            m_TargetLateralOffset,
            ref m_CurrentVelocity.x,
            lateralSmoothTime,
            lateralFollowSpeed
        );

        // Decay dynamic offset
        m_DynamicOffset = Mathf.Lerp(m_DynamicOffset, 0f, Time.deltaTime * dynamicMoveSpeed);

        // Calculate final lateral position
        float finalLateralOffset = m_CurrentLateralOffset + m_DynamicOffset;

        // Calculate camera position with offset
        Vector3 targetPosition = target.position + offset;
        targetPosition.x = finalLateralOffset; // Override X with our calculated offset

        // Look ahead based on movement
        if (lookAheadDistance > 0)
        {
            float lookAhead = Mathf.Abs(m_CurrentVelocity.x) * lookAheadDistance;
            targetPosition.z += lookAhead;
        }

        // Apply position
        transform.position = targetPosition;

        // Handle camera tilt
        if (enableTilt)
        {
            // Calculate target tilt based on lateral velocity
            float targetTilt = -m_CurrentVelocity.x * tiltAngle * 2f;

            // Smooth tilt
            m_CurrentTilt = Mathf.Lerp(m_CurrentTilt, targetTilt, Time.deltaTime * tiltSpeed);

            // Apply tilt rotation
            Quaternion tiltRotation = Quaternion.Euler(0, 0, m_CurrentTilt);
            transform.rotation = target.rotation * tiltRotation;
        }
        else
        {
            transform.rotation = target.rotation;
        }

        m_LastXPosition = target.position.x;
    }

    // Optional: Draw gizmos to visualize camera behavior
    void OnDrawGizmos()
    {
        if (target == null || !Application.isPlaying)
            return;

        // Draw lane positions
        Gizmos.color = Color.yellow;
        Vector3 basePos = target.position;

        for (int i = 0; i < 3; i++)
        {
            Vector3 lanePos = basePos;
            lanePos.x = (i - 1) * laneOffset;
            Gizmos.DrawWireSphere(lanePos, 0.3f);
        }

        // Draw current camera target position
        Gizmos.color = Color.green;
        Vector3 targetPos = target.position + offset;
        targetPos.x = m_CurrentLateralOffset + m_DynamicOffset;
        Gizmos.DrawWireSphere(targetPos, 0.5f);

        // Draw line from camera to target
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, target.position);
    }
}
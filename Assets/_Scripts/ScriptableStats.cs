using UnityEngine;

public class ScriptableStats : ScriptableObject
{
    public LayerMask PlayerLayer;

    [Header("========== Inputs ==========")]
    public bool SnapInput = true;
    public float VerticalDeadZoneThreshold = 0.3f;
    public float HorizontalDeadZoneThreshold = 0.1f;

    [Header("========== Movement ===========")]
    public float MaxSpeed = 10;
    public float Acceleration = 120;
    public float GroundDeceleration = 60;
    public float AirDeceleration = 30;
    public float GroundingForce = -1.5f;
    public float GrounderDistance = 0.05f;

    [Header("========== Jump ==========")]
    public float JumpPower = 36;
    public float MaxFallSpeed = 40;
    public float FallAcceleration = 110;
    public float JumpEndEarlyGravMod = 3;
    public float CoyoteTime = 0.15f; // Coyote jump allows jump to execute even after leaving a ledge
    public float JumpBuffer = 0.2f; // This allows jump input before actually hitting the ground
}

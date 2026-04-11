using UnityEngine;


[CreateAssetMenu(fileName = "GoopData", menuName = "Data/Goop Data")]
public class GoopData : ScriptableObject
{
    public LayerMask PlayerLayer;

    [Header("========== Stats ==========")]
    [Min(1f)] public float MaxHealth = 100;

    [Header("========== Inputs ==========")]
    public bool SnapInput = true;
    [Min(0f)] public float VerticalDeadZoneThreshold = 0.3f;
    [Min(0f)] public float HorizontalDeadZoneThreshold = 0.1f;

    [Header("========== Movement ===========")]
    [Min(1f)] public float MaxSpeed = 10;
    [Min(0f)] public float Acceleration = 120;
    [Min(0f)] public float GroundDeceleration = 60;
    [Min(0f)] public float AirDeceleration = 30;
    [Min(0f)] public float GrounderDistance = 0.2f;
    public float GroundingForce = -1.5f;

    [Header("========== Jump ==========")]
    [Min(1f)] public float JumpPower = 36;
    [Min(0f)] public float MaxFallSpeed = 40;
    [Min(0f)] public float FallAcceleration = 110;
    [Min(0f)] public float JumpEndEarlyGravMod = 3;
    [Min(0f)] public float CoyoteTime = 0.15f; // Coyote jump allows jump to execute even after leaving a ledge
    [Min(0f)] public float JumpBuffer = 0.2f; // This allows jump input before actually hitting the ground
}

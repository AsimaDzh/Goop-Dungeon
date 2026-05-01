using UnityEngine;

enum NPCState
{
    Idle = 0,
    Moving = 1, 
    Interacting = 2,
    Inspecting = 3,
    Accepting = 4,
    Rejecting = 5
}


public class NPCBase : MonoBehaviour
{
    [Header("========== NPC Data ==========")]
    [SerializeField] private NPCData npcData;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;
    private NPCState _currentState = NPCState.Idle;

    [Header("========== Moving ==========")]
    [SerializeField] private float movingRadius = 5f;
    [SerializeField] private float waitTime = 2f;
    private Vector2 _movingTarget;
    private float _waitCounter;

    public NPCData Data => npcData;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => npcData != null ? npcData.MaxHealth : 0f;
    public float MoveSpeed => npcData != null ? npcData.MoveSpeed : 0f;
    public float Damage => npcData != null ? npcData.Damage : 0f;
    public float AttackRange => npcData != null ? npcData.AttackRange : 0f;

    protected Rigidbody2D _rb;


    private void Update()
    {
        // switch case
        // state idle
        // state moving
        // state interacting
        // state Inspecting
        // state Accepting
        // state Rejecting
    }


    private void HandleIdle()
    {
    }


    private void HandleMoving()
    {
    }


    private void GrabObject()
    {
    }


    private void InspectiongObject()
    {

    }


    private void AcceptingObject()
    {
    }


    private void RejectingObject()
    {
    }
}

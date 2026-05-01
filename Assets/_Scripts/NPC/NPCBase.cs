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


    private void Awake()
    {
        currentHealth = npcData != null ? npcData.MaxHealth : 0f;
        _rb = GetComponent<Rigidbody2D>();
    }


    private void Start()
    {
        _waitCounter = waitTime;
    }


    private void Update()
    {
        switch(_currentState)
        {
            case NPCState.Idle:
                HandleIdle(); 
                break;

            case NPCState.Moving:
                HandleMoving(); 
                break;

            case NPCState.Interacting:
                GrabObject();
                break;

            case NPCState.Inspecting:
                InspectiongObject();
                break;

            case NPCState.Accepting:
                AcceptingObject();
                break;

            case NPCState.Rejecting:
                RejectingObject();
                break;
        }
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

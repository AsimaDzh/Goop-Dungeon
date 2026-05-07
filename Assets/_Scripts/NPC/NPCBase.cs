using UnityEngine;


public class NPCBase : CharacterBase
{
    [Header("========== NPC References ==========")]
    [SerializeField] private NPCData npcData;
    private GrabObjects _grabSystem;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;

    public NPCData Data => npcData;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => npcData != null ? npcData.MaxHealth : 0f;
    public float Damage => npcData != null ? npcData.Damage : 0f;
    public float AttackRange => npcData != null ? npcData.AttackRange : 0f;
    override public float MoveSpeed => npcData != null ? npcData.MoveSpeed : 0f;


    private void Awake()
    {
        currentHealth = npcData != null ? npcData.MaxHealth : 0f;
        _rb = GetComponent<Rigidbody2D>();
        _grabSystem = GetComponent<GrabObjects>();

    }


    private void Update()
    {
        if (npcData == null) return;

        if (_grabSystem.IsObjectGrabbed)
            _currentState = CharacterState.Inspecting;


        switch (_currentState)
        {
            case CharacterState.Idle:
                HandleIdle(); 
                break;

            case CharacterState.Moving:
                HandleMoving(); 
                break;

            case CharacterState.Inspecting:
                InspectiongObject();
                break;

            case CharacterState.Accepting:
                AcceptingObject();
                break;

            case CharacterState.Rejecting:
                RejectingObject();
                break;
        }
    }



    private void InspectiongObject()
    {
        Debug.Log("Inspecting object...");
    }


    private void AcceptingObject()
    {
    }


    private void RejectingObject()
    {

    }
}

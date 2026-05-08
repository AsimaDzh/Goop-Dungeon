using System.Collections;
using UnityEngine;


public class NPCBase : CharacterBase
{
    [Header("========== NPC References ==========")]
    [SerializeField] private NPCData npcData;
    private GrabObjects _grabSystem;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;
    private float _inspectingTime = 3f;
    private bool _isInspecting;

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

        if (_grabSystem.IsObjectGrabbed && _currentState == CharacterState.Idle)
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
                InspectingObject();
                break;

            case CharacterState.Accepting:
                AcceptingObject();
                break;

            case CharacterState.Rejecting:
                RejectingObject();
                break;
        }
    }


    private void InspectingObject()
    {
        if (_isInspecting) return;

        _isInspecting = true;

        Debug.Log("Inspecting object...");
        StartCoroutine(WaitAndDecide());
    }


    virtual protected IEnumerator WaitAndDecide()
    {
        yield return new WaitForSeconds(_inspectingTime);

        if (npcData.Likes == _grabSystem.GrabbedObject.name)
            _currentState = CharacterState.Accepting;
        else _currentState = CharacterState.Rejecting;

        _isInspecting = false;
    }


    private void AcceptingObject()
    {
        Debug.Log("Accepted!");
        _grabSystem.RemoveObject();
        _currentState = CharacterState.Idle;
    }


    private void RejectingObject()
    {
        Debug.Log("Rejected!");
        _grabSystem.ThrowObject();
        _currentState = CharacterState.Idle;
    }
}

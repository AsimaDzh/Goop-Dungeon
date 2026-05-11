using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCBase : CharacterBase
{
    [Header("========== References ==========")]
    [SerializeField] private NPCData npcData;
    [SerializeField] private GoopData goopData;
    private GrabObjects _grabSystem;

    [Header("========== Current state (Runtime) ==========")]
    [SerializeField] private float currentHealth;
    private float _inspectingTime = 3f;
    private bool _isInspecting;

    [Header("========== Following ==========")]
    [SerializeField] private GameObject player;
    [SerializeField] private float stepsToFollow;

    [Header("========== Detection ==========")]
    [SerializeField] private CircleCollider2D detectionTrigger;
    private readonly List<EnemyBase> _allEnemiesInScene = new();
    [SerializeField] private EnemyBase _closestEnemy;

    public NPCData Data => npcData;
    public GoopData GoopData => goopData;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => npcData != null ? npcData.MaxHealth : 0f;
    public float Damage => npcData != null ? npcData.Damage : 0f;
    public float DetectionRange => npcData != null ? npcData.DetectionRange : 0f;
    override public float MoveSpeed => npcData != null ? npcData.MoveSpeed : 0f;


    private void Awake()
    {
        currentHealth = npcData != null ? npcData.MaxHealth : 0f;
        _rb = GetComponent<Rigidbody2D>();
        _grabSystem = GetComponent<GrabObjects>();
        detectionTrigger = GetComponent<CircleCollider2D>();
    }


    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerController>()?.gameObject;

        detectionTrigger.radius = npcData != null ? npcData.DetectionRange : 0f;
    }


    private void Update()
    {
        if (npcData == null) return;

        if (_grabSystem.IsObjectGrabbed && _currentState == CharacterState.Idle)
            _currentState = CharacterState.Inspecting;

        if (_currentState == CharacterState.Following)
        {
            FindClosestEnemy();
            if (_closestEnemy != null)
                TryUseSkill();
        }


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

            case CharacterState.Following:
                FollowThePlayer();
                break;
        }
    }


    private void InspectingObject()
    {
        if (_isInspecting) return;
        _isInspecting = true;

        Debug.Log("Inspecting object...");

        _rb.linearVelocity = Vector2.zero;
        StartCoroutine(WaitAndDecide());
    }


    private IEnumerator WaitAndDecide()
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
        _currentState = CharacterState.Following;
    }


    private void RejectingObject()
    {
        Debug.Log("Rejected!");
        _grabSystem.ThrowObject();
        _currentState = CharacterState.Idle;
    }


    private void FollowThePlayer()
    {
        Vector3 _playerPosition = player.transform.position;
        Vector3 _npcPosition = transform.position;

        float _distance = Vector3.Distance(_playerPosition, _npcPosition);

        if (_distance > stepsToFollow)
        {
            float _directionX = Mathf.Sign(_playerPosition.x - _npcPosition.x);
            _rb.linearVelocity = new Vector2(_directionX * goopData.MaxSpeed, 0f);
            Rotate(new Vector2(_directionX, 0f));
        }
        else _rb.linearVelocity = Vector2.zero;
    }


    private void FindClosestEnemy()
    {
        float _distanceToClosestEnemy = Mathf.Infinity;

        foreach (EnemyBase _currentEnemy in _allEnemiesInScene)
        {
            if (_currentEnemy == null) continue;

            float _distanceToEnemy = (_currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (_distanceToEnemy < _distanceToClosestEnemy)
            {
                _distanceToClosestEnemy = _distanceToEnemy;
                _closestEnemy = _currentEnemy;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyBase _enemy))
        {
            if (!_allEnemiesInScene.Contains(_enemy))
                _allEnemiesInScene.Add(_enemy);
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out EnemyBase _enemy))
        {
            _allEnemiesInScene.Remove(_enemy);
            if (_closestEnemy == _enemy)
                _closestEnemy = null;
        }
    }


    private void TryUseSkill()
    {
        UseSkill();
    }


    virtual protected void UseSkill() 
    { 
        Debug.Log("Using skill on " + _closestEnemy.name);
    }
}

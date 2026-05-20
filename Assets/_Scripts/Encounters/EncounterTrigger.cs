using System.Collections.Generic;
using UnityEngine;
using System;


public class EncounterTrigger : MonoBehaviour
{
    [SerializeField] private bool disableTriggerAfterStart = true;
    [SerializeField] private GameObject[] activateOnCompleted;
    [SerializeField] private GameObject[] deactivateOnStarted;

    private string _playerTag = "Player";
    private bool _isEncounterRunning;
    private bool _isEncounterCompleted;
    private Coroutine _encounterCoroutine;
    private Collider _triggerCollider;

    private readonly List<EnemyBase> _aliveEnemies = new List<EnemyBase>();
    private readonly Dictionary<EnemyBase, Action> _deathHandlers = new Dictionary<EnemyBase, Action>();
}

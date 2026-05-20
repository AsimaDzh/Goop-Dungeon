using UnityEngine;


public class ExitWinTrigger : MonoBehaviour
{
    [SerializeField] private GameLoopFlowController flowController;
    private string _playerTag = "Player";


    private void Reset()
    {
        if (flowController == null)
            flowController = FindFirstObjectByType<GameLoopFlowController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!isActiveAndEnabled || !gameObject.activeInHierarchy) return;

        if (!IsPlayerCollider(other)) return;

        if (flowController == null)
        {
            Debug.LogWarning($"{name}: flowController is not assigned.", this);
            return;
        }

        flowController.RequestWinFromExit();
        Debug.Log($"{name}: player entered exit trigger, win requested.", this);
    }


    private bool IsPlayerCollider(Collider other)
    {
        if (other == null) return false;

        if (string.IsNullOrWhiteSpace(_playerTag)) return true;

        if (other.CompareTag(_playerTag)) return true;

        if (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag(_playerTag))
            return true;

        Transform root = other.transform.root;
        return root != null && root.CompareTag(_playerTag);
    }
}

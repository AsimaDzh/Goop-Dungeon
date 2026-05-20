using UnityEngine;

public class ExitWinTrigger : MonoBehaviour
{
    [SerializeField] private GameLoopFlowController flowController;


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
}

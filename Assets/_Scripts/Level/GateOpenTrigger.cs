using UnityEngine;


public class GateOpenTrigger : MonoBehaviour
{
    [SerializeField] private GameObject gateToOpen;


    private void Awake()
    {
        if (gateToOpen == null)
            gateToOpen = GameObject.FindGameObjectWithTag("Gate");
    }
}

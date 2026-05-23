using UnityEngine;


public class GateOpenTrigger : MonoBehaviour
{
    [SerializeField] private GameObject gateToOpen;

    private LayerMask ObjectMask => LayerMask.GetMask("Objects");


    private void Awake()
    {
        if (gateToOpen == null)
            gateToOpen = GameObject.FindGameObjectWithTag("Gate");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == ObjectMask)
        {
            gateToOpen.SetActive(false);
            Destroy(gameObject);
        }
    }
}

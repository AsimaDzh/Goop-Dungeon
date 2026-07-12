using UnityEngine;


public class GateOpenTrigger : MonoBehaviour
{
    [SerializeField] private GameObject gateToOpen;
    private LayerMask _objectMask = 7;


    private void Awake()
    {
        if (gateToOpen == null)
            gateToOpen = GameObject.FindGameObjectWithTag("Gate");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == _objectMask)
        {
            gateToOpen.SetActive(false);
            Destroy(gameObject);
            Debug.Log("Gate opened!");

            AudioManager.PlaySound(AudioType.GateButton, 1f);
        }
    }
}

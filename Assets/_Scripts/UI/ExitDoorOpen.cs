using UnityEngine;

public class ExitDoorOpen : MonoBehaviour
{
    [SerializeField] private GameObject exit;
    [SerializeField] private GrabObjects grabObjects;

    private void Reset()
    {
        if (exit == null)
            exit = FindFirstObjectByType<ExitWinTrigger>().gameObject;
        if (grabObjects == null)
            grabObjects = FindFirstObjectByType<GrabObjects>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Key")) return;

        exit.SetActive(true);
        gameObject.SetActive(false);
        grabObjects.RemoveObject();
    }
}

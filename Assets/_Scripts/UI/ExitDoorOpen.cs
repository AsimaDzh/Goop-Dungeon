using UnityEngine;

public class ExitDoorOpen : MonoBehaviour
{
    [SerializeField] private GameObject exit;


    private void Reset()
    {
        if (exit == null)
            exit = FindFirstObjectByType<ExitWinTrigger>().gameObject;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Key")) return;

        Destroy(collision.gameObject);
        exit.SetActive(true);
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private readonly HashSet<GameObject> _telepotObjects = new HashSet<GameObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActiveAndEnabled || !gameObject.activeInHierarchy) return;
        if (!collision.CompareTag("Player")) return;
        if (_telepotObjects.Contains(collision.gameObject)) return;

        if (destination.TryGetComponent(out Teleport destinationTeleport))
            destinationTeleport._telepotObjects.Add(collision.gameObject);

        collision.transform.position = destination.position;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        StartCoroutine(WaitAndRemove(collision.gameObject));
    }


    // Wait for the next frame to ensure teleportation is complete
    private IEnumerator WaitAndRemove(GameObject obj)
    {
        yield return null; 
        _telepotObjects.Remove(obj);
    }
}

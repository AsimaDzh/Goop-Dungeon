using UnityEngine;
using System.Collections.Generic;


public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private readonly HashSet<GameObject> _teleportedObjects = new HashSet<GameObject>();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (_teleportedObjects.Contains(collision.gameObject)) return;

        _teleportedObjects.Add(collision.gameObject);

        if (destination.TryGetComponent(out Teleport destinationTeleport))
            destinationTeleport._teleportedObjects.Add(collision.gameObject);

        collision.transform.position = destination.position;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        _teleportedObjects.Remove(collision.gameObject);

        if (destination.TryGetComponent(out Teleport destinationTeleport))
            destinationTeleport._teleportedObjects.Remove(collision.gameObject);
    }
}

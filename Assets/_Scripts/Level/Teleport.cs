using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private HashSet<GameObject> _telepotObjects = new HashSet<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActiveAndEnabled || !gameObject.activeInHierarchy) return;
        if (_telepotObjects.Contains(collision.gameObject)) return;

        if (destination.TryGetComponent(out Teleport destinationTeleport))
            destinationTeleport._telepotObjects.Add(collision.gameObject);

        collision.transform.position = destination.position;
    }
}

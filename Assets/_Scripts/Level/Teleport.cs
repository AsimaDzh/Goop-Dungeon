using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform destination;

    private HashSet<GameObject> _telepotObjects = new HashSet<GameObject>();
}

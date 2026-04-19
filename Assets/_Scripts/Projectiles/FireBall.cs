using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask hitLayers;
}

using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;

    private GameObject _grabbedObject;
    private int _layerIndex;


    private void Start()
    {
        _layerIndex = LayerMask.NameToLayer("Objects");
    }


    private void Update()
    {
        RaycastHit2D _hitInfo = Physics2D.Raycast(
            rayPoint.position, 
            transform.right, 
            rayDistance);
    }
}

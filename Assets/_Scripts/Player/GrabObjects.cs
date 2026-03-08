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

        if (_hitInfo.collider != null && _hitInfo.collider.gameObject.layer == _layerIndex)
        {
            // If the player is not already grabbing an object, grab the new one
            if (_grabbedObject == null)
            {
                _grabbedObject = _hitInfo.collider.gameObject;
                _grabbedObject.GetComponent<BoxCollider2D>().enabled = false;
                _grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;
                _grabbedObject.transform.position = grabPoint.position;
                _grabbedObject.transform.SetParent(transform);
            }

            if (Input.GetKeyDown(KeyCode.E) && _grabbedObject != null)
            {
                _grabbedObject.GetComponent<BoxCollider2D>().enabled = true;
                _grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
                _grabbedObject.transform.SetParent(null);
                _grabbedObject = null;
            }
        }

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.red);
    }
}

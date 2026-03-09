using UnityEngine;

public class GrabObjects : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance;
    [SerializeField] private float throwForce;
    [SerializeField] private LayerMask objectLayer;

    private GameObject _grabbedObject;
    private Rigidbody2D _grabbedRb;
    private BoxCollider2D _grabbedCollider;


    private void Update()
    {
        if (_grabbedObject == null) 
            GrabObject();
        else if (Input.GetKeyDown(KeyCode.E) && _grabbedObject != null)
            ThrowObject();

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.red);
    }


    private void GrabObject()
    {
        RaycastHit2D _hitInfo = Physics2D.Raycast(
            rayPoint.position,
            transform.right,
            rayDistance,
            objectLayer);

        if (_hitInfo.collider == null) return;

        _grabbedObject = _hitInfo.collider.gameObject;
        _grabbedRb = _grabbedObject.GetComponent<Rigidbody2D>();
        _grabbedCollider = _grabbedObject.GetComponent<BoxCollider2D>();

        _grabbedCollider.enabled = false;
        _grabbedRb.isKinematic = true;

        _grabbedObject.transform.position = grabPoint.position;
        _grabbedObject.transform.SetParent(transform);
    }


    private void ThrowObject()
    {
        _grabbedCollider.enabled = true;
        _grabbedRb.isKinematic = false;

        _grabbedObject.transform.SetParent(null);

        _grabbedRb.AddForce((transform.right + transform.up) * throwForce, ForceMode2D.Impulse);

        _grabbedObject = null;
    }
}

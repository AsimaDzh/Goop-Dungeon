using UnityEngine;


public class GrabObjects : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance = 0.2f;

    [SerializeField] private float throwForce = 7f;
    [SerializeField] private float throwAngle = 25f;
    [SerializeField] private float throwOffset = 1.8f;
    [SerializeField] private LayerMask objectLayer;

    private GameObject _grabbedObject;
    private Rigidbody2D _grabbedRb;
    private BoxCollider2D _grabbedCollider;

    public bool IsObjectGrabbed => _grabbedObject != null;
    public GameObject GrabbedObject => _grabbedObject;


    private void FixedUpdate()
    {
        if (_grabbedObject == null) GrabObject();

        Debug.DrawRay(rayPoint.position, transform.right * rayDistance, Color.red);
    }


    public void GrabObject()
    {
        RaycastHit2D _hitInfo = Physics2D.Raycast(
            rayPoint.position,
            transform.right,
            rayDistance,
            objectLayer);

        if (!_hitInfo) return;

        _grabbedObject = _hitInfo.collider.gameObject;
        _grabbedRb = _grabbedObject.GetComponent<Rigidbody2D>();
        _grabbedCollider = _grabbedObject.GetComponent<BoxCollider2D>();

        _grabbedRb.linearVelocity = Vector2.zero;
        _grabbedRb.angularVelocity = 0f;
 
        _grabbedRb.bodyType = RigidbodyType2D.Kinematic;
        _grabbedCollider.enabled = false;

        _grabbedObject.transform.SetParent(grabPoint);
        _grabbedObject.transform.localPosition = Vector3.zero;
    }


    public void ThrowObject()
    {
        _grabbedObject.transform.SetParent(null);

        _grabbedCollider.enabled = true;
        _grabbedRb.bodyType = RigidbodyType2D.Dynamic;

        _grabbedObject.transform.position = grabPoint.position + transform.right * throwOffset;
        Vector2 _direction = (Quaternion.Euler(0, 0, throwAngle) * transform.right).normalized;

        _grabbedRb.AddForce(_direction * throwForce, ForceMode2D.Impulse);

        _grabbedObject = null;
        _grabbedRb = null;
        _grabbedCollider = null;
    }

    
    public void RemoveObject()
    {
        Destroy(_grabbedObject);

        _grabbedObject = null;
        _grabbedCollider = null;
        _grabbedRb = null;
    }
}

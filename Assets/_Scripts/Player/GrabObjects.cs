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
        if (_grabbedObject == null) GrabObject();


        if (Input.GetKeyDown(KeyCode.E) && _grabbedObject != null)
        {
            Rigidbody2D _rb2D = _grabbedObject.GetComponent<Rigidbody2D>();

            _grabbedObject.GetComponent<BoxCollider2D>().enabled = true;
            _rb2D.isKinematic = false;
            _grabbedObject.transform.SetParent(null);
            _grabbedObject = null;

            _rb2D.AddForce((transform.right + transform.up) * throwForce, ForceMode2D.Impulse);
        }

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
}

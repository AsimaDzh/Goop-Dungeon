using UnityEngine;


public class GrabItems : MonoBehaviour
{
    [SerializeField] private Transform grabPoint;
    [SerializeField] private Transform rayPoint;
    [SerializeField] private float rayDistance = 0.2f;

    [SerializeField] private float throwForce = 7f;
    [SerializeField] private float throwAngle = 25f;
    [SerializeField] private float throwOffset = 1.8f;
    [SerializeField] private LayerMask itemLayer;

    private ItemData _itemData;
    private GameObject _grabbedItem;
    private Rigidbody2D _grabbedRb;
    private BoxCollider2D _grabbedCollider;

    public bool IsItemGrabbed => _grabbedItem != null;
    public ItemsNames GrabbedItem => _itemData.itemType;


    private void FixedUpdate()
    {
        if (_grabbedItem == null) 
            GrabObject();
    }


    public void GrabObject()
    {
        RaycastHit2D _hitInfo = Physics2D.Raycast(
            rayPoint.position,
            transform.right,
            rayDistance,
            itemLayer);

        if (!_hitInfo) return;

        _grabbedItem = _hitInfo.collider.gameObject;
        _grabbedRb = _grabbedItem.GetComponent<Rigidbody2D>();
        _grabbedCollider = _grabbedItem.GetComponent<BoxCollider2D>();
        _itemData = _grabbedItem.GetComponent<ItemData>();

        _grabbedRb.linearVelocity = Vector2.zero;
        _grabbedRb.angularVelocity = 0f;
 
        _grabbedRb.bodyType = RigidbodyType2D.Kinematic;
        _grabbedCollider.enabled = false;

        _grabbedItem.transform.SetParent(grabPoint);
        _grabbedItem.transform.localPosition = Vector3.zero;
    }


    public void ThrowObject()
    {
        _grabbedItem.transform.SetParent(null);
        _grabbedCollider.enabled = true;
        _grabbedRb.bodyType = RigidbodyType2D.Dynamic;

        _grabbedItem.transform.position = grabPoint.position + transform.right * throwOffset;

        bool _isFacingRight = transform.localRotation.y >= 0;
        float _angle = _isFacingRight ? throwAngle : -throwAngle;
        
        Vector2 _direction = (Quaternion.Euler(0, 0, _angle) * transform.right).normalized;

        _grabbedRb.AddForce(_direction * throwForce, ForceMode2D.Impulse);
        
        ClearLinks();
    }

    
    public void RemoveObject()
    {
        Destroy(_grabbedItem);
        ClearLinks();
    }


    private void ClearLinks()
    {
        _itemData = null;
        _grabbedItem = null;
        _grabbedCollider = null;
        _grabbedRb = null;
    }
}

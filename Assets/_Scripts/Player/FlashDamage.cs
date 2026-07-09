using UnityEngine;

public class FlashDamage : MonoBehaviour
{
    [SerializeField] private float flashDelay = 1f;

    private SpriteRenderer _spriteRenderer;
    private MaterialPropertyBlock _propertyBlock;
    private float _flashFactor;


    private void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _propertyBlock = new MaterialPropertyBlock();
    }


    public void Flash()
    {
        _flashFactor = 1f;
        ApplyFlashFactor();
    }


    private void ApplyFlashFactor()
    {
        _spriteRenderer.GetPropertyBlock(_propertyBlock);
        _propertyBlock.SetFloat("_FlashFactor", _flashFactor);
        _spriteRenderer.SetPropertyBlock(_propertyBlock);
    }
}

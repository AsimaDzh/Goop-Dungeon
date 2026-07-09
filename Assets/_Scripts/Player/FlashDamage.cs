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


    private void Update()
    {
        if (_flashFactor <= 0f) return;

        _flashFactor = Mathf.Lerp(_flashFactor, 0f, Time.deltaTime * flashDelay);

        if (_flashFactor < 0.01f) 
            _flashFactor = 0f;

        ApplyFlashFactor();
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

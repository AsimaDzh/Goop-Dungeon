using UnityEngine;

public class FlashDamage : MonoBehaviour
{
    [SerializeField] private float flashDelay = 1f;

    private SpriteRenderer[] _spriteRenderers;
    private MaterialPropertyBlock _propertyBlock;
    private float _flashFactor;
}

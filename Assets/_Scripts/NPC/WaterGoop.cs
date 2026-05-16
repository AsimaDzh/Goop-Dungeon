using UnityEngine;


public class WaterGoop : NPCBase
{
    [Header("========== Skill ==========")]
    [SerializeField] private Transform shieldPoint;
    private GameObject _bubbleShield;


    public override void UseSkill()
    {
        if (_bubbleShield != null)
            Destroy(_bubbleShield);

        _bubbleShield = Instantiate(
            NPCData.SkillPrefab, 
            player.transform.position, 
            Quaternion.identity
        );

        _bubbleShield.transform.SetParent(shieldPoint);
        _bubbleShield.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    private void LateUpdate()
    {
        if (_bubbleShield != null)
            _bubbleShield.transform.rotation = Quaternion.identity;
    }
}

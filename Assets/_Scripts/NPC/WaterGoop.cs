using UnityEngine;


public class WaterGoop : NPCBase
{
    [Header("========== Skill ==========")]
    [SerializeField] private Transform shieldPoint;
    [SerializeField] private GameObject bubbleShieldTimer;
    private GameObject _bubbleShield;

    public GameObject Timer => bubbleShieldTimer;


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

        bubbleShieldTimer.SetActive(true);
    }


    private void LateUpdate()
    {
        if (_bubbleShield != null)
            _bubbleShield.transform.rotation = Quaternion.identity;
    }
}

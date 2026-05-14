using UnityEngine;


public class WaterGoop : NPCBase
{
    [SerializeField] private Transform shieldPoint;

    public override void UseSkill()
    {
        GameObject _bubbleShield = Instantiate(
            NPCData.SkillPrefab, 
            player.transform.position, 
            Quaternion.identity
        );
        _bubbleShield.transform.SetParent(shieldPoint);
    }
}

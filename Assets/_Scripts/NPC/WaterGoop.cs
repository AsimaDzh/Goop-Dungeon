using UnityEngine;


public class WaterGoop : NPCBase
{

    public override void UseSkill()
    {
        Instantiate(NPCData.SkillPrefab, player.transform.position, Quaternion.identity);
        NPCData.SkillPrefab.transform.SetParent(player.transform);
    }
}

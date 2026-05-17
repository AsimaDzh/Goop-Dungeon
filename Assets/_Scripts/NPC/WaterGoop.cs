using UnityEngine;


public class WaterGoop : NPCBase
{
    [Header("========== Skill ==========")]
    [SerializeField] private Transform shieldPoint;
    [SerializeField] private GameObject bubbleTimerUI;
    [SerializeField] private BubbleShieldTimer bubbleShieldTimer;
    private GameObject _bubbleShield;

    public GameObject IsShieldActive => bubbleTimerUI;


    public override void UseSkill()
    {
        if (_bubbleShield != null)
        {
            Destroy(_bubbleShield);
            bubbleShieldTimer.ResetTimer();
        }

        _bubbleShield = Instantiate(
            NPCData.SkillPrefab, 
            player.transform.position, 
            Quaternion.identity
        );

        _bubbleShield.transform.SetParent(shieldPoint);
        _bubbleShield.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        bubbleTimerUI.SetActive(true);
    }


    private void LateUpdate()
    {
        if (_bubbleShield != null)
            _bubbleShield.transform.rotation = Quaternion.identity;
    }
}

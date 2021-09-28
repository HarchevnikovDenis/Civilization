using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class WarriorUISlot : MonoBehaviour
{
    [SerializeField] private Text warriorIdText;
    [SerializeField] private Text warriorNameText;
    [SerializeField] private Text warriorStatsText;
    [SerializeField] private Text warriorRoundText;

    private Image slotImage;

    private void Awake()
    {
        slotImage = gameObject.GetComponent<Image>();
    }

    public void SetSlotData(WarriorInfo warriorInfo)
    {
        warriorIdText.text = warriorInfo.Id.ToString();
        warriorNameText.text = warriorInfo.WarriorName;
        warriorStatsText.text = $"Initiative: {warriorInfo.Initiative}, Speed: {warriorInfo.Speed}";
        warriorRoundText.text = warriorInfo.RoundIndex.ToString();

        switch (warriorInfo.SideOfEnemy)
        {
            case EnemySide.RED:
                slotImage.color = Color.red;
                break;
            case EnemySide.BLUE:
                slotImage.color = Color.blue;
                break;
            default:
                break;
        }

        gameObject.name =$"{warriorInfo.RoundIndex}. {warriorInfo.WarriorName} - {warriorInfo.Initiative}:{warriorInfo.Speed}";
    }

    public void ClearData()
    {
        warriorIdText.text = string.Empty;
        warriorNameText.text = string.Empty;
        warriorStatsText.text = string.Empty;
    }
}

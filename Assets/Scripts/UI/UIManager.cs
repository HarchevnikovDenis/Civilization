using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private RoundInfo roundInfoPrefab;
    [SerializeField] private WarriorUISlot warriorUISlotPrefab;
    [SerializeField] private RectTransform content;

    [Header("Game Complete Panel")]
    [SerializeField] private Animator gameCompletePanelAnimator;
    [SerializeField] private Button levelRestartButton;
    [SerializeField] private GameObject redWonTextGameObject;
    [SerializeField] private GameObject blueWonTextGameObject;

    private const string COMPLETE_PANEL_ANIMATOR_BOOL_KEY = "IsShow";

    [Header("Interactive Buttons")]
    [SerializeField] private Button skipButton;
    [SerializeField] private Button killButton;

    [Header("Info")]
    [SerializeField] private Text redWarriorsCountText;
    [SerializeField] private Text blueWarriorsCountText;

    private List<RoundInfo> roundInfoSlots = new List<RoundInfo>();
    private List<WarriorUISlot> warriorUISlots = new List<WarriorUISlot>();

    private List<RoundInfo> roundInfoSlotsPool = new List<RoundInfo>();
    private List<WarriorUISlot> warriorUISlotsPool = new List<WarriorUISlot>();

    protected override void Awake()
    {
        base.Awake();
        InitButtons();
    }

    private void InitButtons()
    {
        skipButton.onClick.AddListener(() => Round.SkipTurn());
        killButton.onClick.AddListener(() => Round.KillNextEnemy());

        levelRestartButton.onClick.AddListener(() =>
        {
            Round.StartGame();
            HideLevelCompletePanel();
        });
    }

    public void UpdateSlots(List<WarriorInfo> warriors)
    {
        UpdatePools();
        int round = warriors.First().RoundIndex - 1;
        int childIndex = 0;

        for (int i = 0; i < warriors.Count; i++)
        {
            if (!warriors[i].RoundIndex.Equals(round))
            {
                RoundInfo roundInfo = null;
                round = warriors[i].RoundIndex;

                if (roundInfoSlotsPool.Count.Equals(0))
                {
                    roundInfo = Instantiate(roundInfoPrefab, content);
                    round = warriors[i].RoundIndex;
                }
                else
                {
                    roundInfo = roundInfoSlotsPool.First();
                    roundInfoSlotsPool.RemoveAt(0);
                    roundInfo.transform.SetSiblingIndex(childIndex);
                    roundInfo.gameObject.SetActive(true);
                }

                roundInfo.SetRoundIndex(round);
                roundInfoSlots.Add(roundInfo);
                childIndex++;
            }

            WarriorUISlot slot = null;
            if (warriorUISlotsPool.Count.Equals(0))
            {
                slot = Instantiate(warriorUISlotPrefab, content);
            }
            else
            {
                slot = warriorUISlotsPool.First();
                warriorUISlotsPool.RemoveAt(0);
                slot.transform.SetSiblingIndex(childIndex);
                slot.gameObject.SetActive(true);
            }

            slot.SetSlotData(warriors[i]);
            warriorUISlots.Add(slot);
            childIndex++;
        }

        content.anchoredPosition = new Vector3(content.anchoredPosition.x, -875.0f, 0.0f);
    }

    private void UpdatePools()
    {
        foreach (RoundInfo roundInfo in roundInfoSlots)
        {
            roundInfoSlotsPool.Add(roundInfo);
            roundInfo.gameObject.SetActive(false);
        }
        roundInfoSlots.Clear();

        foreach (WarriorUISlot warriorUISlot in warriorUISlots)
        {
            warriorUISlotsPool.Add(warriorUISlot);
            warriorUISlot.gameObject.SetActive(false);
        }
        warriorUISlots.Clear();
    }

    public void SetRedWarriorsCount(int count)
    {
        redWarriorsCountText.text = count.ToString();
    }

    public void SetBlueWarriorsCount(int count)
    {
        blueWarriorsCountText.text = count.ToString();
    }

    public void ShowLevelCompletePanel(EnemySide wonSide)
    {
        switch (wonSide)
        {
            case EnemySide.RED:
                redWonTextGameObject.gameObject.SetActive(true);
                break;
            case EnemySide.BLUE:
                blueWonTextGameObject.gameObject.SetActive(true);
                break;
            default:
                break;
        }

        gameCompletePanelAnimator.SetBool(COMPLETE_PANEL_ANIMATOR_BOOL_KEY, true);
    }

    private void HideLevelCompletePanel()
    {
        if (redWonTextGameObject.activeInHierarchy) redWonTextGameObject.SetActive(false);
        if (blueWonTextGameObject.activeInHierarchy) blueWonTextGameObject.SetActive(false);

        gameCompletePanelAnimator.SetBool(COMPLETE_PANEL_ANIMATOR_BOOL_KEY, false);
    }
}

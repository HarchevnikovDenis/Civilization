using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArmyController : MonoBehaviour
{
    [SerializeField] private EnemySide enemySide;
    [SerializeField] private List<WarriorInfo> warriors;

    public List<WarriorInfo> Warriors { get; private set; }
    public int WarriorsCount { get; private set; }

    public void CreateDefaultArmy()
    {
        Warriors = new List<WarriorInfo>();

        for (int i = 0; i < warriors.Count; i++)
        {
            Warriors.Add(new WarriorInfo(1, enemySide, warriors[i]));
        }

        SortByStats();
        WarriorsCount = Warriors.Count;
        UpdateUIStats();
    }

    private void SortByStats()
    {
        Warriors = Warriors.OrderBy(warrior => warrior.Initiative).
                            ThenBy(warrior => warrior.Speed).
                            ThenByDescending(warrior => warrior.Id).
                            ThenBy(warrior => warrior.HasPriority).ToList();
    }

    public void KillTheWarrior()
    {
        WarriorsCount--;
        UpdateUIStats();

        if (WarriorsCount < 0) WarriorsCount = 0;
        if (WarriorsCount.Equals(0)) Round.CompleteGame(enemySide);
    }

    private void UpdateUIStats()
    {
        switch (enemySide)
        {
            case EnemySide.RED:
                UIManager.Instance.SetRedWarriorsCount(WarriorsCount);
                break;
            case EnemySide.BLUE:
                UIManager.Instance.SetBlueWarriorsCount(WarriorsCount);
                break;
        }
    }
}

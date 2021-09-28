using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundController : MonoBehaviour
{
    [SerializeField] private int numberOfShownMoves;
    [SerializeField] private ArmyController redArmyController;
    [SerializeField] private ArmyController blueArmyController;

    [SerializeField] private List<WarriorInfo> sortedWarriorsList;

    private void Awake()
    {
        InitController();
    }

    private void Start()
    {
        Round.StartGame();
    }

    private void InitController()
    {
        Round.InitRoundController(this);
    }

    public void StartGame()
    {
        redArmyController.CreateDefaultArmy();
        blueArmyController.CreateDefaultArmy();


        InitDefaultWarriors();
    }

    private void InitDefaultWarriors()
    {
        sortedWarriorsList = new List<WarriorInfo>();
        sortedWarriorsList.AddRange(redArmyController.Warriors);
        sortedWarriorsList.AddRange(blueArmyController.Warriors);

        SortWarriorsList();

        if (sortedWarriorsList.Count < numberOfShownMoves)
        {
            UpdateInfoToFixedNumberOfShownMoves();   
        }

        UpdateSlots();
    }

    private void UpdateSlots()
    {
        UIManager.Instance.UpdateSlots(sortedWarriorsList);
    }

    private void SortWarriorsList()
    {
        sortedWarriorsList = sortedWarriorsList.OrderByDescending(warrior => warrior.RoundIndex).
                                                ThenBy(warrior => warrior.Initiative).
                                                ThenBy(warrior => warrior.Speed).
                                                ThenBy(warrior => warrior.HasPriority).
                                                ThenByDescending(warrior => warrior.Id).ToList();

        sortedWarriorsList.Reverse();
    }

    public void SkipTurn(bool isNeedUpdateCellsCount = false)
    {
        if (sortedWarriorsList.Count.Equals(0)) return;

        WarriorInfo firstWarrior = sortedWarriorsList.First();
        sortedWarriorsList.Remove(firstWarrior);
        firstWarrior.RoundIndex++;

        if (sortedWarriorsList.Find(warrior => warrior.Equals(firstWarrior)))
        {
            foreach (WarriorInfo warrior in sortedWarriorsList)
            {
                WarriorInfo newWarrior = new WarriorInfo(warrior);
                newWarrior.RoundIndex++;

                if (!sortedWarriorsList.Find(warrior => warrior.Equals(newWarrior)))
                {
                    sortedWarriorsList.Add(newWarrior);
                    break;
                }
            }
        }
        else
        {
            sortedWarriorsList.Add(firstWarrior);
        }

        if (isNeedUpdateCellsCount)
        {
            UpdateInfoToFixedNumberOfShownMoves();
        }
        else
        {
            SortWarriorsList();
        }

        UpdateSlots();
    }

    public void KillEnemy()
    {
        EnemySide killedEnemySide = sortedWarriorsList.First().SideOfEnemy;
        killedEnemySide = SwitchEnemySide(killedEnemySide);

        WarriorInfo killedEnemy = sortedWarriorsList.Find(warrior => warrior.SideOfEnemy.Equals(killedEnemySide));

        foreach (WarriorInfo warrior in sortedWarriorsList)
        {
            if (warrior.SideOfEnemy.Equals(killedEnemy.SideOfEnemy) && warrior.Id.Equals(killedEnemy.Id))
            {
                warrior.IsAlive = false;
            }
        }

        sortedWarriorsList.RemoveAll(warrior => warrior.IsAlive.Equals(false));

        // Update UI
        switch (killedEnemySide)
        {
            case EnemySide.RED:
                redArmyController.KillTheWarrior();
                break;
            case EnemySide.BLUE:
                blueArmyController.KillTheWarrior();
                break;
        }


        SkipTurn(true);
    }

    private void UpdateInfoToFixedNumberOfShownMoves()
    {
        // Update Info to 20 cells
        if (sortedWarriorsList.Count < numberOfShownMoves)
        {
            int initialListLength = redArmyController.WarriorsCount + blueArmyController.WarriorsCount;
            int startRoundIndex = sortedWarriorsList[initialListLength - 1].RoundIndex;
            while (sortedWarriorsList.Count < numberOfShownMoves)
            {
                for (int i = 0; i < initialListLength; i++)
                {
                    if (sortedWarriorsList.Find(warrior => warrior.RoundIndex.Equals(startRoundIndex) &&
                                                            warrior.Id.Equals(sortedWarriorsList[i].Id) &&
                                                            warrior.SideOfEnemy.Equals(sortedWarriorsList[i].SideOfEnemy)))
                    {
                        continue;
                    }
                    else
                    {
                        sortedWarriorsList.Add(new WarriorInfo(startRoundIndex, sortedWarriorsList[i]));
                        if (sortedWarriorsList.Count >= numberOfShownMoves) break;
                    }

                }
                startRoundIndex++;
            }
        }
        
        SortWarriorsList();
    }

    private EnemySide SwitchEnemySide(EnemySide enemySide)
    {
        switch (enemySide)
        {
            case EnemySide.RED:
                return EnemySide.BLUE;
            case EnemySide.BLUE:
                return EnemySide.RED;
        }

        // Default
        return EnemySide.RED;
    }

    public void CompleteGame(EnemySide lostSide)
    {
        EnemySide wonSide = SwitchEnemySide(lostSide);
        UIManager.Instance.ShowLevelCompletePanel(wonSide);
    }
}


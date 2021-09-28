using UnityEngine;

[CreateAssetMenu(fileName = "WarriorInfo", menuName = "Gameplay/New WarriorInfo")]
public class WarriorInfo : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private int initiative;
    [SerializeField] private int speed;

    public readonly string WarriorName;

    public EnemySide SideOfEnemy { get; private set; }
    public int Id => id;
    public int Initiative => initiative;
    public int Speed => speed;
    public bool HasPriority
    {
        get
        {
            return (RoundIndex % 2 == 0 && SideOfEnemy.Equals(EnemySide.BLUE)) ||
                   (RoundIndex % 2 != 0 && SideOfEnemy.Equals(EnemySide.RED));
        }
    }
    public int RoundIndex { get; set; }
    public bool IsAlive { get; set; } = true;

    public WarriorInfo(int roundIndex, EnemySide enemySide, WarriorInfo warriorInfo)
    {
        this.SideOfEnemy = enemySide;
        this.RoundIndex = roundIndex;
        this.id = warriorInfo.Id;
        this.WarriorName = $"{this.SideOfEnemy}_{id}";
        this.initiative = warriorInfo.Initiative;
        this.speed = warriorInfo.Speed;

        this.IsAlive = true;
    }

    public WarriorInfo(int roundIndex, WarriorInfo warriorInfo)
    {
        this.SideOfEnemy = warriorInfo.SideOfEnemy;
        this.RoundIndex = roundIndex;
        this.id = warriorInfo.Id;
        this.WarriorName = $"{this.SideOfEnemy}_{id}";
        this.initiative = warriorInfo.Initiative;
        this.speed = warriorInfo.Speed;

        this.IsAlive = true;
    }

    public WarriorInfo(WarriorInfo warriorInfo)
    {
        this.SideOfEnemy = warriorInfo.SideOfEnemy;
        this.RoundIndex = warriorInfo.RoundIndex;
        this.id = warriorInfo.Id;
        this.WarriorName = $"{this.SideOfEnemy}_{id}";
        this.initiative = warriorInfo.Initiative;
        this.speed = warriorInfo.Speed;

        this.IsAlive = true;
    }

    public bool Equals(WarriorInfo other)
    {
        return SideOfEnemy.Equals(other.SideOfEnemy) &&
               RoundIndex.Equals(other.RoundIndex) &&
               id.Equals(other.Id) &&
               WarriorName.Equals(other.WarriorName) &&
               initiative.Equals(other.Initiative) &&
               speed.Equals(other.Speed);
    }
}

public enum EnemySide
{
    RED,
    BLUE
}

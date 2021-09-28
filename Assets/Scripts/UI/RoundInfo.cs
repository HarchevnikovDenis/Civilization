using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundInfo : MonoBehaviour
{
    [SerializeField] private Text roundText;

    public void SetRoundIndex(int round)
    {
        roundText.text = $"ROUND {round}";
    }
}

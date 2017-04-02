using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreElement : MonoBehaviour {

    public Text Number;
    public Text PlayerName;
    public Text Score;

    public void Init (int number, string player, uint score) {
        Number.text     = number.ToString();
        PlayerName.text = player;
        Score.text      = score.ToString();
    }

    public void Init (string message) {
        Number.text = "";
        PlayerName.text = message;
        Score.text = "";
    }
}

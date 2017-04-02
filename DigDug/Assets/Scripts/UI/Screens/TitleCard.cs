using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCard : MonoBehaviour {

	public void OnClickPlay () {
        UIManager.instance.StartGame();
    }

    public void OnClickScores () {
        UIManager.instance.OpenScores();
    }
}

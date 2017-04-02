using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    public Canvas AddNewScore;
    
    void Start () {
        AddNewScore.enabled = true;
    }

    public void OnClickMenu () {
        UIManager.instance.StartMenu();
    }

    public void OnClickReplay () {
        UIManager.instance.StartGame();
    }
}

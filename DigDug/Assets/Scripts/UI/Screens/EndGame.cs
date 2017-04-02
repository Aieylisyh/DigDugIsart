using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    public RectTransform AddNewScore;
    public RectTransform VictoryUI;
    public RectTransform DefeatUI;

    void Start () {
        AddNewScore.gameObject.SetActive(true);
    }

    public void Init (bool hasWin) {
        VictoryUI.gameObject.SetActive( hasWin );
        DefeatUI.gameObject.SetActive( !hasWin );
    }

    public void OnClickMenu () {
        UIManager.instance.StartMenu();
    }

    public void OnClickReplay () {
        UIManager.instance.StartGame();
    }
}

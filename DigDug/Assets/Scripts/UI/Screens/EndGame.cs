using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGame : MonoBehaviour {

    public RectTransform AddNewScore;
    public RectTransform ScorePanel;
    public RectTransform VictoryUI;
    public RectTransform DefeatUI;

    public Text Score;
    public InputField PlayerNameField;

    void Start () {
        ScorePanel.gameObject.SetActive(false);
        AddNewScore.gameObject.SetActive(true);
        Score.text = GameManager.LastScore.ToString();
    }

    public void Init (bool hasWin) {
        VictoryUI.gameObject.SetActive( hasWin );
        DefeatUI.gameObject.SetActive( !hasWin );
    }

    public void OnClickScorePopinNext () {

        string playerName = PlayerNameField.text != "" ? PlayerNameField.text : "Anonymous";
        SaveManager.SaveScore( playerName, GameManager.LastScore );

        ScorePanel.gameObject.SetActive( true );
        AddNewScore.gameObject.SetActive( false );

        GetComponent<ScoresViewer>().InitScores();
    }

    public void OnClickMenu () {
        UIManager.instance.StartMenu();
    }

    public void OnClickReplay () {
        UIManager.instance.StartGame();
    }
}

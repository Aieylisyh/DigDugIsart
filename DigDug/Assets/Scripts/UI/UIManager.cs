using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public string GameSceneName = "Game";
    public Canvas ScoreMenu;
    public Canvas TitleCardMenu;

    Canvas m_opened;

    void Awake () {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void StartGame () {
        SceneManager.LoadScene( GameSceneName );
    }

    public void OpenScores () {
        CloseMenu();
        m_opened = Instantiate( ScoreMenu );
    }

    public void OpenTitleCard () {
        CloseMenu();
        m_opened = Instantiate( TitleCardMenu );
    }

    public void CloseMenu () {
        if (m_opened != null && m_opened.gameObject != null) Destroy( m_opened.gameObject );
    }
}

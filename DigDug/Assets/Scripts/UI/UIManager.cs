using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public static UIManager instance;

    public string GameSceneName = "Game";
    public string MenuSceneName = "Menu";
    public Canvas TitleCardMenu;
    public Canvas ScoreMenu;
    public Canvas EndGameMenu;
    public Hud HUD; 

    Canvas m_opened;

    void Awake () {
        if (instance == null) instance = this;
        else Destroy(this.gameObject);
    }

    public void StartGame () {
        SceneManager.LoadScene( GameSceneName );
    }

    public void OpenHud() {
        if (Hud.instance == null) {
            Instantiate( HUD );
        }
    }

    public void CloseHud() {
        if (Hud.instance != null) {
            Destroy( Hud.instance.gameObject );
        }
    }

    public void StartMenu () {
        SceneManager.LoadSceneAsync( GameSceneName );
        SceneManager.LoadScene( MenuSceneName );
    }

    public void OpenScores () {
        Open( ScoreMenu );
    }

    public void OpenTitleCard () {
        Open( TitleCardMenu );
    }

    public void OpenEndGame (bool hasWin) {
        Open( EndGameMenu );
        m_opened.GetComponent<EndGame>().Init( hasWin );
    }

    public void CloseMenu () {
        if (m_opened != null && m_opened.gameObject != null) Destroy( m_opened.gameObject );
    }

    void Open (Canvas menu) {
        CloseMenu();
        m_opened = Instantiate( menu );
        if (!m_opened.enabled) m_opened.enabled = true;
    }

    void OnDestroy() {
        instance = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static uint LastScore { get; private set; }

    private uint m_score;

    public uint Score {
        get {
            return m_score;
        }
        set {
            if (Hud.instance != null) Hud.instance.SetScore( value );
            m_score = value;
        }
    }

    public static GameManager instance;
    void Awake () {
        instance = this;
    }

    void Start () {
        UIManager.instance.OpenHud();
        Score = 0;
    }

	public void Win () {
        EndGame( true );
        Invoke("FeedBackWin", 2);
        PlayerAction.instance.OnWin();
    }

    public void GameOver () {
        EndGame( false );
        Invoke("FeedBackGameOver", 2);
    }

    void EndGame (bool hasWin) {
        UIManager.instance.CloseHud();
        LastScore = Score;
    }

    public void CheckWin()
    {
        EnemyAction[] enemies = (EnemyAction[])GameObject.FindObjectsOfType(typeof(EnemyAction));
        print("enemies:"+ enemies.Length);
        if (enemies.Length==0)
        {
            Win();
        }
    }

    private void FeedBackWin()
    {
        if(UIManager.instance)
            UIManager.instance.OpenEndGame(true);
    }
    private void FeedBackGameOver()
    {
        if (UIManager.instance)
            UIManager.instance.OpenEndGame(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static uint LastScore { get; private set; }
    public uint Score { get; set; }
    public static GameManager instance;
    void Awake () {
        Score = 0;
        instance = this;
    }

	public void Win () {
        EndGame( true );
        Invoke("FeedBackWin", 2);
    }

    public void GameOver () {
        EndGame( false );
        Invoke("FeedBackGameOver", 2);
    }

    void EndGame (bool hasWin) {
        Score = (uint)Random.Range( 10, 5490 ); // DEBUG
        LastScore = Score;
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

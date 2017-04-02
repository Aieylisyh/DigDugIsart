using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public uint LastScore { get; private set; }
    public uint Score;

    void Awake () {
        Score = 0;
        Score = (uint)Random.Range(10, 5490); // DEBUG
    }

	public void Win () {
        EndGame( true );
    }

    public void GameOver () {
        EndGame( false );
    }

    void EndGame (bool hasWin) {
        LastScore = Score;
        UIManager.instance.OpenEndGame( hasWin );
    }
}

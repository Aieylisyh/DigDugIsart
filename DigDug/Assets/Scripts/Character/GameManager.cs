using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public void Win () {
        EndGame( true );
    }

    public void GameOver () {
        EndGame( false );
    }

    void EndGame (bool hasWin) {
        UIManager.instance.OpenEndGame( hasWin );
    }
}

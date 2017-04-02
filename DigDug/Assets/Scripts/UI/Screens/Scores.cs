using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scores : MonoBehaviour {

    void Start () {
        GetComponent<ScoresViewer>().InitScores();
    }

    public void OnClickTitleCard () {
        UIManager.instance.OpenTitleCard();
    }
}

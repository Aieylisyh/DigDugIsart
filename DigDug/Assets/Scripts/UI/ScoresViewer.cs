using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresViewer : MonoBehaviour {

    public HorizontalOrVerticalLayoutGroup ScoreLayout;
    public ScoreElement ScoreUIElement;

    [Range(1, 20)]
    public int MaxScoreCount = 5;

    List<SaveManager.Score> m_scores;

    public void InitScores() {
        GetScores();
        FillScores();
    }

    void GetScores () {
        m_scores = SaveManager.GetScores();
    }

    void FillScores () {

        m_scores.Sort(delegate (SaveManager.Score x, SaveManager.Score y) {
            if (x.value < y.value) return 1;
            else if (x.value > y.value) return -1;
            else return 0;
        });

        for (int i = 0; i < m_scores.Count; ++i) {
            ScoreElement se = Instantiate( ScoreUIElement );
            se.transform.SetParent( ScoreLayout.transform );

            if (i < MaxScoreCount) {
                se.Init( i + 1, m_scores[i].name, m_scores[i].value );
            } else {
                se.Init( "..." );
                break;
            }
        }
    }
}

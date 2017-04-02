using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {
    
    public static Hud instance;

    [SerializeField]
    private Text Score;

    public void SetScore (uint value) {
        if (value != 0) {
            Score.text = value.ToString();
        } else {
            Score.text = "";
        }
    }

	void Awake () { 
        if (instance == null) instance = this;
        else Destroy( this.gameObject );
    }
	
	void OnDestroy () {
        instance = null;
    }
}

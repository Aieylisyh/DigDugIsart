using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {
    public static InputManager instance;
    [SerializeField]
    private KeyCode attackKey = KeyCode.Space;
    [HideInInspector]
    public float horizontalAxis
    {
        get
        {
            return Input.GetAxisRaw("Horizontal");
        }
    }

    [HideInInspector]
    public float verticalAxis
    {
        get
        {
            return Input.GetAxisRaw("Vertical");
        }
    }

    private bool _attackIsOn = false;

    [HideInInspector]
    public bool attackIsOn
    {
        get
        {
            if (_attackIsOn)
            {
                _attackIsOn = false;
                return true;
            }
            return false;
        }
    }

    void Awake () {
        if (instance == null)
            instance = this;
	}
	
	// Update is called once per frame
	void Update () {
        _attackIsOn = Input.GetKey(attackKey);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class EventCaller : MonoBehaviour {


    public UnityEvent OnStart;

    void Start () {
        OnStart.Invoke();
	}
}

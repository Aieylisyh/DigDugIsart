using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour {
    protected Animator myAnimator;
    [SerializeField]
    private float endTime = 1f;
    private float elapsedTime;
    [SerializeField]
    private Transform flameTransform;
    // Use this for initialization
    void Start () {
        myAnimator = GetComponentInChildren<Animator>();
        if (!myAnimator)
            Debug.LogError("myAnimator is not set!");
        elapsedTime = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        elapsedTime += Time.fixedDeltaTime;
        myAnimator.Play("flame", 0, elapsedTime / endTime);

        if (elapsedTime / endTime > 9f / 12)
        {
            flameTransform.localPosition = Vector3.right * 1.65f;
        }
        else if(elapsedTime / endTime > 7f / 12)
        {
            flameTransform.localPosition = Vector3.right * 1.2f;
        }
        else if (elapsedTime / endTime > 5f / 12)
        {
            flameTransform.localPosition = Vector3.right * 1.65f;
        }
        else if (elapsedTime / endTime > 3f / 12)
        {
            flameTransform.localPosition = Vector3.right * 1.2f;
        }
        else
        {
            flameTransform.localPosition = Vector3.right * 0.86f;
        }
        if (elapsedTime > endTime)
        {
            Destroy(this.gameObject);
        }
    }
}

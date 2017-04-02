using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUIScaleShake : MonoBehaviour {
    public bool willShake = false;
    public bool alwaysShake = false;
    private RectTransform myRectTransform;
    private Vector3 defaultScale;
    [SerializeField]
    private Vector3 targetScale;
    [SerializeField]
    private float scaleTime = 0.1f;
    private Vector3 speedScale;
    private enum ShakeState { Stop, Go, Back}
    //[SerializeField]
    private ShakeState myShakeState = ShakeState.Stop;
    private enum ShakeTypePerAxis { None, Increase, Decrease }
    private ShakeTypePerAxis shakeTypePerAxisX = ShakeTypePerAxis.None;
    private ShakeTypePerAxis shakeTypePerAxisY = ShakeTypePerAxis.None;
    // Use this for initialization
    void Awake () {
        myRectTransform = GetComponent<RectTransform>();
        SetTargetScale(targetScale);
    }
	
    public void SetScaleTime(float value)
    {
        scaleTime = value;
        speedScale = (targetScale - defaultScale) / scaleTime;
    }

    public void SetTargetScale(Vector3 pTargetScale)
    {
        targetScale = pTargetScale;
        defaultScale = myRectTransform.localScale;
        speedScale = (targetScale - defaultScale) / scaleTime;
        if (targetScale.x > defaultScale.x)
            shakeTypePerAxisX = ShakeTypePerAxis.Increase;
        if (targetScale.x < defaultScale.x)
            shakeTypePerAxisX = ShakeTypePerAxis.Decrease;
        if (targetScale.y > defaultScale.y)
            shakeTypePerAxisY = ShakeTypePerAxis.Increase;
        if (targetScale.y < defaultScale.y)
            shakeTypePerAxisY = ShakeTypePerAxis.Decrease;
    }

    // Update is called once per frame
    void Update () {
        if (willShake)
        {
            willShake = false;
            myShakeState = ShakeState.Go;
        }
        if(alwaysShake && myShakeState == ShakeState.Stop)
            myShakeState = ShakeState.Go;
        if (myShakeState == ShakeState.Go)
        {
            myRectTransform.localScale = myRectTransform.localScale + speedScale * Time.deltaTime;
            if ((targetScale.x < myRectTransform.localScale.x && shakeTypePerAxisX== ShakeTypePerAxis.Increase)
                || (targetScale.x > myRectTransform.localScale.x && shakeTypePerAxisX == ShakeTypePerAxis.Decrease)
                || (targetScale.y < myRectTransform.localScale.y && shakeTypePerAxisY == ShakeTypePerAxis.Increase)
                || (targetScale.y > myRectTransform.localScale.y && shakeTypePerAxisY == ShakeTypePerAxis.Decrease)) 
            {
                myShakeState = ShakeState.Back;
                myRectTransform.localScale = targetScale;
            }
        }
        if (myShakeState == ShakeState.Back)
        {
            myRectTransform.localScale = myRectTransform.localScale - speedScale * Time.deltaTime;
            if (   (defaultScale.x > myRectTransform.localScale.x && shakeTypePerAxisX == ShakeTypePerAxis.Increase)
                || (defaultScale.x < myRectTransform.localScale.x && shakeTypePerAxisX == ShakeTypePerAxis.Decrease)
                || (defaultScale.y > myRectTransform.localScale.y && shakeTypePerAxisY == ShakeTypePerAxis.Increase)
                || (defaultScale.y < myRectTransform.localScale.y && shakeTypePerAxisY == ShakeTypePerAxis.Decrease))
            {
                if(alwaysShake)
                    myShakeState = ShakeState.Go;
                else
                    myShakeState = ShakeState.Stop;
                myRectTransform.localScale = defaultScale;
            }
        }
    }
}

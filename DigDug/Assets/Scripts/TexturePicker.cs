using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
[ExecuteInEditMode]
public class TexturePicker : MonoBehaviour {
    //[SerializeField]
    [Range(5, 200)]
    [Tooltip("enter the width if the sprite")]
    public int spriteWidth = 64;
    //[SerializeField]
    [Range(5, 200)]
    [Tooltip("enter the height if the sprite")]
    public int spriteHeight = 64;
    //[SerializeField]
    [Tooltip("enter the left pixel count if the sprite")]
    public int spriteLeft = 0;
    //[SerializeField]
    [Tooltip("enter the top pixel count if the sprite")]
    public int spriteTop = 0;


    public void ExportTexture() {
        Debug.Log("ExportTexture");
    }
    public void ApplyTexture()
    {
        Debug.Log("ApplyTexture");
    }
    // Update is called once per frame
    void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[RequireComponent(typeof(MeshRenderer))]
[CustomEditor(typeof(TexturePicker))]
public class TexturePickerEditor : Editor {
    public List<Texture2D> listTextures = new List<Texture2D>();
    public override void OnInspectorGUI()
    {

        TexturePicker myScript = (TexturePicker)target;
        //myScript.experience = EditorGUILayout.IntField("Experience", myScript.experience);
        //EditorGUILayout.LabelField("Level", myScript.Level.ToString());

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Export Texture"))
        {
            myScript.ExportTexture();
        }
        EditorGUILayout.EndHorizontal();
        if (listTextures.Count == 0)
        {
            FillLTexture(myScript);
        }
        bool foldOutValue = true;
        foldOutValue = EditorGUILayout.Foldout(foldOutValue, "textures");
        if (foldOutValue)
        {
            EditorGUILayout.BeginHorizontal();
            int count = 0;
            foreach(Texture2D tx in listTextures)
            {
                if (GUILayout.Button(tx))
                {

                }
                if (count == 3)
                {
                    EditorGUILayout.EndHorizontal();
                    count = 0;
                    EditorGUILayout.BeginHorizontal();
                }
                    
                count++;
            }
            EditorGUILayout.EndHorizontal();
        }
        base.OnInspectorGUI();
    }

    void FillLTexture(TexturePicker myTexturePicker)
    {
        MeshRenderer mr = myTexturePicker.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            Texture2D tx = mr.material.mainTexture as Texture2D;
            if (tx != null)
            {
                int width = tx.width;
                int height = tx.height;
                int x = 0;
                int y = 0;
                var colors = tx.GetPixels(x, y, 64, 64);
                Texture2D newTexture = new Texture2D(64, 64);
                newTexture.SetPixels(colors);
                newTexture.Apply();
                listTextures.Add(newTexture);
            }
        }
    }
}

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor( typeof(MeshCreator) )]
public class MeshCreatorEditor : Editor {



    List<Texture2D> m_textures = new List<Texture2D>();

    //bool foldOut = false;

    public override void OnInspectorGUI () {
        
        MeshCreator world = (MeshCreator)target;

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button( "test" )) {
            ClearWorld( world );
            BuildWorld( world );
        }

        if (GUILayout.Button( "clear" )) {
            ClearWorld( world );
        }

        EditorGUILayout.EndHorizontal();
        


        if (m_textures.Count == 0) {
            FillTextures( world );
        }

        EditorGUILayout.BeginHorizontal();
        ushort counter = 0;
        for (int i = 0; i < m_textures.Count; i++) {

            if (GUILayout.Button( m_textures[i] )) {
                // do something
            }

            if (++counter == 4) {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                counter = 0;
            }

        }
        EditorGUILayout.EndHorizontal();

        base.OnInspectorGUI();


    }
    void FillTextures (MeshCreator world) {

        MeshRenderer mr = world.GetComponent<MeshRenderer>();

        if (mr != null) {

            Texture2D tx = (Texture2D)mr.material.mainTexture;

            if (tx != null) {

                int width = tx.width;
                int height = tx.height;

                for (int x = 0; x < width; x += 64) {
                    for (int y = 0; y < height; y += 64) {

                        var colors = tx.GetPixels( x, y, 64, 64 );
                        Texture2D newTexture = new Texture2D( 64, 64 );
                        newTexture.SetPixels( colors );
                        newTexture.Apply(); // NE PAS OUBLIER SINON IL NE PREND PAS EN COMPTE LES MODIFS

                        m_textures.Add( newTexture );
                    }
                }
            }
        }
    }

    public void BuildWorld (MeshCreator world) {
        world.SendMessage( "Awake" );
        world.SendMessage( "Start" );
    }

    public void ClearWorld (MeshCreator world) {
        world.Clear();
    }
}

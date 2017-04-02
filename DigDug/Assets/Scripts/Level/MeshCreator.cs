using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class MeshCreator : MonoBehaviour {

    public enum MAP_TYPE {
        EMPTY = 0,
        DIRT = 1,
        STONE = 2
    }

    public byte MapWidth;
    public byte MapHeight;

    public float m_unitTexture = 0.0625f;
    public Vector2 Dirt  = new Vector2( 0, 0 );
    public Vector2 Grass = new Vector2( 0, 0 );
    public Vector2 Stone = new Vector2( 0, 0 );

    private Mesh m_MeshRef;
    private MeshCollider m_ColRef;

    private List<Vector3> m_Vertices = new List<Vector3>();
    private List<int> m_Triangles    = new List<int>();
    private int faceCounter = 0;
    private List<Vector2> m_UV       = new List<Vector2>();

    // vertices et triangles différents pour le mesh collider parce que le mesh est en 2D mais on veut nos collision en 3D. Sinon on pourait carrément réutiliser les vertices et triangles du mesh
    private List<Vector3> m_ColVertices = new List<Vector3>();
    private List<int> m_ColTriangles    = new List<int>();
    private int faceColliderCounter = 0;
    
    private byte[,] Blocks;

    public void RemoveBlock (int x, int y) {
        if (GetBlockType(x, y) != MAP_TYPE.EMPTY) {
            Blocks[x, y] = (byte)MAP_TYPE.EMPTY;
            UpdateVisualMap();
        }
    }

    public bool RemoveBlock (Vector2[] positions) {

        bool changed = false;

        for (int i = 0; i < positions.Length; i++) {
            if (GetBlockType( Mathf.RoundToInt(positions[i].x), Mathf.RoundToInt( positions[i].y) ) != MAP_TYPE.EMPTY) {
                Blocks[Mathf.RoundToInt( positions[i].x), Mathf.RoundToInt( positions[i].y)] = (byte)MAP_TYPE.EMPTY;
                changed = true;
            }
        }

        if (changed) {
            UpdateVisualMap();
        }
        return changed;
    }

    public MAP_TYPE GetBlockType (int x, int y) {
        if (x < 0 || x >= Blocks.GetLength( 0 ) ||
            y < 0 || y >= Blocks.GetLength( 1 )) {
            return MAP_TYPE.EMPTY;
        } else {
            return (MAP_TYPE)Blocks[x, y];
        }
    }

    public void Clear () {
        m_MeshRef.Clear();
        if (m_ColRef.sharedMesh != null) m_ColRef.sharedMesh.Clear();
        m_ColVertices.Clear();
        m_ColTriangles.Clear();
        faceColliderCounter = 0;
        m_Vertices.Clear();
        m_Triangles.Clear();
        m_UV.Clear();
        faceCounter = 0;
    }

    // Use this for initialization
    void Start () {

        m_MeshRef = GetComponent<MeshFilter>().sharedMesh;
        m_ColRef = GetComponent<MeshCollider>();
        
        MapGeneration();
        UpdateVisualMap();
    }

    void UpdateVisualMap () {
        Clear();
        ConstructMapMesh();
        GenerateMesh();
    }

    void BuildMesh (int x, int y, Vector2 texture) {

        m_Vertices.Add( new Vector3( x+0, y+0, 0 ) );
        m_Vertices.Add( new Vector3( x+0, y+1, 0 ) );
        m_Vertices.Add( new Vector3( x+1, y+1, 0 ) );
        m_Vertices.Add( new Vector3( x+1, y+0, 0 ) );

        // triangles : 3 index de m_Vertices pour décrire 1 triangle

        // triangle 1
        m_Triangles.Add( faceCounter * 4 + 0 ); 
        m_Triangles.Add( faceCounter * 4 + 1 ); 
        m_Triangles.Add( faceCounter * 4 + 2 ); 

        // triangle 2
        m_Triangles.Add( faceCounter * 4 + 0 ); 
        m_Triangles.Add( faceCounter * 4 + 2 ); 
        m_Triangles.Add( faceCounter * 4 + 3 );

        // uv 1 point par vertice : les points correspondes aux vertices mais ne doit pas etre négatif !
        /*
        m_UV.Add( new Vector2( 0 * m_unitTexture, 0 * m_unitTexture ) );
        m_UV.Add( new Vector2( 0 * m_unitTexture, 1 * m_unitTexture ) );
        m_UV.Add( new Vector2( 1 * m_unitTexture, 1 * m_unitTexture ) );
        m_UV.Add( new Vector2( 1 * m_unitTexture, 0 * m_unitTexture ) );
        */
        m_UV.Add( new Vector2( texture.x * m_unitTexture,                   texture.y * m_unitTexture ) );
        m_UV.Add( new Vector2( texture.x * m_unitTexture,                   texture.y * m_unitTexture + m_unitTexture ) );
        m_UV.Add( new Vector2( texture.x * m_unitTexture + m_unitTexture,   texture.y * m_unitTexture + m_unitTexture ) );
        m_UV.Add( new Vector2( texture.x * m_unitTexture + m_unitTexture,   texture.y * m_unitTexture ) );

        faceCounter++;
    }

    void BuildCollider (int mx, int my) {
        if (GetBlockType( mx, my - 1 ) == MAP_TYPE.EMPTY) BuildColliderBottom( mx, my );
        if (GetBlockType( mx, my + 1 ) == MAP_TYPE.EMPTY) BuildColliderTop( mx, my );
        if (GetBlockType( mx - 1, my ) == MAP_TYPE.EMPTY) BuildColliderLeft( mx, my );
        if (GetBlockType( mx + 1, my ) == MAP_TYPE.EMPTY) BuildColliderRight( mx, my );
    }

    void BuildColliderTop (int x, int y) {
        m_ColVertices.Add( new Vector3( x + 0, y + 1, 0 ) );
        m_ColVertices.Add( new Vector3( x + 0, y + 1, 1 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 1, 1 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 1, 0 ) );
        ColliderTriangles();
    }

    void BuildColliderBottom (int x, int y) {
        m_ColVertices.Add( new Vector3( x + 0, y + 0, 0 ) );
        m_ColVertices.Add( new Vector3( x + 0, y + 0, 1 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 0, 1 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 0, 0 ) );
        ColliderTriangles();
    }

    void BuildColliderLeft (int x, int y) {
        m_ColVertices.Add( new Vector3( x + 0, y + 0, 0 ) );
        m_ColVertices.Add( new Vector3( x + 0, y + 0, 1 ) );
        m_ColVertices.Add( new Vector3( x + 0, y + 1, 1 ) );
        m_ColVertices.Add( new Vector3( x + 0, y + 1, 0 ) );
        ColliderTriangles();
    }

    void BuildColliderRight (int x, int y) {
        m_ColVertices.Add( new Vector3( x + 1, y + 0, 0 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 0, 1 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 1, 1 ) );
        m_ColVertices.Add( new Vector3( x + 1, y + 1, 0 ) );
        ColliderTriangles();
    }

    void ColliderTriangles () {
        
        m_ColTriangles.Add( faceColliderCounter * 4 + 0 );
        m_ColTriangles.Add( faceColliderCounter * 4 + 1 );
        m_ColTriangles.Add( faceColliderCounter * 4 + 2 );
        m_ColTriangles.Add( faceColliderCounter * 4 + 0 );
        m_ColTriangles.Add( faceColliderCounter * 4 + 2 );
        m_ColTriangles.Add( faceColliderCounter * 4 + 3 );

        faceColliderCounter++;
    }

    void GenerateMesh () {

        // colision
        Mesh colMesh = new Mesh();
        colMesh.vertices  = m_ColVertices.ToArray();
        colMesh.triangles = m_ColTriangles.ToArray();
        m_ColRef.sharedMesh = colMesh;

        m_MeshRef.vertices  = m_Vertices.ToArray();
        m_MeshRef.triangles = m_Triangles.ToArray();
        m_MeshRef.uv        = m_UV.ToArray();
        ;
        m_MeshRef.RecalculateNormals();
    }


    [Header( "-- Stone" )]

    public float StoneScale1 = 0.8f;
    public float StoneAmplitude1 = 15f;
    public float StoneAmplitude2 = 15f;

    public float StoneHoleScale1 = 0.2f;
    public float StoneHoleAmplitude1 = 15f;
    public float StoneHoleLimite = 5f;

    [Header("-- Dirt")]

    public float DirtScale1 = 0.8f;
    public float DirtAmplitude1 = 15f;
    public float DirtAmplitude2 = 15f;

    public float DirtHoleScale1 = 0.2f;
    public float DirtHoleAmplitude1 = 15f;
    public float DirtHoleLimite = 3f;

    [Header( "-- Iland" )]

    public float IlandScale1 = 0.2f;
    public float IlandAmplitude1 = 15f;
    public float IlandLimite = 3f;
    public float IlandLimiteMin = 3f;
    public float IlandMaxHeight = 3f;
    public float IlandMinHeight = 1f;

    void MapGeneration () {

        Blocks = new byte[MapWidth, MapHeight];

        int randXStone = 0;//Random.Range(0, MapWidth);
        int randXDirt = 0;

        for (int mx = 0; mx < Blocks.GetLength(0); mx++) {

            int stone = Noise( mx + randXStone, 0,      MapHeight * StoneScale1,     StoneAmplitude1,     1 );
            stone    += Noise( mx + randXStone, 0,           MapHeight / 2,     StoneAmplitude2,     1 );

            //stone += Noise( mx, Mathf.FloorToInt( MapHeight * 0.75f ), MapHeight / 2, 30 );

            int dirt = Noise( mx + randXDirt, 0, MapHeight * DirtScale1, DirtAmplitude1, 1 );
            dirt    += Noise( mx + randXDirt, 0, MapHeight / 2, DirtAmplitude2, 1 );

            for (int my = 0; my < Blocks.GetLength(1); my++) {

                if (my < stone) {

                    // STONE

                    Blocks[mx, my] = (byte)MAP_TYPE.STONE;

                    if (Noise( mx, my, MapWidth * StoneHoleScale1, StoneHoleAmplitude1, 1 ) < StoneHoleLimite) {
                        Blocks[mx, my] = (byte)MAP_TYPE.EMPTY;
                    }


                } else {


                    if (my < dirt) {

                        // DIRT
                        
                        Blocks[mx, my] = (byte)MAP_TYPE.DIRT;

                        if (Noise( mx, my, MapWidth * DirtHoleScale1, DirtHoleAmplitude1, 1 ) < DirtHoleLimite) {
                            Blocks[mx, my] = (byte)MAP_TYPE.EMPTY;
                        }

                    } else {

                        // AIR

                        Blocks[mx, my] = (byte)MAP_TYPE.EMPTY;

                        if (my < IlandMaxHeight && my > IlandMinHeight) {

                            float iland = Noise( mx, my, MapWidth * IlandScale1, IlandAmplitude1, 1 );
                            if (iland < IlandLimite && iland > IlandLimiteMin) {
                                Blocks[mx, my] = (byte)MAP_TYPE.DIRT;
                            }
                        }
                    }
                    
                }

            }
            
        }
    }

    int Noise (int x, int y, float scale, float mag, float exp = 1) {
        return Mathf.FloorToInt(Mathf.Pow( Mathf.PerlinNoise(x / scale, y / scale) * mag, exp ));
    }

    void ConstructMapMesh () {

        for (int mx = 0; mx < Blocks.GetLength( 0 ); mx++) {

            for (int my = 0; my < Blocks.GetLength( 1 ); my++) {

                if (GetBlockType( mx, my ) == MAP_TYPE.DIRT) {
                    

                    if (GetBlockType( mx, my + 1 ) != MAP_TYPE.EMPTY) {
                        BuildMesh( mx, my, Dirt );
                    } else {
                        BuildMesh( mx, my, Grass );
                    }

                    BuildCollider( mx, my );
                } else if (GetBlockType( mx, my ) == MAP_TYPE.STONE) {
                    BuildMesh( mx, my, Stone );
                    BuildCollider( mx, my );
                }


            }
        }
    }
}

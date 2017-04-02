using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Pick : MonoBehaviour {

    public MeshCreator m_world;

    private Transform m_t;
    private BoxCollider m_box;
    [SerializeField]
    private float m_gap = -0.5f;
    public static Pick instance;
    [HideInInspector]
    public bool isDigging =false;
    private Vector2 lastPosition;
    private bool digged = false;
    void Awake () {
        m_box = GetComponent<BoxCollider>();
        m_t = transform;
        //m_gap = -transform.localPosition.x;
        if (m_world == null)
            Debug.LogWarning("Can not dig without World instance");
        if (instance)
            Debug.LogError("multi instance if player found!");
        instance = this;
        lastPosition = new Vector2(Mathf.RoundToInt(m_t.position.x - m_gap), Mathf.RoundToInt(m_t.position.y - m_gap));
    }

    void FixedUpdate () {
        if(m_world)
            Dig( m_world );
    }

    void Dig (MeshCreator world) {
        Vector2 newPosition = new Vector2(Mathf.RoundToInt(m_t.position.x - m_gap), Mathf.RoundToInt(m_t.position.y - m_gap));
        bool isDiggingThisFrame = world.RemoveBlock(new Vector2[4] {
            new Vector2( (m_t.position.x - m_box.bounds.extents.x) + m_gap, (m_t.position.y - m_box.bounds.extents.y) + m_gap ),
            new Vector2( (m_t.position.x - m_box.bounds.extents.x) + m_gap, (m_t.position.y + m_box.bounds.extents.y) + m_gap ),
            new Vector2( (m_t.position.x + m_box.bounds.extents.x) + m_gap, (m_t.position.y - m_box.bounds.extents.y) + m_gap ),
            new Vector2( (m_t.position.x + m_box.bounds.extents.x) + m_gap, (m_t.position.y + m_box.bounds.extents.y) + m_gap )
            }
        );
        if (isDiggingThisFrame)
            digged = true;
        if (newPosition != lastPosition)
        {
            isDigging = digged;
            digged = false;
        }
        lastPosition = newPosition;
    }
}

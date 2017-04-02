using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Pick : MonoBehaviour {

    public MeshCreator m_world;

    private Transform m_t;
    private BoxCollider m_box;
    private float m_gap;
    public static Pick instance;
    //[HideInInspector]
    public bool isDigging =false;
    private Vector2 lastPositions;
    private bool digged = false;
    void Awake () {
        m_box = GetComponent<BoxCollider>();
        m_t = transform;
        m_gap = transform.position.x;
        if (m_world == null)
            Debug.LogWarning("Can not dig without World instance");
        if (instance)
            Debug.LogError("multi instance if player found!");
        instance = this;
        lastPositions = new Vector2(Mathf.RoundToInt(m_t.position.x - m_gap), Mathf.RoundToInt(m_t.position.y - m_gap));
    }

    void FixedUpdate () {
       Dig( m_world );
    }

    void Dig (MeshCreator world) {
        Vector2 newPositions = new Vector2(Mathf.RoundToInt(m_t.position.x - m_gap), Mathf.RoundToInt(m_t.position.y - m_gap));
        bool isDiggingThisFrame = world.RemoveBlock(new Vector2[4] {
            new Vector2(Mathf.RoundToInt((m_t.position.x - m_box.bounds.extents.x) - m_gap), Mathf.RoundToInt((m_t.position.y - m_box.bounds.extents.y) - m_gap)),
            new Vector2(Mathf.RoundToInt((m_t.position.x - m_box.bounds.extents.x) - m_gap), Mathf.RoundToInt((m_t.position.y + m_box.bounds.extents.y) - m_gap)),
            new Vector2(Mathf.RoundToInt((m_t.position.x + m_box.bounds.extents.x) - m_gap), Mathf.RoundToInt((m_t.position.y - m_box.bounds.extents.y) - m_gap)),
            new Vector2(Mathf.RoundToInt((m_t.position.x + m_box.bounds.extents.x) - m_gap), Mathf.RoundToInt((m_t.position.y + m_box.bounds.extents.y) - m_gap))
        });
        if (isDiggingThisFrame)
            digged = true;
        if (newPositions != lastPositions)
        {
            isDigging = digged;
            digged = false;
        }
        lastPositions = newPositions;
    }
}

using UnityEngine;
using System.Collections;

public class Pick : MonoBehaviour {

    public MeshCreator m_world;

    private Transform m_t;
    private BoxCollider m_box;
    private float m_gap;

    void Awake () {
        m_box = GetComponent<BoxCollider>();
        m_t = transform;
        m_gap = -transform.position.x;

        if (m_world == null) Debug.LogWarning("Can not dig without World instance");
    }

    void Update () {
       if (m_world != null) Dig( m_world );
    }

    void Dig (MeshCreator world) {

        world.RemoveBlock( new Vector2[4] {
            new Vector2( (m_t.position.x - m_box.bounds.extents.x) + m_gap, (m_t.position.y - m_box.bounds.extents.y) + m_gap ),
            new Vector2( (m_t.position.x - m_box.bounds.extents.x) + m_gap, (m_t.position.y + m_box.bounds.extents.y) + m_gap ),
            new Vector2( (m_t.position.x + m_box.bounds.extents.x) + m_gap, (m_t.position.y - m_box.bounds.extents.y) + m_gap ),
            new Vector2( (m_t.position.x + m_box.bounds.extents.x) + m_gap, (m_t.position.y + m_box.bounds.extents.y) + m_gap )
        } );
    }
}

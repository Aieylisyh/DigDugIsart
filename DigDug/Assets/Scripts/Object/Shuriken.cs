using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour {
    [SerializeField]
    private Transform rotatePartTransform;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float moveSpeed;
    [HideInInspector]
    public Vector3 direction = Vector3.right;
    protected MeshCreator m_world;
    private void Start()
    {
        Destroy(this.gameObject, 5);
        m_world = MeshCreator.instance;
    }

    void FixedUpdate () {
        transform.position +=moveSpeed * Time.fixedDeltaTime * direction;
        rotatePartTransform.eulerAngles += Time.fixedDeltaTime * rotateSpeed* (new Vector3(0,0,1));
        if(m_world.GetBlockType(Mathf.RoundToInt(transform.position.x-0.5f), Mathf.RoundToInt(transform.position.y - 0.5f)) != MeshCreator.MAP_TYPE.EMPTY)
        {
            Destroy(this.gameObject);
        }
    }
}

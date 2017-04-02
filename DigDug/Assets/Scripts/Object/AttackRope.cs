using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRope : MonoBehaviour {
    [SerializeField]
    private float lifeTime;
    private float startTime;
    private float targetScale;
    [SerializeField]
    private float positionOffsetFactor=0.1f;
    private float m_scale = 1f;
    protected MeshCreator m_world;
    [SerializeField]
    private Transform attackPartTransform;
    public void Init (CharacterAction.Direction myDirection, float length, Vector3 position, float scale) {
        m_scale = scale;
        Vector3 positionOffset = Vector3.zero;
		if(myDirection == CharacterAction.Direction.Down)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            positionOffset = Vector3.down * positionOffsetFactor;
        }
        else if (myDirection == CharacterAction.Direction.Left)
        {
            transform.eulerAngles = new Vector3(0, 0, -90);
            positionOffset = Vector3.left * positionOffsetFactor;
        }
        else if (myDirection == CharacterAction.Direction.Up)
        {
            transform.eulerAngles = new Vector3(0, 0, 180);
            positionOffset = Vector3.up * positionOffsetFactor;
        }
        else if (myDirection == CharacterAction.Direction.Right)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
            positionOffset = Vector3.right * positionOffsetFactor;
        }
        transform.position = position + positionOffset;
        targetScale = (length / GetComponent<SpriteRenderer>().sprite.bounds.size.y) * m_scale;
        //print(GetComponent<SpriteRenderer>().sprite.bounds.size);
        transform.localScale = new Vector3( m_scale, 0, m_scale );
        startTime = Time.fixedTime;
        m_world = MeshCreator.instance;
    }
	
	void FixedUpdate () {
        float factor = (Time.fixedTime- startTime) / lifeTime;
        if (factor > 1)
        {
            Destroy(this.gameObject);
        }else
        {
            transform.localScale = new Vector3( m_scale, factor * targetScale, m_scale );
        }
        if (m_world.GetBlockType(Mathf.RoundToInt(attackPartTransform.position.x - 0.5f), Mathf.RoundToInt(attackPartTransform.position.y - 0.5f)) != MeshCreator.MAP_TYPE.EMPTY)
        {
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;

public class AsteroidField : MonoBehaviour {
	[SerializeField]
	private GameObject m_blockPrefab;

	protected void Awake() {
		AsteroidFieldBlock block = CreateBlock();
		block.Initiate();
	}

	private AsteroidFieldBlock CreateBlock() {
		GameObject go =  Instantiate(m_blockPrefab);
		AsteroidFieldBlock block = go.GetComponent<AsteroidFieldBlock>();
		go.transform.parent = transform;
		return block;
	}

}

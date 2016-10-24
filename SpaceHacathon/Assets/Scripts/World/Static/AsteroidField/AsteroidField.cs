using UnityEngine;

public class AsteroidField : MonoBehaviour {
	[SerializeField]
	private GameObject m_blockPrefab;

	protected void Awake() {
		AsteroidFieldBlock block = CreateBlock();
		block.Initiate();

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(100, 0 , 0));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(100, 0 , 100));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(100, 0 , -100));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(0, 0 , 100));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(0, 0 , -100));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(-100, 0 , 0));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(-100, 0 , 100));

		block = CreateBlock();
		block.Initiate();
		block.MoveTo(new Vector3(-100, 0 , -100));
	}

	private AsteroidFieldBlock CreateBlock() {
		GameObject go =  Instantiate(m_blockPrefab);
		AsteroidFieldBlock block = go.GetComponent<AsteroidFieldBlock>();
		go.transform.parent = transform;
		return block;
	}

}

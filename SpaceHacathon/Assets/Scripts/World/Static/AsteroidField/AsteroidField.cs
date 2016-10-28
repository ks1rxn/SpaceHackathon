using UnityEngine;

public class AsteroidField : MonoBehaviour {
	[SerializeField]
	private GameObject m_blockPrefab;

	private AsteroidFieldBlock[,] m_blocks;

	protected void Awake() {
		m_blocks = new AsteroidFieldBlock[3, 3];

		for (int i = 0; i != 3; i++) {
			for (int g = 0; g != 3; g++) {
				AsteroidFieldBlock block = CreateBlock();
				block.Initiate();
				block.MoveTo(new Vector3((i - 1) * 100, 0, (g - 1) * 100));
				m_blocks[i, g] = block;
			}
		}
	}

	protected void Update() {
		if (!m_blocks[1, 1].IsShipOnBlock()) {
			FixBlocks();
		}
		ShipPositionOnBlock position = m_blocks[1, 1].GetShipPositionOnBlock();
		if (position == ShipPositionOnBlock.NotOnBlock) {
			return;
		}
		switch (position) {
			case ShipPositionOnBlock.BottomLeft:
				m_blocks[1, 1].UpdateGroups();
				m_blocks[0, 1].UpdateGroups();
				m_blocks[1, 0].UpdateGroups();
				m_blocks[0, 0].UpdateGroups();
				break;
			case ShipPositionOnBlock.TopLeft:
				m_blocks[1, 1].UpdateGroups();
				m_blocks[0, 1].UpdateGroups();
				m_blocks[0, 2].UpdateGroups();
				m_blocks[1, 2].UpdateGroups();
				break;
			case ShipPositionOnBlock.TopRight:
				m_blocks[1, 1].UpdateGroups();
				m_blocks[1, 2].UpdateGroups();
				m_blocks[2, 1].UpdateGroups();
				m_blocks[2, 2].UpdateGroups();
				break;
			case ShipPositionOnBlock.BottomRight:
				m_blocks[1, 1].UpdateGroups();
				m_blocks[1, 0].UpdateGroups();
				m_blocks[2, 1].UpdateGroups();
				m_blocks[2, 0].UpdateGroups();
				break;
		}
	}

	private void FixBlocks() {
		if (m_blocks[2, 1].IsShipOnBlock() || m_blocks[2, 0].IsShipOnBlock() || m_blocks[2, 2].IsShipOnBlock()) {
			SwapColumns(2, 1);
			SwapColumns(0, 2);
			FixColumnPosition(2);
		} else if (m_blocks[0, 1].IsShipOnBlock() || m_blocks[0, 0].IsShipOnBlock() || m_blocks[0, 2].IsShipOnBlock()) {
			SwapColumns(0, 1);
			SwapColumns(0, 2);
			FixColumnPosition(0);
		}
		if (m_blocks[1, 2].IsShipOnBlock() || m_blocks[0, 2].IsShipOnBlock() || m_blocks[2, 2].IsShipOnBlock()) {
			SwapRows(2, 1);
			SwapRows(0, 2);
			FixRowPosition(2);
		} else if (m_blocks[1, 0].IsShipOnBlock() || m_blocks[0, 0].IsShipOnBlock() || m_blocks[2, 0].IsShipOnBlock()) {
			SwapRows(0, 1);
			SwapRows(0, 2);
			FixRowPosition(0);
		}
	}

	private void SwapColumns(int c1, int c2) {
		for (int i = 0; i != 3; i++) {
			AsteroidFieldBlock temp = m_blocks[c1, i];
			m_blocks[c1, i] = m_blocks[c2, i];
			m_blocks[c2, i] = temp;
		}
	}

	private void SwapRows(int r1, int r2) {
		for (int i = 0; i != 3; i++) {
			AsteroidFieldBlock temp = m_blocks[i, r1];
			m_blocks[i, r1] = m_blocks[i, r2];
			m_blocks[i, r2] = temp;
		}
	}

	private void FixColumnPosition(int c) {
		for (int i = 0; i != 3; i++) {
			Vector3 blockPosition = m_blocks[c, i].transform.position;
			blockPosition.x = m_blocks[1, 1].transform.position.x + (c > 1 ? 100 : -100);
			m_blocks[c, i].transform.position = blockPosition;
		}
	}

	private void FixRowPosition(int r) {
		for (int i = 0; i != 3; i++) {
			Vector3 blockPosition = m_blocks[i, r].transform.position;
			blockPosition.z = m_blocks[1, 1].transform.position.z + (r > 1 ? 100 : -100);
			m_blocks[i, r].transform.position = blockPosition;
		}
	}

	private AsteroidFieldBlock CreateBlock() {
		GameObject go =  Instantiate(m_blockPrefab);
		AsteroidFieldBlock block = go.GetComponent<AsteroidFieldBlock>();
		go.transform.parent = transform;
		return block;
	}

}

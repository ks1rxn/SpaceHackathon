
using UnityEngine;

public interface IEnemyShip {
	void Spawn(Vector3 position, float rotation);

	void UpdateShip();

	void Kill();

	Vector3 Position { get; }
}

using UnityEngine;
using Random = System.Random;

class MathHelper {
	private static Random m_random = new Random();

	public static float AngleBetweenVectors(Vector3 a, Vector3 b) {
		float angleToTarget = Vector3.Angle(a, b);
		if (Vector3.Cross(a, b).y < 0) {
			angleToTarget = -angleToTarget;
		}
		return angleToTarget;
	}

	public static float AngleBetweenVectorsZ(Vector3 a, Vector3 b) {
		float angleToTarget = Vector3.Angle(a, b);
		if (Vector3.Cross(a, b).z < 0) {
			angleToTarget = -angleToTarget;
		}
		return angleToTarget;
	}

	public static Random Random {
		get {
			return m_random;
		}
	}
}

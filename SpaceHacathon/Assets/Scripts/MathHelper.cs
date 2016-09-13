using UnityEngine;

class MathHelper {

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

}

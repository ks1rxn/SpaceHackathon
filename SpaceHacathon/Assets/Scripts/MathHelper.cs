using UnityEngine;

class MathHelper {

	public static float AngleBetweenVectors(Vector3 a, Vector3 b) {
		float angleToTarget = Vector3.Angle(a, b);
		if (Vector3.Cross(a, b).y < 0) {
			angleToTarget = -angleToTarget;
		}
		return angleToTarget;
	}

}

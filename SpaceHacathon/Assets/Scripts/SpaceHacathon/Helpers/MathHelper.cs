using UnityEngine;
using Random = System.Random;

namespace SpaceHacathon.Helpers {

	class MathHelper {
		private static readonly Random _random = new Random();

		public static Vector3 GetPointAround(Vector3 initialPoint, float minDistance, float maxDistance) {
			float angle = (float) Random.NextDouble() * 360;
			float distance = Random.Next((int)(maxDistance - minDistance)) + minDistance;
			return new Vector3(initialPoint.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0, initialPoint.z + Mathf.Sin(angle * Mathf.PI / 180) * distance);
		}

		public static Vector3 GetPointAround(Vector3 initialPoint, Vector3 lookVector, float maxAngleFromLook, float minDistance, float maxDistance) {
			float angle = -AngleBetweenVectors(Vector3.right, lookVector) + ((float) Random.NextDouble() * maxAngleFromLook * 2 - maxAngleFromLook);
			float distance = Random.Next((int)(maxDistance - minDistance)) + minDistance;
			return new Vector3(initialPoint.x + Mathf.Cos(angle * Mathf.PI / 180) * distance, 0, initialPoint.z + Mathf.Sin(angle * Mathf.PI / 180) * distance);
		}

		public static Vector3 GetPointOnSphere(Vector3 initialPoint, float minRadius, float maxRadius) {
			float distance = (float)Random.NextDouble() * (maxRadius - minRadius) + minRadius;
			float angleXZ = Random.Next(360);
			float angleY = Random.Next(360);
			return new Vector3(distance * Mathf.Cos(angleXZ * Mathf.PI / 180) * Mathf.Sin(angleY * Mathf.PI / 180),
				distance * Mathf.Cos(angleY * Mathf.PI / 180),
				distance * Mathf.Sin(angleXZ * Mathf.PI / 180) * Mathf.Sin(angleY * Mathf.PI / 180));
		}

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

		public static Vector3 AngleFrom360To180(Vector3 angle) {
			return new Vector3(angle.x > 180 ? angle.x - 360 : angle.x, angle.y > 180 ? angle.y - 360 : angle.y, angle.z > 180 ? angle.z - 360 : angle.z);
		}

		public static float ValueWithDispertion(float value, float dispertion) {
			return (float)Random.NextDouble() * dispertion * 2 - dispertion + value;
		}

		public static Random Random => _random;
	}

}
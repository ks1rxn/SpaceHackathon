﻿using UnityEngine;

public class StunProjectile : IBullet {
	private SettingsStunProjectile m_settings;

	private float m_angle;
	private float m_detonatorActivateTime;

	[SerializeField]
	private TrailRenderer m_trail1;
	[SerializeField]
	private TrailRenderer m_trail2;

	protected override void OnPhysicBodyInitiate() {
		m_settings = BattleContext.Settings.StunProjectile;

		CollisionDetector.RegisterListener(CollisionTags.PlayerShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.DroneCarrier, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.StunShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.RocketShip, OnTargetHit);
		CollisionDetector.RegisterListener(CollisionTags.RamShip, OnTargetHit);
	}

	protected override void OnPhysicBodySpawn(Vector3 position, Vector3 angle) {
		m_angle = angle.y;

		m_detonatorActivateTime = 0.05f;
		GetComponent<Collider>().enabled = false;
		m_trail1.Clear();
		m_trail2.Clear();
	}

	protected override void OnDespawn(DespawnReason reason) {
		if (reason == DespawnReason.Kill) {
			BattleContext.BattleManager.ExplosionsController.LaserExplosion(Position);
		}
	}

	protected override void OnFixedUpdateEntity() {
		Vector3 moveVector = new Vector3(Mathf.Cos(-m_angle * Mathf.PI / 180), 0, Mathf.Sin(-m_angle * Mathf.PI / 180));
		transform.position += moveVector * m_settings.Speed * Time.fixedDeltaTime;

		if (m_detonatorActivateTime <= 0) {
			GetComponent<Collider>().enabled = true;
		} else {
			m_detonatorActivateTime -= Time.fixedDeltaTime;
		}
	}
	
	private void OnTargetHit(GameObject other) {
		Despawn(DespawnReason.Kill);
	}

	protected override float DistanceToDespawn {
		get {
			return m_settings.DistanceFromPlayerToDespawn;
		}
	}
	
}


public class LevelSettings {
	public SettingsGlobal Global;
	public SettingsPlayerShip PlayerShip;

	// Allies //
	public SettingsAlliesController AlliesController;
	public SettingsHealingDroneStation HealingDroneStation;

	// Bonuses //
	public SettingsBonusesController BonusesController;
	public SettingsTimeBonus TimeBonus;

	// Enemies //
	public SettingsEnemiesController EnemiesController;
	public SettingsRamShip RamShip;
	public SettingsRocketShip RocketShip;
	public SettingsDroneCarrier DroneCarrier;
	public SettingsStunShip StunShip;
	public SettingsSpaceMine SpaceMine;

	// Explosions //
	public SettingsSlowingCloud SlowingCloud;

	// Bullets //
	public SettingsBulletsController BulletsController;
	public SettingsCarrierRocket CarrierRocket;
	public SettingsMissile Missile;
	public SettingsLaser Laser;
	public SettingsStunProjectile StunProjectile;

	public LevelSettings() {
		Global = new SettingsGlobal();
		PlayerShip = new SettingsPlayerShip();
		AlliesController = new SettingsAlliesController();
		HealingDroneStation = new SettingsHealingDroneStation();
		BonusesController = new SettingsBonusesController();
		TimeBonus = new SettingsTimeBonus();
		EnemiesController = new SettingsEnemiesController();
		SlowingCloud = new SettingsSlowingCloud();
		CarrierRocket = new SettingsCarrierRocket();
		Laser = new SettingsLaser();
		StunProjectile = new SettingsStunProjectile();
		Missile = new SettingsMissile();
		RamShip = new SettingsRamShip();
		RocketShip = new SettingsRocketShip();
		DroneCarrier = new SettingsDroneCarrier();
		StunShip = new SettingsStunShip();
		SpaceMine = new SettingsSpaceMine();
		BulletsController = new SettingsBulletsController();
	}

}

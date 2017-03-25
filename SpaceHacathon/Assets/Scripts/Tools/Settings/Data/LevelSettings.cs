
public class LevelSettings {
	public SettingsGlobal Global;
	public SettingsPlayerShip PlayerShip;
	public SettingsHealingDroneStation HealingDroneStation;
	public SettingsBonusesController BonusesController;
	public SettingsTimeBonus TimeBonus;
	public SettingsEnemiesController EnemiesController;
	public SettingsSlowingCloud SlowingCloud;
	public SettingsCarrierRocket CarrierRocket;
	public SettingsMissile Missile;
	public SettingsLaser Laser;
	public SettingsStunProjectile StunProjectile;
	public SettingsRamShip RamShip;
	public SettingsRocketShip RocketShip;
	public SettingsBulletsController BulletsController;

	public LevelSettings() {
		Global = new SettingsGlobal();
		PlayerShip = new SettingsPlayerShip();
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
		BulletsController = new SettingsBulletsController();
	}

}

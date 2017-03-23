
public class LevelSettings {
	public SettingsGlobal Global;
	public SettingsPlayerShip PlayerShip;
	public SettingsHealingDroneStation HealingDroneStation;
	public SettingsBonusesController BonusesController;
	public SettingsTimeBonus TimeBonus;
	public SettingsEnemiesController EnemiesController;
	public SettingsRamShip RamShip;
	public SettingsBulletsController BulletsController;

	public LevelSettings() {
		Global = new SettingsGlobal();
		PlayerShip = new SettingsPlayerShip();
		HealingDroneStation = new SettingsHealingDroneStation();
		BonusesController = new SettingsBonusesController();
		TimeBonus = new SettingsTimeBonus();
		EnemiesController = new SettingsEnemiesController();
		RamShip = new SettingsRamShip();
		BulletsController = new SettingsBulletsController();
	}

}

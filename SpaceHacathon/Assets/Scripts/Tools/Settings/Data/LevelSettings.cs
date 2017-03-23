
public class LevelSettings {
	public SettingsGlobal Global { get; set; }
	public SettingsPlayerShip PlayerShip { get; set; }
	public SettingsHealingDroneStation HealingDroneStation { get; set; }
	public SettingsBonusesController BonusesController { get; set; }
	public SettingsTimeBonus TimeBonus { get; set; }
	public SettingsEnemiesController EnemiesController { get; set; }
	public SettingsRamShip RamShip { get; set; }
	public SettingsBulletsController BulletsController { get; set; }

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

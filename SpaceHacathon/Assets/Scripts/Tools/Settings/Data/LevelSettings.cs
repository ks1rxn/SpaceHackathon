
public class LevelSettings {
	public SettingsBonusesController BonusesController { get; set; }
	public SettingsTimeBonus TimeBonus { get; set; }
	public SettingsEnemiesController EnemiesController { get; set; }
	public SettingsRamShip RamShip { get; set; }
	public SettingsBulletsController BulletsController { get; set; }

	public LevelSettings() {
		BonusesController = new SettingsBonusesController();
		TimeBonus = new SettingsTimeBonus();
		EnemiesController = new SettingsEnemiesController();
		RamShip = new SettingsRamShip();
		BulletsController = new SettingsBulletsController();
	}

}

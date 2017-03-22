using System.Text;

public class LevelSettings {
	public BonusesControllerSettings BonusesSettings { get; set; }
	public EnemiesControllerSettings EnemiesSettings { get; set; }

	public LevelSettings() {
		BonusesSettings = new BonusesControllerSettings();
		EnemiesSettings = new EnemiesControllerSettings();
	}

	public override string ToString() {
		StringBuilder builder = new StringBuilder();
		builder.Append("LevelSetting: ");
		builder.Append(BonusesSettings + " ");
		builder.Append(EnemiesSettings + " ");
		return builder.ToString();
	}

}

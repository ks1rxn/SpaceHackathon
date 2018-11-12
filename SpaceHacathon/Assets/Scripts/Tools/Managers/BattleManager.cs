using System.Collections.Generic;
using SpaceHacathon.BattleScene.Game.Time;
using SpaceHacathon.BattleScene.World.Dynamic.Camera;
using SpaceHacathon.BattleScene.World.Dynamic.PlayerShip;
using SpaceHacathon.Statistics;
using UnityEngine;
using UnityEngine.Serialization;

public class BattleManager : MonoBehaviour {
	[SerializeField]
	private ExplosionsController m_explosionsController;
	[SerializeField]
	private EnemiesController m_enemiesController;
	[SerializeField]
	private BulletsController m_bulletsController;
	[SerializeField]
	private BonusesController m_bonusesController;
	[SerializeField]
	private AlliesController m_alliesController;
	[SerializeField]
	private EffectsController m_effectsController;

	[SerializeField]
	private StatisticsManager m_statisticsManager;
	[SerializeField]
	private PrefabsManager m_prefabsManager;
	[SerializeField]
	private Director m_director;
	[SerializeField]
	private TimeManager m_timeManager;
	[SerializeField]
	private BattleCamera m_battleCamera;
	[FormerlySerializedAs("m_guiManager")]
	[SerializeField]
	private GUIManagerObsolete _mGUIManagerObsolete;

	private List<IController> m_controllers;

	public List<IController> Controllers {
		get {
			if (m_controllers == null) {
				m_controllers = new List<IController>();
				m_controllers.Add(m_explosionsController);
				m_controllers.Add(m_enemiesController);
				m_controllers.Add(m_bulletsController);
				m_controllers.Add(m_bonusesController);
				m_controllers.Add(m_alliesController);
				m_controllers.Add(m_effectsController);
			}
			return m_controllers;
		}
	}

	public ExplosionsController ExplosionsController {
		get {
			return m_explosionsController;
		}
	}

	public EnemiesController EnemiesController {
		get {
			return m_enemiesController;
		}
	}

	public BulletsController BulletsController {
		get {
			return m_bulletsController;
		}
	}

	public BonusesController BonusesController {
		get {
			return m_bonusesController;
		}
	}

	public AlliesController AlliesController {
		get {
			return m_alliesController;
		}
	}

	public EffectsController EffectsController {
		get {
			return m_effectsController;
		}
	}

	public PrefabsManager PrefabsManager {
		get {
			return m_prefabsManager;
		}
	}

	public StatisticsManager StatisticsManager {
		get {
			return m_statisticsManager;
		}
	}

	public Director Director {
		get {
			return m_director;
		}
	}

	public TimeManager TimeManager {
		get {
			return m_timeManager;
		}
	}

	public BattleCamera BattleCamera {
		get {
			return m_battleCamera;
		}
	}

	public GUIManagerObsolete GUIManagerObsolete {
		get {
			return _mGUIManagerObsolete;
		}
	}

}
using System;
using UnityEngine;
using UnityWeld.Binding;

namespace SpaceHacathon.BattleScene.GUI.Screens {

    [Binding]
    public class PauseMenu : MonoBehaviour {
        public event Action OnResume = delegate {  };
        public event Action OnRestart = delegate {  };
        public event Action OnExit = delegate {  };
        
        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        [Binding]
        public void OnResumeClick() {
            OnResume();
        }
        
        [Binding]
        public void OnRestartClick() {
            OnRestart();
        }
        
        [Binding]
        public void OnExitClick() {
            OnExit();
        }
        
    }

}
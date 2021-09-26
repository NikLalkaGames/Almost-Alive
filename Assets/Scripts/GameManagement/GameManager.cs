using GhostBehaviours;
using UnityEngine;

namespace GameManagement
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; set; } = null;

        [SerializeField] private SceneLoader _sceneLoader;

        private GhostMovement _ghostControl;

        private GameObject _playerUI;

        private void Awake() 
        {
            Debug.Log("GameManager Awake");
            if (Instance == null) Instance = this;
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(this.gameObject);
        }

        private void Start() 
        {
            StartCoroutine(SceneLoader.instance.LoadScene("MainMenu", 0.5f));
        }

        public void RefreshGamingStats()
        {
            // may be more code
        }

        private void RefreshPlayerData()
        {

        }
    }
}
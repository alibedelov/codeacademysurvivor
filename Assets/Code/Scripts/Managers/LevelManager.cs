using System;
using System.Collections;
using Code.Scripts.Abilities.AbilitiesHolder;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Managers
{
    public class LevelManager : SingletoneBase<LevelManager>
    {
         private AbilityHolder _abilityHolder;
        private int _currentLevel = 2;
        private float _loadProgress;
        public int CurrentLevel => _currentLevel;
        public float LoadProgress => _loadProgress;

        public event Action<float> OnLoadProgressUpdated;
        public event Action OnSceneLoad;
        public event Action OnSceneLoadComplete;

        private static bool isLoading = false;

        private void Awake()
        {
            if (Instance.GetInstanceID() != this.GetInstanceID())
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (!isLoading)
            {
                isLoading = true;
                LoadLevel("SampleScene");
            }
        }

        public void LoadLevel(string levelName)
        {
            SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(levelName);
            SceneManager.UnloadSceneAsync(scene.buildIndex);
            StartCoroutine(LoadSceneAsync(scene.buildIndex));
        }

        public void LoadLevel(int levelIndex)
        {
            StartCoroutine(LoadSceneAsync(levelIndex));
        }

        IEnumerator LoadSceneAsync(int levelIndex)
        {
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(levelIndex);

            OnSceneLoad?.Invoke();

            while (!op.isDone)
            {
                float progress = Mathf.Clamp01(op.progress / .9f);
                _loadProgress = progress;
                OnLoadProgressUpdated?.Invoke(progress);

                yield return null;
            }

            OnSceneLoadComplete?.Invoke();
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            OnSceneLoad?.Invoke();
        }

        public IEnumerator ReloadSceneAsync(Action onComplete = null)
        {
            _abilityHolder = FindObjectOfType<AbilityHolder>();
            _abilityHolder.ResetSkills();
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

            OnSceneLoad?.Invoke();

            while (!op.isDone)
            {
                yield return null;
            }

            OnSceneLoadComplete?.Invoke();

            onComplete?.Invoke();
        }
    }
}
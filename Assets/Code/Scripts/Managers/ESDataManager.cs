using Code.Scripts.Classes;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class ESDataManager : SingletoneBase<ESDataManager>
    {
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            Load();
        }

        private const string DataKey = "gameData";

        [SerializeField] private GameData startData;

        [SerializeField] public GameData gameData;

        public void Load()
        {
            if (ES3.FileExists())
            {
                if (ES3.KeyExists(DataKey)) gameData = ES3.Load(DataKey, gameData);
            }
            else
            {
                Reset();
            }
        }

        public void Save()
        {
            ES3.Save(DataKey, gameData);
        }

        public void Reset()
        {
            gameData = startData;
            Save();
        }
    }
}
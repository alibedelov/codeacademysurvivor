using System;
using CASP.SoundManager;
using Code.Scripts.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Scripts.Managers
{
    public class UIManager : SingletoneBase<UIManager>
    {
        [SerializeField] private ChoiceAbility choiceAbilityPanel;
        [SerializeField] private GameObject luckyTrainWinPanel;
        [SerializeField] private GameObject luckyTrainPanel;

        [SerializeField] private PausePanel pausePanel;
        [SerializeField] private TransitionPanel transitionPanel;
        [SerializeField] private GamePanel gamePanel;
        [SerializeField] private LoosePanel loosePanel;
        [SerializeField] private WinPanel winPanel;
        [SerializeField] private HomePanel homePanel;
        [SerializeField] private QuittingPopup quittingPopup;

        public GamePanel GamePanel => gamePanel;
        public LoosePanel LoosePanel => loosePanel;
        public TransitionPanel TransitionPanel => transitionPanel;
        public QuittingPopup QuittingPopup => quittingPopup;

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
            Time.timeScale = 1f;
            SoundManager.Instance.Play("InGameSong", true);
        }


        public void OpenLoosePanel()
        {
            GameManager.Instance.PauseGame();
            loosePanel.Initialize();
            loosePanel.gameObject.SetActive(true);
            SoundManager.Instance.Play("Loose",false);
            SoundManager.Instance.Stop("InGameSong");
        }

        public void CloseLoosePanel()
        {
            loosePanel.gameObject.SetActive(false);
            SoundManager.Instance.Stop("Loose");

        }

        public void OpenWinPanel()
        {
            SoundManager.Instance.Stop("Disc");
            SoundManager.Instance.Play("Win",false);
            GameManager.Instance.PauseGame();
            winPanel.Initialize();
            winPanel.gameObject.SetActive(true);
        }

        public void CloseWinPanel()
        {
            SoundManager.Instance.Stop("Win");
            winPanel.gameObject.SetActive(false);
        }

        public void OpenHomePanel()
        {
            homePanel.gameObject.SetActive(true);
            SoundManager.Instance.Play("MainMenuSong", true);
        }

        public void CloseHomePanel()
        {
            StartCoroutine(LevelManager.Instance.ReloadSceneAsync());
        }

        public void OpenQuittingPopup()
        {
            quittingPopup.gameObject.SetActive(true);
        }

        public void CloseQuittingPopup()
        {
            quittingPopup.gameObject.SetActive(false);
        }

        public void OpenGamePanel()
        {
            Time.timeScale = 1f;
            GameManager.Instance.ResumeGame();
            SoundManager.Instance.Play("InGameSong", true);
            //gamePanel.gameObject.SetActive(true);
        }


        public void CloseGamePanel()
        {
            gamePanel.gameObject.SetActive(false);
            SoundManager.Instance.Stop("InGameSong");
        }

        public void OpenChoiceAbilityPanel()
        {
            GameManager.Instance.PauseGame();
            choiceAbilityPanel.Initialize(GamePanel.XpLevel);
            choiceAbilityPanel.gameObject.SetActive(true);
        }

        public void CloseChoiceAbilityPanel()
        {
            choiceAbilityPanel.gameObject.SetActive(false);
        }

        public void OpenPausePanel()
        {
            GameManager.Instance.PauseGame();
            pausePanel.gameObject.SetActive(true);
        }

        public void ClosePausePanel()
        {
            GameManager.Instance.ResumeGame();
            pausePanel.gameObject.SetActive(false);
        }

        public void OpenLuckyTrainWinPanel()
        {
            luckyTrainWinPanel.SetActive(true);
        }

        public void CloseLuckyTrainWinPanel()
        {
            luckyTrainWinPanel.SetActive(false);
        }

        public void OpenLuckyTrainPanel()
        {
            GameManager.Instance.PauseGame();
            luckyTrainPanel.SetActive(true);
        }

        public void CloseLuckyTrainPanel()
        {
            GameManager.Instance.ResumeGame();
            luckyTrainPanel.SetActive(false);
        }

        public void BackToHome()
        {
            ClosePausePanel();
            SoundManager.Instance.Stop("InGameSong");
            SoundManager.Instance.Stop("Loose");
            transitionPanel.Transient(() =>
            {
                OpenHomePanel();
                //StartCoroutine(LevelManager.Instance.ReloadSceneAsync());
            });
        }
    }
}
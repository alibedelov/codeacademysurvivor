using System;
using System.Collections.Generic;
using Assets.Code.Scripts.Collectables.CollectablesMono;
using Assets.Code.Scripts.Enemies.Abstraction;
using CASP.SoundManager;
using Code.Scripts.Abilities.AbilitiesMono;
using Code.Scripts.Abilities.Abstraction;
using Code.Scripts.Collectables.CollectablesHolder;
using Code.Scripts.Collectables.CollectablesMono;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.Abstraction;
using Code.Scripts.Player;
using DG.Tweening;
using UnityEngine;

namespace Code.Scripts.PassiveAbilities.StatsManipulation
{
    /// <summary>
    /// This Class Use Passive Abilities to manipulate stats.
    /// Also use this class for Collectables, and collect experience into _experience varible multiple by _modifier;
    /// </summary>
    public class StatsManipulator : SingletoneBase<StatsManipulator>
    {
        [SerializeField] private ActiveAbilityBase defaultAbility;

        [SerializeField] private List<ActiveAbilityBase> _activeAbilities;
        [SerializeField] private List<PassiveAbilityBase> _passiveAbilities;

        public float maxHealth;
        public float currentHealth;
        public float armor;
        public float movementSpeed;
        public float experienceModifier;

        private float _coin;
        private float _maxHealth;
        private float _currentHealth;
        private float _armor;
        private float _movementSpeed;
        private float _experienceModifier;
        [SerializeField] private float _experience;

        private float _tempSpeed;

        public ActiveAbilityBase DefaultAbility => defaultAbility;
        public float Experience => _experience;
        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;
        public float Armor => _armor;
        public float MovementSpeed => _movementSpeed;
        public float ExperienceModifier => _experienceModifier;
        public float Coin => _coin;

        public bool IsDestructable { get; }

        public List<ActiveAbilityBase> ActiveAbilities => _activeAbilities;
        public List<PassiveAbilityBase> PassiveAbilities => _passiveAbilities;

        private CollectableDetector _collectableDetector;
        private Camera _mainCamera;

        private void Awake()
        {
            _maxHealth = maxHealth;
            _currentHealth = currentHealth;
            _armor = armor;
            _experienceModifier = experienceModifier;
            _movementSpeed = movementSpeed;
            _experience = 0;

            _mainCamera = Camera.main;
            _collectableDetector = FindObjectOfType<CollectableDetector>();
        }

        private void OnEnable()
        {
            AddAbility(defaultAbility);
        }

        private void Start()
        {
            _collectableDetector.OnXpDetect += HandleOnXpDetect;
            _collectableDetector.OnBombDetect += HandleOnBombDetect;
            _collectableDetector.OnMagnetDetect += HandleOnMagnetDetect;
            _collectableDetector.OnMeatDetect += HandleOnMeatDetect;
            _collectableDetector.OnCoinDetect += HandleOnCoinDetect;
            _collectableDetector.OnRainbowDetect += HandleOnRainbowDetect;

            Application.quitting += ResetSkills;
            LevelManager.Instance.OnSceneLoad += ResetSkills;
        }

        private void ResetSkills()
        {
            foreach (var abilityBase in _passiveAbilities)
            {
                var ability = abilityBase;
                ability.ResetToDefault();
            }
        }

        private void AddPassiveAbility(PassiveAbilityBase passiveAbility)
        {
            passiveAbility.currentLevel++;

            if (!_passiveAbilities.Contains(passiveAbility))
            {
                _passiveAbilities.Add(passiveAbility);
                passiveAbility.ApplyEffect(this);
            }
            else
            {
                passiveAbility.ApplyEffect(this);
            }
        }

        private void AddActiveAbility(ActiveAbilityBase activeAbility)
        {
            activeAbility.currentLevel++;

            if (!_activeAbilities.Contains(activeAbility))
            {
                _activeAbilities.Add(activeAbility);
            }
            else if (activeAbility is ForceAreaSO forceAreaSO)
            {
                _activeAbilities.RemoveAll(x => x is ForceAreaSO);
                forceAreaSO.DestroyActiveForceArea(transform);
                _activeAbilities.Add(activeAbility);
            }
        }

        public void AddAbility(AbilityBase abilityBase)
        {
            switch (abilityBase)
            {
                case ActiveAbilityBase activeAbility:
                    AddActiveAbility(activeAbility);
                    break;

                case PassiveAbilityBase passiveAbility:
                    AddPassiveAbility(passiveAbility);
                    break;
            }
        }

        public void RemovePassiveAbility(PassiveAbilityBase passiveAbility)
        {
            if (_passiveAbilities.Remove(passiveAbility))
            {
                passiveAbility.RemoveEffect(this);
            }
        }

        private void OnDestroy()
        {
            foreach (var passiveAbility in _passiveAbilities)
            {
                passiveAbility.RemoveEffect(this);
            }
        }

        private void HandleOnCoinDetect(Coin obj)
        {
            obj.AnimateCollectable(() =>
            {
                _coin += obj.CoinValue;
                CollectableHolder.Instance.DestroyCollectable(obj);
                UIManager.Instance.GamePanel.UpdateCoin();
                SoundManager.Instance.Play("Coin", false);
            });
        }

        private void HandleOnMeatDetect(Meat obj)
        {
            obj.AnimateCollectable(() =>
            {
                float remainHealth = _maxHealth - _currentHealth;
                _currentHealth += remainHealth * 70 / 100;
                PlayerCanvas.Instance.UpdatePlayerHealth();
                CollectableHolder.Instance.DestroyCollectable(obj);
            });
        }

        private void HandleOnRainbowDetect(Rainbow obj)
        {
            obj.AnimateCollectable(() =>
            {
                UIManager.Instance.OpenLuckyTrainPanel();
                CollectableHolder.Instance.DestroyCollectable(obj);
            });
        }

        private void HandleOnMagnetDetect(Magnet obj)
        {
            obj.AnimateCollectable(() =>
            {
                SoundManager.Instance.Play("Magnet", false);
                foreach (Xp xp in obj.CollectAllXP())
                {
                    xp.AnimateCollectable(() =>
                    {
                        _experience += xp.XpValue * _experienceModifier;
                        CollectableHolder.Instance.DestroyCollectable(xp);
                        UIManager.Instance.GamePanel.UpdateXp();
                    }, 0.8f);
                }

                CollectableHolder.Instance.DestroyCollectable(obj);
            });
        }

        private void HandleOnBombDetect(Bomb bomb)
        {
            bomb.AnimateCollectable(() =>
            {
                UIManager.Instance.GamePanel.FadeGameUI();

                foreach (var item in FindObjectsOfType<EnemyMonoBase>())
                {
                    Vector2 screenPos = _mainCamera.WorldToScreenPoint(item.transform.position);
                    bool onScreen = screenPos.x > 0f && screenPos.x < Screen.width && screenPos.y > 0f &&
                                    screenPos.y < Screen.height;
                    if (onScreen && item.GetComponent<Renderer>().isVisible)
                    {
                        item.TakeDamage(5000);
                    }
                }
                SoundManager.Instance.Play("Bomb", false);
                CollectableHolder.Instance.DestroyCollectable(bomb);
            });
        }

        private void HandleOnXpDetect(Xp obj)
        {
            obj.AnimateCollectable(() =>
            {
                CollectableHolder.Instance.DestroyCollectable(obj);
                _experience += obj.XpValue * _experienceModifier;
                SoundManager.Instance.Play("Exp", false);
                UIManager.Instance.GamePanel.UpdateXp();
            });
        }

        public void TakeDamage(float damage)
        {
            _currentHealth -= damage / _armor;
            Handheld.Vibrate();
            //Vibrator.Vibrate(15);

            PlayerCanvas.Instance.UpdatePlayerHealth();

            if (_currentHealth <= 0)
            {
                GameManager.Instance.LooseGame();
                SoundManager.Instance.Play("PlayerDeath", false);
            }
        }
    }
}
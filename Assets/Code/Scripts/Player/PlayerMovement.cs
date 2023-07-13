using System.Collections;
using Assets.Code.Scripts.Enemies.Abstraction;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using Spine.Unity;
using UnityEngine;

namespace Code.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private StatsManipulator statsManipulator;
        [SerializeField] private Joystick joystick;


        private SkeletonAnimation _skeletonAnimation;
        private Rigidbody2D _rb;
        private float _horizontal;
        private float _vertical;
        private float _pauseSpeed = 1;
        private bool _isFacingRight = true;
        private bool _isDied;
        private Vector3 targetPosition;
        private void Start()
        {
            _isFacingRight = true;
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
            _rb = GetComponent<Rigidbody2D>();
            joystick = FindObjectOfType<FloatingJoystick>();

            GameManager.Instance.OnLooseGame += OnLooseGame;
        }

        private void OnLooseGame()
        {
            StartCoroutine(EndGame());
        }

        private void OnPauseGame(GameState state)
        {
            switch (state)
            {
                case GameState.Paused:
                    _pauseSpeed = 0;
                    break;

                case GameState.Resumed:
                    _pauseSpeed = 1;
                    break;
            }
        }

        private void Update()
        {
            if (_isDied)
                return;

            _horizontal = joystick.Horizontal;
            _vertical = joystick.Vertical;
             // targetPosition = new Vector3(_horizontal, _vertical, 0) * (statsManipulator.movementSpeed * _pauseSpeed);
             // _skeletonAnimation.AnimationName = transform.position != targetPosition ? "Walk" : "idle2";
             
             _skeletonAnimation.AnimationName = _rb.velocity.magnitude>0.1f? "Walk" : "idle2";

            //
            // // Плавное движение к целевой позиции
            // //transform.position +=  //Vector3.Lerp(transform.position, targetPosition, 5 * Time.deltaTime);
            //     transform.position += targetPosition * (1.35f * Time.deltaTime);
            Flip();
        }

        private void FixedUpdate()
        {
            if (_isDied)
                return;
        
            _rb.velocity = new Vector2(_horizontal, _vertical).normalized * (statsManipulator.movementSpeed * _pauseSpeed);
            //_rb.MovePosition(new Vector2(_horizontal, _vertical).normalized * (statsManipulator.movementSpeed * _pauseSpeed));
        }


        private void Flip()
        {
            if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
            {
                _isFacingRight = !_isFacingRight;
                _skeletonAnimation.skeleton.ScaleX *= -1;
            }
        }

        private IEnumerator EndGame()
        {
            _isDied = true;
            _skeletonAnimation.AnimationName = "die";
            _rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(2f);

            UIManager.Instance.OpenLoosePanel();
        }
    }
}
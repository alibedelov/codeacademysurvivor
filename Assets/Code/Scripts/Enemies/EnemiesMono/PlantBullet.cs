using System;
using Code.Scripts.Managers;
using Code.Scripts.PassiveAbilities.StatsManipulation;
using Code.Scripts.Player;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    public float speed = 5f; // Speed of the bullet
    [SerializeField] Transform player; // Reference to the player's position

    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        // Find the player object by tag (make sure you have assigned the "Player" tag to the player object)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Make the bullet move towards the player on start
        _direction = (player.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = _direction * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerMovement>(out PlayerMovement destructable))
        {
            StatsManipulator.Instance.TakeDamage(4f);
        }
    }

    private void OnPauseAndResumeGame(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Paused:
                _rigidbody.velocity = Vector2.zero;
                break;

            case GameState.Resumed:
                if (this != null)
                {
                    _rigidbody.velocity = _direction * speed;
                }
                break;
        }
    }

    private void Update()
    {
        // If the bullet goes offscreen, destroy it
        if (!GetComponent<Renderer>().isVisible)
        {
            Destroy(gameObject);
        }
    }

}
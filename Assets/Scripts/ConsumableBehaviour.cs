using System.Collections;
using System.Collections.Generic;
using Emotions.Models;
using Enemies;
using Enemies.Interfaces;
using UnityEngine;
using UnityEngine.Events;

public class ConsumableBehaviour : MonoBehaviour, IEnemy
{
    # region Fields

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody2d;
    private HumanController _humanController;
    [SerializeField] private Spawner _spawner;

    private Sprite _deadSprite;
    private IEnemy _enemyImplementation;

    # endregion 

    # region Properties
    
    public Sprite HumanSprite { get => _spriteRenderer.sprite; set => _spriteRenderer.sprite = value; }
    public Sprite DeadSprite { get; set; }

    # endregion
    
    public event System.Action OnKill;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _humanController = GetComponent<HumanController>();
        

        // if (_spawner != null)
        // {
        //     OnKilled += _spawner.GenerateMatchingHuman;
        // }
        // else
        // {
        //     Debug.LogWarning("Spawner object not found");
        // }
    }

    public void Kill()
    {
        OnKill?.Invoke();

        _spriteRenderer.sprite = DeadSprite;
        _boxCollider.enabled = false;
        _rigidbody2d.isKinematic = true;
        _humanController.enabled = false;
        this.enabled = false;
    }


}

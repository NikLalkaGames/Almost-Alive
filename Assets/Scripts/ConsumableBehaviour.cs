using System.Collections;
using System.Collections.Generic;
using Emotions.Models;
using UnityEngine;
using UnityEngine.Events;

public class ConsumableBehaviour : MonoBehaviour
{
    # region Fields

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody2d;
    private HumanController _humanController;
    [SerializeField] private Spawner _spawner;

    private Sprite _deadSprite;
    
    # endregion 

    # region Properties

    public EmotionColor HumanColor;
    public Sprite HumanSprite { get => _spriteRenderer.sprite; set => _spriteRenderer.sprite = value; }
    public Sprite DeadSprite { get; set; }

    # endregion
    
    public static event System.Action<EmotionColor> OnKilled;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody2d = GetComponent<Rigidbody2D>();
        _humanController = GetComponent<HumanController>();
        
        
        // DefineColorByEmotion();

        if (_spawner != null)
        {
            OnKilled += _spawner.GenerateMatchingHuman;
        }
        else
        {
            Debug.LogWarning("Spawner object not found");
        }
    }

    public void Kill()
    {
        OnKilled?.Invoke(HumanColor);

        _spriteRenderer.sprite = DeadSprite;
        _boxCollider.enabled = false;
        _rigidbody2d.isKinematic = true;
        _humanController.enabled = false;
        this.enabled = false;
    }


}

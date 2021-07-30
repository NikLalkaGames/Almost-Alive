using System;
using Emotions.Models;
using Emotions.Object;
using Enemies;
using UnityEngine;
using static Core.Helpers.Helpers;

namespace Emotions.Controllers
{
    public class MobEmotionController : EmotionController
    {
        # region Fields

        private SpriteRenderer _parentSpriteRenderer;
        
        public string startOrb;
        
        #endregion

        #region Properties

        protected override Vector3 DirectionOfDrop => GetRandomDir();
        
        # endregion

        private Color EmotionToColor()
        {
            switch (_emotions[LastEmotion].Color)
            {
                case EmotionColor.Blue: return new Color(0f, 0.5f, 1f);
                case EmotionColor.Green: return new Color(0.22f, 1f, 0.44f);
                case EmotionColor.Pink: return new Color(1f, 0.09f, 1f);
                case EmotionColor.Purple: return new Color(0.79f, 0f, 1f);
                case EmotionColor.Yellow: return new Color(0.96f, 1f, 0.13f);
                default: return Color.white;
            }
        }
        

        protected void Awake()
        {
            var enemy = GetComponentInParent<IEnemy>();
            if (enemy == null) return;

            _parentSpriteRenderer = GetComponentInParent<SpriteRenderer>();

            OnHandle += DefineSkinColor;
            enemy.OnKill += DropEmotionsAfterDeath;
        }

        protected override void Start()
        {
            base.Start();

            var emotion = _poolManager
                .SpawnFromPool(startOrb, transform.position)
                .GetComponent<Emotion>();
            
            Handle(emotion);        // for humans
        }
        
        /// <summary>
        /// Drop emotions callback for humans event OnKilled
        /// </summary>
        private void DropEmotionsAfterDeath()
        {
            Debug.Log("Drop Emotions After Death");

            var emotionsCount = LastEmotion;
            for (var i = 0; i < emotionsCount; i++)
            {
                ThrowEmotion();
                Debug.Log(_emotions.Count);
            }
        }
        
        /// <summary>
        /// Little humans callback for defining colors 
        /// </summary>
        private void DefineSkinColor()
        {
            _parentSpriteRenderer.color = _emotions.Count == 1 ? EmotionToColor() : Color.white;
            // TODO: change color of animations in animation controller
        }

    }
}

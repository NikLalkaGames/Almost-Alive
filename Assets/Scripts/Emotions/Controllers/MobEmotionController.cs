using Emotions.Models;
using Emotions.Object;
using Enemies.Interfaces;
using UnityEngine;
using static Core.Helpers.Helpers;

namespace Emotions.Controllers
{
    public class MobEmotionController : EmotionController
    {
        # region Fields

        [SerializeField] private SpriteRenderer mobSpriteRenderer;
        
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
            OnHandle += DefineSkinColor;
            
            if (!transform.parent.TryGetComponent(out IEnemy enemy)) return;
            enemy.OnKill += DropEmotionsAfterDeath;
        }

        protected override void Start()
        {
            base.Start();

            _poolManager.SpawnFromPool(startOrb, transform.position).TryGetComponent(out Emotion emotion);
            Handle(emotion);                // handle one emotion for humans -> branch class in to subclasses if want get different behaviour
        }
        
        // TODO: Test it!
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
            mobSpriteRenderer.color = _emotions.Count == 1 ? EmotionToColor() : Color.white;
            // TODO: change color of animations in animation controller
        }

    }
}

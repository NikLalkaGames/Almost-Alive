using Emotions.Models;
using UnityEngine;
using static Core.Helpers.Helpers;

namespace Emotions.Controllers
{
    public class MobEmotionController : EmotionController
    {
        # region Fields
    
        private ConsumableBehaviour _consumableHuman;

        #endregion

        #region Properties

        protected override Vector3 DirectionOfDrop => GetRandomDir();
        
        # endregion 

        #region SpriteGetters

        public static Sprite GetHumanSprite(EmotionColor color)
        {
            switch (color)
            {
                default:
                case EmotionColor.blue: return Resources.Load<Sprite>("blue");
                case EmotionColor.green: return Resources.Load<Sprite>("green");
                case EmotionColor.pink: return Resources.Load<Sprite>("pink");
                case EmotionColor.purple: return Resources.Load<Sprite>("purple");
                case EmotionColor.yellow: return Resources.Load<Sprite>("yellow");
                case EmotionColor.white: return Resources.Load<Sprite>("white");
            }
        }

        public static Sprite GetDeadSprite(EmotionColor color)
        {
            switch (color)
            {
                default:
                case EmotionColor.blue: return Resources.Load<Sprite>("blue_dead");
                case EmotionColor.green: return Resources.Load<Sprite>("green_dead");
                case EmotionColor.pink: return Resources.Load<Sprite>("pink_dead");
                case EmotionColor.purple: return Resources.Load<Sprite>("purple_dead");
                case EmotionColor.yellow: return Resources.Load<Sprite>("yellow_dead");
                case EmotionColor.white: return Resources.Load<Sprite>("white_dead");
            }
        }

        # endregion

        protected override void Awake()
        {
            _consumableHuman = GetComponentInParent<ConsumableBehaviour>();
        
            if (_consumableHuman != null)
            {
                OnHandle += DefineSkinColor;
                ConsumableBehaviour.OnKilled += DropEmotionsAfterDeath;
            }
        }

        protected override void Start()
        {
            base.Start();
            var humanEmotion = new Emotion(_consumableHuman.HumanColor);
            Handle(humanEmotion);
        }
        
        /// <summary>
        /// Drop emotions callback for humans event OnKilled
        /// </summary>
        /// <param name="emotionColor"></param>
        private void DropEmotionsAfterDeath(EmotionColor emotionColor)
        {
            Debug.Log("Drop Emotions After Death");

            var emotionsCount = LastEmotion;
            for (int i = 0; i < emotionsCount; i++)
            {
                RemoveAndThrowEmotion();
                Debug.Log(_emotions.Count);
            }
        }
        
        /// <summary>
        /// Little humans callback for defining colors 
        /// </summary>
        public void DefineSkinColor()
        {
            if (_emotions.Count <= 1)
            {
                _consumableHuman.HumanSprite = GetHumanSprite(_consumableHuman.HumanColor);
                _consumableHuman.DeadSprite = GetDeadSprite(_consumableHuman.HumanColor);
                // change animation controller of human
            }
            else
            {
                _consumableHuman.HumanColor = EmotionColor.white;
                _consumableHuman.HumanSprite = GetHumanSprite(_consumableHuman.HumanColor);
                _consumableHuman.DeadSprite = GetDeadSprite(_consumableHuman.HumanColor);
                // change animation controller of human
            }
        }

    }
}

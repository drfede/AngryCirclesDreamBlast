using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AngryCirclesDreamBlast
{
    public class GameManager : Utilities.Singleton<GameManager>
    {

        [Header("UI References")]
        [SerializeField]
        private List<UnityEngine.UI.Image> circleImages;        
        [SerializeField]
        private List<UnityEngine.UI.Text> circleTexts;
        [SerializeField]
        private UnityEngine.UI.Text movesText;

        [Space]
        [SerializeField]
        private Settings settings;
        [SerializeField]
        private CircleMap circleMap;
        [SerializeField]
        private LevelObject levelToLoad;

        public Settings Settings { get => settings; private set => settings = value; }



        public UnityEvent<HashSet<StandardCircle>> onCircleTap;

        private int availableMoves = 0;


        public override void Start()
        {
            if (levelToLoad != null)
            {
                Setup();

            }
        }

        private void Setup()
        {
            availableMoves = levelToLoad.MovesLimit;
            movesText.text = "Moves: " + availableMoves.ToString();
            if (circleImages.Count > levelToLoad.LevelTargets.Count)
            {
                for(int i=circleImages.Count-1; i < levelToLoad.LevelTargets.Count; ++i)
                {
                    circleImages[i].gameObject.SetActive(false);
                    circleTexts[i].gameObject.SetActive(false);
                }
            }
            for (int i = 0; i < circleImages.Count && i < levelToLoad.LevelTargets.Count; ++i)
            {
                circleTexts[i].text = levelToLoad.LevelTargets[i].TargetNumber.ToString();
                var circleMapElem = circleMap.Circles.Find(x => x.Circle.Type == levelToLoad.LevelTargets[i].Type);
                circleImages[i].sprite = circleMapElem.CircleSprite;
                circleImages[i].color = circleMapElem.CircleColor;
            }
        }
    }

}
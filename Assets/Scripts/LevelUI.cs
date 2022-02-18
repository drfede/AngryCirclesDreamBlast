using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryCirclesDreamBlast
{
    public class LevelUI : MonoBehaviour
    {

        [Header("UI References")]
        [SerializeField]
        private List<UnityEngine.UI.Image> circleImages;
        [SerializeField]
        private List<UnityEngine.UI.Text> circleTexts;
        [SerializeField]
        private UnityEngine.UI.Text movesText;


        private void Start()
        {
            SetupUI();
            GameManager.Instance.onAfterPop.AddListener(UpdateUI);
        }

        private void SetupUI()
        {
            if (circleImages.Count > GameManager.Instance.TargetLevels.Count)
            {
                for (int i = GameManager.Instance.TargetLevels.Count; i < circleImages.Count; ++i)
                {
                    circleImages[i].gameObject.SetActive(false);
                    circleTexts[i].gameObject.SetActive(false);
                }
            }
            UpdateUI();
        }

        private void UpdateUI()
        {
            movesText.text = "Moves: " + GameManager.Instance.AvailableMoves.ToString();
            for (int i = 0; i < circleImages.Count && i < GameManager.Instance.TargetLevels.Count; ++i)
            {
                circleTexts[i].text = GameManager.Instance.TargetLevels[i].TargetNumber.ToString();
                var circleMapElem = GameManager.Instance.CircleMap.Circles.Find(x => x.Circle.Type == GameManager.Instance.TargetLevels[i].Type);
                circleImages[i].sprite = circleMapElem.CircleSprite;
                circleImages[i].color = circleMapElem.CircleColor;
            }
        }



    }

}
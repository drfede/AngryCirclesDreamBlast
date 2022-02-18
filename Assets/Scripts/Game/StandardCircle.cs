using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

namespace AngryCirclesDreamBlast
{
    public abstract class StandardCircle : MonoBehaviour, IPointerDownHandler
    {
        public enum CircleType { NONE, BLUE, YELLOW, WHITE, RED };

        [Header("Collider Parameters")]
        [SerializeField,Range(0.1f,2.0f)]
        private float rayCastSizeMultiplier = 0.5f;

        [Header("Explosion Parameters")]
        [SerializeField]
        private float maxExplosionScale = 1.2f;
        [SerializeField]
        private float popOutExplosionTime = .15f;
        [SerializeField]
        private float popInExplosionTime = .2f;
        [SerializeField]
        private LeanTweenType popOutTweenType = LeanTweenType.easeInBounce;
        [SerializeField]
        private LeanTweenType popInTweenType = LeanTweenType.easeOutBounce;

        private CircleCollider2D _collider;

        public abstract CircleType Type { get; }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        public HashSet<StandardCircle> FindCollidingCircles(HashSet<StandardCircle> result = null)
        {

            HashSet<StandardCircle> allOtherCircles = new();
            if (result == null)
                result = new();

            var touchedCircles = Physics2D.OverlapCircleAll(transform.position, _collider.radius * rayCastSizeMultiplier);
            touchedCircles = touchedCircles.Where(x => x.CompareTag(gameObject.tag) && x.gameObject != gameObject).ToArray();

            foreach (var col in touchedCircles)
            {
                var otherCircle = col.GetComponent<StandardCircle>();
                if (!result.Contains(otherCircle))
                {
                    result.Add(otherCircle);
                    allOtherCircles.Add(otherCircle);
                }
            }

            foreach (var circle in allOtherCircles)
            {
                circle.FindCollidingCircles(result);
            }

            return result;
        }

        [NaughtyAttributes.Button]
        public void Pop()
        {
            var touchingCircles = FindCollidingCircles();
            if (touchingCircles.Count >= GameManager.Instance.Settings.CirclesToMatch)
            {
                GameManager.Instance.onCircleTap?.Invoke(touchingCircles);
                touchingCircles.ToList().ForEach(x => x.Explode());
            }
        }

        protected virtual void Explode()
        {
            var startingScale = transform.localScale;
            gameObject.LeanScale(startingScale * maxExplosionScale, popOutExplosionTime).setEase(popOutTweenType).setOnComplete(() =>
              {
                  gameObject.LeanScale(Vector3.zero, popInExplosionTime).setEase(popInTweenType).setOnComplete(() =>
                  {
                      CirclesPooler.Instance.GiveBack(this);
                      transform.localScale = startingScale;
                  });
  
              });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (GameManager.Instance.InputEnabled)
            {
                Pop();
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using System;

namespace AngryCirclesDreamBlast
{
    public abstract class StandardCircle : MonoBehaviour, IPointerDownHandler
    {
        public enum CircleType { NONE, BLUE, YELLOW, WHITE, RED, BOMB };


        [Header("Collider Parameters")]
        [SerializeField, Range(0.1f, 2.0f)]
        private float rayCastSizeMultiplier = 0.5f;

        [Header("Explosion Parameters")]
        [SerializeField]
        protected float maxExplosionScale = 1.2f;
        [SerializeField]
        protected float popOutExplosionTime = .15f;
        [SerializeField]
        protected float popInExplosionTime = .2f;
        [SerializeField]
        protected LeanTweenType popOutTweenType = LeanTweenType.easeInBounce;
        [SerializeField]
        protected LeanTweenType popInTweenType = LeanTweenType.easeOutBounce;

        protected CircleCollider2D _collider;

        public abstract bool IsSpecialType { get; }
        public abstract CircleType Type { get; }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        internal virtual HashSet<StandardCircle> FindCollidingCircles(HashSet<StandardCircle> result = null)
        {

            HashSet<StandardCircle> allOtherCircles = new();
            if (result == null)
                result = new();

            var touchedCircles = FindNeighbors();
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

        protected virtual Collider2D[] FindNeighbors()
        {
            return Physics2D.OverlapCircleAll(transform.position, _collider.radius * rayCastSizeMultiplier);
        }

        [NaughtyAttributes.Button]
        public void Pop()
        {
            var touchingCircles = FindCollidingCircles();
            if (touchingCircles.Count >= GameManager.Instance.Settings.CirclesToMatch || IsSpecialType)
            {
                if (touchingCircles.Count >= GameManager.Instance.Settings.CirclesToSpawnSpecial && !IsSpecialType)
                {
                    SpawnSpecialCircle();
                }
                GameManager.Instance.onCircleTap?.Invoke(touchingCircles);
                touchingCircles.ToList().ForEach(x => x.Explode());
            }
        }

        protected void SpawnSpecialCircle()
        {
            var newCircle = CirclesPooler.Instance.PoolRandomSpecialCircle();
            newCircle.transform.position = transform.position;
            var startingScale = newCircle.transform.localScale;
            newCircle.transform.localScale = Vector3.zero;
            newCircle.gameObject.SetActive(true);
            newCircle.gameObject.LeanScale(startingScale, popInExplosionTime).setEase(popInTweenType);

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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AngryCirclesDreamBlast
{
    public class BombCircle : StandardCircle
    {
        [Header("Bomb Parameters")]
        [SerializeField]
        private float explosionRange;

        public override bool IsSpecialType => true;
        public override CircleType Type =>  CircleType.BOMB;

        internal override HashSet<StandardCircle> FindCollidingCircles(HashSet<StandardCircle> result = null)
        {
            HashSet<StandardCircle> otherSpecialsTouched = new();
            if (result == null)
                result = new();

            var touchedCircles = FindNeighbors();

            foreach (var col in touchedCircles)
            {
                var otherCircle = col.GetComponent<StandardCircle>();
                if (otherCircle != null && !result.Contains(otherCircle))
                {
                    result.Add(otherCircle);
                    if (otherCircle.IsSpecialType)
                        otherSpecialsTouched.Add(otherCircle);
                }
            }

            foreach(var circle in otherSpecialsTouched)
            {
                circle.FindCollidingCircles(result);
            }

            return result;
        }

        protected override Collider2D[] FindNeighbors()
        {
            return Physics2D.OverlapCircleAll(transform.position, _collider.radius * explosionRange);
        }

        protected override void Explode()
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

    }

}
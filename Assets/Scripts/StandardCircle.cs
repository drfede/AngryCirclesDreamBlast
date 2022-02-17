using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AngryCirclesDreamBlast
{
    public class StandardCircle : MonoBehaviour
    {


        private string id;

        private HashSet<GameObject> collidingObjects = new();

        private CircleCollider2D _collider;


        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public HashSet<StandardCircle> FindCollidingCircles(HashSet<StandardCircle> result = null)
        {

            HashSet<StandardCircle> allOtherCircles = new();
            if (result == null)
                result = new();

            var touchedCircles = Physics2D.OverlapCircleAll(transform.position, _collider.radius);
            touchedCircles = touchedCircles.Where(x => x.CompareTag(gameObject.tag) && x.gameObject != gameObject).ToArray();


            foreach(var col in touchedCircles)
            {
                var otherCircle = col.GetComponent<StandardCircle>();
                if (!result.Contains(otherCircle))
                {
                    result.Add(otherCircle);
                    allOtherCircles.Add(otherCircle);
                }
            }

            foreach(var circle in allOtherCircles)
            {
                circle.FindCollidingCircles(result);
            }

            return result;
        }

        [NaughtyAttributes.Button]
        public void Pop()
        {
            var touchingCircles = FindCollidingCircles();
            if (touchingCircles.Count > 3)
            {
                touchingCircles.ToList().ForEach(x => x.Explode());
            }
        }

        protected void Explode()
        {
            GameObject.Destroy(gameObject,.01f);
        }
    }

}
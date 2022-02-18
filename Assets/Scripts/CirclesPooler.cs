using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AngryCirclesDreamBlast
{
    public class CirclesPooler : Utilities.Singleton<CirclesPooler>
    {
        [SerializeField]
        private CircleMap circleMap;

        [SerializeField]
        private Transform pooledParent = null;

        private Dictionary<StandardCircle.CircleType, List<StandardCircle>> circleDic = new();

        public override void Awake()
        {
            if (pooledParent == null)
                pooledParent = transform;

            if (circleDic == null)
                circleDic = new();

            foreach (var elem in circleMap.Circles)
            {
                for (int i = 0; i < elem.CountToPool; ++i)
                {
                    var newObj = InstantiateNewcircle(elem.Circle.Type);
                    newObj.gameObject.SetActive(false);
                }
            }
        }

        private StandardCircle InstantiateNewcircle(StandardCircle.CircleType type)
        {
            var circleToInstantiate = circleMap.Circles.Find(x => x.Circle.Type == type).Circle;
            StandardCircle newCircle = null;
            if (circleToInstantiate != null)
            {
                newCircle = Instantiate(circleToInstantiate, pooledParent);

                circleDic.TryGetValue(newCircle.Type, out var circleList);

                if (circleList == null)
                    circleList = new();

                circleList.Add(newCircle);
                circleDic[newCircle.Type] = circleList;

            }
            return newCircle;
        }

        public StandardCircle PoolCircle(StandardCircle.CircleType typeToPool)
        {
            StandardCircle circle = null;


            if (circleDic.TryGetValue(typeToPool, out var list))
            {
                circle = list.Where(x => !x.gameObject.activeInHierarchy).FirstOrDefault();
            }

            if (circle == null)
            {
                circle = InstantiateNewcircle(typeToPool);
            }

            return circle;
        }

        public void GiveBack(StandardCircle circle)
        {
            if (circleDic.TryGetValue(circle.Type, out var list))
            {
                if (list == null)
                    list = new();

                if (!list.Contains(circle))
                    list.Add(circle);

                circle.gameObject.SetActive(false);
            }
        }



    }

}
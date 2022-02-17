using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryCirclesDreamBlast
{
    [CreateAssetMenu(fileName = "New Circle Map", menuName = "ACDB/Circle Map")]
    public class CircleMap : ScriptableObject
    {


        [SerializeField, NaughtyAttributes.ReorderableList]
        private List<CircleMapElement> circles = new();

        public List<CircleMapElement> Circles { get => circles; }
    }

    [Serializable]
    public class CircleMapElement
    {
        [SerializeField]
        private StandardCircle circle;
        [SerializeField, Min(0)]
        private int countToPool;
        [SerializeField]
        private Sprite circleSprite;
        [SerializeField]
        private Color circleColor;

        public StandardCircle Circle { get => circle; }
        public int CountToPool { get => countToPool; }
        public Sprite CircleSprite { get => circleSprite; }
        public Color CircleColor { get => circleColor; }
    }

}
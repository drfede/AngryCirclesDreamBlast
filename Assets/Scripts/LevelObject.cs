using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryCirclesDreamBlast
{

    [CreateAssetMenu(fileName = "New Level", menuName = "ACDB/NewLevel")]
    public class LevelObject : ScriptableObject
    {



        [SerializeField, NaughtyAttributes.ReorderableList]
        private List<CircleLevelTarget> levelTargets = new();

        [SerializeField]
        private int movesLimit;

        [SerializeField]
        private int startingCircles;

        public List<CircleLevelTarget> LevelTargets { get => levelTargets; set => levelTargets = value; }
        public int MovesLimit { get => movesLimit; set => movesLimit = value; }
        public int StartingCircles { get => startingCircles; set => startingCircles = value; }
    }

    [System.Serializable]
    public class CircleLevelTarget
    {
        [SerializeField]
        private StandardCircle.CircleType type;
        [SerializeField]
        private int targetNumber;

        public StandardCircle.CircleType Type { get => type; set => type = value; }
        public int TargetNumber { get => targetNumber; set => targetNumber = value; }
    }

}
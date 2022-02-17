using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryCirclesDreamBlast
{
    [CreateAssetMenu(fileName ="New Settings", menuName ="ACDB/Settings")]
    public class Settings : ScriptableObject
    {

        [Header("Main Settings")]
        [SerializeField]
        private int circlesToMatch = 3;


        public int CirclesToMatch { get => circlesToMatch; }
    }

}
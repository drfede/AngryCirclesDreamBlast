using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryCirclesDreamBlast
{
    public class RedCircle : StandardCircle
    {
        public override bool IsSpecialType => false;

        public override CircleType Type => CircleType.RED;

    }
}

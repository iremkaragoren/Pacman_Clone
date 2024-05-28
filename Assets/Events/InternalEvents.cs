using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace Events
{
    public static class InternalEvents
    {
        public static UnityAction EnemyOnNode;
        public static UnityAction<int> NormalFoodEating;
        public static UnityAction ExtraFoodEating;
        public static UnityAction PowerfullFoodEating;
        public static UnityAction PlayerDeath;
        public static UnityAction PlayerDeathAnimationComplete;
        public static UnityAction<Vector2> GhostDirectionChanged;
        public static UnityAction<Vector2> PlayerDirectionChanged;
        public static UnityAction PlayerEatenGhost;
        public static UnityAction PlayTurnPosition;



    }
}
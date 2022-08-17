using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class StateMemory 
    {
        public StateAttribute<Vector3> CurrentPosition { get; set; } = new StateAttribute<Vector3>() { Key = "currentPos" };
        public StateAttribute<DetectableTarget> CurrentTarget { get; set; } = new StateAttribute<DetectableTarget>() { Key = "currentTarget" };
        public bool HasTarget => CurrentTarget.Value != null;
        public StateAttribute<float> HungerLevel { get; set; } = new StateAttribute<float>() { Key = "hungerLevel" };

    }
}

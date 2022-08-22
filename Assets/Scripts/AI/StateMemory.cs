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
        HashSet<KeyValuePair<string, object>> WorldStates = new HashSet<KeyValuePair<string, object>>();

        public HashSet<KeyValuePair<string, object>> GetWorldState => WorldStates;

        public StateMemory()
        {
            //WorldStates.Add(new KeyValuePair<string, object>("currentPosition", Vector3.zero));
            //WorldStates.Add(new KeyValuePair<string, object>("currentTarget", null));
            //WorldStates.Add(new KeyValuePair<string, object>("isHungry", false));
            //WorldStates.Add(new KeyValuePair<string, object>("hasTargets", false));
        }

        public StateMemory Instantiate(StateMemory mem)
        {
            StateMemory copiedWorldState = new StateMemory();
            foreach (KeyValuePair<string, object> state in mem.WorldStates)
            {
                copiedWorldState.WorldStates.Add(state);
            }
            return copiedWorldState;
        }


        public void ChangeValue(string key, object value)
        {
            if (WorldStates.Any(a => a.Key.Equals(key)))
                WorldStates.Remove(WorldStates.Where(a => a.Key == key).FirstOrDefault());
            WorldStates.Add(new KeyValuePair<string, object>(key, value));
        }

        public object GetValue(string key)
        {
            return WorldStates.Where(a => a.Key == key).FirstOrDefault().Value;
        }

        public StateMemory Clone()
        {
            return Instantiate(this);
        }

        public bool AddWorldState(string key, object value)
        {
            try
            {
                WorldStates.Add(new KeyValuePair<string, object>(key, value));
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}

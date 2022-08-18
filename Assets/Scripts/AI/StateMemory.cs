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
            WorldStates.Add(new KeyValuePair<string, object>("currentPosition", Vector3.zero));
            WorldStates.Add(new KeyValuePair<string, object>("currentTarget", null));
            WorldStates.Add(new KeyValuePair<string, object>("isHungry", false));
            WorldStates.Add(new KeyValuePair<string, object>("hasTargets", false));
        }

        public void ChangeValue<T>(string key, T value)
        {
            System.Type type = WorldStates.Where(a => a.Key == key).FirstOrDefault().Value.GetType();
            Debug.Log("Key: " + key + " Type of Value: " + type.Name);
            if (type == typeof(T))
            {
                WorldStates.Remove(WorldStates.Where(a => a.Key == key).FirstOrDefault());
                WorldStates.Add(new KeyValuePair<string, object>(key, value));
            }

        }

        public void ChangeValue(string key, object value)
        {
            WorldStates.Remove(WorldStates.Where(a => a.Key == key).FirstOrDefault());
            WorldStates.Add(new KeyValuePair<string, object>(key, value));
        }

        public object GetValue(string key)
        {
            return WorldStates.Where(a => a.Key == key).FirstOrDefault().Value;
        }

        
    }
}

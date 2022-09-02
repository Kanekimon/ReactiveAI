using System.Collections.Generic;
using System.Linq;


public class StateMemory
{
    List<KeyValuePair<string, object>> WorldStates = new List<KeyValuePair<string, object>>();

    public List<KeyValuePair<string, object>> GetWorldState => WorldStates;

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
        if (!WorldStates.Any(a => a.Key.Equals(key)))
            return null;
        return WorldStates.Where(a => a.Key == key).FirstOrDefault().Value;
    }

    public T GetValue<T>(string key)
    {
        if (!WorldStates.Any(a => a.Key.Equals(key)))
            return default(T);
        return (T)System.Convert.ChangeType(WorldStates.Where(a => a.Key == key).FirstOrDefault().Value, typeof(T));
    }

    public StateMemory Clone()
    {
        return Instantiate(this);
    }

    public bool AddWorldState(string key, object value)
    {
        if (WorldStates.Any(a => a.Key.Equals(key)))
        {
            ChangeValue(key, value);
            return true;
        }

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

    public void RemoveWorldState(string key)
    {
        KeyValuePair<string, object> toDelete = new KeyValuePair<string, object>();
        if (WorldStates.Any(a => a.Key.Equals(key)))
        {
            toDelete = WorldStates.Where(a => a.Key.Equals(key)).FirstOrDefault();
        }
        WorldStates.Remove(toDelete);

    }
}



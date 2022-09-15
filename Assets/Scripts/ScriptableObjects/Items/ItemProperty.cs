using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ItemPropertyType
{
    consume, 
    damage,
    durability
}

[Serializable]
public class ItemProperty
{
    public ItemPropertyType Type;
    public string Name;
    public float Value;
}


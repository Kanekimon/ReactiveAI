using System;

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


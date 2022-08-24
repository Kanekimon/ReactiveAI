using System.Collections.Generic;


public enum ItemType
{
    Food,
    Weapon,
    Armor
}

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<ItemType> ItemTypes { get; set; }




}

using UnityEngine;

public enum ToolType
{
    axe,
    pickaxe,
    shovel,
    none
}


[CreateAssetMenu(menuName = "Items/Tool")]
public class ToolItem : Item
{
    public ToolType ToolType;


}

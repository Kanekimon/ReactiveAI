public abstract class GameEvent
{
    public string EventDescription;
}

public class GatherGameEvent : GameEvent
{
    public Item Item;
    public int Amount;

    public GatherGameEvent(Item item, int amount)
    {
        Item = item;
        Amount = amount;
    }
}

public class CraftingGameEvent : GameEvent
{
    public Recipe Recipe;
    public CraftingGameEvent(Recipe recipe)
    {
        Recipe = recipe;
    }
}



public class TeachGameEvent : GameEvent
{
    public InteractionType Action;
    public int Amount;

    public TeachGameEvent(InteractionType action, int amount)
    {
        Action = action;
        Amount = amount;
    }
}

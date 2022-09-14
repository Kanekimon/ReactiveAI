using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class CraftObjective : QuestObjective
{
    public Recipe Recipe;
    public int Amout;

    public override string GetDescription()
    {
        return $"Craft {RequiredAmount}x {Recipe.Result.Name}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<CraftingGameEvent>(OnCraft);
    }

    private void OnCraft(CraftingGameEvent eventInfo)
    {
        if (eventInfo.Recipe == Recipe)
        {
            CurrentAmount++;
            Evaluate();
        }
    }

}


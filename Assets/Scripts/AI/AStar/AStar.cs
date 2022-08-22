using Assets.Scripts.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/**
    G: The length of the path from the start node to this node.
    H: The straight-line distance from this node to the end node.
    F: An estimate of the total distance if taking this route. It’s calculated simply using F = G + H.
*/

class Node
{
    public float GCost = 0;
    public float HCost = 0;
    public StateMemory PreWorldState = new StateMemory();
    public StateMemory PostWorldState = new StateMemory();
    public float FCost => GCost + HCost;

    public Node(ActionBase action, float gCost)
    {
        this.GCost = action.Cost() + gCost;
    }
    public Node()
    {
        
    }

}

public class AStar
{
    public static Queue<ActionBase> PlanActionSequence(StateMemory currentWorldState, StateMemory requiredWorldState, List<ActionBase> allActions)
    {
        Queue<ActionBase> queue = new Queue<ActionBase>();

        List<Node> AllPossibleNodes = ConvertActionsToNode(allActions, currentWorldState);


        Node currentNode = new Node() { GCost = 0, HCost = 0, PostWorldState = currentWorldState.Clone(), PreWorldState = requiredWorldState };
        List<Node> InWork = GetTransitionableActions(AllPossibleNodes, currentNode.PreWorldState);
        bool run = true;
        List<Node> closed = new List<Node>();

        List<StateMemory> explored = new List<StateMemory>();
        explored.Add(currentNode.PostWorldState);

        while (run)
        {
            if (InWork.Count == 0)
                return null;

            if (WorldStateEquals(currentNode.PostWorldState, requiredWorldState))
            {
                run = false;
                break;
            }

            closed.Add(currentNode);
            currentNode = GetLowestCost(AllPossibleNodes);
            


        }




        return queue;
    }
    
    static Node GetLowestCost(List<Node> nodes)
    {
        Node lowest = null;
        float lowestCost = float.MaxValue;
        foreach(var node in nodes)
        {
            if(node.GCost < lowestCost)
            {
                lowest = node;
                lowestCost = node.GCost;
            }
        }

        return lowest;
    }

    static bool WorldStateEquals(StateMemory first, StateMemory second)
    {
        bool equals = true;

        foreach(KeyValuePair<string, object> state in first.GetWorldState)
        {
            if (state.Value is string && state.Value.ToString() == "dontCare")
                continue;
            equals &= second.GetWorldState.Any(a => a.Key == state.Key && a.Value == state.Value);
        }

        return equals;
    }


    static List<Node> GetTransitionableActions(List<Node> AllPossibleNodes, StateMemory requiredWorldState)
    {
        List<Node> TransitionableActions = new List<Node>();

        foreach(Node n in AllPossibleNodes)
        {

        }



        return TransitionableActions;
    }

    



    static List<Node> ConvertActionsToNode(List<ActionBase> actions, StateMemory currentWorldState)
    {
        List<Node> possibleActions = new List<Node>();
        foreach(var vA in actions)
        {
            Node n = new Node(vA,0);
            n.PostWorldState = currentWorldState;
            foreach (KeyValuePair<string, object> vB in vA.GetEffects())
            {
                n.PostWorldState.ChangeValue(vB.Key, vB.Value);
            }
            n.PreWorldState = new StateMemory();

            foreach(KeyValuePair<string, object> state in currentWorldState.GetWorldState)
            {
                KeyValuePair<string, object> precon = vA.GetPreconditions().Where(a => a.Equals(state)).FirstOrDefault();
                if (!precon.Equals(null))
                    n.PreWorldState.ChangeValue(precon.Key, precon.Value);
                else
                    n.PreWorldState.ChangeValue(state.Key, "dontCare");
            }

        }

        return possibleActions;
    }




}


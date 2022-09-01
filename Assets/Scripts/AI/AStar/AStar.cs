using Assets.Scripts.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
    G: The length of the path from the start node to this node.
    H: The straight-line distance from this node to the end node.
    F: An estimate of the total distance if taking this route. It’s calculated simply using F = G + H.
*/

class Node
{
    public float GCost = 0;
    public float HCost = 0;
    public Node PreviousNode;
    public List<KeyValuePair<string, object>> openPreconditions = new List<KeyValuePair<string, object>>();
    public ActionBase Action;

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
    public static Queue<ActionBase> PlanActionSequence(BaseGoal goal, List<ActionBase> allActions, StateMemory worldState)
    {

        Queue<ActionBase> queue = new Queue<ActionBase>();

        int counter = 100;

        if (goal is ChopTreeGoal)
            Debug.Log("test");

        Node currentNode = new Node() { GCost = 0, HCost = goal.Preconditions.Count, openPreconditions = new List<KeyValuePair<string, object>>(goal.Preconditions) };
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        currentNode = CheckForAlreadySatisfiedWorldStates(currentNode, worldState);

        bool run = true;
        open.Add(currentNode);

        while (counter > 0 && run)
        {
            if (open.Count == 0)
                return null;

            currentNode = GetLowestCostNode(open);
            open.Remove(currentNode);

            if (currentNode.openPreconditions.Count == 0)
            {
                closed.Add(currentNode);
                run = false;
                break;
            }


            List<ActionBase> possibleActions = GetPossibleActions(allActions, currentNode);

            foreach (ActionBase action in possibleActions)
            {
                float gCost = currentNode.GCost + action.Cost();
                float hCost = CountUnsatisfiedPreconditions(action, currentNode.openPreconditions);
                float fCost = gCost + hCost;
                if (open.Any(a => a.Action == action))
                {
                    Node inOpen = open.Where(a => a.Action == action).FirstOrDefault();
                    if (inOpen.FCost > fCost)
                    {
                        inOpen.PreviousNode = currentNode;
                        inOpen.GCost = gCost;
                        inOpen.HCost = hCost;
                        inOpen.openPreconditions.Clear();
                        inOpen.openPreconditions = GetOpenPreconditions(action, currentNode.openPreconditions);
                    }
                }
                else
                {
                    Node n = new Node()
                    {
                        Action = action,
                        GCost = gCost,
                        HCost = hCost,
                        PreviousNode = currentNode,
                        openPreconditions = GetOpenPreconditions(action, currentNode.openPreconditions)
                    };
                    open.Add(n);
                }
            }

            closed.Add(currentNode);

            counter--;
        }


        while (currentNode.PreviousNode != null)
        {
            queue.Enqueue(currentNode.Action);
            currentNode = currentNode.PreviousNode;
        }

        return queue;
    }

    static Node CheckForAlreadySatisfiedWorldStates(Node current, StateMemory states)
    {
        for (int i = current.openPreconditions.Count - 1; i >= 0; i--)
        {
            KeyValuePair<string, object> precon = current.openPreconditions[i];

            if (states.GetValue(precon.Key) != null)
            {
                if (states.GetValue(precon.Key).Equals(precon.Value))
                    current.openPreconditions.Remove(precon);
            }
        }
        return current;
    }

    static Node GetLowestCostNode(List<Node> open)
    {
        return open.OrderBy(a => a.FCost).FirstOrDefault();
    }

    static List<KeyValuePair<string, object>> GetOpenPreconditions(ActionBase action, List<KeyValuePair<string, object>> openPreconditions)
    {

        List<KeyValuePair<string, object>> stillOpen = new List<KeyValuePair<string, object>>();
        foreach (KeyValuePair<string, object> item in openPreconditions)
        {
            bool satisfied = false;
            foreach (KeyValuePair<string, object> effect in action.GetEffects())
            {
                if (effect.Key == item.Key && effect.Value.Equals(item.Value))
                    satisfied = true;
            }
            if (!satisfied)
                stillOpen.Add(new KeyValuePair<string, object>(item.Key, item.Value));
        }

        stillOpen.AddRange(action.GetOpenPreconditions());
        return stillOpen;
    }


    private static float CountUnsatisfiedPreconditions(ActionBase action, List<KeyValuePair<string, object>> openPreconditions)
    {
        float count = 0;

        foreach (KeyValuePair<string, object> item in openPreconditions)
        {
            bool satisifed = false;
            foreach (KeyValuePair<string, object> effect in action.GetEffects())
            {
                if (effect.Key == item.Key && effect.Value.Equals(item.Value))
                {
                    satisifed = true;
                    break;
                }
            }
            if (!satisifed)
                count++;
        }

        return count + action.OpenPreconditionCount();
    }

    private static Node GetLowestCostAction(List<ActionBase> allActions, Node lastNode)
    {

        List<ActionBase> validActions = allActions.Where(a => SatisfiesOneConditon(a.GetEffects(), lastNode.openPreconditions)).ToList();
        List<Node> validNodes = ConvertActionsToNode(validActions, lastNode);

        return validNodes.OrderBy(a => a.FCost).FirstOrDefault();

    }

    private static bool SatisfiesOneConditon(List<KeyValuePair<string, object>> a, List<KeyValuePair<string, object>> b)
    {
        int count = 0;
        for (int i = 0; i < a.Count; i++)
        {
            foreach (KeyValuePair<string, object> item in b)
            {
                if (a[i].Key == item.Key)
                {
                    if (a[i].Value.Equals(item.Value))
                        count++;
                    else
                        return false;

                }
            }

        }
        return count > 0;
    }

    static List<Node> GetPossibleTransitions(List<ActionBase> allActions, Node lastNode)
    {
        List<ActionBase> validActions = allActions.Where(a => SatisfiesOneConditon(a.GetEffects(), lastNode.openPreconditions)).ToList();
        List<Node> validNodes = ConvertActionsToNode(validActions, lastNode);

        return validNodes;
    }

    static List<ActionBase> GetPossibleActions(List<ActionBase> allActions, Node lastNode)
    {
        return allActions.Where(a => SatisfiesOneConditon(a.GetEffects(), lastNode.openPreconditions) && a.CanRun()).ToList();
    }
    static List<Node> ConvertActionsToNode(List<ActionBase> actions, Node lastNode)
    {
        List<Node> possibleActions = new List<Node>();
        foreach (var vA in actions)
        {

            Node n = new Node(vA, vA.Cost() + lastNode.GCost);
            n.PreviousNode = lastNode;
            n.openPreconditions = lastNode.openPreconditions;
            n.Action = vA;
            foreach (var effect in vA.GetEffects())
            {
                if (n.openPreconditions.Any(a => a.Key == effect.Key))
                {
                    KeyValuePair<string, object> keyMatch = n.openPreconditions.Where(a => a.Key == effect.Key).FirstOrDefault();
                    if (keyMatch.Value.Equals(effect.Value))
                        n.openPreconditions.Remove(keyMatch);
                }
            }

            if (vA.GetPreconditions().Count > 0)
                n.openPreconditions.AddRange(vA.GetPreconditions());

            n.HCost = n.openPreconditions.Count;
            possibleActions.Add(n);
        }

        return possibleActions;
    }




}


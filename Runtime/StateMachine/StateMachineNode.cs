using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class StateMachineNode : Node
{
    [Input] public int entry;
    [Output] public int exit;

    // Use this for initialization
    protected override void Init()
    {
        base.Init();

    }

    public virtual void Execute()
    {
    }

    public void NextNode(string _exit)
    {
        foreach (var port in this.Outputs)
        {
            port.GetInputValue();
        }
    }

    // public override object GetValue(NodePort port)
    // {

    // }

    public override void OnCreateConnection(NodePort from, NodePort to)
    {
        base.OnCreateConnection(from, to);


    }
}
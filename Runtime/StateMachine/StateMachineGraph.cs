using System.Collections.Generic;
using UnityEngine;
using XNode;
using System;

[CreateAssetMenu]
public class StateMachineGraph : NodeGraph
{
    [Serializable]
    public class TriggerValue
    {
        [SerializeField]
        string name;
        [SerializeField]
        bool value;
        [NonSerialized]
        float timer;
    }

    [SerializeField]
    public List<TriggerValue> triggers = new List<TriggerValue>();

}
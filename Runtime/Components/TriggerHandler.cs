using System.Collections.Generic;
using System;

namespace EricGames.Core.Components
{
    public class TriggerHandler<TriggerType> where TriggerType : Enum
    {
        private class TriggerTypeState
        {
            public bool value = false;
            public float counter = 0.0f;
        }

        private Dictionary<TriggerType, TriggerTypeState> triggerTypeStates = new Dictionary<TriggerType, TriggerTypeState>();

        private TriggerType[] triggerTypes;

        public TriggerHandler()
        {
            triggerTypes = (TriggerType[])Enum.GetValues(typeof(TriggerType));

            foreach (TriggerType triggerType in triggerTypes)
            {
                triggerTypeStates.Add(triggerType, new TriggerTypeState());
            }
        }

        public void Tick(float deltaTime)
        {
            foreach (var state in triggerTypeStates.Values)
            {
                if (state.value)
                {
                    if (state.counter > 0.0f)
                    {
                        state.counter -= deltaTime;
                    }

                    if (state.counter <= 0.0f)
                    {
                        state.value = false;
                    }
                }
            }
        }

        public void SetTrigger(TriggerType triggerType, float time)
        {
            var state = GetState(triggerType);
            state.value = true;
            state.counter = time;
        }

        public void ResetTrigger(TriggerType triggerType)
        {
            var state = GetState(triggerType);
            state.value = false;
            state.counter = -0.1f;
        }

        private TriggerTypeState GetState(TriggerType triggerType)
        {
            if (!triggerTypeStates.TryGetValue(triggerType, out var state))
            {
                state = new TriggerTypeState();
                triggerTypeStates.Add(triggerType, state);
            }

            return state;
        }

        public bool GetTriggerValue(TriggerType triggerType)
        {
            return GetState(triggerType).value;
        }
    }
}
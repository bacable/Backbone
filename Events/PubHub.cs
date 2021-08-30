using System;
using System.Collections.Generic;
using System.Linq;

namespace Backbone.Events
{
    public class PubHub<T>
    {
        public static T LastEvent { get; private set; }

        private static Dictionary<T, List<ISubscriber<T>>> Subscriptions = new Dictionary<T, List<ISubscriber<T>>>();

        private static Dictionary<T, List<int>> RegisteredGroupActions = new Dictionary<T, List<int>>();

        private static Dictionary<T, List<IPrerequisite<T>>> Prerequisites = new Dictionary<T, List<IPrerequisite<T>>>();

        public static bool CheckCanContinue(T eventType, List<T> allowedLastStates = null)
        {
            return CheckPrerequisites(eventType) && (allowedLastStates == null || allowedLastStates.Any(x => x.Equals(LastEvent)));
        }

        public static bool CheckPrerequisites(T eventType)
        {
            var prerequisites = Prerequisites[eventType];

            if(prerequisites != null)
            {
                return prerequisites.All(x => x.Check(eventType));
            }

            return true;
        }

        public static void SetPrerequisite(T eventType, IPrerequisite<T> prerequisite)
        {
            if (!Prerequisites.ContainsKey(eventType))
            {
                Prerequisites.Add(eventType, new List<IPrerequisite<T>>());
            }

            Prerequisites[eventType].Add(prerequisite);
        }

        public static void RegisterAction(T eventType, int id)
        {
            if(!RegisteredGroupActions.ContainsKey(eventType))
            {
                RegisteredGroupActions.Add(eventType, new List<int>());
            }

            RegisteredGroupActions[eventType].Add(id);
        }

        public static void FinishedAction(T eventType, int id, object payload)
        {
            var actionList = RegisteredGroupActions[eventType];
            if (actionList != null && actionList.Contains(id))
            {
                actionList.Remove(id);
            }

            if(actionList.Count == 0)
            {
                Raise(eventType, payload);
            }
        }

        public static void Sub(T eventType, ISubscriber<T> subscriber)
        {
            if (!Subscriptions.ContainsKey(eventType))
            {
                Subscriptions.Add(eventType, new List<ISubscriber<T>>());
            }
            Subscriptions[eventType].Add(subscriber);
        }

        public static void Raise(T eventType, object payload)
        {
            if(!Subscriptions.ContainsKey(eventType))
            {
                return;
            }

            LastEvent = eventType;

            var eventSubscribers = Subscriptions[eventType];

            eventSubscribers.ForEach(x => x.HandleEvent(eventType, payload));
        }

        public static void Unsubscribe(T eventType, ISubscriber<T> subscriber)
        {
            if(Subscriptions[eventType].Contains(subscriber))
            {
                Subscriptions[eventType].Remove(subscriber);
            }
        }
    }
}

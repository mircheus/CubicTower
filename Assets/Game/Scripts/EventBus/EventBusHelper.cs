using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Scripts
{
    public static class EventBusHelper
    {
        private static readonly Dictionary<Type, List<Type>> CashedSubscriberTypes = new();

        public static List<Type> GetSubscriberTypes(IGlobalSubscriber globalSubscriber)
        {
            Type type = globalSubscriber.GetType();
            if (CashedSubscriberTypes.TryGetValue(type, out List<Type> types))
            {
                return types;
            }

            List<Type> subscriberTypes = type.GetInterfaces()
                .Where(t => t.GetInterfaces().Contains(typeof(IGlobalSubscriber))).ToList();

            CashedSubscriberTypes[type] = subscriberTypes;
            return subscriberTypes;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Abstract_Entities
{
    internal class Grouping
    {
        public  ThreadSafeList<Prop> Props = new ThreadSafeList<Prop>();
        public Grouping(Prop prop1, Prop prop2)
        {
            Props.Add(prop1);
            Props.Add(prop2);
        }

        public Light GetLightState(Character? lookingCharacter)
        {
            Prop prop = Props.First();
            return prop.GetLightState(lookingCharacter);
        }

        public bool RunAllOnBeforeMoveFromEventsInGroup(Soul soul)
        {
            bool interruptMove = false;
            foreach (Prop prop in Props)
            {
                if (prop.RunOnBeforeMoveFromEvents(soul))
                    interruptMove = true;
            }
            return interruptMove;
        }
    }
}

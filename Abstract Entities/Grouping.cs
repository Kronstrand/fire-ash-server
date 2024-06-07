using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;

namespace fire_ash_server.Abstract_Entities
{
    internal class Grouping
    {
        public  ThreadSafeList<Prop> Characters = new ThreadSafeList<Prop>();
        public Grouping(Prop prop1, Prop prop2)
        {
            Characters.Add(prop1);
            Characters.Add(prop2);
        }
    }
}

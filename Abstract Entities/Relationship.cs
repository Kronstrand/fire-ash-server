using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server
{
    internal class Relationship
    {
        public Faction Faction1;
        public Faction Faction2;
        public int Value;

        public Relationship(Faction faction1, Faction faction2, int value) 
        {
            Faction1 = faction1;
            Faction2 = faction2;
            Value = value;
        }

        public bool IsHostile()
        {

            return GetStatus() == RelationshipStatus.bad;
        }

        public RelationshipStatus GetStatus()
        {
            if (Value < 0)
                return RelationshipStatus.bad;
            if (Value >= 0 || Value <= 10)
                return RelationshipStatus.neutral;

            return RelationshipStatus.good;
        }

        public static Relationship CreateNew(Faction faction1, Faction faction2)
        {
            Relationship rel = new Relationship(faction1, faction2, 3);
            Program.WorldSoul.Relationships.Add(rel);
            return rel;
        }

    }
}

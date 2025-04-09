using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server
{
    [Serializable]
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

            // under 0 = bad
            // fra 0 til 10 = neutral
            // over 10 = good

            if (Value < 0)
                return RelationshipStatus.bad;
            if (Value >= 0 && Value <= 10)
                return RelationshipStatus.neutral;

            return RelationshipStatus.good;
        }

        public static Relationship CreateNew(FactionKey factionKey1, FactionKey factionKey2, int value)
        {
            return CreateNew(
                Faction.Get(factionKey1),
                Faction.Get(factionKey2),
                value);
        }

        public static Relationship CreateNew(Faction faction1, Faction faction2)
        {
            return CreateNew(faction1, faction2, 3);
        }

        public static Relationship CreateNew(Faction faction1, Faction faction2, int value)
        {
            if (Get(faction1, faction2) != null)
                throw new Exception($"There is already a Relationship with {faction1.Name} & {faction2.Name}");

            Relationship rel = new Relationship(faction1, faction2, value);
            Program.WorldSoul.Relationships.Add(rel);
            return rel;
        }

        public static Relationship? Get(FactionKey key1, FactionKey key2)
        {
            Faction faction1 = Program.WorldSoul.GetFaction(key1);
            Faction faction2 = Program.WorldSoul.GetFaction(key2);

            return Get(faction1, faction2);
        }

        public static Relationship? Get(Faction faction1, Faction faction2)
        {
            return Program.WorldSoul.Relationships.Where(r => (r.Faction1 == faction1 && r.Faction2 == faction2) || (r.Faction1 == faction2 && r.Faction2 == faction1)).FirstOrDefault();
        }

        public static void Set(FactionKey key1, FactionKey key2, int value)
        {
            Relationship? rel = Get(key1, key2);

            if (rel == null)
            {
                CreateNew(key1, key2, value);
                return;
            }
            
            rel.Value = value;
        }

    }
}

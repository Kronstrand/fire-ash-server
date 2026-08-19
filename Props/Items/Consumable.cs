using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props.Items.Weapons;

namespace fire_ash_server.Props.Items
{
    internal class Consumable : Item
    {
        //public Func<Soul,Task> Consume;
        [JsonInclude]   public ConsumableKey ConsumeKey;
        //[JsonIgnore]    public Func<Soul, RangeType, Weapon?, Prop?, bool>? Requirement;
        //[JsonIgnore]    public Action<Soul>? NotAvailable;
        [JsonInclude]   public Weapon? Weapon;
        [JsonInclude]   public RangeType Range = RangeType.None;
        [JsonInclude]   public bool HasTarget = false;
        [JsonInclude]   public string CharacterId = "";
        [JsonIgnore]    public bool WasNotConsumed;

        /*public Consumable(string name, string description, Func<Soul, Task> consume, double value) : base(name, description, value)
        {
            Consume = consume;
        }*/

        public Consumable() {}

        public Consumable(string name, string description, ConsumableKey consumeKey, double value) : base(name, description, value)
        {
            ConsumeKey = consumeKey;
        }

        public Character? GetCharacterFromId()
        {
            return Character.GetCharacterFromId(CharacterId);
        }
    }
}

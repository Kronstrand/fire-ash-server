using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;

namespace fire_ash_server.Abstract_Entities
{
    [Serializable]
    internal class GroupedCountedProp
    {
        public Prop? Prop { get; set; } //unpickupable item or exit
        public Dictionary<string,CountedCharacter> CountedCharacters { get; set; }
        public bool UniqueName { get; set; }

        public GroupedCountedProp(Prop? prop)
        {
            Prop = prop;
            CountedCharacters = new Dictionary<string, CountedCharacter>();
        }

        public void AddToCountedCharacters(Character character)
        {
            string characterName = character.Name;
            if (CountedCharacters.ContainsKey(characterName))
                CountedCharacters[characterName] = new CountedCharacter(
                    CountedCharacters[characterName].Count + 1,
                    CountedCharacters[characterName].UniqueName);
            else
                CountedCharacters.Add(
                    characterName,
                    new CountedCharacter(
                        1,
                        character.UniqueName));
        }
    }

    [Serializable]
    internal class CountedCharacter
    {
        public int Count { get; set; }
        public bool UniqueName { get; set; }

        public CountedCharacter(int count, bool uniqueName)
        {
            Count = count;
            UniqueName = uniqueName;
        }
    }
}

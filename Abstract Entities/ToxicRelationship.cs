using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;

namespace fire_ash_server.Abstract_Entities
{
    [Serializable]
    internal class ToxicRelationship
    {
        public Character ToxicCharacter;
        public bool ToxicCharacterIsInitiator;
        
        public ToxicRelationship(Character toxicCharacter, bool toxicCharacterIsInitiator)
        {
            ToxicCharacter = toxicCharacter;
            ToxicCharacterIsInitiator = toxicCharacterIsInitiator;
        }
    }
}

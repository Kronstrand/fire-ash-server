using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using fire_ash_server.Props;

namespace fire_ash_server.Abstract_Entities
{
    [Serializable]
    internal class Journal
    {
        Character Character;
        public Dictionary<JournalKey, List<JournalEntry>> Entries = new Dictionary<JournalKey, List<JournalEntry>>();

        public Journal(Character character)
        {
            Character = character;
        }
        
        public void AddNewEntry(JournalKey entryKey, string text)
        {
            //JournalKey 
            if (!Entries.ContainsKey(entryKey))
                Entries.Add(entryKey, new List<JournalEntry>());
            
            Entries[entryKey].Add(new JournalEntry(text));
            _ = Character.Soul.SendAsync($"New journal entry added. Press {MoveKey.j} to view.");
        }
    }

    [Serializable]
    internal class JournalEntry
    {
        public string Text;
        public bool Completed = false;
        public JournalEntry (string text)
        {
            Text = text;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using fire_ash_server.Abstract_Entities;
using fire_ash_server.Enums;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.Moves
{
    internal class SaveGameState : Move
    {
        public SaveGameState(Soul soul) : base(MoveKey.sg.ToString(), $"Save Game")
        {
            Type = MoveType.MinorAction;
            AllowedInCombat = false;
            Hidden = true;
            Action = async () =>
            {
                var options = new JsonSerializerOptions
                {
                    IncludeFields = false,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                Directory.CreateDirectory(Program.SaveFolder);

                //save soulsted character (excluding players)
                List<Character> charsInSoulstones = Program.WorldSoul.SoulstonedCharacters.Values.Where(c => c.NPC).ToList();
                string jsonSoulstoneChars = JsonSerializer.Serialize(charsInSoulstones, options);
                File.WriteAllText(
                    Path.Combine(Program.SaveFolder, $"soulstoned_NPCs.json"),
                    jsonSoulstoneChars);

                foreach (Room room in Program.WorldSoul.Rooms.Values)
                {
                    //save Exit states
                    List<ExitState> exitStates = new List<ExitState>();
                    foreach(Exit exit in room.Exits)
                        exitStates.Add(exit.State);
                    string jsonRoomExitsStates = JsonSerializer.Serialize(exitStates, options);
                    File.WriteAllText(
                        Path.Combine(Program.SaveFolder, $"{room.Name}_exitstates.json"), 
                        jsonRoomExitsStates
                    );

                    //save Items
                    List<Item> AllItemsInRoom = room.Items.OfType<Item>().ToList();
                    string jsonRoomItems = JsonSerializer.Serialize(AllItemsInRoom, options);

                    File.WriteAllText(
                        Path.Combine(Program.SaveFolder, $"{room.Name}_items.json"),
                        jsonRoomItems
                    );

                    //save Chars
                    List<Character> AllCharsInRoom = room.Characters.ToList();
                    string jsonRoomChars = JsonSerializer.Serialize(AllCharsInRoom, options);
                    File.WriteAllText(
                        Path.Combine(Program.SaveFolder, $"{room.Name}_characters.json"),
                        jsonRoomChars
                    );

                    //save Groupings
                    List<Grouping> AllGroupsInRoom = room.Groupings.ToList();
                    List<List<string>> AllGroupsInRoomById = new List<List<string>>();
                    foreach(Grouping grouping in AllGroupsInRoom)
                    {
                        List<string> group = new List<string>();
                        
                        foreach(Prop prop in grouping.Props)
                        {
                            if (prop.GetType() == typeof(Character))
                                if (((Character)prop).Soul.IsDaemon == false)
                                    continue;
                            group.Add(prop.Id);
                        }

                        if(group.Any())
                            AllGroupsInRoomById.Add(group);
                    }
                    string jsonRoomGroups = JsonSerializer.Serialize(AllGroupsInRoomById, options);
                    File.WriteAllText(
                        Path.Combine(Program.SaveFolder, $"{room.Name}_groups.json"),
                        jsonRoomGroups
                    );
                }

                string json = JsonSerializer.Serialize(ItemPopulation.Limits, options);
                File.WriteAllText(
                        Path.Combine(Program.SaveFolder, $"ItemLimits.json"),
                        json
                    );


                /*string json = JsonSerializer.Serialize(soul.Character, options);
                File.WriteAllText(
                        Path.Combine(Program.SaveFolder, $"player.json"),
                        json
                    );*/

                _ = soul.SendAsync("Game has been saved.");

                await Task.CompletedTask;
            };
        }
    }
}

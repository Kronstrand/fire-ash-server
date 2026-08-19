using System.Text.Json.Serialization;
using fire_ash_server.Props;

namespace fire_ash_server.Abstract_Entities
{
    internal class ExitState
    {
        [JsonInclude] public string ExitId { get; set; }
        [JsonInclude] public bool IsOpen { get; set; } = true;
        [JsonInclude] public string VisableClosedDiscription { get; set; } = "";

        public ExitState() { }

        public ExitState(string exitId)
        {
            ExitId = exitId;
        }
        public void ConnectToExitInRoom(Room room)
        {
            foreach(Exit exit in room.Exits)
            {
                if (exit.Id == ExitId)
                {
                    exit.State = this;
                    return;
                }
            }
        }
    }
}
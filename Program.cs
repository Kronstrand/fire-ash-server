using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.WebSockets;
using System.Reflection;
using System.Text.Json.Serialization;
using fire_ash_server;
using fire_ash_server.World;
using fire_ash_server.World.BioMechWorld;

class Program
{
    public static bool NewGlobalTurn = false;
    public static int SecondsPerTurn = 6;
    public static WorldSoul WorldSoul = new WorldSoul();
    public static WorldTick WorldTick = new WorldTick();
    public static GlobalVariables GlobalVariables;
    private static GameLoop GameLoop;
    public static string SaveFolder = "Save";
    //public static ConcurrentDictionary<Guid,Socket> Sockets = new ConcurrentDictionary<Guid,Socket>();
    public static ConcurrentDictionary<Guid, WebSocket> Sockets = new ConcurrentDictionary<Guid, WebSocket>();
    static async Task Main(string[] args)
    {
        int port = 4123; // Use any appropriate port number here


        WorldSoul.InitWorldSoul();
        //GlobalVariables = new GlobalVariables();
        GameLoop = new GameLoop();
        await GameLoop.Open(port, args);
    }
}



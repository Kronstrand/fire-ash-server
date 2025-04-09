using System.Collections.Concurrent;
using System.ComponentModel;
using System.Net.Sockets;
using System.Reflection;
using fire_ash_server;
using fire_ash_server.World;
using fire_ash_server.World.BioMechWorld;

class Program
{
    public static WorldSoul WorldSoul = new WorldSoul();
    public static GlobalVariables GlobalVariables = new GlobalVariables();
    private static GameLoop GameLoop = new GameLoop();
    public static ConcurrentDictionary<Guid,Socket> Sockets = new ConcurrentDictionary<Guid,Socket>();
    static async Task Main(string[] args)
    {
        int port = 4123; // Use any appropriate port number here
        
        await GameLoop.Open(port);
    }
}



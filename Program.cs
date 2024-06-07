using System.ComponentModel;
using System.Reflection;
using fire_ash_server.World;

class Program
{
    private static WorldSoul worldSoul = new WorldSoul();
    static async Task Main(string[] args)
    {
        int port = 4123; // Use any appropriate port number here
        
        await worldSoul.Open(port);
    }
    public static WorldSoul WorldSoul
    {
        get
        {
            return worldSoul;
        }
    }
}



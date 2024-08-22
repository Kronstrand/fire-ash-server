using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Props;
using fire_ash_server.Props.Items;

namespace fire_ash_server.World.BioMechWorld.Temple
{
    internal static class EriskasChamber
    {
        public static Room Create()
        {
            string bookDescription =
            "This ancient tome is bound in cracked, leathered skin, its cover adorned with the faded image of a serpent. " +
            "The pages within are brittle and yellowed with age, but the ink remains clear, as if the words themselves resist the passage of time. " +
            "A marked passage reads: " +

            @"
            Serpent's Tear

            Within the hallowed walls of the Temple of Coiled Fate lies the Serpent’s Tear
            Legend tells that the Tear was shed when the Serpent foresaw the coming of an age where flesh and metal would merge, disrupting the natural order.
            The Tear holds the essence of the Serpent's wisdom, a bridge between the physical and spiritual realms. It is said that only those who understand
            the true balance of these forces may wield its power, capable of mending the deepest of rifts within a soul.

            The Chrono Serpent

            Guarding the Serpent's Tear is the Chrono Serpent, a being woven from the very fabric of time.
            This ethereal creature is a manifestation of the eternal cycle, its form constantly shifting between the past, present, and future.
            The Chrono Serpent possesses the power to manipulate time within the temple, slowing or accelerating its flow to protect the Tear.
            It is said that the serpent’s gaze can see all possible futures, and it will allow only those who are truly worthy to approach the Tear.
            Those who fail its test are said to be lost in time, their fate forever sealed within the coils of the Serpent’s domain.
            ";

            Item book = new Item("An Account of the Nine Serpents", bookDescription);

            return new Room("", "");
        }
    }
}

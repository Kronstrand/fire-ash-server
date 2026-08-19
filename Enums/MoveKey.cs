using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fire_ash_server.Enums
{
    internal enum MoveKey
    {
        a,  //attack
        aa, //duelwield attack
        ra, //ranged attack
        ba, //bite attack
        bg, //browse goods
        e,  //equip
        g,  //grap
        i,  //investigate, look own inventory
        j,  //journal
        l,  //lookat
        m,  //move to
        bc, //pick pocket
        r,  //room change
        h,  //stealth/hide
        sg, //save game
        lg, //load game
        sp, //speak to
        f,  //flee
        b,  //back
        bi, //buy item
        d,  //drop
        le, //look effect
        st, //show items
        c,  //character stats
        cs, //consume
        s, //say
    }
}

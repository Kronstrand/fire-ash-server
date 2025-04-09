using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using fire_ash_server.Enums;
using static fire_ash_server.Helpers;

namespace fire_ash_server.Props.Items.Weapons
{
    [Serializable]
    internal class TailSnakeBite : Weapon
    {
        public TailSnakeBite() : base("Tail Snake Bite", "Tail Snake Bite", new Die(1, 6), DamageType.Piercing, 1.2)
        {
            SetGeneralAttackDescriptionsForType();
            SetHumanoidAttackDescriptions();
        }

        public static void SetGeneralAttackDescriptionsForType()
        {
            if (!GeneralAttackDescriptionsForType.ContainsKey(typeof(TailSnakeBite)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();

                descriptions.Add((a, r, w) => $"{a} twists their snake-like tail, the venomous head snapping at {r}");
                descriptions.Add((a, r, w) => $"{a} lashes their tail forward, the snake striking out at {r}");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail coils before darting forward to bite {r}");
                descriptions.Add((a, r, w) => $"{a} flicks their tail, the attached snake lunging at {r}");
                descriptions.Add((a, r, w) => $"{a} whips their tail, the snake aiming its fangs at {r}");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake strikes with a venomous bite, targeting {FormatPossessive(r)} exposed skin");
                descriptions.Add((a, r, w) => $"{a} twists sharply, their tail snake darting toward {r}");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake hisses before snapping its venomous fangs at {r}");
                descriptions.Add((a, r, w) => $"{a} coils their tail tightly before launching a venomous strike toward {r}");
                descriptions.Add((a, r, w) => $"{a} snaps their tail toward {r}, the snake aiming to inject venom into their veins");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake lashes out at {r}, venom dripping from its fangs as it strikes");
                descriptions.Add((a, r, w) => $"{a} pivots sharply, their tail snake lunging with a venomous bite at {r}");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake darts forward, fangs bared, attempting to bite {r}");
                descriptions.Add((a, r, w) => $"{a} sweeps their tail low, the snake snapping toward {FormatPossessive(r)} ankle");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake strikes fiercely at {FormatPossessive(r)} side, venom glistening on its fangs");

                GeneralAttackDescriptionsForType.Add(typeof(TailSnakeBite), descriptions);
            }
        }

        public static void SetHumanoidAttackDescriptions()
        {
            if (!HumanoidAttackDescriptionsForType.ContainsKey(typeof(TailSnakeBite)))
            {
                List<Func<string, string, Weapon, string>> descriptions = new List<Func<string, string, Weapon, string>>();
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake attempts to bite into {FormatPossessive(r)} neck, injecting venom");
                descriptions.Add((a, r, w) => $"{a} snaps their tail snake toward {FormatPossessive(r)} wrist, fangs dripping venom");
                descriptions.Add((a, r, w) => $"{a} lashes their tail at {FormatPossessive(r)} leg, the snake attempting a venomous bite");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake coils around {FormatPossessive(r)} arm, sinking its venomous fangs into their flesh");
                descriptions.Add((a, r, w) => $"{a} lunges their tail snake at {FormatPossessive(r)} chest, trying to inject venom");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake strikes at {FormatPossessive(r)} throat, aiming to inject venom");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake bites at {FormatPossessive(r)} calf, venom seeping into the wound");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail wraps around {FormatPossessive(r)} side, the snake's fangs sinking deep");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake darts toward {FormatPossessive(r)} ribs, venom dripping as it strikes");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake strikes upward, aiming its venomous bite at {FormatPossessive(r)} face");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake coils tightly around {FormatPossessive(r)} arm, biting down");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake lunges at {FormatPossessive(r)} shoulder, fangs bared");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake snaps toward {FormatPossessive(r)} ankle, injecting venom into the wound");
                descriptions.Add((a, r, w) => $"{FormatPossessive(a)} tail snake strikes fiercely at {FormatPossessive(r)} torso, venom seeping into the bite");

                HumanoidAttackDescriptionsForType.Add(typeof(TailSnakeBite), descriptions);
            }
        }
    }
}

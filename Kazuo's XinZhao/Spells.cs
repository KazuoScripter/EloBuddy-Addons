using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace XinZhao
{
    class Spells
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Targeted E;
        public static Spell.Active R;
        public static Spell.Targeted Ignite;
        public static Spell.Targeted Smite;

        public static void LoadSpells()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Targeted(SpellSlot.E, 600);
            R = new Spell.Active(SpellSlot.R);
            Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("SummonerIgnite"), 600);
            //Smite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("SummonerSmite"), 500);
        }
    }
}
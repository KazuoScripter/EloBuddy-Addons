using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;

namespace XinZhao
{
    class Items
    {
        public static Item Hydra;
        public static Item Tiamat;
        public static Item Titanic;
        public static Item BotRK;
        public static Item Bilgewater;
        public static Item Youmuu;

        public static void LoadItems()
        {
            Tiamat = new Item(ItemId.Tiamat_Melee_Only, 400);
            Hydra = new Item(ItemId.Ravenous_Hydra_Melee_Only, 400);
            Titanic = new Item(ItemId.Titanic_Hydra, Player.Instance.GetAutoAttackRange());
            BotRK = new Item(ItemId.Blade_of_the_Ruined_King, 550);
            Bilgewater = new Item(ItemId.Bilgewater_Cutlass, 550);
            Youmuu = new Item(ItemId.Youmuus_Ghostblade);
        }
    }
}

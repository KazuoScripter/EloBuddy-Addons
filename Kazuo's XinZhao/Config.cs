using System.Drawing;
using EloBuddy;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace XinZhao
{
    class Config
    {
        public static Menu CoreMenu;
        public static Menu ComboMenu;
        public static Menu HarassMenu;
        public static Menu LaneClear;
        public static Menu JungleClear;
        public static Menu DrawMenu;
        public static Menu MiscMenu;

        public static void DesignMenu()
        {
            CoreMenu = MainMenu.AddMenu("Kazuo's XinZhao", "Kazuo's XinZhao");
            CoreMenu.AddGroupLabel("Made by Kazuo!");
            CoreMenu.AddLabel("≧◔◡◔≦");
            //ComboMenu
            ComboMenu = CoreMenu.AddSubMenu("Combo");
            {
                ComboMenu.AddGroupLabel("Combo Settings");
                ComboMenu.Add("Qcb", new CheckBox("Use Q"));
                ComboMenu.Add("Wcb", new CheckBox("Use W"));
                ComboMenu.Add("Ecb", new CheckBox("Use E"));
                ComboMenu.Add("dE", new Slider("Use E if Enemy distance >", 250, 0, 600));
                ComboMenu.Add("tE", new CheckBox("Don't use E under Turret", true));
                ComboMenu.Add("Rcb", new CheckBox("Use R", false));
                ComboMenu.Add("RcbENM", new Slider("Minium Enemies in R range", 0, 1, 5));
            } 
            //HarassMenu
            HarassMenu = CoreMenu.AddSubMenu("Harass");
            {
                HarassMenu.AddGroupLabel("Harass Settings");
                HarassMenu.Add("Qhr", new CheckBox("Use Q"));
                HarassMenu.Add("Whr", new CheckBox("Use W"));
                HarassMenu.AddLabel("Mana Manager");
                HarassMenu.Add("ManaMNGlc", new Slider("If Mana Percent below {0}% stop", 45, 0, 100));
            }
            //LaneClearMenu
            LaneClear = CoreMenu.AddSubMenu("LaneClear");
            {
                LaneClear.AddGroupLabel("LaneClear Settings");
                LaneClear.Add("Qlc", new CheckBox("Use Q"));
                LaneClear.Add("Wlc", new CheckBox("Use W"));
                LaneClear.Add("Elc", new CheckBox("Use E"));
                LaneClear.AddLabel("Mana Manager");
                LaneClear.Add("ManaMNGlc", new Slider("If Mana Percent below {0}% stop", 45, 0, 100));
            }
            //JungleClearMenu
            JungleClear = CoreMenu.AddSubMenu("JungleClear");
            {
                JungleClear.AddGroupLabel("JungleClear Settings");
                JungleClear.Add("Qjc", new CheckBox("Use Q"));
                JungleClear.Add("Wjc", new CheckBox("Use W"));
                JungleClear.Add("Ejc", new CheckBox("Use E"));
                LaneClear.AddLabel("Mana Manager");
                JungleClear.Add("ManaMNGjc", new Slider("If Mana Percent below {0}% stop", 45, 0, 100));
            }
            //DrawMenu
            DrawMenu = CoreMenu.AddSubMenu("Drawings");
            {
                DrawMenu.AddGroupLabel("Draw Settings");
                DrawMenu.Add("draw", new CheckBox("Enable Drawings"));
                DrawMenu.Add("Edr", new CheckBox("Draw E"));
                DrawMenu.Add("Rdr", new CheckBox("Draw R"));
                DrawMenu.Add("dmg", new CheckBox("Draw Damage Indicator"));
                DrawMenu.Add("Color", new ColorPicker("Damage Indicator Color", Color.FromArgb(255, 255, 236, 0)));
            }
            //MiscMenu
            MiscMenu = CoreMenu.AddSubMenu("Misc");
            {
                MiscMenu.AddGroupLabel("Misc Settings");
                MiscMenu.AddLabel("Reset AutoAttack");
                MiscMenu.Add("Qrs", new CheckBox("Use Q - Reset AA"));
                MiscMenu.AddLabel("Anti Gapcloser");
                MiscMenu.Add("Rag", new CheckBox("Use R - Anti Gapcloser"));
                MiscMenu.Add("HPMNGlc", new Slider("If Health Percent below {0}%", 15, 0, 100));
                MiscMenu.AddLabel("Interrupt Settings");
                MiscMenu.Add("Rint", new CheckBox("Use R - Interupt"));
                MiscMenu.AddLabel("KillSteal Settings");
                MiscMenu.Add("Eks", new CheckBox("Use E to KS"));
                MiscMenu.Add("Rks", new CheckBox("Use R to KS"));
                MiscMenu.Add("Iks", new CheckBox("Use Ignite to KS"));
                MiscMenu.Add("Sks", new CheckBox("Use Smite to KS <SOON>"));
            }
        }
    }
}
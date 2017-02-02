using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System;
using System.Linq;

namespace XinZhao
{
    class DamageIndicator
    {
        public static float STE(Obj_AI_Base target)
        {
            if (Spells.E.IsReady())
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, new[] { 0f, 70, 110f, 150f, 190f, 230f }[Spells.E.Level] + 0.6f * ObjectManager.Player.FlatMagicDamageMod);
            else
                return 0f;
        }
        public static float STR(Obj_AI_Base target)
        {
            if (Spells.R.IsReady())
                return Player.Instance.CalculateDamageOnUnit(target, DamageType.Physical, new[] { 0f, 75f, 175f, 275f }[Spells.R.Level] + 1f * ObjectManager.Player.FlatPhysicalDamageMod + 0.15f * target.Health);
            else
                return 0f;
        }
        public static float SpellsDMG(Obj_AI_Base target)
        {
            if (target == null)
            {
                return 0;
            }
            else if (target != null)
            {
                return STE(target) + STR(target);
            }
            else return 0f;
        }
        public static void Damage_Indicator(EventArgs args)
        {
            if (Config.DrawMenu["draw"].Cast<CheckBox>().CurrentValue)
                if (Config.DrawMenu["dmg"].Cast<CheckBox>().CurrentValue)
                {
                    foreach (var unit in EntityManager.Heroes.Enemies.Where(u => u.IsValidTarget() && u.IsHPBarRendered)
                        )
                    {

                        var damage = SpellsDMG(unit);

                        if (damage <= 0)
                        {
                            continue;
                        }
                        var Special_X = unit.ChampionName == "Jhin" || unit.ChampionName == "Annie" ? -12 : 0;
                        var Special_Y = unit.ChampionName == "Jhin" || unit.ChampionName == "Annie" ? -3 : 9;

                        var DamagePercent = ((unit.TotalShieldHealth() - damage) > 0
                            ? (unit.TotalShieldHealth() - damage)
                            : 0) / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);
                        var currentHealthPercent = unit.TotalShieldHealth() / (unit.MaxHealth + unit.AllShield + unit.AttackShield + unit.MagicShield);

                        var StartPoint = new Vector2((int)(unit.HPBarPosition.X + Special_X + DamagePercent * 107) + 1,
                            (int)unit.HPBarPosition.Y + Special_Y);
                        var EndPoint = new Vector2((int)(unit.HPBarPosition.X + Special_X + currentHealthPercent * 107) + 1,
                            (int)unit.HPBarPosition.Y + Special_Y);
                        var Color = Config.DrawMenu["Color"].Cast<ColorPicker>().CurrentValue;
                        Drawing.DrawLine(StartPoint, EndPoint, 9.82f, Color);
                    }
                }
        }
    }
}
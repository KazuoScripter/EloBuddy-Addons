using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Events;
using System;
using EloBuddy.SDK.Enumerations;
using SharpDX;

namespace XinZhao
{
    class Modes
    {
        //Combo
        public static void DC()
        {
            var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Mixed);
            var dE = Config.ComboMenu["dE"].Cast<Slider>().CurrentValue;
            var tE = Config.ComboMenu["tE"].Cast<CheckBox>().CurrentValue; 
            if (target != null)
            {
                if (Config.ComboMenu["Ecb"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Spells.E.Range) && (dE <= target.Distance(Player.Instance) || target.IsDashing()))
                {
                    if (tE)
                    {
                        if (!target.Position.IsUnderTurret())
                        {
                            Spells.E.Cast(target);
                        }
                    }
                    else
                    {
                        Spells.E.Cast(target);
                    }
                }
                if (Config.ComboMenu["Wcb"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(250) && !target.IsDead && !target.IsZombie)
                {
                    Spells.W.Cast();
                }
            }
        }
        //Harass
        public static void DH()
        {
            var t = TargetSelector.GetTarget(Spells.E.Range, DamageType.Mixed);
            if (Config.HarassMenu["Whr"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && t.IsValidTarget(250) && !t.IsDead && !t.IsZombie)
            {
                Spells.W.Cast();
            }
        }
        //LaneClear
        public static void DL()
        {
            var minions = ObjectManager.Get<Obj_AI_Base>().OrderBy(m => m.Health).Where(m => m.IsMinion && m.IsEnemy && !m.IsDead);
            if (Config.LaneClear["ManaMNGlc"].Cast<Slider>().CurrentValue <= Player.Instance.ManaPercent) 
            foreach (var m in minions)
            {
                if (Config.LaneClear["Wlc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && m.IsValidTarget(250))
                {
                    Spells.W.Cast();
                }
                if (Config.LaneClear["Elc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && m.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(m);
                }
            }
        }
        //JungleClear
        public static void DJ()
        {
            var jungleMonsters = EntityManager.MinionsAndMonsters.GetJungleMonsters().OrderByDescending(j => j.Health).FirstOrDefault(j => j.IsValidTarget(250));
            if (Config.LaneClear["ManaMNGlc"].Cast<Slider>().CurrentValue <= Player.Instance.ManaPercent)
            {
                if (Config.JungleClear["Wjc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && jungleMonsters.IsValidTarget(250))
                {
                    Spells.W.Cast();
                }
                if (Config.JungleClear["Ejc"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() && jungleMonsters.IsValidTarget(Spells.E.Range))
                {
                    Spells.E.Cast(jungleMonsters);
                }
            }
        }
        //KillSteal
        public static void DK()
        {
            var btht = EntityManager.Heroes.Enemies.Where(hero => (hero.IsValidTarget(Spells.R.Range) || hero.IsValidTarget(Spells.E.Range)) && !hero.HasBuff("FioraW") && !hero.HasBuff("JudicatorIntervention") && !hero.HasBuff("kindredrnodeathbuff") && !hero.HasBuff("Undying Rage") && !hero.HasBuff("SpellShield") && !hero.HasBuff("NocturneShield") && !hero.IsDead && !hero.IsZombie);
            if (Config.MiscMenu["Eks"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(t => t != null
                    && t.IsValidTarget()
                    && Spells.E.IsInRange(t)
                    && t.Health <= DamageIndicator.STE(t)), DamageType.Magical);
                if (target != null && target == btht)
                {
                        Spells.E.Cast(target);
                }
            }
            if (Config.MiscMenu["Rks"].Cast<CheckBox>().CurrentValue && Spells.R.IsReady())
            {
                var target = TargetSelector.GetTarget(EntityManager.Heroes.Enemies.Where(t => t != null
                    && t.IsValidTarget()
                    && Spells.R.IsInRange(t)
                    && t.Health <= DamageIndicator.STR(t)), DamageType.Magical);
                if (target != null && target == btht)
                {
                    Spells.R.Cast(target);
                }
            }
            if (Spells.Ignite != null 
                && Config.MiscMenu["Iks"].Cast<CheckBox>().CurrentValue 
                && Spells.Ignite.IsReady())
            {
                foreach (var target in btht)
                if (target.Health < Program.XinThongDit.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                {
                    Spells.Ignite.Cast(target);
                }
            }
        }
        //Flee
        public static void DF()
        {
            var Enemies = EntityManager.Heroes.Enemies.FirstOrDefault(e => e.IsValidTarget(Spells.E.Range));
            var minions = EntityManager.MinionsAndMonsters.EnemyMinions.FirstOrDefault(m => m.IsValidTarget(Spells.E.Range));
            if (Enemies != null && Spells.E.IsReady())
            {
                if (Enemies.IsInRange(Game.CursorPos, 250))
                {
                    Spells.E.Cast(Enemies);
                }
            }

            if (minions != null && Spells.E.IsReady())
            {
                if (minions.IsInRange(Game.CursorPos, 250))
                {
                    Spells.E.Cast(minions);
                }
            }
        }
        //InteruptSpells
        public static void InteruptSpells(Obj_AI_Base casting, Interrupter.InterruptableSpellEventArgs i)
        {
            var Interupt = Config.MiscMenu["Rint"].Cast<CheckBox>().CurrentValue;
            if (!casting.IsEnemy || !(casting is AIHeroClient) || Player.Instance.IsRecalling())
            {
                return;
            }

            if (Interupt && Spells.R.IsReady() && i.DangerLevel == DangerLevel.High && Spells.R.IsInRange(casting))
            {
                Spells.R.Cast();
            }
        }
        //AntiGapCloser
        public static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            if (Config.MiscMenu["HPMNGlc"].Cast<Slider>().CurrentValue <= Player.Instance.HealthPercent)
            {
                if (Spells.R.IsReady()
                    && sender != null
                    && sender.IsEnemy
                    && sender.IsValid
                    && (sender.IsAttackingPlayer || Player.Instance.Distance(args.End) < 200 || args.End.IsInRange(Player.Instance, Spells.R.Range))
                    && Config.MiscMenu["Rag"].Cast<CheckBox>().CurrentValue)
                {
                    Spells.R.Cast(args.End);
                }
            }
        }
        //ResetAA
        public static void ResetAA(AttackableUnit target, EventArgs args)
        {
            if (!Config.MiscMenu["Qrs"].Cast<CheckBox>().CurrentValue) return;
            if (target != null && target.IsEnemy && !target.IsInvulnerable && !target.IsDead && !target.IsZombie && target.Distance(Program.XinThongDit) <= Spells.Q.Range)
                if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                    (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                        (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                            (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))))) return;
            var e = target as Obj_AI_Base;
            if (!Config.ComboMenu["Qcb"].Cast<CheckBox>().CurrentValue || !e.IsEnemy) return;
            //if (target == ObjectManager.Get<Obj_AI_Minion>().Where(p => p.IsValidTarget(Spells.E.Range) && p.BaseSkinName.Contains("SRU_Plant"))) return;
            if (target == null) return;
            if (e.IsValidTarget() && Spells.Q.IsReady())
            {
                Spells.Q.Cast();
            }
        }      
    }
}


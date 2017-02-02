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
            var HHP = Config.MiscMenu["HHP"].Cast<Slider>().CurrentValue;
            var EHP = Config.MiscMenu["EHP"].Cast<Slider>().CurrentValue;
            if (target != null)
            {
                if (Config.ComboMenu["Ecb"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() && target.IsValidTarget(Spells.E.Range) && (dE <= target.Distance(Player.Instance)))
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
                        if (target.IsDashing())
                        {
                            Spells.E.Cast(target);
                        }
                    }
                }
                if (Config.ComboMenu["Wcb"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && target.IsValidTarget(250) && !target.IsDead && !target.IsZombie)
                {
                    Spells.W.Cast();
                }
                if (Config.ComboMenu["Rcb"].Cast<CheckBox>().CurrentValue && Spells.R.IsReady())
                {
                    var Count = Program.XinThongDit.CountEnemyChampionsInRange(450);
                    if (Count >= Config.ComboMenu["RcbENM"].Cast<Slider>().CurrentValue)
                    {
                        Spells.R.Cast();
                    }
                }
                if (Config.MiscMenu["BotRK"].Cast<CheckBox>().CurrentValue && Items.Bilgewater.IsReady() && Items.Bilgewater.IsOwned() && target.IsValidTarget(450))
                {
                    Items.Bilgewater.Cast(target);
                }
                if (Config.MiscMenu["BotRK"].Cast<CheckBox>().CurrentValue && Items.BotRK.IsReady() && target.IsValidTarget(450) && (Player.Instance.HealthPercent <= HHP || target.HealthPercent < EHP))
                {
                    Items.BotRK.Cast(target);
                }
                if (Config.MiscMenu["Ym"].Cast<CheckBox>().CurrentValue && Items.Youmuu.IsReady() && target.IsValidTarget(Program.XinThongDit.GetAutoAttackRange(target) + 100))
                {
                    Items.Youmuu.Cast();
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
                    if (Config.LaneClear["Elc"].Cast<CheckBox>().CurrentValue && Spells.W.IsReady() && m.IsValidTarget(450))
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
                if (Config.JungleClear["Ejc"].Cast<CheckBox>().CurrentValue && Spells.E.IsReady() && jungleMonsters.IsValidTarget(450))
                {
                    Spells.E.Cast(jungleMonsters);
                }
            }
        }
        //KillSteal
        public static void DK()
        {
            foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(600) && !x.IsZombie))
            {
                if (Spells.E.IsReady() && Config.MiscMenu["Eks"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Spells.E.Range))
                {
                    if (target != null && target.IsValidTarget(Spells.E.Range) && target.Health < DamageIndicator.STE(target) && !target.HasUndyingBuff())
                    {
                        Spells.E.Cast(target);
                    }
                }
                if (Config.MiscMenu["Rks"].Cast<CheckBox>().CurrentValue && target.IsValidTarget(Spells.R.Range) && Spells.R.IsReady())
                {
                    if (target != null && target.Health < DamageIndicator.STR(target) && !target.HasUndyingBuff())
                    {
                        Spells.R.Cast();
                    }
                }
                if (Spells.Ignite != null && Config.MiscMenu["Iks"].Cast<CheckBox>().CurrentValue && Spells.Ignite.IsReady())
                {
                        if (target.Health < Program.XinThongDit.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                        {
                            Spells.Ignite.Cast(target);
                        }
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
            if (Config.MiscMenu["Rag"].Cast<CheckBox>().CurrentValue && Spells.R.IsReady())
            {
                if (Player.Instance.HealthPercent < Config.MiscMenu["HPMNGlc"].Cast<Slider>().CurrentValue)
                {
                    if (sender != null
                            && sender.IsEnemy
                                && sender.IsValid
                                    && (sender.IsAttackingPlayer || Player.Instance.Distance(args.End) < Spells.R.Range || args.End.IsInRange(Player.Instance, Spells.R.Range)))
                    {
                        Spells.R.Cast(args.End);
                    }
                }
            }
        }
        //ResetAA
        public static void ResetAA(AttackableUnit target, EventArgs args)
        {
            var Hydra = Config.MiscMenu["Hydra"].Cast<CheckBox>().CurrentValue;
            if (!Config.MiscMenu["Qrs"].Cast<CheckBox>().CurrentValue) return;
            if (target != null && target.IsEnemy && !target.IsInvulnerable && !target.IsDead && !target.IsZombie && target.Distance(Program.XinThongDit) <= Spells.Q.Range)
                if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                    (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) &&
                        (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear)))) return;
            var e = target as Obj_AI_Base;
            if (!Config.ComboMenu["Qcb"].Cast<CheckBox>().CurrentValue || !e.IsEnemy) return;
            //if (target == ObjectManager.Get<Obj_AI_Minion>().Where(p => p.IsValidTarget(Spells.E.Range) && p.BaseSkinName.Contains("SRU_Plant"))) return;
            if (target == null) return;
            if (e.IsValidTarget() && Spells.Q.IsReady())
            {
                Spells.Q.Cast();
                if (Items.Hydra.IsOwned() && Items.Hydra.IsReady() && target.IsValidTarget(325))
                {
                    Items.Hydra.Cast();
                }

                if (Items.Tiamat.IsOwned() && Items.Tiamat.IsReady() && target.IsValidTarget(325))
                {
                    Items.Tiamat.Cast();
                }

                if (Items.Titanic.IsOwned() && target.IsValidTarget(325) && Items.Titanic.IsReady())
                {
                    Items.Titanic.Cast();
                }
            }
        }
    }
}



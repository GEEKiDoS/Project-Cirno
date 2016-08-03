using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class PerkColaFunction : BaseScript
    {
        public static List<Entity> GetClosingZombies(Entity player)
        {
            var list = new List<Entity>();
            foreach (var ent in Utility.Players)
            {
                if (ent.GetTeam() == "axis" && ent.Origin.DistanceTo(player.Origin) <= 500)
                {
                    list.Add(ent);
                }
            }

            return list;
        }

        public static List<Entity> GetClosingZombies(Vector3 origin)
        {
            var list = new List<Entity>();
            foreach (var ent in Utility.Players)
            {
                if (ent.GetTeam() == "axis" && ent.Origin.DistanceTo(origin) <= 500)
                {
                    list.Add(ent);
                }
            }

            return list;
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
                return;

            if (attacker.GetTeam() == "allies")
            {
                if (attacker.GetField<int>("perk_phd") == 1 && mod == "MOD_MELEE")
                {
                    attacker.Health = 1000;
                    AfterDelay(100, () =>
                    {
                        attacker.Notify("radius_exploed", attacker.Origin);
                    });
                    AfterDelay(200, () =>
                    {
                        attacker.SetMaxHealth();
                    });
                }
                if (attacker.GetField<int>("perk_doubletap") == 1 && mod.Contains("BULLET"))
                {
                    player.Call("finishplayerdamage", inflictor, attacker, damage, dFlags, mod, weapon, point, dir, hitLoc);
                }
                if (attacker.GetField<int>("perk_deadshot") == 1 && hitLoc.ToLower().Contains("head"))
                {
                    player.Health = 3;
                }
                if (attacker.GetField<int>("perk_widow") == 1 && mod.Contains("BULLET"))
                {
                    attacker.SetField("perk_widow", 2);
                    WidowsWineThink(attacker, player.Origin);
                }
                if (attacker.GetField<int>("perk_widow") == 1 && (mod == "MOD_MELEE" || mod == "MOD_EXPLOSIVE" || mod == "MOD_PROJECTILE_SPLASH" || mod == "MOD_GRENADE_SPLASH"))
                {
                    player.SetSpeed(0.5f);
                }
            }
            else if (attacker.GetTeam() == "axis")
            {
                if (player.GetField<int>("perk_phd") == 1 && (mod == "MOD_EXPLOSIVE" || mod == "MOD_PROJECTILE_SPLASH" || mod == "MOD_GRENADE_SPLASH"))
                {
                    player.Health = 1000;
                    AfterDelay(200, () => player.SetMaxHealth());
                }
                if (player.GetField<int>("perk_cherry") == 1 && mod == "MOD_MELEE")
                {
                    player.SetField("perk_cherry", 2);
                    player.Health = 1000;
                    AfterDelay(200, () => player.SetMaxHealth());
                    ElectricCherryThink(player);
                }
                if (player.GetField<int>("perk_revive") == 1 && Call<int>("getteamscore", "allies") == 1 && player.Health <= damage)
                {
                    player.Health = 1000;
                    AfterDelay(100, () =>
                    {
                        player.Notify("radius_exploed", attacker.Origin);
                    });
                    AfterDelay(200, () =>
                    {
                        player.SetMaxHealth();
                    });
                    if (!player.HasPerkCola(PerkColaType.TOMBSTONE))
                    {
                        player.RemoveAllPerkCola();
                    }
                    else
                    {
                        player.RemovePerkCola(PerkColaType.QUICK_REVIVE, true);
                    }
                    player.GamblerText("Quick Revive!", new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                }
            }
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (player.GetTeam() == "allies" && Call<int>("getteamscore", "allies") != 1)
            {
                if (player.HasPerkCola(PerkColaType.QUICK_REVIVE))
                {
                    player.SetTeam("allies");
                    player.GamblerText("Quick Revive!", new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                }
            }
        }

        private void ElectricCherryThink(Entity player)
        {
            var zombies = GetClosingZombies(player);
            foreach (var zombie in zombies)
            {
                zombie.Notify("cherry_exploed", player);
            }
            AfterDelay(5000, () => player.SetField("perk_cherry", 1));
        }

        private void WidowsWineThink(Entity player, Vector3 origin)
        {
            Effects.PlayFx(Effects.smallempfx, origin);
            foreach (var item in GetClosingZombies(origin))
            {
                item.SetSpeed(0.5f);
            }
            AfterDelay(15000, () => player.SetField("perk_widow", 1));
        }
    }
}

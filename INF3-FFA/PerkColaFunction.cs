using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class PerkColaFunction : BaseScript
    {
        public static List<Entity> GetClosingEnemys(Entity player)
        {
            var list = new List<Entity>();
            foreach (var ent in Utility.Players)
            {
                if (ent.GetTeam() != player.GetTeam() && ent.Origin.DistanceTo(player.Origin) <= 500)
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
                var vector = point;
                vector.X -= 5;
                vector.Y -= 5;
                vector.Z -= 5;

                Call("magicbullet", weapon, vector, point, attacker);
            }
            if (attacker.GetField<int>("perk_deadshot") == 1 && hitLoc.ToLower().Contains("head"))
            {
                player.Health = 3;
            }
            if (attacker.GetField<int>("perk_widow") == 1 && mod.Contains("BULLET"))
            {
                attacker.SetField("perk_widow", 2);
                if (player.Origin.DistanceTo(attacker.Origin) <= 200)
                {
                    attacker.SetMaxHealth();
                }
                WidowsWineThink(attacker, player.Origin);
            }
            if (attacker.GetField<int>("perk_widow") == 1 && (mod == "MOD_MELEE" || mod == "MOD_EXPLOSIVE" || mod == "MOD_PROJECTILE_SPLASH" || mod == "MOD_GRENADE_SPLASH"))
            {
                player.SetField("speed", 0.5f);
            }
            if (player.GetField<int>("perk_phd") == 1 && mod != "MOD_MELEE" && (mod == "MOD_EXPLOSIVE" || mod == "MOD_PROJECTILE_SPLASH" || mod == "MOD_GRENADE_SPLASH"))
            {
                player.SetMaxHealth();
            }
        }

        private void WidowsWineThink(Entity player, Vector3 origin)
        {
            player.Notify("widow_exploed", origin);
            AfterDelay(15000, () => player.SetField("perk_widow", 1));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class Effects : BaseScript
    {
        //Fx's
        public static readonly int bloodfx = Function.Call<int>("loadfx", "impacts/flesh_hit_body_fatal_exit");
        public static readonly int nukefx = Function.Call<int>("loadfx", "explosions/player_death_nuke");
        public static readonly int nuke2fx = Function.Call<int>("loadfx", "explosions/player_death_nuke_flash");
        public static readonly int empfx = Function.Call<int>("loadfx", "explosions/emp_flash_mp");
        public static readonly int barrelfirefx = Function.Call<int>("loadfx", "props/barrel_fire");
        public static readonly int normalexploed = Function.Call<int>("loadfx", "explosions/oxygen_tank_explosion");
        public static readonly int selfexploedfx = Function.Call<int>("loadfx", "props/barrelexp");
        public static readonly int radiusexploed = Function.Call<int>("loadfx", "explosions/tanker_explosion");
        public static readonly int smallfirefx = Function.Call<int>("loadfx", "fire/firelp_small_pm");
        public static readonly int flarefx = Function.Call<int>("loadfx", "smoke/signal_smoke_airdrop");
        public static readonly int redbeaconfx = Function.Call<int>("loadfx", "misc/flare_ambient");
        public static readonly int greenbeaconfx = Function.Call<int>("loadfx", "misc/flare_ambient_green");
        public static readonly int moneyfx = Function.Call<int>("loadfx", "props/cash_player_dro");

        public Effects()
        {
            OnNotify("money", origin => DropMoney(origin.As<Vector3>()));

            PlayerConnected += player =>
            {
                player.OnNotify("self_exploed", ent => SelfExploed(ent));
                player.OnNotify("radius_exploed", (ent, origin) => RadiusExploed(ent, origin.As<Vector3>()));
                player.OnNotify("cherry_exploed", (ent, attacker) => ElectricCherryExploed(ent, attacker.As<Entity>()));
                player.OnNotify("widow_exploed", (ent, origin) => WidowsWineExploed(ent, origin.As<Vector3>()));
            };
        }

        public static void PlayFx(int fx, Vector3 origin)
        {
            Function.SetEntRef(-1);
            Function.Call("playfx", fx, origin);
        }

        public static Entity SpawnFx(int fx, Vector3 origin)
        {
            Function.SetEntRef(-1);
            var ent = Function.Call<Entity>("spawnfx", fx, origin);
            Function.Call("triggerfx", ent);

            return ent;
        }

        private void SelfExploed(Entity player)
        {
            PlayFx(selfexploedfx, player.Call<Vector3>("gettagorigin", "j_head"));
            player.PlaySound(Sound.ExploedMineSound);
            player.Call("finishplayerdamage", player, player, player.GetField<int>("maxhealth"), 0, 0, "nuke_mp", player.Origin, "MOD_EXPLOSIVE", 0);
        }

        private void RadiusExploed(Entity player, Vector3 origin)
        {
            Call("RadiusDamage", origin, 500, 500, 50, player, "MOD_EXPLOSIVE", "nuke_mp");
            PlayFx(radiusexploed, origin);
            player.PlaySound(Sound.CrashSound);
        }

        private void ElectricCherryExploed(Entity player, Entity attacker)
        {
            player.ShellShock("concussion_grenade_mp", 3);
            player.Call("finishplayerdamage", player, attacker, 50, 0, 0, "bomb_site_mp", player.Origin, "MOD_EXPLOSIVE", 0);
        }
        private void WidowsWineExploed(Entity player, Vector3 origin)
        {
            Call("RadiusDamage", origin, 300, 100, 20, player, "MOD_EXPLOSIVE", "bomb_site_mp");
            PlayFx(barrelfirefx, origin);
        }
        private void DropMoney(Vector3 origin)
        {
            PlayFx(moneyfx, origin);
        }
    }
}
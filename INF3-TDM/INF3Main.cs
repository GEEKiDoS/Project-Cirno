using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class INF3Main : BaseScript
    {
        public INF3Main() : base()
        {
            //Box
            Utility.PreCacheShader("hudicon_neutral");
            Utility.PreCacheShader("waypoint_ammo_friendly");
            Utility.PreCacheShader("cardicon_8ball");
            Utility.PreCacheShader("cardicon_tictacboom");
            Utility.PreCacheShader("cardicon_bulb");
            Utility.PreCacheShader("cardicon_award_jets");
            Utility.PreCacheShader("cardicon_bear");
            Utility.PreCacheShader("dpad_killstreak_ac130");

            //PowerUp
            Utility.PreCacheShader("dpad_killstreak_nuke");

            //Perks
            Utility.PreCacheShader("specialty_fastreload"); //Speed Cola
            Utility.PreCacheShader("cardicon_juggernaut_1"); //Juggernog
            Utility.PreCacheShader("specialty_longersprint"); //Stamin-Up
            Utility.PreCacheShader("specialty_moredamage"); //Double Tap
            Utility.PreCacheShader("cardicon_headshot"); //Dead Shot
            Utility.PreCacheShader("_specialty_blastshield"); //PhD
            Utility.PreCacheShader("cardicon_soap_bar"); //Widow's Wine
            Utility.PreCacheShader("specialty_scavenger"); //Vultrue Aid

            //Other
            Utility.PreCacheShader("compass_waypoint_target");
            Utility.PreCacheShader("waypoint_flag_friendly");
            Utility.PreCacheModel("prop_flag_neutral");
            Utility.PreCacheModel(Utility.GetFlagModel());
            Utility.PreCacheModel("weapon_scavenger_grenadebag");
            Utility.PreCacheModel("weapon_oma_pack");
            Utility.PreCacheModel("com_laptop_2_open");

            //Power-Up
            Utility.SetDvar("allies_double_points", 0);
            Utility.SetDvar("allies_insta_kill", 0);
            Utility.SetDvar("allies_fire_sale", 0);
            Utility.SetDvar("axis_double_points", 0);
            Utility.SetDvar("axis_insta_kill", 0);
            Utility.SetDvar("axis_fire_sale", 0);

            PlayerConnected += player =>
            {
                //Init Hud
                player.InitPerkHud();
                player.InitGambleTextHud();

                player.SetPerkColaCount(0);

                player.SetField("aiz_cash", 500);
                player.SetField("speed", 1f);
                player.SetField("isgambling", 0);

                OnSpawned(player);
                player.SpawnedPlayer += () => OnSpawned(player);

                player.OnInterval(100, ent =>
                {
                    player.Call("setmovespeedscale", player.GetField<float>("speed"));
                    return player.IsPlayer;
                });

                var welcomemessages = new List<string>
                {
                    "Welcome " + player.Name,
                    "Project Cirno (INF3) for TDM",
                    "Create by A2ON.",
                    "Source code in: https://github.com/A2ON/",
                    "Current Map: "+Utility.MapName,
                    "Enjoy playing!"
                };

                player.WelcomeMessage(welcomemessages, new Vector3(1, 1, 1), new Vector3(1f, 0.5f, 1f), 1, 0.85f);

                player.CreateCashHud();
                player.Credits();
            };
        }

        public void OnSpawned(Entity player)
        {
            player.Call("freezecontrols", false);

            player.OnInterval(100, e =>
            {
                if (player.GetField<int>("aiz_cash") >= 13000)
                {
                    player.SetField("aiz_cash", 13000);
                }

                return player.IsAlive;
            });

            player.SetField("speed", 1f);
            player.SetField("usingteleport", 0);

            player.ResetPerkCola();
            player.SetField("incantation", 0);

            if (player.GetTeam() == "allies")
            {
                player.Call("setviewmodel", "viewmodel_base_viewhands");
            }
            else if (player.GetTeam() == "axis")
            {
                Utility.SetAxisModel(player);
            }
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
                return;

            if (attacker.GetTeam() == "allies" && Utility.GetDvar<int>("allies_insta_kill") == 1)
            {
                player.Health = 3;
                return;
            }
            else if (attacker.GetTeam() == "axis" && Utility.GetDvar<int>("axis_insta_kill") == 1)
            {
                player.Health = 3;
                return;
            }
            if (weapon.Contains("iw5_msr") || weapon.Contains("iw5_l96a1") || weapon.Contains("iw5_as50"))
            {
                player.Health = 3;
                return;
            }
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
                return;

            if (attacker.GetTeam() == "allies" && Utility.GetDvar<int>("allies_double_points") == 1)
            {
                attacker.WinCash(200);
            }
            else if (attacker.GetTeam() == "axis" && Utility.GetDvar<int>("axis_double_points") == 1)
            {
                attacker.WinCash(200);
            }
            else
            {
                attacker.WinCash(100);
            }
            player.WinCash(20);

            if (player.GetField<int>("incantation") == 1)
            {
                attacker.Health = 1000;
                AfterDelay(100, () =>
                {
                    attacker.Notify("radius_exploed", player.Origin);
                    player.GamblerText("Incantation!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                });
                AfterDelay(200, () => attacker.SetMaxHealth());
            }
        }
    }
}
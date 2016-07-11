using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using InfinityScript;

namespace INF3
{
    public class INF3Main : BaseScript
    {
        public INF3Main()
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
            Utility.PreCacheShader("specialty_finalstand"); //Quick Revive
            Utility.PreCacheShader("specialty_fastreload"); //Speed Cola
            Utility.PreCacheShader("cardicon_juggernaut_1"); //Juggernog
            Utility.PreCacheShader("specialty_longersprint"); //Stamin-Up
            Utility.PreCacheShader("specialty_twoprimaries"); //Mule Kick
            Utility.PreCacheShader("specialty_moredamage"); //Double Tap
            Utility.PreCacheShader("cardicon_headshot"); //Dead Shot
            Utility.PreCacheShader("_specialty_blastshield"); //PhD
            Utility.PreCacheShader("cardicon_cod4"); //Electric Cherry
            Utility.PreCacheShader("cardicon_soap_bar"); //Widow's Wine
            Utility.PreCacheShader("specialty_scavenger"); //Vulture Aid

            //Other
            Utility.PreCacheShader("compass_waypoint_target");
            Utility.PreCacheShader("waypoint_flag_friendly");
            Utility.PreCacheModel("prop_flag_neutral");
            Utility.PreCacheModel(Utility.GetFlagModel());
            Utility.PreCacheModel("weapon_scavenger_grenadebag");
            Utility.PreCacheModel("weapon_oma_pack");
            Utility.PreCacheModel("com_laptop_2_open");

            Utility.SetDvar("scr_aiz_power", 1);

            //Power-Up
            Utility.SetDvar("bonus_double_points", 0);
            Utility.SetDvar("bonus_insta_kill", 0);
            Utility.SetDvar("bonus_fire_sale", 0);

            //GobbleGum
            //Utility.SetDvar("global_gobble_burnedout", 0);
            //Utility.SetDvar("global_gobble_temporalgift", 0);

            //Debug
            Utility.SetDvar("inf3_allow_test", 0);

            PlayerConnected += player =>
            {
                //Init Hud
                player.InitPerkHud();
                player.InitGambleTextHud();

                player.SetPerkColaCount(0);

                player.SetField("aiz_cash", 500);
                player.SetField("aiz_point", 0);

                player.SetField("isgambling", 0);

                OnSpawned(player);
                player.SpawnedPlayer += () => OnSpawned(player);

                player.OnInterval(100, ent =>
                {
                    player.Call("setmovespeedscale", player.GetField<float>("speed"));
                    return player.IsPlayer;
                });

                #region Magic Weapons

                player.Call("notifyonplayercommand", "attack", "+attack");
                player.OnNotify("attack", self =>
                {
                    if (player.GetTeam() == "allies")
                    {
                        if (player.CurrentWeapon == "stinger_mp" && player.GetWeaponAmmoClip("stinger_mp") != 0)
                        {
                            if (player.Call<float>("playerads") >= 1f)
                            {
                                if (player.Call<int>("getweaponammoclip", player.CurrentWeapon) != 0)
                                {
                                    Vector3 vector = Call<Vector3>("anglestoforward", player.Call<Vector3>("getplayerangles"));
                                    Vector3 dsa = new Vector3(vector.X * 1000000f, vector.Y * 1000000f, vector.Z * 1000000f);
                                    Call("magicbullet", "stinger_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self);
                                    player.Call("setweaponammoclip", player.CurrentWeapon, 0);
                                }
                            }
                            else
                            {
                                player.PrintlnBold("You must be aim first!");
                            }
                        }
                    }
                });

                player.OnNotify("weapon_fired", delegate (Entity self, Parameter weapon)
                {
                    //if (player.GetField<int>("gobble_stockoption") == 1)
                    //{
                    //    if (weapon.As<string>().Contains("stinger") && weapon.As<string>().Contains("javelin") && weapon.As<string>().Contains("uav"))
                    //    {
                    //        if (player.GetWeaponAmmoStock(weapon.As<string>()) != 0)
                    //        {
                    //            player.Call("setweaponammoclip", weapon.As<string>(), 999);
                    //            player.Call("setweaponammostock", weapon.As<string>(), player.GetWeaponAmmoStock(weapon.As<string>()) - 1);
                    //        }
                    //    }
                    //}
                    if (weapon.As<string>() == "uav_strike_marker_mp")
                    {
                        if (player.GetField<int>("perk_vulture") == 1)
                        {
                            Vector3 vector = Call<Vector3>("anglestoforward", new Parameter[] { player.Call<Vector3>("getplayerangles", new Parameter[0]) });
                            Vector3 dsa = new Vector3(vector.X * 1000000f, vector.Y * 1000000f, vector.Z * 1000000f);
                            switch (Utility.Random.Next(5))
                            {
                                case 0:
                                    Call("magicbullet", "stinger_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self);
                                    break;
                                case 1:
                                    Call("magicbullet", "javelin_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self);
                                    break;
                                case 2:
                                    AfterDelay(0, () => Call("magicbullet", "ac130_105mm_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self));
                                    break;
                                case 3:
                                    AfterDelay(0, () => Call("magicbullet", "remote_tank_projectile_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self));
                                    break;
                                case 4:
                                    AfterDelay(0, () => Call("magicbullet", "ims_projectile_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self));
                                    break;
                            }
                        }
                        else
                        {
                            Vector3 vector = Call<Vector3>("anglestoforward", new Parameter[] { player.Call<Vector3>("getplayerangles", new Parameter[0]) });
                            Vector3 dsa = new Vector3(vector.X * 1000000f, vector.Y * 1000000f, vector.Z * 1000000f);
                            switch (Utility.Random.Next(3))
                            {
                                case 0:
                                    Call("magicbullet", "rpg_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self);
                                    break;
                                case 1:
                                    Call("magicbullet", "iw5_smaw_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self);
                                    break;
                                case 2:
                                    AfterDelay(0, () => Call("magicbullet", "sam_projectile_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self));
                                    AfterDelay(200, () => Call("magicbullet", "sam_projectile_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self));
                                    AfterDelay(400, () => Call("magicbullet", "sam_projectile_mp", player.Call<Vector3>("gettagorigin", "tag_weapon_left"), dsa, self));
                                    break;
                            }
                        }
                    }
                });

                #endregion

                var welcomemessages = new List<string>
                {
                    "Welcome " + player.Name,
                    "Project Cirno (INF3) v1.0 Beta",
                    "Create by A2ON.",
                    "Source code in: https://github.com/A2ON/",
                    "Current Map: "+Utility.MapName,
                    "Enjoy playing!",
                    Utility.TestMode ? "Test Mode" : ""
                };

                player.WelcomeMessage(welcomemessages, new Vector3(1, 1, 1), new Vector3(1f, 0.5f, 1f), 1, 0.85f);

                player.CreateCashHud();
                player.CreatePointHud();
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
                if (player.GetField<int>("aiz_point") >= 200)
                {
                    player.SetField("aiz_point", 200);
                }

                return player.IsAlive;
            });

            player.SetField("speed", 1f);
            player.SetField("usingteleport", 0);
            player.SetField("xpUpdateTotal", 0);

            player.ResetPerkCola();

            if (player.GetTeam() == "allies")
            {
                //if (player.GetCurrentGobbleGum().Type == GobbleGumType.Aftertaste)
                //{
                //    player.ReturnPerkCola();
                //    player.ActiveGobbleGum();
                //}
                //else
                //{
                //    player.ResetPerkCola();
                //}
                player.SetField("incantation", 0);

                player.Call("setviewmodel", "viewmodel_base_viewhands");

                player.Call("clearperks");
                player.SetPerk("specialty_assists", true, false);
                player.SetPerk("specialty_paint", true, false);
                player.SetPerk("specialty_paint_pro", true, false);

                //player.SetCurrentGobbleGum(new GobbleGum(GobbleGumType.None));
            }
            else if (player.GetTeam() == "axis")
            {
                player.SetField("zombie_incantation", 0);

                Utility.SetZombieModel(player);

                player.Call("clearperks");
                player.SetPerk("specialty_falldamage", true, false);
                player.SetPerk("specialty_lightweight", true, false);
                player.SetPerk("specialty_longersprint", true, false);
                player.SetPerk("specialty_grenadepulldeath", true, false);
                player.SetPerk("specialty_fastoffhand", true, false);
                player.SetPerk("specialty_fastreload", true, false);
                player.SetPerk("specialty_paint", true, false);
                player.SetPerk("specialty_autospot", true, false);
                player.SetPerk("specialty_stalker", true, false);
                player.SetPerk("specialty_marksman", true, false);
                player.SetPerk("specialty_quickswap", true, false);
                player.SetPerk("specialty_quickdraw", true, false);
                player.SetPerk("specialty_fastermelee", true, false);
                player.SetPerk("specialty_selectivehearing", true, false);
                player.SetPerk("specialty_steadyaimpro", true, false);
                player.SetPerk("specialty_sitrep", true, false);
                player.SetPerk("specialty_detectexplosive", true, false);
                player.SetPerk("specialty_fastsprintrecovery", true, false);
                player.SetPerk("specialty_fastmeleerecovery", true, false);
                player.SetPerk("specialty_bulletpenetration", true, false);
                player.SetPerk("specialty_bulletaccuracy", true, false);

                //fix revive bug
                if (player.CurrentWeapon.Contains("g36c"))
                {
                    player.TakeAllWeapons();
                    player.GiveWeapon("iw5_usp45_mp_tactical");
                    player.Call("setweaponammoclip", "iw5_usp45_mp_tactical", 0);
                    player.Call("setweaponammostock", "iw5_usp45_mp_tactical", 0);
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_usp45_mp_tactical"));
                }
            }
        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
                return;

            if (attacker.GetTeam() == "allies")
            {
                if (Call<int>("getdvarint", "bonus_insta_kill") == 1)
                {
                    player.Health = 3;
                    return;
                }
                else
                {
                    //if (player.GetField<int>("gobble_swordflay") == 1 && mod == "MOD_MELEE")
                    //{
                    //    player.Health = 3;
                    //}
                    if (weapon.Contains("iw5_msr") || weapon.Contains("iw5_l96a1") || weapon.Contains("iw5_as50"))
                    {
                        player.Health = 3;
                        return;
                    }
                    //if (attacker.GetField<int>("gobble_headdrama") == 1)
                    //{
                    //    var vector = player.Call<Vector3>("gettagorigin", "j_head");
                    //    var vector2 = new Vector3(vector.X -= 5, vector.Y -= 5, vector.Z -= 5);

                    //    Call("magicbullet", weapon, vector2, vector, attacker);
                    //}
                    //if (attacker.GetField<int>("gobble_killingtime") == 1 && mod.Contains("BULLET"))
                    //{
                    //    player.SetField("killingtime_beacon", 1);
                    //    player.SetSpeed(0);
                    //    player.AfterDelay(10000, e =>
                    //    {
                    //        if (player.GetField<int>("killingtime_beacon") == 1)
                    //        {
                    //            player.SetField("killingtime_beacon", 0);
                    //            player.Notify("self_exploed");
                    //        }
                    //    });
                    //}
                }
            }
            //else if (attacker.GetTeam() == "axis")
            //{
            //    if (player.GetField<int>("gobble_swordflay") == 1)
            //    {
            //        player.Health = 1000;
            //        player.AfterDelay(300, e => player.SetMaxHealth());
            //    }
            //    if (player.GetCurrentGobbleGum().Type == GobbleGumType.PopShocks)
            //    {
            //        player.ActiveGobbleGum();
            //        var list = PerkColaFunction.GetClosingZombies(player);
            //        foreach (var item in list)
            //        {
            //            item.SetSpeed(0);
            //        }
            //    }
            //}
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
                return;

            if (attacker.GetTeam() == "allies")
            {
                if (player.GetField<int>("rtd_flag") == 1)
                {
                    if (Call<int>("getdvarint", "bonus_double_points") == 1)
                    {
                        attacker.WinCash(400);
                    }
                    else
                    {
                        attacker.WinCash(200);
                    }
                }
                else if (player.GetField<int>("rtd_king") == 1)
                {
                    if (Call<int>("getdvarint", "bonus_double_points") == 1)
                    {
                        attacker.WinCash(1000);
                    }
                    else
                    {
                        attacker.WinCash(500);
                    }
                }
                else
                {
                    if (Call<int>("getdvarint", "bonus_double_points") == 1)
                    {
                        attacker.WinCash(200);
                    }
                    else
                    {
                        attacker.WinCash(100);
                    }
                }
                attacker.WinPoint(1);
                if (player.GetField<int>("zombie_incantation") == 1)
                {
                    attacker.Health = 1000;
                    AfterDelay(100, () =>
                    {
                        attacker.Notify("radius_exploed", player.Origin);
                        player.GamblerText("Incantation!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                    });
                    AfterDelay(200, () => attacker.SetMaxHealth());
                }
                if (player.HasField("killingtime_beacon") && player.GetField<int>("killingtime_beacon") == 1)
                {
                    player.SetField("killingtime_beacon", 0);
                }
            }
            else
            {
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

            attacker.Call("visionsetnakedforplayer", "mpnuke", 1);
            attacker.AfterDelay(500, e => attacker.Call("visionsetnakedforplayer", Utility.MapName, 1));
        }

        public override void OnSay(Entity player, string name, string message)
        {
            if (message == "!s" || message == "!sc")
            {
                player.Suicide();
            }

            if (message=="!givemoney")
            {
                player.WinCash(10000);
            }

            if (message=="!buff")
            {
                if (player.GetTeam()=="allies")
                {
                    player.GiveAllPerkCola();
                }
            }
        }
    }
}

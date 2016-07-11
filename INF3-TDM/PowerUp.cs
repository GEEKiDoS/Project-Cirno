using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using InfinityScript;

namespace INF3
{
    public enum PowerUpType
    {
        /// <summary>
        /// 子弹全满
        /// </summary>
        MaxAmmo,
        /// <summary>
        /// 双倍点数
        /// </summary>
        DoublePoints,
        /// <summary>
        /// 一击必杀
        /// </summary>
        InstaKill,
        /// <summary>
        /// 核爆
        /// </summary>
        Nuke,
        /// <summary>
        /// 廉价火力
        /// </summary>
        FireSale,
        /// <summary>
        /// 点数奖励
        /// </summary>
        BonusPoints,
        /// <summary>
        /// 修复所有门
        /// </summary>
        Carpenter,
    }

    //public class PowerUpEntity : IDisposable
    //{
    //    private bool disposed = false;

    //    public PowerUpType Type { get; }
    //    public Entity Entity { get; }
    //    public Entity FX { get; }
    //    public Vector3 Origin { get; }
    //    public int Timeout { get; set; }
    //    public string Notify
    //    {
    //        get
    //        {
    //            switch (Type)
    //            {
    //                case PowerUpType.MaxAmmo:
    //                    return "max_ammo";
    //                case PowerUpType.DoublePoints:
    //                    return "double_points";
    //                case PowerUpType.InstaKill:
    //                    return "insta_kill";
    //                case PowerUpType.Nuke:
    //                    return "nuke_drop";
    //                case PowerUpType.FireSale:
    //                    return "fire_sale";
    //                case PowerUpType.BonusPoints:
    //                    return "bonus_points";
    //                default:
    //                    return "";
    //            }
    //        }
    //    }

    //    public PowerUpEntity(PowerUpType type, string model, Vector3 origin, Vector3 angle, int fx)
    //    {
    //        Type = type;
    //        origin = origin += new Vector3(0, 0, 10);

    //        if (Type == PowerUpType.Nuke)
    //        {
    //            origin += new Vector3(0, 0, 40);
    //        }

    //        Entity = Utility.Spawn("script_model", origin);
    //        Entity.Call("setmodel", model);
    //        Entity.SetField("angles", angle);
    //        FX = Effects.SpawnFx(fx, origin);
    //        Origin = origin;
    //        Timeout = 30;

    //        PowerUpTimer();
    //        PowerUpRotate();

    //        Entity.AfterDelay(1000, e =>
    //        {
    //            PowerUpThink();
    //        });
    //    }

    //    private void PowerUpThink()
    //    {
    //        Entity.OnInterval(100, e =>
    //        {
    //            foreach (var player in Utility.Players)
    //            {
    //                if (player.IsAlive && player.Origin.DistanceTo(Origin) < 50)
    //                {
    //                    player.Notify(Notify);
    //                    Dispose();
    //                }
    //            }

    //            return !disposed;
    //        });
    //    }

    //    private void PowerUpTimer()
    //    {
    //        int timer = 0;
    //        Entity.OnInterval(1000, e =>
    //        {
    //            timer++;
    //            if (timer == 20)
    //            {
    //                PowerUpTimeoutWarning();
    //                return false;
    //            }

    //            return !disposed;
    //        });
    //    }

    //    private void PowerUpTimeoutWarning()
    //    {
    //        const int MAX = 20;

    //        bool ishide = false;
    //        int num = 0;
    //        Entity.OnInterval(1000, e =>
    //        {
    //            if (!ishide)
    //            {
    //                Entity.Call("hide");
    //                ishide = true;
    //            }
    //            else
    //            {
    //                Entity.Call("show");
    //                ishide = false;
    //            }
    //            num++;

    //            if (num == MAX)
    //            {
    //                Dispose();
    //            }

    //            return !disposed;
    //        });
    //    }

    //    private void PowerUpRotate()
    //    {
    //        Entity.OnInterval(5000, e =>
    //        {
    //            Entity.Call("rotateyaw", -360, 5);

    //            return !disposed;
    //        });
    //    }

    //    public void Dispose()
    //    {
    //        disposed = true;

    //        FX.Call("delete");
    //        Entity.Call("delete");
    //    }
    //}

    public class PowerUp : BaseScript
    {
        public PowerUp()
        {
            PlayerConnected += player =>
            {
                player.OnNotify("max_ammo", e => MaxAmmo(player));
                player.OnNotify("double_points", e => DoublePoints(player));
                player.OnNotify("insta_kill", e => InstaKill(player));
                player.OnNotify("nuke_drop", e => Nuke(player));
                player.OnNotify("fire_sale", e => FireSale(player));
                player.OnNotify("bonus_points", e => BonusPoints(player));
            };
        }

        //public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        //{
        //    if (mod == "MOD_SUICIDE")
        //        return;

        //    var random = Utility.Random.Next(5);
        //    var randomequels = Utility.Random.Next(5);
        //    if (random == randomequels)
        //    {
        //        PowerUpDrop(player, attacker);
        //    }
        //}

        //public void PowerUpDrop(Entity player, Entity attacker)
        //{
        //    switch ((PowerUpType)Utility.Random.Next(Enum.GetNames(typeof(PowerUpType)).Length))
        //    {
        //        case PowerUpType.MaxAmmo:
        //            new PowerUpEntity(PowerUpType.MaxAmmo, "com_plasticcase_friendly", player.Origin, player.GetField<Vector3>("angles"), Effects.greenbeaconfx);
        //            break;
        //        case PowerUpType.DoublePoints:
        //            new PowerUpEntity(PowerUpType.DoublePoints, "com_plasticcase_friendly", player.Origin, player.GetField<Vector3>("angles"), Effects.redbeaconfx);
        //            break;
        //        case PowerUpType.InstaKill:
        //            new PowerUpEntity(PowerUpType.InstaKill, "com_plasticcase_trap_friendly", player.Origin, player.GetField<Vector3>("angles"), Effects.smallfirefx);
        //            break;
        //        case PowerUpType.Nuke:
        //            new PowerUpEntity(PowerUpType.Nuke, "projectile_cbu97_clusterbomb", player.Origin, player.GetField<Vector3>("angles") - new Vector3(90, 0, 0), Effects.smallfirefx);
        //            break;
        //        case PowerUpType.FireSale:
        //            new PowerUpEntity(PowerUpType.FireSale, "com_plasticcase_enemy", player.Origin, player.GetField<Vector3>("angles"), Effects.greenbeaconfx);
        //            break;
        //        case PowerUpType.BonusPoints:
        //            new PowerUpEntity(PowerUpType.BonusPoints, "com_plasticcase_enemy", player.Origin, player.GetField<Vector3>("angles"), Effects.redbeaconfx);
        //            break;
        //    }
        //}

        //private void PowerUpHudTimer(Entity player,HudElem hud)
        //{
        //    int timer = 0;
        //    player.OnInterval(1000, e =>
        //    {
        //        timer++;
        //        if (timer == 20)
        //        {
        //            float i = 0;
        //            bool ishide = false;
        //            player.OnInterval(500, e1 =>
        //            {
        //                if (!ishide)
        //                {
        //                    hud.Call("fadeovertime", 0.5f);
        //                    hud.Alpha = 0;
        //                    ishide = true;
        //                }
        //                else
        //                {
        //                    hud.Call("fadeovertime", 0.5f);
        //                    hud.Alpha = 1;
        //                    ishide = false;
        //                }
        //                i++;
        //                if (i >= 5)
        //                {
        //                    player.OnInterval(250, e2 =>
        //                    {
        //                        if (!ishide)
        //                        {
        //                            hud.Call("fadeovertime", 0.25f);
        //                            hud.Alpha = 0;
        //                            ishide = true;
        //                        }
        //                        else
        //                        {
        //                            hud.Call("fadeovertime", 0.25f);
        //                            hud.Alpha = 1;
        //                            ishide = false;
        //                        }
        //                        i += 0.5f;
        //                        if (i >= 10)
        //                        {
        //                            hud.Call("destroy");
        //                            return false;
        //                        }

        //                        return true;
        //                    });
        //                    return false;
        //                }
        //                return true;
        //            });
        //            return false;
        //        }
        //        return true;
        //    });
        //}

        public void MaxAmmo(Entity player)
        {
            player.PlayLocalSound("mp_bonus_start");
            player.PrintlnBold("^2Max Ammo");
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() == player.GetTeam())
                {
                    player.Call("givemaxammo", player.CurrentWeapon);
                }
            }
        }

        public void DoublePoints(Entity player)
        {
            player.PlayLocalSound("mp_bonus_start");
            player.PrintlnBold("^2Double Points");
            Utility.SetDvar(player.GetTeam() + "_double_points", 1);
            player.AfterDelay(30000, e =>
            {
                Utility.SetDvar(player.GetTeam() + "_double_points", 0);
                player.PrintlnBold("Double Points off");
            });
        }

        public void InstaKill(Entity player)
        {
            player.PlayLocalSound("mp_bonus_start");
            player.PrintlnBold("^2Insta-Kill");
            Utility.SetDvar(player.GetTeam() + "_insta_kill", 1);
            player.AfterDelay(30000, e =>
            {
                Utility.SetDvar(player.GetTeam() + "_insta_kill", 0);
                player.PrintlnBold("Insta-Kill off");
            });
        }

        public void Nuke(Entity player)
        {
            player.PlayLocalSound("mp_bonus_start");
            player.WinCash(400);
            player.PrintlnBold("^2Nuke");

            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() != player.GetTeam() && item.IsAlive)
                {
                    item.AfterDelay(600, e => item.Notify("self_exploed"));
                }
            }
        }

        public void FireSale(Entity player)
        {
            player.PlayLocalSound("mp_bonus_start");
            player.PrintlnBold("^2Fire Sale");
            Utility.SetDvar(player.GetTeam() + "_fire_sale", 1);
            player.AfterDelay(30000, e =>
            {
                Utility.SetDvar(player.GetTeam() + "_fire_sale", 0);
                player.PrintlnBold("Fire Sale off");
            });
        }

        public void BonusPoints(Entity player)
        {
            player.PlayLocalSound("mp_bonus_start");
            player.PrintlnBold("^2Bonus Points");
            player.WinCash(500);
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() == player.GetTeam() && item.IsAlive && item != player)
                {
                    item.WinCash(500);
                }
            }
        }
    }
}

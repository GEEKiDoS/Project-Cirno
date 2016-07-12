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

    public class PowerUpEntity : IDisposable
    {
        private bool disposed = false;

        public PowerUpType Type { get; }
        public Entity Entity { get; }
        public Entity FX { get; }
        public Vector3 Origin { get; }
        public string Notify
        {
            get
            {
                switch (Type)
                {
                    case PowerUpType.MaxAmmo:
                        return "max_ammo";
                    case PowerUpType.DoublePoints:
                        return "double_points";
                    case PowerUpType.InstaKill:
                        return "insta_kill";
                    case PowerUpType.Nuke:
                        return "nuke_drop";
                    case PowerUpType.FireSale:
                        return "fire_sale";
                    case PowerUpType.BonusPoints:
                        return "bonus_points";
                    default:
                        return "";
                }
            }
        }

        public PowerUpEntity(PowerUpType type, string model, Vector3 origin, Vector3 angle, int fx)
        {
            Type = type;
            origin = origin += new Vector3(0, 0, 10);

            if (Type == PowerUpType.Nuke)
            {
                origin += new Vector3(0, 0, 40);
            }

            Entity = Utility.Spawn("script_model", origin);
            Entity.Call("setmodel", model);
            Entity.SetField("angles", angle);
            FX = Effects.SpawnFx(fx, origin);
            Origin = origin;

            PowerUpTimer();
            PowerUpRotate();

            Entity.AfterDelay(1000, e =>
            {
                PowerUpThink();
            });
        }

        private void PowerUpThink()
        {
            Entity.OnInterval(100, e =>
            {
                foreach (var player in Utility.Players)
                {
                    if (player.IsAlive && player.GetTeam() == "allies" && player.Origin.DistanceTo(Origin) < 50)
                    {
                        player.Notify(Notify);
                        Dispose();
                    }
                }

                return !disposed;
            });
        }

        private void PowerUpTimer()
        {
            int timer = 0;
            Entity.OnInterval(1000, e =>
            {
                timer++;
                if (timer == 20)
                {
                    PowerUpTimeoutWarning();
                    return false;
                }

                return !disposed;
            });
        }

        private void PowerUpTimeoutWarning()
        {
            const int MAX = 20;

            bool ishide = false;
            int num = 0;
            Entity.OnInterval(500, e =>
            {
                if (!ishide)
                {
                    Entity.Call("hide");
                    ishide = true;
                }
                else
                {
                    Entity.Call("show");
                    ishide = false;
                }
                num++;

                if (num == MAX)
                {
                    Dispose();
                }

                return !disposed;
            });
        }

        private void PowerUpRotate()
        {
            Entity.OnInterval(5000, e =>
            {
                Entity.Call("rotateyaw", -360, 5);

                return !disposed;
            });
        }

        public void Dispose()
        {
            disposed = true;

            FX.Call("delete");
            Entity.Call("delete");
        }
    }

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
                player.OnNotify("carpenter", e => Carpenter(player));
            };
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
                return;

            if (attacker.GetTeam() == "axis")
            {
                return;
            }

            var random = Utility.Random.Next(10);
            var randomequels = Utility.Random.Next(10);
            if (random == randomequels)
            {
                //PowerUpDrop(player, attacker);
                switch ((PowerUpType)Utility.Random.Next(Enum.GetNames(typeof(PowerUpType)).Length))
                {
                    case PowerUpType.MaxAmmo:
                        attacker.Notify("max_ammo");
                        break;
                    case PowerUpType.DoublePoints:
                        attacker.Notify("double_points");
                        break;
                    case PowerUpType.InstaKill:
                        attacker.Notify("insta_kill");
                        break;
                    case PowerUpType.Nuke:
                        attacker.Notify("nuke_drop");
                        break;
                    case PowerUpType.FireSale:
                        attacker.Notify("fire_sale");
                        break;
                    case PowerUpType.BonusPoints:
                        attacker.Notify("bonus_points");
                        break;
                    case PowerUpType.Carpenter:
                        attacker.Notify("carpenter");
                        break;
                }
            }
        }

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
        //            new PowerUpEntity(PowerUpType.InstaKill, "com_plasticcase_trap_friendly", player.Origin, player.GetField<Vector3>("angles"), Effects.redbeaconfx);
        //            break;
        //        case PowerUpType.Nuke:
        //            new PowerUpEntity(PowerUpType.Nuke, "projectile_cbu97_clusterbomb", player.Origin, player.GetField<Vector3>("angles") - new Vector3(90, 0, 0), Effects.redbeaconfx);
        //            break;
        //        case PowerUpType.FireSale:
        //            new PowerUpEntity(PowerUpType.FireSale, "com_plasticcase_enemy", player.Origin, player.GetField<Vector3>("angles"), Effects.greenbeaconfx);
        //            break;
        //        case PowerUpType.BonusPoints:
        //            new PowerUpEntity(PowerUpType.BonusPoints, "com_plasticcase_enemy", player.Origin, player.GetField<Vector3>("angles"), Effects.redbeaconfx);
        //            break;
        //    }
        //}

        private void PowerUpHudTimer(Entity player, HudElem hud)
        {
            int timer = 0;
            player.OnInterval(1000, e =>
            {
                timer++;
                if (timer == 20)
                {
                    float i = 0;
                    bool ishide = false;
                    player.OnInterval(500, e1 =>
                    {
                        if (!ishide)
                        {
                            hud.Call("fadeovertime", 0.5f);
                            hud.Alpha = 0;
                            ishide = true;
                        }
                        else
                        {
                            hud.Call("fadeovertime", 0.5f);
                            hud.Alpha = 1;
                            ishide = false;
                        }
                        i++;
                        if (i >= 5)
                        {
                            player.OnInterval(250, e2 =>
                            {
                                if (!ishide)
                                {
                                    hud.Call("fadeovertime", 0.25f);
                                    hud.Alpha = 0;
                                    ishide = true;
                                }
                                else
                                {
                                    hud.Call("fadeovertime", 0.25f);
                                    hud.Alpha = 1;
                                    ishide = false;
                                }
                                i += 0.5f;
                                if (i >= 10)
                                {
                                    hud.Call("destroy");
                                    return false;
                                }

                                return true;
                            });
                            return false;
                        }
                        return true;
                    });
                    return false;
                }
                return true;
            });
        }

        public void PowerUpInfo(string text, Vector3 color)
        {
            foreach (var player in Utility.Players)
            {
                if (player.IsAlive && player.GetTeam() == "allies")
                {
                    if (color.Equals(new Vector3(1, 1, 1)))
                    {
                        player.GamblerText(text, new Vector3(1, 1, 1), color, 1f, 0);
                    }
                    else
                    {
                        player.PlayLocalSound("mp_bonus_start");
                        player.GamblerText(text, new Vector3(1, 1, 1), color, 1f, 0.85f);
                    }
                }
            }
        }

        public void MaxAmmo(Entity player)
        {
            PowerUpInfo("Max Ammo", new Vector3(0, 1, 0));
            foreach (var item in Utility.Players)
            {
                if (item.IsAlive && item.GetTeam() == "allies")
                {
                    item.Call("givemaxammo", Sharpshooter._firstWeapon.Code);
                    item.Call("givemaxammo", Sharpshooter._mulekickWeapon.Code);
                    item.Call("givemaxammo", Sharpshooter._secondeWeapon.Code);
                }
            }
        }

        public void DoublePoints(Entity player)
        {
            if (Utility.GetDvar<int>("bonus_double_points") == 1)
            {
                return;
            }

            PowerUpInfo("Double Points", new Vector3(0, 1, 0));
            Utility.SetDvar("bonus_double_points", 1);
            player.AfterDelay(30000, e =>
            {
                PowerUpInfo("Double Points Off", new Vector3(1, 1, 1));
                Utility.SetDvar("bonus_double_points", 0);
            });
        }

        public void InstaKill(Entity player)
        {
            if (Utility.GetDvar<int>("bonus_insta_kill") == 1)
            {
                return;
            }

            PowerUpInfo("Insta-Kill", new Vector3(0, 1, 0));
            Utility.SetDvar("bonus_insta_kill", 1);
            player.AfterDelay(30000, e =>
            {
                PowerUpInfo("Insta-Kill Off", new Vector3(1, 1, 1));
                Utility.SetDvar("bonus_insta_kill", 0);
            });
        }

        public void Nuke(Entity player)
        {
            PowerUpInfo("Nuke", new Vector3(0, 1, 0));

            player.WinCash(400);
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() != player.GetTeam() && item.IsAlive)
                {
                    item.Notify("self_exploed");
                }
            }
        }

        public void FireSale(Entity player)
        {
            if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
            {
                return;
            }

            PowerUpInfo("Fire Sale", new Vector3(0, 1, 0));
            Utility.SetDvar("bonus_fire_sale", 1);
            player.AfterDelay(30000, e =>
            {
                PowerUpInfo("Fire Sale Off", new Vector3(1, 1, 1));
                Utility.SetDvar("bouns_fire_sale", 0);
            });
        }

        public void BonusPoints(Entity player)
        {
            PowerUpInfo("Bonus Points", new Vector3(0, 1, 0));
            player.WinCash(500);
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() == player.GetTeam() && item.IsAlive && item != player)
                {
                    item.WinCash(500);
                }
            }
        }

        public void Carpenter(Entity player)
        {
            PowerUpInfo("Carpenter", new Vector3(0, 1, 0));

            foreach (var item in MapEdit.doors)
            {
                if (item.State == Door.DoorState.Broken)
                {
                    item.State = Door.DoorState.Open;
                }
                item.HP = item.MaxHP;
            }
        }
    }
}

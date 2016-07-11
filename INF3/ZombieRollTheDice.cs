using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    /// <summary>
    /// 表示僵尸重生后随机获得的增强或减弱
    /// </summary>
    public enum RollType
    {
        /// <summary>
        /// 普通僵尸
        /// </summary>
        Nothing,
        /// <summary>
        /// 旗子僵尸，击杀可获得2倍现金奖励
        /// </summary>
        FlagZombie,
        /// <summary>
        /// 香港记者僵尸，跑的比西方记者都快
        /// </summary>
        StaminUp,
        /// <summary>
        /// 纸糊僵尸，一击必杀
        /// </summary>
        OneHitKill,
        /// <summary>
        /// 重装兵僵尸，3倍于普通僵尸的血量
        /// </summary>
        Juggernaut,
        /// <summary>
        /// 手持SMAW的僵尸
        /// </summary>
        SMAW,
        /// <summary>
        /// 手持RPG的僵尸
        /// </summary>
        RPG,
        /// <summary>
        /// 手持毒刺的僵尸（然而并没有什么卵用）
        /// </summary>
        Stinger,
        /// <summary>
        /// 手持AA12的僵尸
        /// </summary>
        AA12,
        /// <summary>
        /// 手持SPAS-12的僵尸
        /// </summary>
        Spas,
        /// <summary>
        /// 低速僵尸
        /// </summary>
        Turtle,
        /// <summary>
        /// 手持标枪导弹的僵尸
        /// </summary>
        Javelin,
        /// <summary>
        /// 前后都有盾牌保护的僵尸
        /// </summary>
        Riotshield,
        /// <summary>
        /// 王の重装兵僵尸，10倍于普通僵尸的血量，击杀可获得5倍现金奖励
        /// </summary>
        KingOfJuggernaut,
        /// <summary>
        /// 手持沙鹰的僵尸
        /// </summary>
        DesertEagle,
        /// <summary>
        /// 手持飞刀的僵尸
        /// </summary>
        ThrowingKnife,
        /// <summary>
        /// 手持M320的僵尸
        /// </summary>
        M320,
        /// <summary>
        /// 手持XM25的僵尸
        /// </summary>
        XM25,
        /// <summary>
        /// 手持只有一发子弹的MK14的僵尸
        /// </summary>
        MK14,
        /// <summary>
        /// 手持只有一发子弹的SVD的僵尸
        /// </summary>
        SVD,
        /// <summary>
        /// 清真僵尸（雾），手持引爆器，可以进行自杀式攻击的僵尸
        /// </summary>
        ISIS,
        /// <summary>
        /// 清真重装兵（雾），手持引爆器，可以进行自杀式攻击的重装兵僵尸
        /// </summary>
        ISISJuggernaut,
        /// <summary>
        /// 被诅咒僵尸，被击杀可引发大爆炸炸死周围僵尸
        /// </summary>
        ZombieIncantation,
        /// <summary>
        /// 自带墓碑僵尸，可以从被击杀的位置重生
        /// </summary>
        Tombstone,
        /// <summary>
        /// 前后都用盾牌保护的重装兵僵尸
        /// </summary>
        Tank,
        /// <summary>
        /// Boomer（复刻L4D2），被击杀后可使周围人类被喷上粘液弄花视线
        /// </summary>
        Boomer,
        /// <summary>
        /// Spider（复刻L4D2），被击杀会产生一片酸液，伤害任何站在酸液中的人类
        /// </summary>
        Spider,
        /// <summary>
        /// 可以跳的更高的僵尸
        /// </summary>
        JetPack,
        /// <summary>
        /// 可以跳的更高的重装兵
        /// </summary>
        JetPackJuggernaut,
        /// <summary>
        /// 可以跳的更高的清真僵尸
        /// </summary>
        JetPackISISZombie,
        /// <summary>
        /// 迪斯科僵尸（复刻植物大战僵尸），可以让其他僵尸重生在所在的位置
        /// </summary>
        DiscoZombie,
    }

    public enum RollGoodType
    {
        Bad,
        Good,
        Excellent
    }

    public class RTDItem : IRandom
    {
        public RollType Type { get; set; }

        public int Weight
        {
            get
            {
                switch (Type)
                {
                    case RollType.Nothing:
                        return 8;
                    case RollType.FlagZombie:
                        return 8;
                    case RollType.StaminUp:
                        return 4;
                    case RollType.OneHitKill:
                        return 2;
                    case RollType.Juggernaut:
                        return 4;
                    case RollType.SMAW:
                        return 1;
                    case RollType.RPG:
                        return 1;
                    case RollType.Stinger:
                        return 3;
                    case RollType.AA12:
                        return 1;
                    case RollType.Spas:
                        return 1;
                    case RollType.Turtle:
                        return 3;
                    case RollType.Javelin:
                        return 2;
                    case RollType.Riotshield:
                        return 4;
                    case RollType.KingOfJuggernaut:
                        return 1;
                    case RollType.DesertEagle:
                        return 1;
                    case RollType.ThrowingKnife:
                        return 2;
                    case RollType.M320:
                        return 1;
                    case RollType.XM25:
                        return 1;
                    case RollType.MK14:
                        return 1;
                    case RollType.SVD:
                        return 1;
                    case RollType.ISIS:
                        return 3;
                    case RollType.ISISJuggernaut:
                        return 1;
                    case RollType.ZombieIncantation:
                        return 4;
                    case RollType.Tombstone:
                        return 2;
                    case RollType.Tank:
                        return 1;
                    case RollType.Boomer:
                        return 4;
                    case RollType.Spider:
                        return 4;
                    case RollType.JetPack:
                        return 4;
                    case RollType.JetPackJuggernaut:
                        return 1;
                    case RollType.JetPackISISZombie:
                        return 1;
                    case RollType.DiscoZombie:
                        return 2;
                    default:
                        return 0;
                }
            }
        }

        public Action<Entity> OnRolled
        {
            get
            {
                switch (Type)
                {
                    case RollType.Nothing:
                        return new Action<Entity>(player =>
                        {
                        });
                    case RollType.FlagZombie:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_flag", 1);
                            player.Call("attach", GetCarryFlag(), "j_spine4", 1);
                        });
                    case RollType.StaminUp:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("speed", 1.5f);
                        });
                    case RollType.OneHitKill:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("maxhealth", 1);
                            player.Health = 1;
                        });
                    case RollType.Juggernaut:
                        return new Action<Entity>(player =>
                        {
                            player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                            player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                            player.Health *= 3;
                        });
                    case RollType.SMAW:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("iw5_smaw_mp");
                            player.Call("setweaponammoclip", "iw5_smaw_mp", 1);
                            player.Call("setweaponammostock", "iw5_smaw_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_smaw_mp"));
                        });
                    case RollType.RPG:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("rpg_mp");
                            player.Call("setweaponammoclip", "rpg_mp", 1);
                            player.Call("setweaponammostock", "rpg_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("rpg_mp"));
                        });
                    case RollType.Stinger:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("stinger_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("stinger_mp"));
                        });
                    case RollType.AA12:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("iw5_aa12_mp");
                            player.Call("setweaponammostock", "iw5_aa12_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_aa12_mp"));
                        });
                    case RollType.Spas:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("iw5_spas12_mp");
                            player.Call("setweaponammostock", "iw5_spas12_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_spas12_mp"));
                        });
                    case RollType.Turtle:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("speed", 0.7f);
                        });
                    case RollType.Javelin:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("javelin_mp");
                            player.Call("setweaponammostock", "javelin_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("javelin_mp"));
                        });
                    case RollType.Riotshield:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("riotshield_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("riotshield_mp"));
                            player.Call("attachshieldmodel", "weapon_riot_shield_mp", "tag_shield_back");
                        });
                    case RollType.KingOfJuggernaut:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_king", 1);
                            player.Call("attach", GetCarryFlag(), "j_spine4", 1);
                            player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                            player.SetField("maxhealth", player.GetField<int>("maxhealth") * 10);
                            player.Health *= 10;
                        });
                    case RollType.DesertEagle:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("iw5_deserteagle_mp");
                            player.Call("setweaponammostock", "iw5_deserteagle_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_deserteagle_mp"));
                        });
                    case RollType.ThrowingKnife:
                        return new Action<Entity>(player =>
                        {
                            player.GiveWeapon("throwingknife_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("throwingknife_mp"));
                        });
                    case RollType.M320:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("m320_mp");
                            player.Call("setweaponammostock", "m320_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("m320_mp"));
                        });
                    case RollType.XM25:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("xm25_mp");
                            player.Call("setweaponammostock", "xm25_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("xm25_mp"));
                        });
                    case RollType.MK14:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("iw5_mk14_mp");
                            player.Call("setweaponammoclip", "iw5_mk14_mp", 1);
                            player.Call("setweaponammostock", "iw5_mk14_mp", 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_mk14_mp"));
                        });
                    case RollType.SVD:
                        return new Action<Entity>(player =>
                        {
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon(Utilities.BuildWeaponName("iw5_dragunov", "none", "none", 0, 0));
                            player.Call("setweaponammoclip", Utilities.BuildWeaponName("iw5_dragunov", "none", "none", 0, 0), 1);
                            player.Call("setweaponammostock", Utilities.BuildWeaponName("iw5_dragunov", "none", "none", 0, 0), 0);
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate(Utilities.BuildWeaponName("iw5_dragunov", "none", "none", 0, 0)));
                        });
                    case RollType.ISIS:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_isis", 1);
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("c4death_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                        });
                    case RollType.ISISJuggernaut:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_isis", 1);
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("c4death_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                            player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                            player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                            player.Health *= 3;
                        });
                    case RollType.ZombieIncantation:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("zombie_incantation", 1);
                        });
                    case RollType.Tombstone:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_tombstone", 1);
                        });
                    case RollType.Tank:
                        return new Action<Entity>(player =>
                        {
                            player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                            player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                            player.Health *= 3;
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("riotshield_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("riotshield_mp"));
                            player.Call("attachshieldmodel", "weapon_riot_shield_mp", "tag_shield_back");
                        });
                    case RollType.Boomer:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_boomer", 1);
                            Utility.SetZombieSniperModel(player);
                        });
                    case RollType.Spider:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_spider", 1);
                            Utility.SetZombieSniperModel(player);
                        });
                    case RollType.JetPack:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_exo", 1);
                        });
                    case RollType.JetPackJuggernaut:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_exo", 1);
                            player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                            player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                            player.Health *= 3;
                        });
                    case RollType.JetPackISISZombie:
                        return new Action<Entity>(player =>
                        {
                            player.SetField("rtd_exo", 1);
                            player.SetField("rtd_isis", 1);
                            player.TakeWeapon(player.CurrentWeapon);
                            player.GiveWeapon("c4death_mp");
                            player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                        });
                    case RollType.DiscoZombie:
                        return new Action<Entity>(player =>
                        {
                            if (ZombieRollTheDice.hasDiscoZombie)
                            {
                                OnRolled(player);
                                return;
                            }
                            player.SetField("rtd_disco", 1);
                            ZombieRollTheDice.hasDiscoZombie = true;
                            ZombieRollTheDice.curretDiscoZombie = player;
                            player.SetField("maxhealth", player.GetField<int>("maxhealth") * 2);
                            player.Health *= 2;
                        });
                    default:
                        return null;
                }
            }
        }

        public string FullName
        {
            get
            {
                switch (Type)
                {
                    case RollType.Nothing:
                        return "^1Nothing";
                    case RollType.FlagZombie:
                        return "^1Flag Zombie";
                    case RollType.StaminUp:
                        return "^2HK Journalist Zombie";
                    case RollType.OneHitKill:
                        return "^1You are one hit kill";
                    case RollType.Juggernaut:
                        return "^2Juggernaut";
                    case RollType.SMAW:
                        return "^2SMAW";
                    case RollType.RPG:
                        return "^2RPG";
                    case RollType.Stinger:
                        return "^1Stinger";
                    case RollType.AA12:
                        return "^2One Magazine AA12";
                    case RollType.Spas:
                        return "^2One Magazine SPAS-12";
                    case RollType.Turtle:
                        return "^1Turtle";
                    case RollType.Javelin:
                        return "^2Javelin";
                    case RollType.Riotshield:
                        return "^2Riotshield";
                    case RollType.KingOfJuggernaut:
                        return "^3King of the Juggernaut";
                    case RollType.DesertEagle:
                        return "^2One Magazine Desert Eagle";
                    case RollType.ThrowingKnife:
                        return "^2One Throwing Knife";
                    case RollType.M320:
                        return "^2One Ammo M320";
                    case RollType.XM25:
                        return "^2One Magazine XM25";
                    case RollType.MK14:
                        return "^2One Ammo MK14";
                    case RollType.SVD:
                        return "^2One Ammo SVD";
                    case RollType.ISIS:
                        return "^2ISIS Zombie";
                    case RollType.ISISJuggernaut:
                        return "^3ISIS Juggernaut";
                    case RollType.ZombieIncantation:
                        return "^1Zombie Incantation";
                    case RollType.Tombstone:
                        return "^2Tombstone";
                    case RollType.Tank:
                        return "^3Tank";
                    case RollType.Boomer:
                        return "^2Boomer";
                    case RollType.Spider:
                        return "^2Spider";
                    case RollType.JetPack:
                        return "^2JetPack Zombie";
                    case RollType.JetPackJuggernaut:
                        return "^3JetPack Juggernaut";
                    case RollType.JetPackISISZombie:
                        return "^3JetPack ISIS Zombie";
                    case RollType.DiscoZombie:
                        return "^3Disco Zombie";
                    default:
                        return "";
                }
            }
        }

        public static string GetCarryFlag()
        {
            if (Utility.AfricaMaps.Contains(Utility.MapName))
            {
                return "prop_flag_africa_carry";
            }
            else if (Utility.ICMaps.Contains(Utility.MapName))
            {
                return "prop_flag_ic_carry";
            }
            else
            {
                return "prop_flag_speznas_carry";
            }
        }

        public RTDItem(RollType type)
        {
            Type = type;
        }
    }

    public class ZombieRollTheDice : BaseScript
    {
        private List<RTDItem> _rtditems = new List<RTDItem>
        {
            new RTDItem(RollType.AA12),
            new RTDItem(RollType.Boomer),
            new RTDItem(RollType.DesertEagle),
            new RTDItem(RollType.DiscoZombie),
            new RTDItem(RollType.JetPackISISZombie),
            new RTDItem(RollType.JetPackJuggernaut),
            new RTDItem(RollType.JetPack),
            new RTDItem(RollType.FlagZombie),
            new RTDItem(RollType.ISIS),
            new RTDItem(RollType.ISISJuggernaut),
            new RTDItem(RollType.Javelin),
            new RTDItem(RollType.Juggernaut),
            new RTDItem(RollType.KingOfJuggernaut),
            new RTDItem(RollType.M320),
            new RTDItem(RollType.Nothing),
            new RTDItem(RollType.OneHitKill),
            new RTDItem(RollType.Riotshield),
            new RTDItem(RollType.RPG),
            new RTDItem(RollType.SMAW),
            new RTDItem(RollType.Spas),
            new RTDItem(RollType.Spider),
            new RTDItem(RollType.StaminUp),
            new RTDItem(RollType.Stinger),
            new RTDItem(RollType.SVD),
            new RTDItem(RollType.Tank),
            new RTDItem(RollType.ThrowingKnife),
            new RTDItem(RollType.Tombstone),
            new RTDItem(RollType.Turtle),
            new RTDItem(RollType.XM25),
            new RTDItem(RollType.ZombieIncantation),
        };

        public static bool hasDiscoZombie = false;
        public static Entity curretDiscoZombie = null;

        public ZombieRollTheDice()
        {
            PlayerConnected += player =>
            {
                player.SetField("rtd_canroll", 1);
                player.SetField("zombie_incantation", 0);
                player.SetField("rtd_flag", 0);
                player.SetField("rtd_king", 0);
                player.SetField("rtd_isis", 0);
                player.SetField("rtd_boomer", 0);
                player.SetField("rtd_spider", 0);
                player.SetField("rtd_crawler", 0);
                player.SetField("rtd_exo", 0);
                player.SetField("rtd_disco", 0);
                player.SetField("rtd_tombstone", 0);
                player.SetField("rtd_tombstoneorigin", new Vector3());

                player.Call("notifyonplayercommand", "attack", "+attack");
                player.OnNotify("attack", self =>
                {
                    if (player.GetTeam() == "axis" && player.GetField<int>("rtd_isis") == 1 && player.CurrentWeapon == "c4death_mp" && player.IsAlive && !IsClosingPhD(player))
                    {
                        player.SetField("rtd_isis", 0);
                        AfterDelay(1000, () => player.Notify("isis_exploed"));
                    }
                });

                OnSpawned(player);
                player.SpawnedPlayer += () => OnSpawned(player);
            };
        }

        public void OnSpawned(Entity player)
        {
            if (player.GetTeam() == "axis")
            {
                if (player.GetField<int>("rtd_canroll") == 1)
                {
                    player.AfterDelay(50, e => DoRandom(player));
                }
                if (player.GetField<int>("rtd_tombstone") == 1)
                {
                    player.Call("setorigin", player.GetField<Vector3>("rtd_tombstoneorigin"));
                }
                else if (hasDiscoZombie && curretDiscoZombie != null && curretDiscoZombie.IsPlayer && curretDiscoZombie.IsAlive && curretDiscoZombie.GetField<int>("rtd_disco") == 1)
                {
                    player.Call("setorigin", GetDiscoZombieDeployPoint());
                }
                player.SetField("speed", 1);
                player.SetField("rtd_canroll", 1);
                player.SetField("zombie_incantation", 0);
                player.SetField("rtd_flag", 0);
                player.SetField("rtd_king", 0);
                player.SetField("rtd_isis", 0);
                player.SetField("rtd_boomer", 0);
                player.SetField("rtd_spider", 0);
                player.SetField("rtd_exo", 0);
                player.SetField("rtd_disco", 0);
                player.SetField("rtd_tombstone", 0);
                player.SetField("rtd_tombstoneorigin", new Vector3());

                player.SetField("onhitacid", 0);
            }
        }

        public void DoRandom(Entity player)
        {
            var type = Utility.RandomByWeight(_rtditems);

            if (type == null)
            {
                Log.Debug("type is null!!!");
            }
            else
            {
                type.OnRolled.DynamicInvoke(player);
                PrintRollName(player, type);
            }
        }

        private void PrintRollName(Entity player, RTDItem item)
        {
            player.PrintlnBold("You rolled: " + item.FullName);
            Utility.Println(player.Name + " rolled - " + item.FullName);
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (mod == "MOD_SUICIDE" || attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
            {
                player.SetField("rtd_canroll", 0);
            }

            if (player.GetField<int>("rtd_flag") == 1)
            {
                player.Call("detach", RTDItem.GetCarryFlag(), "j_spine4");
            }
            if (player.GetField<int>("rtd_king") == 1)
            {
                player.Call("detach", RTDItem.GetCarryFlag(), "j_spine4");
            }
            if (player.GetField<int>("rtd_tombstone") == 1)
            {
                player.SetField("rtd_tombstoneorigin", player.Origin);
            }
            if (player.GetField<int>("rtd_boomer") == 1)
            {
                foreach (var item in GetClosingHumans(player))
                {
                    //if (item.GetCurrentGobbleGum().Type == GobbleGumType.Wiper)
                    //{
                    //    item.ActiveGobbleGum();
                    //}
                    //else
                    //{
                    if (item.GetField<int>("perk_cherry") == 1)
                    {
                        item.Call("setblurforplayer", 6, 0.5f);
                        item.Call("shellshock", "concussion_grenade_mp", 2);
                        item.AfterDelay(2000, e =>
                        {
                            item.Call("setblurforplayer", 0, 0.3f);
                        });
                    }
                    else
                    {
                        item.Call("setblurforplayer", 6, 0.5f);
                        item.Call("shellshock", "concussion_grenade_mp", 5);
                        item.AfterDelay(5000, e =>
                        {
                            item.Call("setblurforplayer", 0, 0.3f);
                        });
                    }
                    //}
                }
            }
            if (player.GetField<int>("rtd_spider") == 1)
            {
                SpiderAcidArea(player, player.Origin);
            }
            if (player.GetField<int>("rtd_disco") == 1)
            {
                hasDiscoZombie = false;
                curretDiscoZombie = null;
            }

            if (mod == "MOD_HEAD_SHOT")
            {
                player.Call("detachall");
            }
        }

        private bool IsClosingPhD(Entity player)
        {
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() == "allies" && item.GetField<int>("perk_phd") == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private List<Entity> GetClosingHumans(Entity player)
        {
            var list = new List<Entity>();
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() == "allies" && item.IsAlive && item.Origin.DistanceTo(player.Origin) <= 500)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        private Vector3 GetDiscoZombieDeployPoint()
        {
            if (curretDiscoZombie == null || !curretDiscoZombie.IsPlayer || !curretDiscoZombie.IsAlive)
                return new Vector3();

            return curretDiscoZombie.Origin;
        }

        private void SpiderAcidArea(Entity player, Vector3 origin)
        {
            bool flag = true;
            var fx = Call<Entity>("spawnfx", Effects.redbeaconfx, origin);
            Call("triggerfx", fx);
            OnInterval(100, () =>
            {
                foreach (var item in Utility.Players)
                {
                    if (item.GetTeam() == "allies" && item.IsAlive && item.Origin.DistanceTo(origin) <= 300)
                    {
                        if (item.GetField<int>("onhitacid") == 0)
                        {
                            //if (player.GetCurrentGobbleGum().Type == GobbleGumType.Antidote)
                            //{
                            //    player.ActiveGobbleGum();
                            //    player.SetField("onhitacid", 1);
                            //    player.AfterDelay(6000, e => player.SetField("onhitacid", 0));
                            //}
                            //else
                            //{
                            SpiderAcidAreaThink(item, player);
                            //}
                        }
                    }
                }

                return flag;
            });
            AfterDelay(6000, () =>
            {
                fx.Call("delete");
                flag = false;
            });
        }

        private void SpiderAcidAreaThink(Entity player, Entity attacker)
        {
            if (player.GetField<int>("perk_cherry") == 1)
            {
                player.Notify("acid_damage", attacker, 20);
            }
            else
            {
                player.Notify("acid_damage", attacker, 40);
            }
            player.SetField("onhitacid", 1);

            player.AfterDelay(1500, e => player.SetField("onhitacid", 0));
        }
    }
}

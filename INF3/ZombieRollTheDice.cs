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
        /// 手持毒刺的僵尸
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
        /// 手持手雷的僵尸
        /// </summary>
        FragGrenade,
        /// <summary>
        /// 手持M320的僵尸
        /// </summary>
        M320,
        /// <summary>
        /// 手持XM25的僵尸
        /// </summary>
        XM25,
        /// <summary>
        /// 手持MK14的僵尸
        /// </summary>
        MK14,
        /// <summary>
        /// 手持SVD的僵尸
        /// </summary>
        SVD,
        /// <summary>
        /// 手持MSR的僵尸
        /// </summary>
        MSR,
        /// <summary>
        /// 清真僵尸（雾），手持引爆器，可以进行自杀式攻击的僵尸
        /// </summary>
        ISIS,
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
        /// Somker （复刻L4D2），被击杀会产生一片烟雾干扰人类的视线
        /// </summary>
        Somker,
        /// <summary>
        /// 可以跳的更高的僵尸
        /// </summary>
        JetPack,
        /// <summary>
        /// 被击杀可以降低周围僵尸的移动速度
        /// </summary>
        IceZombie,
        /// <summary>
        /// 增加手刀距离
        /// </summary>
        ExtendedMelee,
        /// <summary>
        /// 被击杀要么是Incantation效果（伤害僵尸），要么是ISIS爆炸效果（伤害人类）
        /// </summary>
        MysteryZombie,
        /// <summary>
        /// 视野变成灰暗效果
        /// </summary>
        Darkness,
        /// <summary>
        /// 视野变成相片底片效果
        /// </summary>
        Negative,
        /// <summary>
        /// 视野变成核爆效果
        /// </summary>
        Fallout,
        /// <summary>
        /// 视野变成夜视仪效果
        /// </summary>
        Nightvision,
        /// <summary>
        /// 天黑了？？？
        /// </summary>
        Late,
        /// <summary>
        /// 五秒内无敌
        /// </summary>
        GodMode,
        /// <summary>
        /// 五秒后死亡
        /// </summary>
        Die,
        /// <summary>
        /// 免疫来自人类的爆炸物伤害
        /// </summary>
        PhD,
        /// <summary>
        /// 无视人类Juggernog并且死亡后有EMP效果的僵尸
        /// </summary>
        Tesla,
        /// <summary>
        /// 重生后被没收主武器的僵尸
        /// </summary>
        WalkingDeath,
        /// <summary>
        /// 复制人类武器的僵尸
        /// </summary>
        CopyCat,
        /// <summary>
        /// 使用重机枪的僵尸
        /// </summary>
        DeathMachine,
        /// <summary>
        /// 手持自杀式炸弹引爆器的重装兵
        /// </summary>
        ISISJuggernaut,
        /// <summary>
        /// 手持自杀式炸弹引爆器，并且可以跳的更高的僵尸
        /// </summary>
        JetPackISIS,
        /// <summary>
        /// 可以跳的更高的重装兵
        /// </summary>
        JetPackJuggernaut,
        /// <summary>
        /// Tesla重装兵
        /// </summary>
        TeslaJuggernaut,
        /// <summary>
        /// 同时具有加速跑、喷气包以及增加手刀距离的僵尸
        /// </summary>
        Tyrant,
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
                        return 10;
                    case RollType.FlagZombie:
                        return 10;
                    case RollType.StaminUp:
                        return 4;
                    case RollType.OneHitKill:
                        return 2;
                    case RollType.Juggernaut:
                        return 6;
                    case RollType.SMAW:
                        return 1;
                    case RollType.RPG:
                        return 1;
                    case RollType.Stinger:
                        return 1;
                    case RollType.AA12:
                        return 1;
                    case RollType.Spas:
                        return 1;
                    case RollType.Javelin:
                        return 1;
                    case RollType.Riotshield:
                        return 4;
                    case RollType.KingOfJuggernaut:
                        return 1;
                    case RollType.DesertEagle:
                        return 1;
                    case RollType.ThrowingKnife:
                        return 1;
                    case RollType.FragGrenade:
                        return 1;
                    case RollType.M320:
                        return 1;
                    case RollType.XM25:
                        return 1;
                    case RollType.MK14:
                        return 1;
                    case RollType.SVD:
                        return 1;
                    case RollType.MSR:
                        return 1;
                    case RollType.ISIS:
                        return 4;
                    case RollType.ZombieIncantation:
                        return 6;
                    case RollType.Tombstone:
                        return 2;
                    case RollType.Tank:
                        return 1;
                    case RollType.Boomer:
                        return 4;
                    case RollType.Spider:
                        return 4;
                    case RollType.Somker:
                        return 4;
                    case RollType.JetPack:
                        return 4;
                    case RollType.IceZombie:
                        return 3;
                    case RollType.ExtendedMelee:
                        return 3;
                    case RollType.MysteryZombie:
                        return 4;
                    case RollType.Darkness:
                        return 2;
                    case RollType.Negative:
                        return 2;
                    case RollType.Fallout:
                        return 2;
                    case RollType.Nightvision:
                        return 2;
                    case RollType.Late:
                        return 2;
                    case RollType.GodMode:
                        return 1;
                    case RollType.Die:
                        return 2;
                    case RollType.PhD:
                        return 2;
                    case RollType.Tesla:
                        return 1;
                    case RollType.WalkingDeath:
                        return 4;
                    case RollType.CopyCat:
                        return 1;
                    case RollType.DeathMachine:
                        return 1;
                    case RollType.ISISJuggernaut:
                        return 2;
                    case RollType.JetPackJuggernaut:
                        return 1;
                    case RollType.JetPackISIS:
                        return 2;
                    case RollType.TeslaJuggernaut:
                        return 1;
                    case RollType.Tyrant:
                        return 1;
                    default:
                        return 0;
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
                        return "^2Stinger";
                    case RollType.AA12:
                        return "^2AA12";
                    case RollType.Spas:
                        return "^2SPAS-12";
                    case RollType.Javelin:
                        return "^2Javelin";
                    case RollType.Riotshield:
                        return "^2Riotshield";
                    case RollType.KingOfJuggernaut:
                        return "^3King of Juggernaut";
                    case RollType.DesertEagle:
                        return "^2Desert Eagle";
                    case RollType.ThrowingKnife:
                        return "^2One Throwing Knife";
                    case RollType.FragGrenade:
                        return "^2One Frag Grenade";
                    case RollType.M320:
                        return "^2M320";
                    case RollType.XM25:
                        return "^2XM25";
                    case RollType.MK14:
                        return "^2MK14";
                    case RollType.SVD:
                        return "^2SVD";
                    case RollType.ISIS:
                        return "^2ISIS Zombie";
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
                    case RollType.Somker:
                        return "^2Somker";
                    case RollType.JetPack:
                        return "^2JetPack Zombie";
                    case RollType.IceZombie:
                        return "^1Ice Zombie";
                    case RollType.ExtendedMelee:
                        return "^2Extended Melee";
                    case RollType.MysteryZombie:
                        return "^3Mystery Zombie";
                    case RollType.Darkness:
                        return "^1Darkness";
                    case RollType.Negative:
                        return "^1Negative";
                    case RollType.Fallout:
                        return "^1Fallout";
                    case RollType.Nightvision:
                        return "^1Night Vision";
                    case RollType.Late:
                        return "^1It's late...";
                    case RollType.GodMode:
                        return "^3GodMode for 5 second";
                    case RollType.Die:
                        return "^1You dead in 5 second";
                    case RollType.PhD:
                        return "^2PhD";
                    case RollType.MSR:
                        return "^2MSR";
                    case RollType.Tesla:
                        return "^3Tesla Zombie";
                    case RollType.WalkingDeath:
                        return "^1Walking Death";
                    case RollType.CopyCat:
                        return "^3CopyCat";
                    case RollType.DeathMachine:
                        return "^3Death Machine";
                    case RollType.ISISJuggernaut:
                        return "^3ISIS Juggernaut";
                    case RollType.JetPackJuggernaut:
                        return "^3JetPack Juggernaut";
                    case RollType.JetPackISIS:
                        return "^3JetPack ISIS Zombie";
                    case RollType.TeslaJuggernaut:
                        return "^3Tesla Zombie";
                    case RollType.Tyrant:
                        return "^3Tyrant";
                    default:
                        return "";
                }
            }
        }

        public void DoRolled(Entity player)
        {
            switch (Type)
            {
                case RollType.Nothing:
                    break;
                case RollType.FlagZombie:
                    player.SetField("rtd_flag", 1);
                    player.Call("attach", GetCarryFlag(), "j_spine4", 1);
                    break;
                case RollType.StaminUp:
                    player.SetSpeed(1.5f);
                    break;
                case RollType.OneHitKill:
                    player.SetField("maxhealth", 1);
                    player.Health = 1;
                    break;
                case RollType.Juggernaut:
                    player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                    player.Health *= 3;
                    break;
                case RollType.SMAW:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("iw5_smaw_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_smaw_mp"));
                    break;
                case RollType.RPG:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("rpg_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("rpg_mp"));
                    break;
                case RollType.Stinger:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("stinger_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("stinger_mp"));
                    break;
                case RollType.AA12:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("iw5_aa12_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_aa12_mp"));
                    break;
                case RollType.Spas:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("iw5_spas12_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_spas12_mp"));
                    break;
                case RollType.Javelin:
                    player.GiveMaxAmmoWeapon("javelin_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("javelin_mp"));
                    break;
                case RollType.Riotshield:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon("riotshield_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("riotshield_mp"));
                    player.AttachShieldModel( "weapon_riot_shield_mp", "tag_shield_back");
                    break;
                case RollType.KingOfJuggernaut:
                    player.SetField("rtd_king", 1);
                    player.SetSpeed(0.7f);
                    player.Call("attach", GetCarryFlag(), "j_spine4", 1);
                    player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 10);
                    player.Health *= 10;
                    break;
                case RollType.DesertEagle:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("iw5_deserteagle_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_deserteagle_mp"));
                    break;
                case RollType.ThrowingKnife:
                    player.GiveWeapon("throwingknife_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("throwingknife_mp"));
                    break;
                case RollType.FragGrenade:
                    player.GiveWeapon("frag_grenade_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("frag_grenade_mp"));
                    break;
                case RollType.M320:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("m320_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("m320_mp"));
                    break;
                case RollType.XM25:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("xm25_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("xm25_mp"));
                    break;
                case RollType.MK14:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("iw5_mk14_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_mk14_mp"));
                    break;
                case RollType.SVD:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon(Utilities.BuildWeaponName("iw5_dragunov", "none", "none", 0, 0));
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate(Utilities.BuildWeaponName("iw5_dragunov", "none", "none", 0, 0)));
                    break;
                case RollType.ISIS:
                    player.SetField("rtd_isis", 1);
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon("c4death_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                    break;
                case RollType.ZombieIncantation:
                    player.SetField("zombie_incantation", 1);
                    break;
                case RollType.Tombstone:
                    player.SetField("rtd_tombstone", 1);
                    break;
                case RollType.Tank:
                    player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                    player.Health *= 3;
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon("riotshield_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("riotshield_mp"));
                    player.AttachShieldModel( "weapon_riot_shield_mp", "tag_shield_back");
                    break;
                case RollType.Boomer:
                    player.SetField("rtd_boomer", 1);
                    Utility.SetZombieSniperModel(player);
                    break;
                case RollType.Spider:
                    player.SetField("rtd_spider", 1);
                    Utility.SetZombieSniperModel(player);
                    break;
                case RollType.Somker:
                    player.SetField("rtd_somker", 1);
                    Utility.SetZombieSniperModel(player);
                    break;
                case RollType.JetPack:
                    player.SetField("rtd_exo", 1);
                    break;
                case RollType.IceZombie:
                    player.SetField("rtd_ice", 1);
                    break;
                case RollType.ExtendedMelee:
                    player.SetPerk("specialty_extendedmelee", true, true);
                    break;
                case RollType.MysteryZombie:
                    player.SetField("rtd_mystery", 1);
                    break;
                case RollType.Darkness:
                    player.SetField("rtd_vision", 1);
                    player.Notify("vision", "cheat_chaplinnight");
                    break;
                case RollType.Negative:
                    player.SetField("rtd_vision", 1);
                    player.Notify("vision", "cheat_invert_contrast");
                    break;
                case RollType.Fallout:
                    player.SetField("rtd_vision", 1);
                    player.Notify("vision", "mpnuke");
                    break;
                case RollType.Nightvision:
                    player.SetField("rtd_vision", 1);
                    player.Notify("vision", "default_night_mp");
                    break;
                case RollType.Late:
                    player.SetField("rtd_vision", 1);
                    player.Notify("vision", "cobra_sunset3");
                    break;
                case RollType.GodMode:
                    player.Health = -1;
                    player.AfterDelay(5000, e => player.Health = player.GetField<int>("maxhealth"));
                    break;
                case RollType.Die:
                    player.AfterDelay(5000, e => player.Notify("self_exploed"));
                    break;
                case RollType.PhD:
                    player.SetField("rtd_phd", 1);
                    player.SetPerk("_specialty_blastshield", true, false);
                    player.SetPerk("specialty_throwback", true, false);
                    break;
                case RollType.MSR:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon(Utilities.BuildWeaponName("iw5_msr", "none", "none", 0, 0));
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate(Utilities.BuildWeaponName("iw5_msr", "none", "none", 0, 0)));
                    break;
                case RollType.Tesla:
                    player.SetField("rtd_tesla", 1);
                    break;
                case RollType.WalkingDeath:
                    player.TakeWeapon(player.CurrentWeapon);
                    break;
                case RollType.CopyCat:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon(Sharpshooter._firstWeapon.Code);
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate(Sharpshooter._firstWeapon.Code));
                    break;
                case RollType.DeathMachine:
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveMaxAmmoWeapon("iw5_m60jugg_mp_eotechlmg_rof_camo08");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_m60jugg_mp_eotechlmg_rof_camo08"));
                    break;
                case RollType.ISISJuggernaut:
                    player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                    player.Health *= 3;
                    player.SetField("rtd_isis", 1);
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon("c4death_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                    break;
                case RollType.JetPackJuggernaut:
                    player.SetField("rtd_isis", 1);
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon("c4death_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                    player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                    player.Health *= 3;
                    break;
                case RollType.JetPackISIS:
                    player.SetField("rtd_isis", 1);
                    player.TakeWeapon(player.CurrentWeapon);
                    player.GiveWeapon("c4death_mp");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("c4death_mp"));
                    player.SetField("rtd_exo", 1);
                    break;
                case RollType.TeslaJuggernaut:
                    player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 3);
                    player.Health *= 3;
                    player.SetField("rtd_tesla", 1);
                    break;
                case RollType.Tyrant:
                    player.SetSpeed(1.5f);
                    player.SetField("rtd_exo", 1);
                    player.SetPerk("specialty_extendedmelee", true, true);
                    break;
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
        private List<RTDItem> _normal = new List<RTDItem>
        {
            new RTDItem(RollType.Boomer),
            new RTDItem(RollType.Darkness),
            new RTDItem(RollType.Die),
            new RTDItem(RollType.ExtendedMelee),
            new RTDItem(RollType.Fallout),
            new RTDItem(RollType.FlagZombie),
            new RTDItem(RollType.GodMode),
            new RTDItem(RollType.IceZombie),
            new RTDItem(RollType.ISIS),
            new RTDItem(RollType.JetPack),
            new RTDItem(RollType.Juggernaut),
            new RTDItem(RollType.Late),
            new RTDItem(RollType.Negative),
            new RTDItem(RollType.Nightvision),
            new RTDItem(RollType.Nothing),
            new RTDItem(RollType.OneHitKill),
            new RTDItem(RollType.PhD),
            new RTDItem(RollType.Riotshield),
            new RTDItem(RollType.Somker),
            new RTDItem(RollType.Spider),
            new RTDItem(RollType.StaminUp),
            new RTDItem(RollType.ThrowingKnife),
            new RTDItem(RollType.Tombstone),
            new RTDItem(RollType.WalkingDeath),
            new RTDItem(RollType.ZombieIncantation),
        };

        private List<RTDItem> _super = new List<RTDItem>
        {
            new RTDItem(RollType.Boomer),
            new RTDItem(RollType.ExtendedMelee),
            new RTDItem(RollType.FlagZombie),
            new RTDItem(RollType.GodMode),
            new RTDItem(RollType.ISIS),
            new RTDItem(RollType.Javelin),
            new RTDItem(RollType.JetPack),
            new RTDItem(RollType.Juggernaut),
            new RTDItem(RollType.KingOfJuggernaut),
            new RTDItem(RollType.MysteryZombie),
            new RTDItem(RollType.PhD),
            new RTDItem(RollType.Riotshield),
            new RTDItem(RollType.Somker),
            new RTDItem(RollType.Spider),
            new RTDItem(RollType.StaminUp),
            new RTDItem(RollType.Tank),
            new RTDItem(RollType.Tesla),
            new RTDItem(RollType.ThrowingKnife),
            new RTDItem(RollType.Tombstone),
            new RTDItem(RollType.ZombieIncantation),
            new RTDItem(RollType.ISISJuggernaut),
            new RTDItem(RollType.JetPackISIS),
            new RTDItem(RollType.JetPackJuggernaut),
            new RTDItem(RollType.TeslaJuggernaut),
            new RTDItem(RollType.Tyrant),
        };

        private List<RTDItem> _burnedout = new List<RTDItem>
        {
            new RTDItem(RollType.Darkness),
            new RTDItem(RollType.Die),
            new RTDItem(RollType.Fallout),
            new RTDItem(RollType.FlagZombie),
            new RTDItem(RollType.IceZombie),
            new RTDItem(RollType.Late),
            new RTDItem(RollType.Negative),
            new RTDItem(RollType.Nightvision),
            new RTDItem(RollType.Nothing),
            new RTDItem(RollType.OneHitKill),
            new RTDItem(RollType.WalkingDeath),
            new RTDItem(RollType.ZombieIncantation),
        };

        private List<RTDItem> _first = new List<RTDItem>
        {
            new RTDItem(RollType.AA12),
            new RTDItem(RollType.CopyCat),
            new RTDItem(RollType.DeathMachine),
            new RTDItem(RollType.DesertEagle),
            new RTDItem(RollType.Javelin),
            new RTDItem(RollType.M320),
            new RTDItem(RollType.MK14),
            new RTDItem(RollType.MSR),
            new RTDItem(RollType.Stinger),
            new RTDItem(RollType.SVD),
            new RTDItem(RollType.ISIS),
            new RTDItem(RollType.JetPack),
            new RTDItem(RollType.Juggernaut),
            new RTDItem(RollType.KingOfJuggernaut),
            new RTDItem(RollType.MysteryZombie),
            new RTDItem(RollType.PhD),
            new RTDItem(RollType.ISISJuggernaut),
            new RTDItem(RollType.JetPackISIS),
            new RTDItem(RollType.JetPackJuggernaut),
            new RTDItem(RollType.TeslaJuggernaut),
            new RTDItem(RollType.Tyrant),
        };

        public ZombieRollTheDice()
        {
            PlayerConnected += player =>
            {
                player.SetField("deathstreak", 0);
                player.SetField("onhitacid", 0);
                player.SetField("rtd_canroll", 1);
                player.SetField("zombie_incantation", 0);
                player.SetField("rtd_flag", 0);
                player.SetField("rtd_king", 0);
                player.SetField("rtd_isis", 0);
                player.SetField("rtd_boomer", 0);
                player.SetField("rtd_spider", 0);
                player.SetField("rtd_somker", 0);
                player.SetField("rtd_exo", 0);
                player.SetField("rtd_ice", 0);
                player.SetField("rtd_mystery", 0);
                player.SetField("rtd_vision", 0);
                player.SetField("rtd_phd", 0);
                player.SetField("rtd_tesla", 0);
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
                player.OnNotify("vision", (self, vision) => SetVision(self, vision.As<string>()));
                OnSpawned(player);
                player.SpawnedPlayer += () => OnSpawned(player);
            };
        }

        public void OnSpawned(Entity player)
        {
            if (player.GetTeam() == "axis")
            {
                if (player.GetField<int>("rtd_tombstone") == 1)
                {
                    player.SetOrigin( player.GetField<Vector3>("rtd_tombstoneorigin"));
                }
                player.SetField("zombie_incantation", 0);
                player.SetField("rtd_flag", 0);
                player.SetField("rtd_king", 0);
                player.SetField("rtd_isis", 0);
                player.SetField("rtd_boomer", 0);
                player.SetField("rtd_spider", 0);
                player.SetField("rtd_somker", 0);
                player.SetField("rtd_exo", 0);
                player.SetField("rtd_ice", 0);
                player.SetField("rtd_mystery", 0);
                player.SetField("rtd_vision", 0);
                player.SetField("rtd_phd", 0);
                player.SetField("rtd_tesla", 0);
                player.SetField("rtd_tombstone", 0);
                player.SetField("rtd_tombstoneorigin", new Vector3());
                if (player.GetField<int>("rtd_canroll") == 1)
                {
                    RandomByDeadstreak(player);
                }
                player.SetField("rtd_canroll", 1);

                if (Utility.GetDvar<int>("zombie_double_health") == 1)
                {
                    player.SetField("maxhealth", player.GetField<int>("maxhealth") * 2);
                    player.Health *= 2;
                }
            }
        }

        public void RandomByDeadstreak(Entity player)
        {
            if (this.Call<int>("getteamscore", "axis") == 1)
            {
                DoRandom(player, _first);
            }
            else
            {
                if (Utility.GetDvar<int>("bonus_burned_out") == 1)
                {
                    DoRandom(player, _burnedout);
                }
                else if (player.GetField<int>("deathstreak") < 10)
                {
                    DoRandom(player, _normal);
                }
                else if (player.GetField<int>("deathstreak") >= 10)
                {
                    DoRandom(player, _super);
                }
            }
        }

        public void DoRandom(Entity player, List<RTDItem> _rtditems)
        {
            var type = Utility.RandomByWeight(_rtditems);

            type.DoRolled(player);
            PrintRollName(player, type);
        }

        private void PrintRollName(Entity player, RTDItem item, bool rollagain = false)
        {
            if (rollagain)
            {
                player.PrintlnBold("You rolled: " + item.FullName + " and Roll Again");
                Utility.Println(player.Name + " rolled - " + item.FullName + " and Roll Again");
            }
            else
            {
                player.PrintlnBold("You rolled: " + item.FullName);
                Utility.Println(player.Name + " rolled - " + item.FullName);
            }
        }

        public override void OnPlayerKilled(Entity player, Entity inflictor, Entity attacker, int damage, string mod, string weapon, Vector3 dir, string hitLoc)
        {
            if (mod == "MOD_SUICIDE" || attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
            {
                player.SetField("rtd_canroll", 0);
            }
            else
            {
                if (player.GetTeam() == "axis")
                {
                    player.SetField("deathstreak", player.GetField<int>("deathstreak") + 1);
                }
                else if (attacker.GetTeam() == "axis")
                {
                    player.SetField("deathstreak", 0);
                }
            }

            if (weapon == "nuke_mp")
            {
                player.Call("detachall");
                return;
            }

            else if (player.GetField<int>("rtd_flag") == 1)
            {
                player.Call("detach", RTDItem.GetCarryFlag(), "j_spine4");
            }
            else if (player.GetField<int>("rtd_king") == 1)
            {
                player.Call("detach", RTDItem.GetCarryFlag(), "j_spine4");
            }
            else if (player.GetField<int>("rtd_tombstone") == 1)
            {
                player.SetField("rtd_tombstoneorigin", player.Origin);
            }
            else if (player.GetField<int>("rtd_boomer") == 1)
            {
                foreach (var item in GetClosingHumans(player))
                {
                    if (item.GetField<int>("perk_cherry") == 1)
                    {
                        item.Call("setblurforplayer", 6, 0.5f);
                        item.ShellShock( "concussion_grenade_mp", 2);
                        player.AfterDelay(2000, e =>
                        {
                            item.Call("setblurforplayer", 0, 0.3f);
                        });
                    }
                    else
                    {
                        item.Call("setblurforplayer", 6, 0.5f);
                        item.ShellShock( "concussion_grenade_mp", 5);
                        player.AfterDelay(5000, e =>
                        {
                            item.Call("setblurforplayer", 0, 0.3f);
                        });
                    }
                }
            }
            else if (player.GetField<int>("rtd_spider") == 1)
            {
                SpiderAcidArea(player, player.Origin);
            }
            else if (player.GetField<int>("rtd_somker") == 1)
            {
                player.Notify("smoker");
            }
            else if (player.GetField<int>("rtd_ice") == 1)
            {
                Effects.PlayFx(Effects.smallempfx, player.Origin);
                foreach (var item in PerkColaFunction.GetClosingZombies(player))
                {
                    if (item.GetField<float>("speed") >= 1f)
                    {
                        item.SetSpeed(0.5f);
                        player.AfterDelay(5000, e => item.SetSpeed(1));
                    }
                }
            }
            else if (player.GetField<int>("rtd_mystery") == 1)
            {
                switch (Utility.Random.Next(2))
                {
                    case 0:
                        attacker.Health = 1000;
                        attacker.Notify("radius_exploed", player.Origin);
                        player.GamblerText("Incantation!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                        AfterDelay(200, () => attacker.SetMaxHealth());
                        break;
                    case 1:
                        player.Notify("isis_exploed");
                        break;
                }
            }
            else if (player.GetField<int>("rtd_tesla") == 1)
            {
                foreach (var item in GetClosingHumans(player))
                {
                    item.Notify("emp_grenaded", player);
                }
            }

            if (mod == "MOD_HEAD_SHOT")
            {
                player.Call("detachall");
            }

        }

        public override void OnPlayerDamage(Entity player, Entity inflictor, Entity attacker, int damage, int dFlags, string mod, string weapon, Vector3 point, Vector3 dir, string hitLoc)
        {
            if (mod == "MOD_SUICIDE" || attacker == null || !attacker.IsPlayer || attacker.GetTeam() == player.GetTeam())
            {
                return;
            }

            if (weapon == "nuke_mp")
            {
                return;
            }
            if (attacker.GetTeam() == "axis" && attacker.GetField<int>("rtd_tesla") == 1 && player.GetField<int>("perk_cherry") != 1 && mod == "MOD_MELEE")
            {
                player.Health = 3;
            }
            if (player.GetTeam() == "axis" && player.GetField<int>("rtd_phd") == 1 && (mod == "MOD_EXPLOSIVE" || mod == "MOD_PROJECTILE_SPLASH" || mod == "MOD_GRENADE_SPLASH"))
            {
                player.Health = 1000;
                AfterDelay(200, () => player.SetMaxHealth());
            }
        }

        private bool IsClosingPhD(Entity player)
        {
            foreach (var item in GetClosingHumans(player))
            {
                if (item.GetField<int>("perk_phd") == 1)
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

        private List<Entity> GetClosingHumans(Vector3 origin)
        {
            var list = new List<Entity>();
            foreach (var item in Utility.Players)
            {
                if (item.GetTeam() == "allies" && item.IsAlive && item.Origin.DistanceTo(origin) <= 250)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        private void SpiderAcidArea(Entity player, Vector3 origin)
        {
            bool flag = true;
            var fx = this.Call<Entity>("spawnfx", Effects.redbeaconfx, origin);
            this.Call("triggerfx", fx);
            OnInterval(100, () =>
            {
                foreach (var item in GetClosingHumans(origin))
                {
                    if (item.GetField<int>("onhitacid") == 0)
                    {
                        SpiderAcidAreaThink(item, player);
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
            if (player.HasPerkCola(PerkColaType.ELECTRIC_CHERRY))
            {
                player.FinishPlayerDamage(player, attacker, 10, 0, "MOD_EXPLOSIVE", "bomb_site_mp", player.Origin, player.Origin, "none", 0f);
            }
            else
            {
                player.FinishPlayerDamage( player, attacker, 30, 0, "MOD_EXPLOSIVE", "bomb_site_mp", player.Origin, player.Origin, "none", 0f);
            }
            player.Call("iprintlnbold", "^1You are into the Spider acid aera. Get out from here right now!");
            player.SetField("onhitacid", 1);
            BaseScript.AfterDelay(1500, () => player.SetField("onhitacid", 0));
        }

        private void SetVision(Entity player, string vision)
        {
            OnInterval(100, () =>
            {
                player.Call("visionsetnakedforplayer", vision, 1);

                if (!player.IsAlive || player.GetField<int>("rtd_vision") == 0)
                {
                    player.Call("visionsetnakedforplayer", Utility.MapName, 1);
                    return false;
                }
                return true;
            });
        }
    }
}

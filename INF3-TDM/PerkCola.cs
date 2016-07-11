using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using InfinityScript;

namespace INF3
{
    /// <summary>
    /// 指定Perk-a-Cola的类型
    /// </summary>
    public enum PerkColaType
    {
        /// <summary>
        /// 快手可乐
        /// </summary>
        SPEED_COLA = 1,
        /// <summary>
        /// 重装蛋奶酒
        /// </summary>
        JUGGERNOG = 2,
        /// <summary>
        /// 加速剂
        /// </summary>
        STAMIN_UP = 3,
        /// <summary>
        /// 子弹分裂原浆啤酒II
        /// </summary>
        DOUBLE_TAP = 5,
        /// <summary>
        /// 死亡射手鸡尾酒
        /// </summary>
        DEAD_SHOT = 6,
        /// <summary>
        /// 防爆泡泡水
        /// </summary>
        PHD = 7,
        /// <summary>
        /// 黑寡妇红酒
        /// </summary>
        WIDOW_S_WINE = 9,
        /// <summary>
        /// 秃鹫辅助药水
        /// </summary>
        VULTURE_AID = 10,
    }

    public class PerkCola : IRandom
    {
        /// <summary>
        /// 注册所有Perk-a-Cola，用于权重随机
        /// </summary>
        public static List<PerkCola> perkcolas = new List<PerkCola>
        {
            new PerkCola(PerkColaType.SPEED_COLA),
            new PerkCola(PerkColaType.JUGGERNOG),
            new PerkCola(PerkColaType.STAMIN_UP),
            new PerkCola(PerkColaType.DOUBLE_TAP),
            new PerkCola(PerkColaType.DEAD_SHOT),
            new PerkCola(PerkColaType.PHD),
            new PerkCola(PerkColaType.WIDOW_S_WINE),
            new PerkCola(PerkColaType.VULTURE_AID)
        };

        public PerkColaType Type { get; }
        public int Weight
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.SPEED_COLA:
                        return 3;
                    case PerkColaType.JUGGERNOG:
                        return 1;
                    case PerkColaType.STAMIN_UP:
                        return 2;
                    case PerkColaType.DOUBLE_TAP:
                        return 3;
                    case PerkColaType.DEAD_SHOT:
                        return 7;
                    case PerkColaType.PHD:
                        return 6;
                    case PerkColaType.WIDOW_S_WINE:
                        return 1;
                    case PerkColaType.VULTURE_AID:
                        return 7;
                    default:
                        return 0;
                }
            }
        }
        public string GetFullName()
        {
            switch (Type)
            {
                case PerkColaType.SPEED_COLA:
                    return "Speed Cola";
                case PerkColaType.JUGGERNOG:
                    return "Juggernog";
                case PerkColaType.STAMIN_UP:
                    return "Stamin-Up";
                case PerkColaType.DOUBLE_TAP:
                    return "Double Tap Root Beer II";
                case PerkColaType.DEAD_SHOT:
                    return "Deadshot Daiquiri";
                case PerkColaType.PHD:
                    return "PhD Flopper";
                case PerkColaType.WIDOW_S_WINE:
                    return "Widow's Wine";
                case PerkColaType.VULTURE_AID:
                    return "Vulture Aid Elixir";
                default:
                    return null;
            }
        }

        public string GetIcon()
        {
            switch (Type)
            {
                case PerkColaType.SPEED_COLA:
                    return "specialty_fastreload";
                case PerkColaType.JUGGERNOG:
                    return "cardicon_juggernaut_1";
                case PerkColaType.STAMIN_UP:
                    return "specialty_longersprint";
                case PerkColaType.DOUBLE_TAP:
                    return "specialty_moredamage";
                case PerkColaType.DEAD_SHOT:
                    return "cardicon_headshot";
                case PerkColaType.PHD:
                    return "specialty_blastshield";
                case PerkColaType.WIDOW_S_WINE:
                    return "cardicon_soap_bar";
                case PerkColaType.VULTURE_AID:
                    return "specialty_scavenger";
                default:
                    return null;
            }
        }
        public int GetCost()
        {
            switch (Type)
            {
                case PerkColaType.SPEED_COLA:
                    return 400;
                case PerkColaType.JUGGERNOG:
                    return 800;
                case PerkColaType.STAMIN_UP:
                    return 600;
                case PerkColaType.DOUBLE_TAP:
                    return 600;
                case PerkColaType.DEAD_SHOT:
                    return 600;
                case PerkColaType.PHD:
                    return 500;
                case PerkColaType.WIDOW_S_WINE:
                    return 1000;
                case PerkColaType.VULTURE_AID:
                    return 300;
                default:
                    return 0;
            }
        }
        public string GetDvar()
        {
            switch (Type)
            {
                case PerkColaType.SPEED_COLA:
                    return "perk_speedcola";
                case PerkColaType.JUGGERNOG:
                    return "perk_juggernog";
                case PerkColaType.STAMIN_UP:
                    return "perk_staminup";
                case PerkColaType.DOUBLE_TAP:
                    return "perk_doubletap";
                case PerkColaType.DEAD_SHOT:
                    return "perk_deadshot";
                case PerkColaType.PHD:
                    return "perk_phd";
                case PerkColaType.WIDOW_S_WINE:
                    return "perk_widow";
                case PerkColaType.VULTURE_AID:
                    return "perk_vultrue";
                default:
                    return null;
            }
        }

        public PerkCola(PerkColaType type)
        {
            Type = type;
        }

        public Action<Entity> GetFunction()
        {
            switch (Type)
            {
                case PerkColaType.SPEED_COLA:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_speedcola", 1);
                        player.SetPerk("specialty_fastreload", true, false);
                        player.SetPerk("specialty_quickswap", true, false);
                        player.SetPerk("specialty_quickdraw", true, false);
                        player.SetPerk("specialty_armorpiercing", true, false);
                    });
                case PerkColaType.JUGGERNOG:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_juggernog", 1);
                        player.SetField("oldmodel", player.GetField<string>("model"));
                        if (player.GetTeam()=="allies")
                        {
                            player.Call("setmodel", "mp_fullbody_ally_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_ally");
                        }
                        else
                        {
                            player.Call("setmodel", "mp_fullbody_opforce_juggernaut");
                            player.Call("setviewmodel", "viewhands_juggernaut_opforce");
                        }
                        player.SetField("maxhealth", 500);
                        player.Health = 500;
                    });
                case PerkColaType.STAMIN_UP:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_staminup", 1);
                        player.SetField("speed", 1.3f);
                        player.SetPerk("specialty_marathon", true, false);
                        player.SetPerk("specialty_lightweight", true, false);
                        player.SetPerk("specialty_fastmantle", true, false);
                        player.SetPerk("specialty_fastsprintrecovery", true, false);
                    });
                case PerkColaType.DOUBLE_TAP:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_doubletap", 1);
                        player.SetPerk("specialty_rof", true, false);
                        player.SetPerk("specialty_moredamage", true, false);
                    });
                case PerkColaType.DEAD_SHOT:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_deadshot", 1);
                        player.OnInterval(100, e =>
                        {
                            player.Call("recoilscaleon", 0);
                            return player.IsPlayer && player.IsAlive && player.HasPerkCola(this);
                        });
                        player.SetPerk("specialty_reducedsway", true, false);
                        player.SetPerk("specialty_bulletaccuracy", true, false);
                    });
                case PerkColaType.PHD:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_phd", 1);
                        player.SetPerk("_specialty_blastshield", true, false);
                    });
                case PerkColaType.WIDOW_S_WINE:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_widow", 1);
                    });
                case PerkColaType.VULTURE_AID:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_vultrue", 1);
                        player.SetPerk("specialty_scavenger", true, false);
                    });
                default:
                    return null;
            }
        }

        public Action<Entity> TakeFunction()
        {
            switch (Type)
            {
                case PerkColaType.SPEED_COLA:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_speedcola", 0);
                        player.DeletePerk("specialty_fastreload");
                        player.DeletePerk("specialty_quickswap");
                        player.DeletePerk("specialty_quickdraw");
                    });
                case PerkColaType.JUGGERNOG:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_juggernog", 0);
                        player.Call("setmodel", player.GetField<string>("oldmodel"));
                        player.Call("setviewmodel", "viewmodel_base_viewhands");
                        player.SetField("maxhealth", 100);
                        player.Health = 100;
                    });
                case PerkColaType.STAMIN_UP:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_staminup", 0);
                        player.SetSpeed(1f);
                        player.DeletePerk("specialty_marathon");
                        player.DeletePerk("specialty_lightweight");
                        player.DeletePerk("specialty_fastsprintrecovery");
                    });
                case PerkColaType.DOUBLE_TAP:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_doubletap", 0);
                    });
                case PerkColaType.DEAD_SHOT:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_deadshot", 0);
                        player.DeletePerk("specialty_reducedsway");
                        player.DeletePerk("specialty_bulletaccuracy");
                    });
                case PerkColaType.PHD:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_phd", 0);
                        player.DeletePerk("_specialty_blastshield");
                    });
                case PerkColaType.WIDOW_S_WINE:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_widow", 0);
                    });
                case PerkColaType.VULTURE_AID:
                    return new Action<Entity>(player =>
                    {
                        player.SetField("perk_vultrue", 0);
                        player.DeletePerk("specialty_scavenger");
                    });
                default:
                    return null;
            }
        }

        public void GiveToPlayer(Entity player)
        {
            player.SetPerkColaCount(player.PerkColasCount() + 1);
            player.AddPerkHud( player.PerkHudNoEffect(GetIcon()));
            GetFunction().DynamicInvoke(player);
        }

        public void TakePerkCola(Entity player)
        {
            TakeFunction().DynamicInvoke();
        }
    }
}

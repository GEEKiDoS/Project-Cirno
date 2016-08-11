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
        QUICK_REVIVE = 0,
        SPEED_COLA = 1,
        JUGGERNOG = 2,
        STAMIN_UP = 3,
        MULE_KICK = 4,
        DOUBLE_TAP = 5,
        DEAD_SHOT = 6,
        PHD = 7,
        ELECTRIC_CHERRY = 8,
        WIDOW_S_WINE = 9,
        VULTURE_AID = 10,
        TOMBSTONE = 11,
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
            new PerkCola(PerkColaType.MULE_KICK),
            new PerkCola(PerkColaType.DOUBLE_TAP),
            new PerkCola(PerkColaType.DEAD_SHOT),
            new PerkCola(PerkColaType.PHD),
            new PerkCola(PerkColaType.ELECTRIC_CHERRY),
            new PerkCola(PerkColaType.WIDOW_S_WINE),
            new PerkCola(PerkColaType.VULTURE_AID),
        };

        public PerkColaType Type { get; }
        public string FullName
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.QUICK_REVIVE:
                        return "Quick Revive";
                    case PerkColaType.SPEED_COLA:
                        return "Speed Cola";
                    case PerkColaType.JUGGERNOG:
                        return "Juggernog";
                    case PerkColaType.STAMIN_UP:
                        return "Stamin-Up";
                    case PerkColaType.MULE_KICK:
                        return "Mule Kick";
                    case PerkColaType.DOUBLE_TAP:
                        return "Double Tap Root Beer II";
                    case PerkColaType.DEAD_SHOT:
                        return "Deadshot Daiquiri";
                    case PerkColaType.PHD:
                        return "PhD Flopper";
                    case PerkColaType.ELECTRIC_CHERRY:
                        return "Electric Cherry";
                    case PerkColaType.WIDOW_S_WINE:
                        return "Widow's Wine";
                    case PerkColaType.VULTURE_AID:
                        return "Vulture Aid Elixir";
                    case PerkColaType.TOMBSTONE:
                        return "Tombstone Soda";
                    default:
                        return null;
                }
            }
        }
        public string HudName
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.QUICK_REVIVE:
                        return "Quick Revive";
                    case PerkColaType.SPEED_COLA:
                        return "Speed Cola";
                    case PerkColaType.JUGGERNOG:
                        return "Juggernog";
                    case PerkColaType.STAMIN_UP:
                        return "Stamin-Up";
                    case PerkColaType.MULE_KICK:
                        return "Mule Kick";
                    case PerkColaType.DOUBLE_TAP:
                        return "Double Tap II";
                    case PerkColaType.DEAD_SHOT:
                        return "Deadshot";
                    case PerkColaType.PHD:
                        return "PhD";
                    case PerkColaType.ELECTRIC_CHERRY:
                        return "Electric Cherry";
                    case PerkColaType.WIDOW_S_WINE:
                        return "Widow's Wine";
                    case PerkColaType.VULTURE_AID:
                        return "Vulture Aid";
                    case PerkColaType.TOMBSTONE:
                        return "Tombstone";
                    default:
                        return null;
                }
            }
        }
        public Vector3 HudColor
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.QUICK_REVIVE:
                        return new Vector3(0, 0.5f, 0.5f);
                    case PerkColaType.SPEED_COLA:
                        return new Vector3(0.3f, 0.9f, 0.3f);
                    case PerkColaType.JUGGERNOG:
                        return new Vector3(1, 0.3f, 0.3f);
                    case PerkColaType.STAMIN_UP:
                        return new Vector3(0.9f, 0.9f, 0.3f);
                    case PerkColaType.MULE_KICK:
                        return new Vector3(0.9f, 0.3f, 0.3f);
                    case PerkColaType.DOUBLE_TAP:
                        return new Vector3(0.9f, 0.5f, 0.2f);
                    case PerkColaType.DEAD_SHOT:
                        return new Vector3(0.1f, 0.1f, 0.1f);
                    case PerkColaType.PHD:
                        return new Vector3(0.7f, 0.5f, 1f);
                    case PerkColaType.ELECTRIC_CHERRY:
                        return new Vector3(1, 0.3f, 0.3f);
                    case PerkColaType.WIDOW_S_WINE:
                        return new Vector3(0.5f, 0.5f, 0.5f);
                    case PerkColaType.VULTURE_AID:
                        return new Vector3(0.9f, 0.5f, 0.2f);
                    case PerkColaType.TOMBSTONE:
                        return new Vector3(0.7f, 0.5f, 1);
                    default:
                        return new Vector3();
                }
            }
        }
        public string Icon
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.QUICK_REVIVE:
                        return "specialty_finalstand";
                    case PerkColaType.SPEED_COLA:
                        return "specialty_fastreload";
                    case PerkColaType.JUGGERNOG:
                        return "cardicon_juggernaut_1";
                    case PerkColaType.STAMIN_UP:
                        return "specialty_longersprint";
                    case PerkColaType.MULE_KICK:
                        return "specialty_twoprimaries";
                    case PerkColaType.DOUBLE_TAP:
                        return "specialty_moredamage";
                    case PerkColaType.DEAD_SHOT:
                        return "cardicon_headshot";
                    case PerkColaType.PHD:
                        return "specialty_blastshield";
                    case PerkColaType.ELECTRIC_CHERRY:
                        return "cardicon_cod4";
                    case PerkColaType.WIDOW_S_WINE:
                        return "cardicon_soap_bar";
                    case PerkColaType.VULTURE_AID:
                        return "specialty_scavenger";
                    case PerkColaType.TOMBSTONE:
                        return "specialty_hardline";
                    default:
                        return null;
                }
            }
        }
        public int Pay
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.QUICK_REVIVE:
                        return 500;
                    case PerkColaType.SPEED_COLA:
                        return 1500;
                    case PerkColaType.JUGGERNOG:
                        return 1200;
                    case PerkColaType.STAMIN_UP:
                        return 1700;
                    case PerkColaType.MULE_KICK:
                        return 2000;
                    case PerkColaType.DOUBLE_TAP:
                        return 1800;
                    case PerkColaType.DEAD_SHOT:
                        return 1300;
                    case PerkColaType.PHD:
                        return 1200;
                    case PerkColaType.ELECTRIC_CHERRY:
                        return 1200;
                    case PerkColaType.WIDOW_S_WINE:
                        return 2000;
                    case PerkColaType.VULTURE_AID:
                        return 1500;
                    case PerkColaType.TOMBSTONE:
                        return 1000;
                    default:
                        return 0;
                }
            }
        }
        public string Dvar
        {
            get
            {
                return QueryDvar(Type);
            }
        }

        public int Weight
        {
            get
            {
                switch (Type)
                {
                    case PerkColaType.SPEED_COLA:
                        return 3;
                    case PerkColaType.JUGGERNOG:
                        return 2;
                    case PerkColaType.STAMIN_UP:
                        return 3;
                    case PerkColaType.MULE_KICK:
                        return 3;
                    case PerkColaType.DOUBLE_TAP:
                        return 6;
                    case PerkColaType.DEAD_SHOT:
                        return 17;
                    case PerkColaType.PHD:
                        return 11;
                    case PerkColaType.ELECTRIC_CHERRY:
                        return 15;
                    case PerkColaType.WIDOW_S_WINE:
                        return 1;
                    case PerkColaType.VULTURE_AID:
                        return 12;
                    default:
                        return 0;
                }
            }
        }

        public PerkCola(PerkColaType type)
        {
            Type = type;
        }

        private void GivePerkCola(Entity player)
        {
            switch (Type)
            {
                case PerkColaType.QUICK_REVIVE:
                    player.SetField("perk_revive", 1);
                    player.SetField("hasrevive", 1);
                    break;
                case PerkColaType.SPEED_COLA:
                    player.SetField("perk_speedcola", 1);
                    player.SetPerk("specialty_fastreload", true, false);
                    player.SetPerk("specialty_quickswap", true, false);
                    player.SetPerk("specialty_quickdraw", true, false);
                    player.SetPerk("specialty_armorpiercing", true, false);
                    break;
                case PerkColaType.JUGGERNOG:
                    player.SetField("perk_juggernog", 1);
                    player.SetField("oldmodel", player.GetField<string>("model"));
                    player.Call("setmodel", "mp_fullbody_ally_juggernaut");
                    player.Call("setviewmodel", "viewhands_juggernaut_ally");
                    player.SetField("maxhealth", 200);
                    player.Health = 200;
                    break;
                case PerkColaType.STAMIN_UP:
                    player.SetField("perk_staminup", 1);
                    player.SetSpeed(1.3f);
                    player.SetPerk("specialty_marathon", true, false);
                    player.SetPerk("specialty_lightweight", true, false);
                    player.SetPerk("specialty_fastmantle", true, false);
                    player.SetPerk("specialty_fastsprintrecovery", true, false);
                    break;
                case PerkColaType.MULE_KICK:
                    player.SetField("perk_mulekick", 1);
                    player.GiveWeapon(Sharpshooter._mulekickWeapon.Code);
                    player.Call("givemaxammo", Sharpshooter._mulekickWeapon.Code);
                    break;
                case PerkColaType.DOUBLE_TAP:
                    player.SetField("perk_doubletap", 1);
                    break;
                case PerkColaType.DEAD_SHOT:
                    player.SetField("perk_deadshot", 1);
                    player.OnInterval(100, e =>
                    {
                        player.Call("recoilscaleon", 0);
                        return player.IsPlayer && player.IsAlive && player.HasPerkCola(Type);
                    });
                    player.SetPerk("specialty_reducedsway", true, false);
                    player.SetPerk("specialty_bulletaccuracy", true, false);
                    break;
                case PerkColaType.PHD:
                    player.SetField("perk_phd", 1);
                    player.SetPerk("_specialty_blastshield", true, false);
                    player.SetPerk("specialty_throwback", true, false);
                    break;
                case PerkColaType.ELECTRIC_CHERRY:
                    player.SetField("perk_cherry", 1);
                    break;
                case PerkColaType.WIDOW_S_WINE:
                    player.SetField("perk_widow", 1);
                    break;
                case PerkColaType.VULTURE_AID:
                    player.SetField("perk_vulture", 1);
                    Ammo.MaxAmmo(player);
                    player.SetPerk("specialty_scavenger", true, false);
                    break;
                case PerkColaType.TOMBSTONE:
                    player.SetField("perk_tombstone", 1);
                    player.SetField("hastombstone", 1);
                    break;
            }
        }

        private void TakePerkCola(Entity player)
        {
            switch (Type)
            {
                case PerkColaType.QUICK_REVIVE:
                    player.SetField("perk_revive", 0);
                    break;
                case PerkColaType.SPEED_COLA:
                    player.SetField("perk_speedcola", 0);
                    player.DeletePerk("specialty_fastreload");
                    player.DeletePerk("specialty_quickswap");
                    player.DeletePerk("specialty_quickdraw");
                    break;
                case PerkColaType.JUGGERNOG:
                    player.SetField("perk_juggernog", 0);
                    player.Call("setmodel", player.GetField<string>("oldmodel"));
                    player.Call("setviewmodel", "viewmodel_base_viewhands");
                    player.SetField("maxhealth", 100);
                    player.Health = 100;
                    break;
                case PerkColaType.STAMIN_UP:
                    player.SetField("perk_staminup", 0);
                    player.SetSpeed(1f);
                    player.DeletePerk("specialty_marathon");
                    player.DeletePerk("specialty_lightweight");
                    player.DeletePerk("specialty_fastsprintrecovery");
                    break;
                case PerkColaType.MULE_KICK:
                    player.SetField("perk_mulekick", 0);
                    player.GiveMaxAmmoWeapon(Sharpshooter._mulekickWeapon.Code);
                    break;
                case PerkColaType.DOUBLE_TAP:
                    player.SetField("perk_doubletap", 0);
                    break;
                case PerkColaType.DEAD_SHOT:
                    player.SetField("perk_deadshot", 0);
                    player.DeletePerk("specialty_reducedsway");
                    player.DeletePerk("specialty_bulletaccuracy");
                    break;
                case PerkColaType.PHD:
                    break;
                case PerkColaType.ELECTRIC_CHERRY:
                    player.SetField("perk_cherry", 0);
                    break;
                case PerkColaType.WIDOW_S_WINE:
                    player.SetField("perk_widow", 0);
                    break;
                case PerkColaType.VULTURE_AID:
                    player.SetField("perk_vulture", 0);
                    player.DeletePerk("specialty_scavenger");
                    break;
                case PerkColaType.TOMBSTONE:
                    player.SetField("perk_tombstone", 0);
                    break;
            }
        }

        public static string QueryDvar(PerkColaType type)
        {
            switch (type)
            {
                case PerkColaType.QUICK_REVIVE:
                    return "perk_revive";
                case PerkColaType.SPEED_COLA:
                    return "perk_speedcola";
                case PerkColaType.JUGGERNOG:
                    return "perk_juggernog";
                case PerkColaType.STAMIN_UP:
                    return "perk_staminup";
                case PerkColaType.MULE_KICK:
                    return "perk_mulekick";
                case PerkColaType.DOUBLE_TAP:
                    return "perk_doubletap";
                case PerkColaType.DEAD_SHOT:
                    return "perk_deadshot";
                case PerkColaType.PHD:
                    return "perk_phd";
                case PerkColaType.ELECTRIC_CHERRY:
                    return "perk_cherry";
                case PerkColaType.WIDOW_S_WINE:
                    return "perk_widow";
                case PerkColaType.VULTURE_AID:
                    return "perk_vulture";
                case PerkColaType.TOMBSTONE:
                    return "perk_tombstone";
                default:
                    return null;
            }
        }

        public static PerkCola GetRandomPerkCola()
        {
            var current = Utility.RandomByWeight(perkcolas);

            return current;
        }

        public void GiveToPlayer(Entity player, bool haseffect)
        {
            player.SetPerkColaCount(player.PerkColasCount() + 1);
            player.AddPerkHud(haseffect ? player.PerkHud(this) : player.PerkHudNoEffect(this));
            GivePerkCola(player);
        }

        public void TakePlayerPerkCola(Entity player)
        {
            TakePerkCola(player);
        }
    }
}

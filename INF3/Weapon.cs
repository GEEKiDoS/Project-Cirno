using System;
using System.Linq;
using InfinityScript;

namespace INF3
{
    public class Weapon
    {
        public enum WeaponType
        {
            AR,
            SMG,
            LMG,
            Snipe,
            Shotgun,
            Pistol,
            AP,
            Launcher,
            Special,
            Killstreak,
            KillstreakHandheld,
            Other,
        }

        public string BaseName { get; private set; }
        public string Attachment1 { get; private set; }
        public string Attachment2 { get; private set; }
        public int Camo { get; private set; }
        public int Reticle { get; private set; }

        public string Name
        {
            get
            {
                return GetWeaponName(BaseName);
            }
        }
        public WeaponType Type
        {
            get
            {
                return GetWeaponType(BaseName);
            }
        }
        public string Code
        {
            get
            {
                if (Type == WeaponType.Launcher || Type == WeaponType.Special || Type == WeaponType.Killstreak || Type == WeaponType.KillstreakHandheld || Type == WeaponType.Other)
                {
                    return BaseName + "_mp";
                }
                else
                {
                    return Utilities.BuildWeaponName(BaseName, Attachment1, Attachment2, Camo, Reticle);
                }
            }
        }
        public string ShortName
        {
            get
            {
                if (BaseName == "iw5_pecheneg")
                {
                    return "pkp";
                }
                if (BaseName == "iw5_sa80")
                {
                    return "l86";
                }
                if (BaseName == "iw5_l96a1")
                {
                    return "l118a";
                }
                if (BaseName == "iw5_m9")
                {
                    return "pm9";
                }
                if (BaseName == "iw5_dragunov")
                {
                    return "svd";
                }
                if (BaseName == "iw5_barrett")
                {
                    return "m82";
                }
                if (BaseName == "uav_strike_marker")
                {
                    return "nz3";
                }
                if (BaseName == "riotshield")
                {
                    return "riot";
                }
                return BaseName.Replace("iw5_", "");
            }
        }

        private static Random _rng = new Random();

        private static string[] _ar1List = new string[] { "iw5_fad", "iw5_acr", "iw5_type95", "iw5_mk14", "iw5_scar", "iw5_g36c", "iw5_cm901" };
        private static string[] _ar2List = new string[] { "iw5_m16", "iw5_m4" };
        private static string[] _ar3List = new string[] { "iw5_ak47" };
        private static string[] _autoPistolList = new string[] { "iw5_fmg9", "iw5_skorpion", "iw5_mp9", "iw5_g18" };
        private static string[] _killStreakList = new string[] { "uav_strike_marker" };
        private static string[] _launcherList = new string[] { "rpg", "iw5_smaw", "xm25", "m320", "javelin", "stinger" };
        private static string[] _lmgList = new string[] { "iw5_m60", "iw5_mk46", "iw5_pecheneg", "iw5_sa80", "iw5_mg36" };
        private static string[] _pistol1List = new string[] { "iw5_usp45", "iw5_p99", "iw5_fnfiveseven" };
        private static string[] _pistol2List = new string[] { "iw5_44magnum", "iw5_mp412", "iw5_deserteagle" };
        private static string[] _shotgunList = new string[] { "iw5_1887", "iw5_striker", "iw5_aa12", "iw5_usas12", "iw5_spas12", "iw5_ksg" };
        private static string[] _smgList = new string[] { "iw5_mp5", "iw5_m9", "iw5_p90", "iw5_pp90m1", "iw5_ump45", "iw5_mp7" };
        private static string[] _sniperList = new string[] { "iw5_dragunov", "iw5_msr", "iw5_barrett", "iw5_rsass", "iw5_as50", "iw5_l96a1" };
        private static string[] _specialList = new string[] { "gl", "riotshield", };
        private static string[] GetAr1Attachments1 = new string[] { "none", "acog", "reflex", "hybrid", "eotech", "thermal" };
        private static string[] GetAr1Attachments2 = new string[] { "none", "silencer", "m320", "shotgun", "xmags", "heartbeat" };
        private static string[] GetAr2Attachments1 = new string[] { "none", "acog", "reflex", "hybrid", "eotech", "thermal" };
        private static string[] GetAr2Attachments2 = new string[] { "none", "silencer", "gl", "shotgun", "xmags", "heartbeat" };
        private static string[] GetAr3Attachments1 = new string[] { "none", "acog", "reflex", "hybrid", "eotech", "thermal" };
        private static string[] GetAr3Attachments2 = new string[] { "none", "silencer", "gp25", "shotgun", "xmags", "heartbeat" };
        private static string[] GetAutoPistolAttachments1 = new string[] { "none", "reflex", "eotech", "akimbo" };
        private static string[] GetAutoPistolAttachments2 = new string[] { "none", "silencer02", "xmags" };
        private static string[] GetLmgAttachments1 = new string[] { "none", "acog", "reflex", "eotech", "thermal" };
        private static string[] GetLmgAttachments2 = new string[] { "none", "silencer", "grip", "rof", "xmags", "heartbeat" };
        private static string[] GetPistol1Attachments = new string[] { "none", "silencer02", "xmags", "akimbo", "tactical" };
        private static string[] GetPistol2Attachments = new string[] { "none", "akimbo", "tactical" };
        private static string[] GetShotgunAttachments1 = new string[] { "none", "reflex", "eotech" };
        private static string[] GetShotgunAttachments2 = new string[] { "none", "silencer03", "grip", "rof", "xmags" };
        private static string[] GetSmgAttachments1 = new string[] { "none", "acog", "reflex", "hamrhybrid", "eotech", "thermal" };
        private static string[] GetSmgAttachments2 = new string[] { "none", "rof", "silencer", "xmags" };
        private static string[] GetSnipeAttachments1 = new string[] { "none", "acog", "vzscope", "thermal" };
        private static string[] GetSnipeAttachments2 = new string[] { "none", "silencer03", "xmags", "heartbeat" };

        private static string[] _firstWeaponList = new string[]
        {
            "iw5_fad", "iw5_acr", "iw5_type95", "iw5_mk14", "iw5_scar", "iw5_g36c", "iw5_cm901",
            "iw5_m16", "iw5_m4",
            "iw5_ak47",
            "iw5_fmg9", "iw5_skorpion", "iw5_mp9", "iw5_g18",
            "rpg", "iw5_smaw", "xm25", "m320", "javelin", "stinger",
            "iw5_m60", "iw5_mk46", "iw5_pecheneg", "iw5_sa80", "iw5_mg36",
            "iw5_1887", "iw5_striker", "iw5_aa12", "iw5_usas12", "iw5_spas12", "iw5_ksg",
            "uav_strike_marker","gl","riotshield",
            "iw5_mp5", "iw5_m9", "iw5_p90", "iw5_pp90m1", "iw5_ump45", "iw5_mp7",
            "iw5_dragunov", "iw5_msr", "iw5_barrett", "iw5_rsass", "iw5_as50", "iw5_l96a1"
        };

        private static string[] _secondWeaponList = new string[]
        {
            "iw5_usp45", "iw5_p99", "iw5_fnfiveseven",
            "iw5_44magnum", "iw5_mp412", "iw5_deserteagle",
        };

        private Weapon()
        {

        }

        private void GetRandomAttachments(string baseWeapon, out string attachment1, out string attachment2)
        {
            string str = "none";
            string str2 = "none";

            if (_pistol1List.Contains(baseWeapon))
            {
                str = GetPistol1Attachments[_rng.Next(0, GetPistol1Attachments.Length)];
                str2 = "none";
            }
            else if (_pistol2List.Contains(baseWeapon))
            {
                str = GetPistol2Attachments[_rng.Next(0, GetPistol2Attachments.Length)];
                str2 = "none";
            }
            else if (_autoPistolList.Contains(baseWeapon))
            {
                str = GetAutoPistolAttachments1[_rng.Next(0, GetAutoPistolAttachments1.Length)];
                str2 = GetAutoPistolAttachments2[_rng.Next(0, GetAutoPistolAttachments2.Length)];
            }
            else if (_smgList.Contains(baseWeapon))
            {
                str = GetSmgAttachments1[_rng.Next(0, GetSmgAttachments1.Length)];
                str2 = GetSmgAttachments2[_rng.Next(0, GetSmgAttachments2.Length)];
            }
            else if (_ar1List.Contains(baseWeapon))
            {
                str = GetAr1Attachments1[_rng.Next(0, GetAr1Attachments1.Length)];
                str2 = GetAr1Attachments2[_rng.Next(0, GetAr1Attachments2.Length)];
            }
            else if (_ar2List.Contains(baseWeapon))
            {
                str = GetAr2Attachments1[_rng.Next(0, GetAr2Attachments1.Length)];
                str2 = GetAr2Attachments2[_rng.Next(0, GetAr2Attachments2.Length)];
            }
            else if (_ar3List.Contains(baseWeapon))
            {
                str = GetAr3Attachments1[_rng.Next(0, GetAr3Attachments1.Length)];
                str2 = GetAr3Attachments2[_rng.Next(0, GetAr3Attachments2.Length)];
            }
            else if (_lmgList.Contains(baseWeapon))
            {
                str = GetLmgAttachments1[_rng.Next(0, GetLmgAttachments1.Length)];
                str2 = GetLmgAttachments2[_rng.Next(0, GetLmgAttachments2.Length)];
            }
            else if (_sniperList.Contains(baseWeapon))
            {
                str = GetSnipeAttachments1[_rng.Next(0, GetSnipeAttachments1.Length)];
                str2 = GetSnipeAttachments2[_rng.Next(0, GetSnipeAttachments2.Length)];
            }
            else if (_shotgunList.Contains(baseWeapon) && !baseWeapon.Contains("1887"))
            {
                str = GetShotgunAttachments1[_rng.Next(0, GetShotgunAttachments1.Length)];
                str2 = GetShotgunAttachments2[_rng.Next(0, GetShotgunAttachments2.Length)];
            }

            if (str == "hybrid" && (str2 == "gl" || str2 == "m320" || str2 == "gp25" || str2 == "shotgun"))
            {
                str2 = "none";
            }

            attachment1 = str;
            attachment2 = str2;
        }

        private int GetRandomCamo() => _rng.Next(14);

        private int GetRandomReticle() => _rng.Next(7);

        private string GetWeaponName(string text)
        {
            switch (text)
            {
                case "iw5_g18": return "G18";
                case "iw5_as50": return "AS50";
                case "javelin": return "Javelin";
                case "iw5_m4": return "M4A1";
                case "iw5_m60": return "M60E4";
                case "iw5_deserteagle": return "Desert Eagle";
                case "iw5_m16": return "M16A4";
                case "rpg": return "RPG-7";
                case "iw5_m9": return "PM-9";
                case "iw5_cm901": return "CM901";
                case "iw5_1887": return "Model 1887";
                case "iw5_ump45": return "UMP45";
                case "iw5_scar": return "SCAR-L";
                case "iw5_striker": return "Striker";
                case "iw5_pecheneg": return "PKP Pecheneg";
                case "iw5_mk46": return "MK46";
                case "iw5_acr": return "ACR 6.8";
                case "iw5_ak47": return "AK-47";
                case "iw5_sa80": return "L86 LSW";
                case "iw5_m60jugg": return "^2AUG HBAR";
                case "iw5_fad": return "FAD";
                case "iw5_g36c": return "G36C";
                case "iw5_msr": return "MSR";
                case "iw5_usas12": return "USAS-12";
                case "iw5_smaw": return "SMAW";
                case "stinger": return "^3DL Stinger";
                case "iw5_skorpion": return "Skorpion";
                case "iw5_ksg": return "KSG";
                case "m320": return "M320";
                case "iw5_44magnum": return ".44 Magnum";
                case "iw5_fnfiveseven": return "Fiveseven";
                case "iw5_mp9": return "MP9";
                case "iw5_l96a1": return "L118A";
                case "iw5_usp45": return "USP .45";
                case "iw5_spas12": return "SPAS-12";
                case "iw5_mp5": return "MP5";
                case "iw5_mg36": return "MG36";
                case "iw5_mp7": return "MP7";
                case "iw5_p99": return "P99";
                case "iw5_rsass": return "RSASS";
                case "iw5_mk14": return "MK14";
                case "iw5_p90": return "P90";
                case "iw5_mp412": return "MP412";
                case "iw5_pp90m1": return "PP90M1";
                case "xm25": return "XM25";
                case "iw5_barrett": return "Barrett .50 Cal";
                case "uav_strike_marker": return "^3NZ3 Box Launcher";
                case "iw5_fmg9": return "FMG9";
                case "iw5_type95": return "Type 95";
                case "riotshield": return "Riotshield";
                case "iw5_aa12": return "AA-12";
                case "iw5_dragunov": return "Dragunov";
                case "gl": return "M203";
                case "ac130_105mm": return "AC130 105MM Cannon";
                case "ac130_40mm": return "AC130 40MM Cannon";
                case "ac130_25mm": return "AC130 25MM Minigun";
                case "nuke": return "M.O.A.B.";
                default: return "Other Weapon";
            }
        }

        private WeaponType GetWeaponType(string baseWeapon)
        {
            if (_pistol1List.Contains(baseWeapon))
            {
                return WeaponType.Pistol;
            }
            else if (_pistol2List.Contains(baseWeapon))
            {
                return WeaponType.Pistol;
            }
            else if (_autoPistolList.Contains(baseWeapon))
            {
                return WeaponType.AP;
            }
            else if (_smgList.Contains(baseWeapon))
            {
                return WeaponType.SMG;
            }
            else if (_ar1List.Contains(baseWeapon))
            {
                return WeaponType.AR;
            }
            else if (_ar2List.Contains(baseWeapon))
            {
                return WeaponType.AR;
            }
            else if (_ar3List.Contains(baseWeapon))
            {
                return WeaponType.AR;
            }
            else if (_lmgList.Contains(baseWeapon))
            {
                return WeaponType.LMG;
            }
            else if (_sniperList.Contains(baseWeapon))
            {
                return WeaponType.Snipe;
            }
            else if (_shotgunList.Contains(baseWeapon))
            {
                return WeaponType.Shotgun;
            }
            else if (_launcherList.Contains(baseWeapon))
            {
                return WeaponType.Launcher;
            }
            else if (_specialList.Contains(baseWeapon))
            {
                return WeaponType.Special;
            }
            else if (_killStreakList.Contains(baseWeapon))
            {
                return WeaponType.Killstreak;
            }
            else
            {
                return WeaponType.Other;
            }
        }

        public static Weapon GetRandomFirstWeapon()
        {
            string baseweapon = _firstWeaponList[_rng.Next(_firstWeaponList.Length)];
            string att1;
            string att2;

            Weapon weapon = new Weapon();

            weapon.GetRandomAttachments(baseweapon, out att1, out att2);

            weapon.BaseName = baseweapon;
            weapon.Attachment1 = att1;
            weapon.Attachment2 = att2;
            weapon.Camo = weapon.GetRandomCamo();
            weapon.Reticle = weapon.GetRandomReticle();

            return weapon;
        }

        public static Weapon GetRandomSecondWeapon()
        {
            string baseweapon = _secondWeaponList[_rng.Next(_secondWeaponList.Length)];
            string att1;
            string att2;

            Weapon weapon = new Weapon();

            weapon.GetRandomAttachments(baseweapon, out att1, out att2);

            weapon.BaseName = baseweapon;
            weapon.Attachment1 = att1;
            weapon.Attachment2 = att2;
            weapon.Camo = weapon.GetRandomCamo();
            weapon.Reticle = weapon.GetRandomReticle();

            return weapon;
        }

        public static bool IsKillstreakWeapon(string weaponcode)
        {
            return false;
        }

        public override string ToString()
        {
            return Code;
        }
    }
}

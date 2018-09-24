using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public static class Sound
    {
        public static readonly string BombExploedSound = "exp_suitcase_bomb_main";
        public static readonly string ExploedMineSound = "explo_mine";
        public static readonly string CrashSound = "cobra_helicopter_crash";
        public static readonly string AmmoCrateSound = "ammo_crate_use";

        public static string AxisSoundAlias()
        {
            string prefix = string.Empty;
            Function.SetEntRef(-1);
            string mapname = Function.Call<string>("getdvar", "mapname");
            if (mapname == "mp_alpha")
                prefix = "RU_";
            else if (mapname == "mp_bootleg")
                prefix = "RU_";
            else if (mapname == "mp_bravo")
                prefix = "AF_";
            else if (mapname == "mp_carbon")
                prefix = "AF_";
            else if (mapname == "mp_dome")
                prefix = "RU_";
            else if (mapname == "mp_exchange")
                prefix = "RU_";
            else if (mapname == "mp_hardhat")
                prefix = "RU_";
            else if (mapname == "mp_interchange")
                prefix = "RU_";
            else if (mapname == "mp_lambeth")
                prefix = "RU_";
            else if (mapname == "mp_mogadishu")
                prefix = "AF_";
            else if (mapname == "mp_paris")
                prefix = "RU_";
            else if (mapname == "mp_plaza2")
                prefix = "IC_";
            else if (mapname == "mp_seatown")
                prefix = "IC_";
            else if (mapname == "mp_radar")
                prefix = "RU_";
            else if (mapname == "mp_underground")
                prefix = "RU_";
            else if (mapname == "mp_village")
                prefix = "RU_";
            else if (mapname == "mp_cement")
                prefix = "RU_";
            else if (mapname == "mp_italy")
                prefix = "IC_";
            else if (mapname == "mp_meteora")
                prefix = "IC_";
            else if (mapname == "mp_morningwood")
                prefix = "RU_";
            else if (mapname == "mp_overwatch")
                prefix = "RU_";
            else if (mapname == "mp_park")
                prefix = "RU_";
            else if (mapname == "mp_qadeem")
                prefix = "IC_";
            else if (mapname == "mp_boardwalk")
                prefix = "RU_";
            else if (mapname == "mp_moab")
                prefix = "RU_";
            else if (mapname == "mp_nola")
                prefix = "RU_";
            else if (mapname == "mp_roughneck")
                prefix = "RU_";
            else if (mapname == "mp_shipbreaker")
                prefix = "AF_";
            else if (mapname == "mp_terminal_cls")
                prefix = "RU_";
            else if (mapname == "mp_aground_ss")
                prefix = "IC_";
            else if (mapname == "mp_courtyard_ss")
                prefix = "IC_";
            else if (mapname == "mp_hillside_ss")
                prefix = "IC_";
            else if (mapname == "mp_restrepo_ss")
                prefix = "RU_";
            else if (mapname == "mp_crosswalk_ss")
                prefix = "RU_";
            else if (mapname == "mp_burn_ss")
                prefix = "RU_";
            else if (mapname == "mp_six_ss")
                prefix = "IC_";
            else
                prefix = "RU_";
            return prefix;
        }
        public static string AlliesSoundAlias()
        {
            string prefix = string.Empty;
            Function.SetEntRef(-1);
            string mapname = Function.Call<string>("getdvar", "mapname");
            if (mapname == "mp_alpha")
                prefix = "US_";
            else if (mapname == "mp_bootleg")
                prefix = "PC_";
            else if (mapname == "mp_bravo")
                prefix = "PC_";
            else if (mapname == "mp_carbon")
                prefix = "PC_";
            else if (mapname == "mp_dome")
                prefix = "US_";
            else if (mapname == "mp_exchange")
                prefix = "US_";
            else if (mapname == "mp_hardhat")
                prefix = "US_";
            else if (mapname == "mp_interchange")
                prefix = "US_";
            else if (mapname == "mp_lambeth")
                prefix = "US_";
            else if (mapname == "mp_mogadishu")
                prefix = "PC_";
            else if (mapname == "mp_paris")
                prefix = "FR_";
            else if (mapname == "mp_plaza2")
                prefix = "FR_";
            else if (mapname == "mp_seatown")
                prefix = "UK_";
            else if (mapname == "mp_radar")
                prefix = "US_";
            else if (mapname == "mp_underground")
                prefix = "UK_";
            else if (mapname == "mp_village")
                prefix = "PC_";
            else if (mapname == "mp_cement")
                prefix = "US_";
            else if (mapname == "mp_italy")
                prefix = "FR_";
            else if (mapname == "mp_meteora")
                prefix = "UK_";
            else if (mapname == "mp_morningwood")
                prefix = "US_";
            else if (mapname == "mp_overwatch")
                prefix = "US_";
            else if (mapname == "mp_park")
                prefix = "US_";
            else if (mapname == "mp_qadeem")
                prefix = "US_";
            else if (mapname == "mp_boardwalk")
                prefix = "US_";
            else if (mapname == "mp_moab")
                prefix = "US_";
            else if (mapname == "mp_nola")
                prefix = "US_";
            else if (mapname == "mp_roughneck")
                prefix = "US_";
            else if (mapname == "mp_shipbreaker")
                prefix = "PC_";
            else if (mapname == "mp_terminal_cls")
                prefix = "US_";
            else if (mapname == "mp_aground_ss")
                prefix = "UK_";
            else if (mapname == "mp_courtyard_ss")
                prefix = "UK_";
            else if (mapname == "mp_hillside_ss")
                prefix = "US_";
            else if (mapname == "mp_restrepo_ss")
                prefix = "US_";
            else if (mapname == "mp_crosswalk_ss")
                prefix = "US_";
            else if (mapname == "mp_burn_ss")
                prefix = "US_";
            else if (mapname == "mp_six_ss")
                prefix = "US_";
            else
                prefix = "RU_";
            return prefix;
        }

        public static void PlaySound(this Entity player, string sound) => player.Call("playsound", sound);
        public static void PlayLocalSound(this Entity player, string sound) => player.Call("playlocalsound", sound);
        public static void PlaySoundAsMaster(this Entity player, string sound) => player.Call("playsoundasmaster", sound);
    }
}

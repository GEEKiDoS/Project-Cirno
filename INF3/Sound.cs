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
        public static readonly string PaySound = "earn_perk";
        public static readonly string TeleporterMusic = "new_emblem_unlocks";
        public static readonly string WeaponCycledSound = "new_perk_unlocks";
        public static readonly string PowerMusic = "new_title_unlocks";
        public static readonly string ISISExploed = "sam_earned";
        public static readonly string DoorSound = "new_weapon_unlocks";

        public static void PlaySound(this Entity player, string sound) => player.Call("playsound", sound, true);
        public static void PlayLocalSound(this Entity player, string sound) => player.Call("playlocalsound", sound, true);
        public static void PlaySoundAsMaster(this Entity player, string sound) => player.Call("playsoundasmaster", sound, true);
        public static void PlaySoundToPlayer(this Entity ent, string sound, Entity player) => ent.Call("playsoundtoplayer", sound, player);
    }
}

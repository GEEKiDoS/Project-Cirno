using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfinityScript;

namespace INF3
{
    /// <summary>
    /// 提供一个方法集，允许 INF3 与 InfinityScript 之间进行交互，此外还提供控制玩家属性和行为的拓展方法
    /// </summary>
    public static class Utility
    {
        #region Global

        /// <summary>
        /// 提供一个公共随机数生成器，此字段为只读
        /// </summary>
        public static readonly Random Random = new Random();

        /// <summary>
        /// 获取服务器的当前地图，此字段为只读
        /// </summary>
        public static readonly string MapName = CallWrapper.Call<string>("getdvar", "mapname");

        /// <summary>
        /// 获取服务器内所有玩家，并返回包含玩家实体的集合
        /// </summary>
        public static List<Entity> Players
        {
            get
            {
                var list = new List<Entity>();
                for (int i = 0; i < 18; i++)
                {
                    var ent = Entity.GetEntity(i);
                    if (ent != null && ent.IsPlayer)
                    {
                        list.Add(ent);
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// 获取 INF3 是否已开启测试模式
        /// </summary>
        public static bool TestMode
        {
            get
            {
                Function.SetEntRef(-1);
                return GetDvar<int>("inf3_allow_test") == 1;
            }
        }

        /// <summary>
        /// 获取服务器当前模式自开始运行到现在的毫秒数
        /// </summary>
        public static int Time
        {
            get
            {
                Function.SetEntRef(-1);
                return CallWrapper.Call<int>("gettime");
            }
        }

        /// <summary>
        /// 根据权重值进行随机，必须实现IRandom接口
        /// </summary>
        /// <typeparam name="T">随机返回的实例的类型</typeparam>
        /// <param name="rolls">传入的包含所有实例的集合，该实例必须实现IRandom接口</param>
        /// <returns>单个实例</returns>
        public static T RandomByWeight<T>(List<T> rolls) where T : IRandom
        {
            int sum = 0;
            foreach (var item in rolls)
            {
                if (item.Weight <= 0)
                {
                    rolls.Remove(item);
                }
                else
                {
                    sum += item.Weight;
                }
            }

            List<KeyValuePair<int, int>> wlist = new List<KeyValuePair<int, int>>();
            for (int i = 0; i < rolls.Count; i++)
            {
                int w = (rolls[i].Weight) + Random.Next(0, sum);
                wlist.Add(new KeyValuePair<int, int>(i, w));
            }

            wlist.Sort((KeyValuePair<int, int> kvp1, KeyValuePair<int, int> kvp2) => kvp2.Value - kvp1.Value);

            return rolls[wlist[0].Key];
        }

        /// <summary>
        /// 使用 InfinityScript 运行时部署一个实体，并返回这个实体
        /// </summary>
        /// <param name="spawntype">实体的类型</param>
        /// <param name="origin">部署在地图中的位置</param>
        /// <returns>返回的实体，如果实体创建失败，则返回null</returns>
        public static Entity Spawn(string spawntype, Vector3 origin)
        {
            Function.SetEntRef(-1);
            return CallWrapper.Call<Entity>("spawn", spawntype, origin);
        }

        /// <summary>
        /// 使用 InfinityScript 运行时预载指定图标，以供 HudElem 使用
        /// </summary>
        /// <param name="shader">图标代码</param>
        public static void PreCacheShader(string shader)
        {
            Function.SetEntRef(-1);
            CallWrapper.Call("PreCacheShader", shader);
        }

        /// <summary>
        /// 使用 InfinityScript 运行时预载指定模型，以供实体使用
        /// </summary>
        /// <param name="model"></param>
        public static void PreCacheModel(string model)
        {
            Function.SetEntRef(-1);
            CallWrapper.Call("PreCacheModel", model);
        }

        /// <summary>
        /// 使用 InfinityScript 运行时设置一个服务器全局变量，并为其赋值
        /// </summary>
        /// <param name="dvar">要设置的变量名称</param>
        /// <param name="value">变量值，该值类型只能是int、float、string和Vector3</param>
        public static void SetDvar(string dvar, Parameter value)
        {
            Function.SetEntRef(-1);
            CallWrapper.Call("setdvar", dvar, value);
        }

        /// <summary>
        /// 获取一个服务器全局变量的值
        /// </summary>
        /// <typeparam name="T">要获取的服务器全局变量的值的类型，只能是int、float、string和Vector3</typeparam>
        /// <param name="dvar">要获取的服务器全局变量名称</param>
        /// <returns>获取的服务器全局变量的值</returns>
        public static T GetDvar<T>(string dvar)
        {
            if (typeof(T) == typeof(int))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(CallWrapper.Call<int>("getdvarint", dvar), typeof(T));
            }
            else if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(CallWrapper.Call<float>("getdvarfloat", dvar), typeof(T));
            }
            else if (typeof(T) == typeof(Vector3))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(CallWrapper.Call<Vector3>("getdvarvector", dvar), typeof(T));
            }
            else if (typeof(T) == typeof(string))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(CallWrapper.Call<string>("getdvar", dvar), typeof(T));
            }
            else
            {
                throw new Exception("Unknown Type.");
            }
        }

        #endregion

        #region Model

        public static string GetFlagModel()
        {
            switch (MapName)
            {
                case "mp_alpha":
                case "mp_dome":
                case "mp_hardhat":
                case "mp_interchange":
                case "mp_cement":
                case "mp_crosswalk_ss":
                case "mp_hillside_ss":
                case "mp_morningwood":
                case "mp_overwatch":
                case "mp_park":
                case "mp_qadeem":
                case "mp_restrepo_ss":
                case "mp_terminal_cls":
                case "mp_roughneck":
                case "mp_boardwalk":
                case "mp_moab":
                case "mp_nola":
                case "mp_radar":
                case "mp_six_ss":
                    return "prop_flag_delta";
                case "mp_exchange":
                    return "prop_flag_american05";
                case "mp_bootleg":
                case "mp_bravo":
                case "mp_mogadishu":
                case "mp_village":
                case "mp_shipbreaker":
                    return "prop_flag_pmc";
                case "mp_paris":
                    return "prop_flag_gign";
                case "mp_plaza2":
                case "mp_aground_ss":
                case "mp_courtyard_ss":
                case "mp_italy":
                case "mp_meteora":
                case "mp_underground":
                    return "prop_flag_sas";
                case "mp_seatown":
                case "mp_carbon":
                case "mp_lambeth":
                    return "prop_flag_seal";
            }
            return "";
        }

        public static string GetViewModelEnv()
        {
            switch (MapName)
            {
                case "mp_alpha":
                case "mp_bootleg":
                case "mp_exchange":
                case "mp_hardhat":
                case "mp_interchange":
                case "mp_mogadishu":
                case "mp_paris":
                case "mp_plaza2":
                case "mp_underground":
                case "mp_cement":
                case "mp_hillside_ss":
                case "mp_overwatch":
                case "mp_terminal_cls":
                case "mp_aground_ss":
                case "mp_courtyard_ss":
                case "mp_meteora":
                case "mp_morningwood":
                case "mp_qadeem":
                case "mp_crosswalk_ss":
                case "mp_italy":
                case "mp_boardwalk":
                case "mp_roughneck":
                case "mp_nola":
                    return "urban";
                case "mp_dome":
                case "mp_restrepo_ss":
                case "mp_burn_ss":
                case "mp_seatown":
                case "mp_shipbreaker":
                case "mp_moab":
                    return "desert";
                case "mp_bravo":
                case "mp_carbon":
                case "mp_park":
                case "mp_six_ss":
                case "mp_village":
                case "mp_lambeth":
                    return "woodland";
                case "mp_radar":
                    return "arctic";
            }
            return "";
        }
        public static string GetModelEnv()
        {
            switch (MapName)
            {
                case "mp_alpha":
                case "mp_dome":
                case "mp_paris":
                case "mp_plaza2":
                case "mp_terminal_cls":
                case "mp_bootleg":
                case "mp_restrepo_ss":
                case "mp_hillside_ss":
                    return "urban";
                case "mp_exchange":
                case "mp_hardhat":
                case "mp_underground":
                case "mp_cement":
                case "mp_overwatch":
                case "mp_nola":
                case "mp_boardwalk":
                case "mp_roughneck":
                case "mp_crosswalk_ss":
                    return "air";
                case "mp_interchange":
                case "mp_lambeth":
                case "mp_six_ss":
                case "mp_moab":
                case "mp_park":
                    return "woodland";
                case "mp_radar":
                    return "arctic";
            }

            return string.Empty;
        }

        public static string[] ICMaps = new string[]
        {
                "mp_seatown",
                "mp_aground_ss",
                "mp_courtyard_ss",
                "mp_italy",
                "mp_meteora",
                "mp_morningwood",
                "mp_qadeem",
                "mp_burn_ss"
        };
        public static string[] AfricaMaps = new string[]
        {
                "mp_bravo",
                "mp_carbon",
                "mp_mogadishu",
                "mp_village",
                "mp_shipbreaker",
        };

        public static void SetZombieModel(Entity player)
        {
            if (AfricaMaps.Contains(MapName))
            {
                player.Call("setmodel", "mp_body_opforce_africa_militia_sniper");
            }
            else if (ICMaps.Contains(MapName))
            {
                player.Call("setmodel", "mp_body_opforce_henchmen_sniper");
            }
            else if (MapName != "mp_radar")
            {
                player.Call("setmodel", "mp_body_opforce_russian_" + GetModelEnv() + "_sniper");
            }

            if (AfricaMaps.Contains(MapName))
            {
                player.Call("setviewmodel", "viewhands_militia");
            }
            else if (!ICMaps.Contains(MapName))
            {
                player.Call("setviewmodel", "viewhands_op_force");
            }
        }

        public static void SetZombieSniperModel(Entity player)
        {
            if (MapName == "mp_radar")
            {
                player.Call("setmodel", "mp_body_opforce_ghillie_desert_sniper");
            }
            else
            {
                if (AfricaMaps.Contains(MapName))
                {
                    player.Call("setmodel", "mp_body_opforce_ghillie_africa_militia_sniper");
                }
                else if (MapName != "mp_radar")
                {
                    player.Call("setmodel", "mp_body_opforce_ghillie_" + GetViewModelEnv() + "_sniper");
                }
            }
            player.Call("setviewmodel", "viewhands_iw5_ghillie_" + GetViewModelEnv());
        }

        #endregion

        #region Cash System

        public static int GetCash(this Entity player)
        {
            return player.GetField<int>("aiz_cash");
        }

        public static void WinCash(this Entity player, int cash)
        {
            player.SetField("aiz_cash", player.GetField<int>("aiz_cash") + cash);
        }

        public static void PayCash(this Entity player, int cash)
        {
            player.SetField("aiz_cash", player.GetField<int>("aiz_cash") - cash);
        }

        public static void ClearCash(this Entity player)
        {
            player.SetField("aiz_cash", 0);
        }

        #endregion

        #region Print

        /// <summary>
        /// 向所有玩家发送消息，该消息将显示在玩家屏幕中间位置
        /// </summary>
        /// <param name="message">要发送的消息</param>
        public static void PrintlnBold(string message)
        {
            Function.SetEntRef(-1);
            CallWrapper.Call("iprintlnbold", message);
        }

        /// <summary>
        /// 向指定玩家发送消息，该消息将显示在玩家屏幕中间位置
        /// </summary>
        /// <param name="player">要发给的玩家</param>
        /// <param name="message">要发送的消息</param>
        public static void PrintlnBold(this Entity player, string message)
        {
            player.Call("iprintlnbold", message);
        }

        /// <summary>
        /// 向所有玩家发送消息，该消息将显示在玩家屏幕左下角位置
        /// </summary>
        /// <param name="message">要发送的消息</param>
        public static void Println(string message)
        {
            Function.SetEntRef(-1);
            CallWrapper.Call("iprintln", message);
        }

        /// <summary>
        /// 向指定玩家发送消息，该消息将显示在玩家屏幕左下角位置
        /// </summary>
        /// <param name="player">要发给的玩家</param>
        /// <param name="message">要发送的消息</param>
        public static void Println(this Entity player, string message)
        {
            player.Call("iprintln", message);
        }

        /// <summary>
        /// 在服务器内记录日志，日志将会输出在服务器控制台以及保存至服务器日志文件
        /// </summary>
        /// <param name="message">要记录的内容</param>
        public static void PrintLog(string message)
        {
            Log.Write(LogLevel.All, message);
        }

        #endregion

        #region Player Function

        /// <summary>
        /// 获取指定玩家的阵营信息
        /// </summary>
        /// <param name="player">要获取的玩家</param>
        /// <returns>玩家的阵营代码</returns>
        public static string GetTeam(this Entity player)
        {
            return player.GetField<string>("sessionteam");
        }

        /// <summary>
        /// 设置玩家的阵营信息
        /// </summary>
        /// <param name="player">要设置的玩家</param>
        /// <param name="team">要设置的阵营</param>
        public static void SetTeam(this Entity player, string team)
        {
            player.SetField("sessionteam", team);
            player.SetField("team", team);
            player.Notify("menuresponse", "team_marinesopfor", team);
        }

        /// <summary>
        /// 设置玩家的移动速度的倍数
        /// </summary>
        /// <param name="player">要设置的玩家</param>
        /// <param name="speed">倍数</param>
        public static void SetSpeed(this Entity player, float speed) => player.SetField("speed", speed);

        /// <summary>
        /// 给指定玩家指定的武器并补给该武器全部弹药
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="weapon">武器代码</param>
        public static void GiveMaxAmmoWeapon(this Entity player, string weapon)
        {
            player.GiveWeapon(weapon);
            player.GiveMaxAmmo( weapon);
        }

        /// <summary>
        /// 立即将指定玩家生命值恢复到生命值上限
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void SetMaxHealth(this Entity player) => player.Health = player.GetField<int>("maxhealth");

        /// <summary>
        /// 击杀玩家，并设置击杀标示为自杀
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void Suicide(this Entity player) => BaseScript.AfterDelay(100, () => player.Call("suicide"));

        /// <summary>
        /// 删除指定玩家的指定技能，注意该技能是游戏内技能而不是Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="perk">要删除的技能的代码</param>
        public static void DeletePerk(this Entity player, string perk) => player.Call("unsetperk", perk);

        /// <summary>
        /// 为指定玩家设置玩家特效
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="shock">特效代码</param>
        /// <param name="time">持续时间</param>
        public static void ShellShock(this Entity player, string shock, int time) => player.ShellShock( shock, time);

        #endregion

        #region Perk-a-Cola

        private const int MAX_PERKCOLAS = 11;

        /// <summary>
        /// 获取当前玩家拥有的Perk-a-Cola数量
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <returns></returns>
        public static int PerkColasCount(this Entity player)
        {
            return player.GetField<int>("aiz_perks");
        }

        /// <summary>
        /// 更新玩家Perk-a-Cola数量
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="count">更新后数量</param>
        public static void SetPerkColaCount(this Entity player, int count)
        {
            if (count > MAX_PERKCOLAS)
            {
                return;
            }
            player.SetField("aiz_perks", count);
        }

        /// <summary>
        /// 获取玩家是否拥有指定Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="perk">要查询的Perk-a-Cola的对象</param>
        /// <returns></returns>
        public static bool HasPerkCola(this Entity player, PerkColaType perk)
        {
            if (player.HasField(PerkCola.QueryDvar(perk)) && player.GetField<int>(PerkCola.QueryDvar(perk)) != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 给与指定玩家指定的Perk-a-Cola，但是不播放给与特效
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="perk">要给与的Perk-a-Cola的对象</param>
        public static void GivePerkColaNoEffect(this Entity player, PerkColaType type)
        {
            var perk = new PerkCola(type);
            perk.GiveToPlayer(player, false);
        }

        /// <summary>
        /// 删除指定玩家指定的Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="perk">要删除的Perk-a-Cola的对象</param>
        public static void RemovePerkCola(this Entity player, PerkColaType type, bool tempdisable = false)
        {
            var perk = new PerkCola(type);
            perk.TakePlayerPerkCola(player);

            if (tempdisable)
            {
                var list = player.GetPerkColaHud();
                foreach (var item in list)
                {
                    if (item.GetField("perk").As<PerkColaType>() == type)
                    {
                        item.FadeOverTime(0.5f);
                        item.Alpha = 0;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 给与指定玩家所有Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void GiveAllPerkCola(this Entity player)
        {
            if (!player.HasPerkCola((PerkColaType.QUICK_REVIVE)))
            {
                player.GivePerkColaNoEffect(PerkColaType.QUICK_REVIVE);
            }
            if (!player.HasPerkCola(PerkColaType.SPEED_COLA))
            {
                player.GivePerkColaNoEffect(PerkColaType.SPEED_COLA);
            }
            if (!player.HasPerkCola(PerkColaType.JUGGERNOG))
            {
                player.GivePerkColaNoEffect(PerkColaType.JUGGERNOG);
            }
            if (!player.HasPerkCola(PerkColaType.STAMIN_UP))
            {
                player.GivePerkColaNoEffect(PerkColaType.STAMIN_UP);
            }
            if (!player.HasPerkCola(PerkColaType.MULE_KICK))
            {
                player.GivePerkColaNoEffect(PerkColaType.MULE_KICK);
            }
            if (!player.HasPerkCola(PerkColaType.DOUBLE_TAP))
            {
                player.GivePerkColaNoEffect(PerkColaType.DOUBLE_TAP);
            }
            if (!player.HasPerkCola(PerkColaType.DEAD_SHOT))
            {
                player.GivePerkColaNoEffect(PerkColaType.DEAD_SHOT);
            }
            if (!player.HasPerkCola(PerkColaType.PHD))
            {
                player.GivePerkColaNoEffect(PerkColaType.PHD);
            }
            if (!player.HasPerkCola(PerkColaType.ELECTRIC_CHERRY))
            {
                player.GivePerkColaNoEffect(PerkColaType.ELECTRIC_CHERRY);
            }
            if (!player.HasPerkCola(PerkColaType.WIDOW_S_WINE))
            {
                player.GivePerkColaNoEffect(PerkColaType.WIDOW_S_WINE);
            }
            if (!player.HasPerkCola(PerkColaType.VULTURE_AID))
            {
                player.GivePerkColaNoEffect(PerkColaType.VULTURE_AID);
            }
        }

        /// <summary>
        /// 删除指定玩家所有Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void RemoveAllPerkCola(this Entity player)
        {
            if (player.HasPerkCola(PerkColaType.QUICK_REVIVE))
            {
                player.RemovePerkCola(PerkColaType.QUICK_REVIVE);
            }
            if (player.HasPerkCola(PerkColaType.SPEED_COLA))
            {
                player.RemovePerkCola(PerkColaType.SPEED_COLA);
            }
            if (player.HasPerkCola(PerkColaType.JUGGERNOG))
            {
                player.RemovePerkCola(PerkColaType.JUGGERNOG);
            }
            if (player.HasPerkCola(PerkColaType.STAMIN_UP))
            {
                player.RemovePerkCola(PerkColaType.STAMIN_UP);
            }
            if (player.HasPerkCola(PerkColaType.MULE_KICK))
            {
                player.RemovePerkCola(PerkColaType.MULE_KICK);
            }
            if (player.HasPerkCola(PerkColaType.DOUBLE_TAP))
            {
                player.RemovePerkCola(PerkColaType.DOUBLE_TAP);
            }
            if (player.HasPerkCola(PerkColaType.DEAD_SHOT))
            {
                player.RemovePerkCola(PerkColaType.DEAD_SHOT);
            }
            if (player.HasPerkCola(PerkColaType.PHD))
            {
                player.RemovePerkCola(PerkColaType.PHD);
            }
            if (player.HasPerkCola(PerkColaType.ELECTRIC_CHERRY))
            {
                player.RemovePerkCola(PerkColaType.ELECTRIC_CHERRY);
            }
            if (player.HasPerkCola(PerkColaType.WIDOW_S_WINE))
            {
                player.RemovePerkCola(PerkColaType.WIDOW_S_WINE);
            }
            if (player.HasPerkCola(PerkColaType.VULTURE_AID))
            {
                player.RemovePerkCola(PerkColaType.VULTURE_AID);
            }

            player.ResetPerkCola();
        }

        /// <summary>
        /// 给与玩家一个随机的Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void GiveRandomPerkCola(this Entity player, bool effect)
        {
            if (player.PerkColasCount() == MAX_PERKCOLAS)
            {
                return;
            }

            var perk = PerkCola.GetRandomPerkCola();
            if (player.HasPerkCola(perk.Type) || (perk.Type == PerkColaType.TOMBSTONE && !player.HasPerkCola(PerkColaType.QUICK_REVIVE)))
            {
                player.GiveRandomPerkCola(effect);
                return;
            }

            perk.GiveToPlayer(player, effect);
        }

        /// <summary>
        /// 重置玩家的Perk-a-Cola状态，并删除玩家的Perk-a-Cola的显示图标。通常在玩家死亡或清空Perk-a-Cola后执行
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void ResetPerkCola(this Entity player)
        {
            player.SetPerkColaCount(0);

            player.SetField("perk_revive", 0);
            player.SetField("perk_speedcola", 0);
            player.SetField("perk_juggernog", 0);
            player.SetField("perk_staminup", 0);
            player.SetField("perk_mulekick", 0);
            player.SetField("perk_doubletap", 0);
            player.SetField("perk_deadshot", 0);
            player.SetField("perk_phd", 0);
            player.SetField("perk_cherry", 0);
            player.SetField("perk_widow", 0);
            player.SetField("perk_vulture", 0);
            player.SetField("perk_tombstone", 0);

            if (player.GetPerkColaHud() != null)
            {
                foreach (var item in player.GetPerkColaHud())
                {
                    item.Destroy();
                }
            }

            player.GetPerkColaHud().Clear();
        }

        public static void ReturnPerkCola(this Entity player)
        {
            int juggernog = 0;
            int staminup = 0;
            int mulekick = 0;
            int doubletap = 0;
            int deadshot = 0;
            int phd = 0;
            int cherry = 0;
            int widow = 0;
            int vultrue = 0;

            if (player.GetField<int>("perk_juggernog") == 1)
            {
                juggernog = 1;
            }
            if (player.GetField<int>("perk_staminup") == 1)
            {
                staminup = 1;
            }
            if (player.GetField<int>("perk_mulekick") == 1)
            {
                mulekick = 1;
            }
            if (player.GetField<int>("perk_doubletap") == 1)
            {
                doubletap = 1;
            }
            if (player.GetField<int>("perk_deadshot") == 1)
            {
                deadshot = 1;
            }
            if (player.GetField<int>("perk_phd") == 1)
            {
                phd = 1;
            }
            if (player.GetField<int>("perk_cherry") == 1)
            {
                cherry = 1;
            }
            if (player.GetField<int>("perk_widow") == 1)
            {
                widow = 1;
            }
            if (player.GetField<int>("perk_vulture") == 1)
            {
                vultrue = 1;
            }

            player.ResetPerkCola();

            if (juggernog == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.JUGGERNOG);
            }
            if (staminup == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.STAMIN_UP);
            }
            if (mulekick == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.MULE_KICK);
            }
            if (doubletap == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.DOUBLE_TAP);
            }
            if (deadshot == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.DEAD_SHOT);
            }
            if (phd == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.PHD);
            }
            if (cherry == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.ELECTRIC_CHERRY);
            }
            if (widow == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.WIDOW_S_WINE);
            }
            if (vultrue == 1)
            {
                player.GivePerkColaNoEffect(PerkColaType.VULTURE_AID);
            }
        }

        #endregion

        #region Gobble Gum
        ///// <summary>
        ///// 获取玩家当前拥有的泡泡糖
        ///// </summary>
        ///// <param name="player">指定玩家</param>
        ///// <returns>泡泡糖对象</returns>
        //public static GobbleGum GetCurrentGobbleGum(this Entity player)
        //{
        //    return GobbleGumFunction._currentGobblegum[player.EntRef];
        //}

        ///// <summary>
        ///// 给与玩家指定的泡泡糖
        ///// </summary>
        ///// <param name="player">指定玩家</param>
        ///// <param name="type">要给与的泡泡糖的对象</param>
        //public static void SetCurrentGobbleGum(this Entity player, GobbleGum type)
        //{
        //    GobbleGumFunction._currentGobblegum[player.EntRef] = type;
        //}

        //public static void GiveGubbleGum(this Entity player)
        //{
        //    GobbleGumFunction.GetRandomGobbleGum(player);
        //}

        //public static void ActiveGobbleGum(this Entity player)
        //{
        //    if (player.GetCurrentGobbleGum().Type != GobbleGumType.None)
        //    {
        //        GobbleGumFunction.ActiveGobbleGum(player);
        //    }
        //}

        #endregion
    }
}

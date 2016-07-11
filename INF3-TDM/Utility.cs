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
        public static readonly Random Random = new Random(GetRandomSeed());

        /// <summary>
        /// 获取服务器的当前地图，此字段为只读
        /// </summary>
        public static readonly string MapName = Function.Call<string>("getdvar", "mapname");

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
                return Function.Call<int>("gettime");
            }
        }

        /// <summary>
        /// 随机种子
        /// </summary>
        /// <returns>种子值</returns>
        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
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
            return Function.Call<Entity>("spawn", spawntype, origin);
        }

        /// <summary>
        /// 使用 InfinityScript 运行时预载指定图标，以供 HudElem 使用
        /// </summary>
        /// <param name="shader">图标代码</param>
        public static void PreCacheShader(string shader)
        {
            Function.SetEntRef(-1);
            Function.Call("PreCacheShader", shader);
        }

        /// <summary>
        /// 使用 InfinityScript 运行时预载指定模型，以供实体使用
        /// </summary>
        /// <param name="model"></param>
        public static void PreCacheModel(string model)
        {
            Function.SetEntRef(-1);
            Function.Call("PreCacheModel", model);
        }

        /// <summary>
        /// 使用 InfinityScript 运行时设置一个服务器全局变量，并为其赋值
        /// </summary>
        /// <param name="dvar">要设置的变量名称</param>
        /// <param name="value">变量值，该值类型只能是int、float、string和Vector3</param>
        public static void SetDvar(string dvar, Parameter value)
        {
            Function.SetEntRef(-1);
            Function.Call("setdvar", dvar, value);
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
                return (T)Convert.ChangeType(Function.Call<int>("getdvarint", dvar), typeof(T));
            }
            else if (typeof(T) == typeof(float) || typeof(T) == typeof(double))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(Function.Call<float>("getdvarfloat", dvar), typeof(T));
            }
            else if (typeof(T) == typeof(Vector3))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(Function.Call<Vector3>("getdvarvector", dvar), typeof(T));
            }
            else if (typeof(T) == typeof(string))
            {
                Function.SetEntRef(-1);
                return (T)Convert.ChangeType(Function.Call<string>("getdvar", dvar), typeof(T));
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

        public static void SetAxisModel(Entity player)
        {
            if (AfricaMaps.Contains(MapName))
            {
                player.Call("setviewmodel", "viewhands_militia");
            }
            else if (!ICMaps.Contains(MapName))
            {
                player.Call("setviewmodel", "viewhands_op_force");
            }
        }

        #endregion

        #region Cash Point System

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
            Function.Call("iprintlnbold", message);
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
            Function.Call("iprintln", message);
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
            player.Suicide();
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
            player.Call("givemaxammo", weapon);
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
        public static void Suicide(this Entity player) => player.AfterDelay(100, e => player.Call("suicide"));

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
        public static void ShellShock(this Entity player, string shock, int time) => player.Call("shellshock", shock, time);

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
        public static bool HasPerkCola(this Entity player, PerkCola perk)
        {
            if (player.HasField(perk.GetDvar()) && player.GetField<int>(perk.GetDvar()) != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 给与指定玩家指定的Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="perk">要给与的Perk-a-Cola的对象</param>
        public static void GivePerkCola(this Entity player, PerkCola perk)
        {
            perk.GiveToPlayer(player);
        }

        /// <summary>
        /// 删除指定玩家指定的Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        /// <param name="perk">要删除的Perk-a-Cola的对象</param>
        public static void RemovePerkCola(this Entity player, PerkCola perk)
        {
            perk.TakePerkCola(player);
        }

        /// <summary>
        /// 给与指定玩家所有Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void GiveAllPerkCola(this Entity player)
        {
            if (!player.HasPerkCola(new PerkCola(PerkColaType.SPEED_COLA)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.SPEED_COLA));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.JUGGERNOG)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.JUGGERNOG));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.STAMIN_UP)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.STAMIN_UP));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.DOUBLE_TAP)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.DOUBLE_TAP));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.DEAD_SHOT)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.DEAD_SHOT));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.PHD)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.PHD));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.WIDOW_S_WINE)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.WIDOW_S_WINE));
            }
            if (!player.HasPerkCola(new PerkCola(PerkColaType.VULTURE_AID)))
            {
                player.GivePerkCola(new PerkCola(PerkColaType.VULTURE_AID));
            }
        }

        /// <summary>
        /// 删除指定玩家所有Perk-a-Cola
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void RemoveAllPerkCola(this Entity player)
        {
            if (player.HasPerkCola(new PerkCola(PerkColaType.SPEED_COLA)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.SPEED_COLA));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.JUGGERNOG)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.JUGGERNOG));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.STAMIN_UP)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.STAMIN_UP));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.DOUBLE_TAP)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.DOUBLE_TAP));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.DEAD_SHOT)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.DEAD_SHOT));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.PHD)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.PHD));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.WIDOW_S_WINE)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.WIDOW_S_WINE));
            }
            if (player.HasPerkCola(new PerkCola(PerkColaType.VULTURE_AID)))
            {
                player.RemovePerkCola(new PerkCola(PerkColaType.VULTURE_AID));
            }

            player.ResetPerkCola();
        }

        /// <summary>
        /// 重置玩家的Perk-a-Cola状态，并删除玩家的Perk-a-Cola的显示图标。通常在玩家死亡或清空Perk-a-Cola后执行
        /// </summary>
        /// <param name="player">指定玩家</param>
        public static void ResetPerkCola(this Entity player)
        {
            player.SetPerkColaCount(0);

            player.SetField("perk_speedcola", 0);
            player.SetField("perk_juggernog", 0);
            player.SetField("perk_staminup", 0);
            player.SetField("perk_doubletap", 0);
            player.SetField("perk_deadshot", 0);
            player.SetField("perk_phd", 0);
            player.SetField("perk_widow", 0);
            player.SetField("perk_vultrue", 0);

            if (player.GetPerkColaHud() != null)
            {
                foreach (var item in player.GetPerkColaHud())
                {
                    item.Call("destroy");
                }
            }

            player.GetPerkColaHud().Clear();
        }
        #endregion
    }
}

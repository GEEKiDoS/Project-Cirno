using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class MapEdit : BaseScript
    {
        public static Entity _airdropCollision;
        public static Entity _nullCollision;

        private static string _currentfile;

        public static bool HasPower = false;
        public static List<BoxEntity> boxents = new List<BoxEntity>();
        public static List<PerkColaType> spawnedPerkColas = new List<PerkColaType>();

        public MapEdit()
        {
            var _e = Call<Entity>("getent", "care_package", "targetname");
            _airdropCollision = Call<Entity>("getent", _e.GetField<string>("target"), "targetname");

            _nullCollision = Utility.Spawn("script_origin", new Vector3());

            PlayerConnected += player =>
            {
                player.Call("notifyonplayercommand", "triggeruse", "+activate");
                player.OnNotify("triggeruse", ent => UsableThink(ent));
                UsableHud(player);

                player.SetField("attackeddoor", 0);
                player.SetField("usingteleport", 0);
            };

            InitMapEdit();
        }

        public void InitMapEdit()
        {
            if (Directory.Exists("scripts\\inf3-maps\\" + Utility.MapName))
            {
                Directory.CreateDirectory("scripts\\inf3-maps\\" + Utility.MapName);
            }
            var files = Directory.GetFiles("scripts\\inf3-maps\\" + Utility.MapName + "\\");
            if (files.Length > 0)
            {
                _currentfile = files[Utility.Random.Next(files.Length)];
                if (File.Exists(_currentfile))
                {
                    LoadMapEdit();
                }
            }
        }

        private void UsableThink(Entity player)
        {
            foreach (var item in boxents)
            {
                if (player.IsAlive && item.Origin.DistanceTo(player.Origin) <= item.Range)
                {
                    item.Usable(player);
                }
            }
        }

        private void UsableHud(Entity player)
        {
            HudElem message = HudElem.CreateFontString(player, "big", 1.5f);
            message.SetPoint("CENTER", "CENTER", 1, 115);
            message.Alpha = 0.65f;

            HudElem perk = HudElem.NewClientHudElem(player);

            player.OnInterval(100, e =>
            {
                try
                {
                    bool flag = false;
                    bool flag2 = false;
                    foreach (var item in boxents)
                    {
                        if (player.Origin.DistanceTo(item.Origin) >= item.Range)
                        {
                            continue;
                        }
                        message.SetText(item.UsableText(player));
                        if (item.Type == BoxType.PerkColaMachine)
                        {
                            var type = ((PerkColaMachine)item).PerkCola;
                            perk.SetShader(type.GetIcon(), 15, 15);
                            perk.X = item.Origin.X;
                            perk.Y = item.Origin.Y;
                            perk.Z = item.Origin.Z + 50f;
                            perk.Call("setwaypoint", 1, 1);
                            perk.Alpha = 0.7f;
                            flag2 = true;
                        }
                        flag = true;
                    }
                    if (!flag)
                    {
                        message.SetText("");
                    }
                    if (!flag2)
                    {
                        perk.Alpha = 0f;
                    }
                }
                catch (Exception)
                {
                    message.SetText("");
                    perk.Alpha = 0;
                }

                return player.IsPlayer;
            });
        }

        public static Entity CreateLaptop(Vector3 origin)
        {
            origin.Z += 17;
            Entity laptop = Utility.Spawn("script_model", origin);
            laptop.Call("setmodel", "com_laptop_2_open");
            bool flag = true;
            laptop.OnNotify("stop_rotate", e => flag = false);
            laptop.OnInterval(100, e =>
            {
                laptop.Call("rotateyaw", -360, 7);
                return flag;
            });

            return laptop;
        }

        public void CreateRamp(Vector3 top, Vector3 bottom)
        {
            int num2 = (int)Math.Ceiling(top.DistanceTo(bottom) / 30f);
            Vector3 vector = new Vector3((top.X - bottom.X) / num2, (top.Y - bottom.Y) / num2, (top.Z - bottom.Z) / num2);
            Vector3 vector2 = Call<Vector3>("vectortoangles", top - bottom);
            Vector3 angles = new Vector3(vector2.Z, vector2.Y + 90f, vector2.X);
            for (int i = 0; i <= num2; i++)
            {
                SpawnCrate(bottom + (vector * i), angles);
            }
        }

        public void CreateElevator(Vector3 enter, Vector3 exit)
        {
            Entity flag = Utility.Spawn("script_model", enter);
            flag.Call("setmodel", Utility.GetFlagModel());
            Utility.Spawn("script_model", exit).Call("setmodel", "prop_flag_neutral");
            Hud.CreateFlagShader(enter);
            Hud.CreateObjective(enter, "compass_waypoint_target");
            OnInterval(100, () =>
            {
                foreach (Entity entity in Utility.Players)
                {
                    if (entity.Origin.DistanceTo(enter) <= 50f)
                    {
                        entity.Call("setorigin", exit);
                    }
                }
                return true;
            });
        }

        public void CreateHiddenTP(Vector3 enter, Vector3 exit)
        {
            Utility.Spawn("script_model", enter).Call("setmodel", "weapon_scavenger_grenadebag");
            Utility.Spawn("script_model", enter).Call("setmodel", "weapon_oma_pack");
            OnInterval(100, delegate
            {
                foreach (Entity entity in Utility.Players)
                {
                    if (entity.Origin.DistanceTo(enter) <= 50f)
                    {
                        entity.Call("setorigin", exit);
                    }
                }
                return true;
            });
        }

        public Entity CreateWall(Vector3 start, Vector3 end)
        {
            float num = new Vector3(start.X, start.Y, 0f).DistanceTo(new Vector3(end.X, end.Y, 0f));
            float num2 = new Vector3(0f, 0f, start.Z).DistanceTo(new Vector3(0f, 0f, end.Z));
            int num3 = (int)Math.Round(num / 55f, 0);
            int num4 = (int)Math.Round(num2 / 30f, 0);
            Vector3 v = end - start;
            Vector3 vector2 = new Vector3(v.X / num3, v.Y / num3, v.Z / num4);
            float x = vector2.X / 4f;
            float y = vector2.Y / 4f;
            Vector3 angles = Call<Vector3>("vectortoangles", v);
            angles = new Vector3(0f, angles.Y, 90f);
            Entity entity = Utility.Spawn("script_origin", new Vector3((start.X + end.X) / 2f, (start.Y + end.Y) / 2f, (start.Z + end.Z) / 2f));
            for (int i = 0; i < num4; i++)
            {
                Entity entity2 = SpawnCrate((start + new Vector3(x, y, 10f)) + (new Vector3(0f, 0f, vector2.Z) * i), angles);
                entity2.Call("enablelinkto");
                entity2.Call("linkto", entity);
                for (int j = 0; j < num3; j++)
                {
                    entity2 = SpawnCrate(((start + (new Vector3(vector2.X, vector2.Y, 0f) * j)) + new Vector3(0f, 0f, 10f)) + (new Vector3(0f, 0f, vector2.Z) * i), angles);
                    entity2.Call("enablelinkto");
                    entity2.Call("linkto", entity);
                }
                entity2 = SpawnCrate((new Vector3(end.X, end.Y, start.Z) + new Vector3(x * -1f, y * -1f, 10f)) + (new Vector3(0f, 0f, vector2.Z) * i), angles);
                entity2.Call("enablelinkto");
                entity2.Call("linkto", entity);
            }
            return entity;
        }

        public Entity CreateInvWall(Vector3 start, Vector3 end)
        {
            float num = new Vector3(start.X, start.Y, 0f).DistanceTo(new Vector3(end.X, end.Y, 0f));
            float num2 = new Vector3(0f, 0f, start.Z).DistanceTo(new Vector3(0f, 0f, end.Z));
            int num3 = (int)Math.Round(num / 55f, 0);
            int num4 = (int)Math.Round(num2 / 30f, 0);
            Vector3 v = end - start;
            Vector3 vector2 = new Vector3(v.X / num3, v.Y / num3, v.Z / num4);
            float x = vector2.X / 4f;
            float y = vector2.Y / 4f;
            Vector3 angles = Call<Vector3>("vectortoangles", v);
            angles = new Vector3(0f, angles.Y, 90f);
            Entity entity = Utility.Spawn("script_origin", new Vector3((start.X + end.X) / 2f, (start.Y + end.Y) / 2f, (start.Z + end.Z) / 2f));
            for (int i = 0; i < num4; i++)
            {
                Entity entity2 = SpawnInvCrate((start + new Vector3(x, y, 10f)) + (new Vector3(0f, 0f, vector2.Z) * i), angles);
                entity2.Call("enablelinkto");
                entity2.Call("linkto", entity);
                for (int j = 0; j < num3; j++)
                {
                    entity2 = SpawnInvCrate(((start + (new Vector3(vector2.X, vector2.Y, 0f) * j)) + new Vector3(0f, 0f, 10f)) + (new Vector3(0f, 0f, vector2.Z) * i), angles);
                    entity2.Call("enablelinkto");
                    entity2.Call("linkto", entity);
                }
                entity2 = SpawnInvCrate((new Vector3(end.X, end.Y, start.Z) + new Vector3(x * -1f, y * -1f, 10f)) + (new Vector3(0f, 0f, vector2.Z) * i), angles);
                entity2.Call("enablelinkto");
                entity2.Call("linkto", entity);
            }
            return entity;
        }

        public Entity CreateFloor(Vector3 corner1, Vector3 corner2)
        {
            float num = corner1.X - corner2.X;
            if (num < 0f)
            {
                num *= -1f;
            }
            float num2 = corner1.Y - corner2.Y;
            if (num2 < 0f)
            {
                num2 *= -1f;
            }
            int num3 = (int)Math.Round(num / 50f, 0);
            int num4 = (int)Math.Round(num2 / 30f, 0);
            Vector3 vector = corner2 - corner1;
            Vector3 vector2 = new Vector3(vector.X / num3, vector.Y / num4, 0f);
            Entity entity = Utility.Spawn("script_origin", new Vector3((corner1.X + corner2.X) / 2f, (corner1.Y + corner2.Y) / 2f, corner1.Z));
            for (int i = 0; i < num3; i++)
            {
                for (int j = 0; j < num4; j++)
                {
                    Entity entity2 = SpawnCrate((corner1 + (new Vector3(vector2.X, 0f, 0f) * i)) + (new Vector3(0f, vector2.Y, 0f) * j), new Vector3(0f, 0f, 0f));
                    entity2.Call("enablelinkto");
                    entity2.Call("linkto", entity);
                }
            }
            return entity;
        }

        public static Entity SpawnCrate(Vector3 origin, Vector3 angles)
        {
            Entity entity = Utility.Spawn("script_model", origin);
            entity.Call("setmodel", "com_plasticcase_friendly");
            entity.SetField("angles", angles);
            entity.Call("clonebrushmodeltoscriptmodel", _airdropCollision);
            return entity;
        }

        public static Entity SpawnInvCrate(Vector3 origin, Vector3 angles)
        {
            Entity entity = Utility.Spawn("script_model", origin);
            entity.SetField("angles", angles);
            entity.Call("clonebrushmodeltoscriptmodel", _airdropCollision);
            return entity;
        }

        public static Entity SpawnModel(string model, Vector3 origin, Vector3 angles)
        {
            Entity entity = Utility.Spawn("script_model", origin);
            entity.Call("setmodel", model);
            entity.SetField("angles", angles);
            return entity;
        }

        private void SpawnBox(BoxType type, params Parameter[] args)
        {
            BoxEntity ent;

            switch (type)
            {
                case BoxType.Teleporter:
                    ent = new Teleporter(args[0].As<Vector3>(), args[2].As<Vector3>(), args[1].As<Vector3>());
                    break;
                case BoxType.Trampline:
                    ent = new Trampoline(args[0].As<Vector3>(), args[1].As<Vector3>(), args[2].As<int>());
                    break;
                case BoxType.Gambler:
                    ent = new Gambler(args[0].As<Vector3>(), args[1].As<Vector3>());
                    break;
                case BoxType.PerkColaMachine:
                    ent = new PerkColaMachine(args[0].As<Vector3>(), args[1].As<Vector3>(), (PerkColaType)args[2].As<int>());
                    break;
                default:
                    throw new Exception("Unknown BoxEntity");
            }

            boxents.Add(ent);
        }

        private void LoadMapEdit()
        {
            try
            {
                StreamReader reader = new StreamReader(_currentfile);
                while (!reader.EndOfStream)
                {
                    string str = reader.ReadLine();
                    if (!str.StartsWith("//") && !str.Equals(string.Empty))
                    {
                        string[] strArray = str.Split(new char[] { ':' });
                        if (strArray.Length >= 1)
                        {
                            string str2 = strArray[0];
                            switch (str2)
                            {
                                case "crate":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        SpawnCrate(ParseVector3(strArray[0]) + new Vector3(0, 0, 5), new Vector3(0, ParseVector3(strArray[1]).Y, 0));
                                    }
                                    continue;
                                case "ramp":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        CreateRamp(ParseVector3(strArray[0]), ParseVector3(strArray[1]));
                                    }
                                    continue;
                                case "elevator":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        CreateElevator(ParseVector3(strArray[0]), ParseVector3(strArray[1]));
                                    }
                                    continue;
                                case "HiddenTP":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        CreateHiddenTP(ParseVector3(strArray[0]), ParseVector3(strArray[1]));
                                    }
                                    continue;
                                case "wall":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        CreateWall(ParseVector3(strArray[0]), ParseVector3(strArray[1]));
                                    }
                                    continue;
                                case "invwall":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        CreateInvWall(ParseVector3(strArray[0]), ParseVector3(strArray[1]));
                                    }
                                    continue;
                                case "floor":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        CreateFloor(ParseVector3(strArray[0]), ParseVector3(strArray[1]));
                                    }
                                    continue;
                                case "model":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 3)
                                    {
                                        SpawnModel(strArray[0], ParseVector3(strArray[1]), new Vector3(0, ParseVector3(strArray[2]).Y, 0));
                                    }
                                    continue;
                                case "turret":
                                    continue;
                                case "sentry":
                                    continue;
                                case "gl":
                                    continue;
                                case "sam":
                                    continue;
                                case "teleporter":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 3)
                                    {
                                        SpawnBox(BoxType.Teleporter, ParseVector3(strArray[0]) + new Vector3(0, 0, 10), new Vector3(0, ParseVector3(strArray[1]).Y, 0), ParseVector3(strArray[2]));
                                    }
                                    continue;
                                case "trampoline":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 3)
                                    {
                                        SpawnBox(BoxType.Trampline, ParseVector3(strArray[0]) + new Vector3(0, 0, 10), new Vector3(0, ParseVector3(strArray[1]).Y, 0), Convert.ToInt32(strArray[2]));
                                    }
                                    continue;
                                case "gambler":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 2)
                                    {
                                        SpawnBox(BoxType.Gambler, ParseVector3(strArray[0]) + new Vector3(0, 0, 10), new Vector3(0, ParseVector3(strArray[1]).Y, 0));
                                    }
                                    continue;
                                case "perk":
                                    strArray = strArray[1].Split(new char[] { ';' });
                                    if (strArray.Length >= 3)
                                    {
                                        SpawnBox(BoxType.PerkColaMachine, ParseVector3(strArray[0]) + new Vector3(0, 0, 10), new Vector3(0, ParseVector3(strArray[1]).Y, 0), new Parameter((PerkColaType)Convert.ToInt32(strArray[2])));
                                    }
                                    continue;
                            }
                            Print("Unknown MapEdit Entry {0}... ignoring", str2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Print("error loading mapedit for map {0}: {1}", Utility.MapName, ex.Message);
            }
        }

        private static void Print(string format, params object[] args)
        {
            Log.Write(LogLevel.All, format, args);
        }

        private static Vector3 ParseVector3(string vec3)
        {
            vec3 = vec3.Replace(" ", string.Empty);
            vec3 = vec3.Replace("(", string.Empty);
            vec3 = vec3.Replace(")", string.Empty);
            string[] strArray = vec3.Split(new char[] { ',' });
            return new Vector3(float.Parse(strArray[0]), float.Parse(strArray[1]), float.Parse(strArray[2]));
        }
    }
}

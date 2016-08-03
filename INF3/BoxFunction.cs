using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public enum BoxType
    {
        Door,
        PayDoor,
        Zipline,
        Teleporter,
        Trampline,
        Power,
        Ammo,
        Gambler,
        RandomAirstrike,
        PerkColaMachine,
        RandomPerkCola,
    }

    public abstract class BoxEntity : IUsable, IDisposable
    {
        private bool isObjective = false;
        private int objectiveId = -1;

        public virtual event Func<Entity, string> UsableText;
        public virtual event Action<Entity> UsableThink;


        public Entity Entity { get; private set; }
        public int EntRef
        {
            get
            {
                return Entity.EntRef;
            }
        }
        public Vector3 Origin
        {
            get
            {
                return Entity.Origin;
            }
        }
        public BoxType Type { get; protected set; }
        public Entity Laptop { get; protected set; }
        public string Icon { get; protected set; }
        public HudElem Shader { get; protected set; }
        public int ObjectiveId
        {
            get
            {
                return objectiveId;
            }
            protected set
            {
                if (!isObjective)
                {
                    isObjective = true;
                }
                objectiveId = value;
            }
        }
        public int Range { get; protected set; }
        public int Cost { get; protected set; }

        public BoxEntity(BoxType type, Vector3 origin, Vector3 angle, int range)
        {
            try
            {
                Type = type;
                Range = range;

                if (Type == BoxType.Door || Type == BoxType.PayDoor)
                {
                    Entity = Utility.Spawn("script_model", origin);
                    Entity.SetField("angles", angle);
                }
                else
                {
                    if (Type == BoxType.Power && MapEdit.HasPower)
                    {
                        return;
                    }
                    Entity = MapEdit.SpawnCrate(origin, angle);
                }

                MapEdit.usables.Add(this);
            }
            catch (Exception)
            {
                Dispose(false);
            }
        }

        #region ontimer
        public void OnInterval(int interval, Func<Entity, bool> function)
        {
            Entity.OnInterval(interval, function);
        }

        public void AfterDelay(int delay, Action<Entity> function)
        {
            Entity.AfterDelay(delay, function);
        }
        #endregion

        public virtual string GetUsableText(Entity player)
        {
            return UsableText(player);
        }

        public virtual void DoUsableFunc(Entity player)
        {
            UsableThink(player);
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Laptop != null)
                    {
                        Laptop.Notify("stop_rotate");
                    }
                }

                if (Entity != null)
                {
                    Entity.Call("delete");
                }
                if (Laptop != null)
                {
                    Laptop.Call("delete");
                }
                if (Shader != null)
                {
                    Shader.Call("destroy");
                }

                if (isObjective)
                {
                    Function.SetEntRef(-1);
                    Function.Call("objective_delete", ObjectiveId);
                }

                MapEdit.usables.Remove(this);

                disposedValue = true;
            }
        }

        /// <summary>
        /// 释放当前箱子实体关联的所有非托管代码，并关闭所有当前箱子实体的事件
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        public virtual int CompareTo(Vector3 player)
        {
            return (int)player.DistanceTo(Origin);
        }
    }

    public class Door : BoxEntity
    {
        public enum DoorState
        {
            Open,
            Close,
            Broken
        }

        public Vector3 OpenOrigin { get; private set; }
        public Vector3 CloseOrigin { get; private set; }
        public int Size { get; private set; }
        public int Height { get; private set; }
        public int HP { get; set; }
        public int MaxHP { get; private set; }
        public DoorState State { get; set; }

        public Door(Vector3 open, Vector3 close, Vector3 angle, int size, int height, int hp, int range) : base(BoxType.Door, open, new Vector3(), range)
        {
            double num = ((size / 2) - 0.5) * -1.0;
            for (int i = 0; i < size; i++)
            {
                Entity ent1 = MapEdit.SpawnCrate(open + new Vector3(0f, 30f, 0f) * ((float)num), new Vector3(0f, 0f, 0f));
                ent1.Call("setmodel", "com_plasticcase_enemy");
                ent1.Call("enablelinkto");
                ent1.Call("linkto", Entity);
                for (int j = 1; j < height; j++)
                {
                    Entity ent2 = MapEdit.SpawnCrate((open + new Vector3(0f, 30f, 0f) * ((float)num)) - (new Vector3(70f, 0f, 0f) * j), new Vector3(0f, 0f, 0f));
                    ent2.Call("setmodel", "com_plasticcase_enemy");
                    ent2.Call("enablelinkto");
                    ent2.Call("linkto", Entity);
                }
                num++;
            }
            Entity.SetField("angles", angle);

            OpenOrigin = open;
            CloseOrigin = close;
            Size = size;
            Height = height;
            HP = hp;
            MaxHP = hp;
            State = DoorState.Open;

            MapEdit.doors.Add(this);

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    switch (State)
                    {
                        case DoorState.Open:
                            return "Door is Open. Press ^3[{+activate}] ^7to close it. (" + HP + "/" + MaxHP + ")";
                        case DoorState.Close:
                            return "Door is Closed. Press ^3[{+activate}] ^7to open it. (" + HP + "/" + MaxHP + ")";
                        case DoorState.Broken:
                            return "^1Door is Broken.";
                    }
                }
                else if (player.GetTeam() == "axis")
                {
                    switch (State)
                    {
                        case DoorState.Open:
                            return "Door is Open.";
                        case DoorState.Close:
                            return "Continuous press ^3[{+activate}] ^7to attack the door.";
                        case DoorState.Broken:
                            return "^1Door is Broken";
                    }
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (HP > 0)
                {
                    if (player.GetTeam() == "allies")
                    {
                        if (State == DoorState.Open)
                        {
                            Close();
                        }
                        else if (State == DoorState.Close)
                        {
                            Open();
                        }
                    }
                    else if (player.GetTeam() == "axis")
                    {
                        if (State == DoorState.Close)
                        {
                            Attack(player);
                        }
                    }
                }
                else if (HP == 0 && State != DoorState.Broken)
                {
                    Open();
                    State = DoorState.Broken;
                }
            };
        }

        public void Close()
        {
            Entity.PlaySound(Sound.DoorSound);
            Entity.Call("moveto", CloseOrigin, 1);
            AfterDelay(1000, ent =>
            {
                State = DoorState.Close;
            });
        }

        public void Open()
        {
            Entity.PlaySound(Sound.DoorSound);
            Entity.Call("moveto", OpenOrigin, 1);
            AfterDelay(1000, ent =>
            {
                State = DoorState.Open;
            });
        }

        public void Attack(Entity player)
        {
            if (player.GetField<int>("attackeddoor") == 0)
            {
                int hitchance = 0;
                switch (player.Call<string>("getstance"))
                {
                    case "prone":
                        hitchance = 20;
                        break;
                    case "couch":
                        hitchance = 45;
                        break;
                    case "stand":
                        hitchance = 90;
                        break;
                    default:
                        break;
                }
                if (Utility.Random.Next(100) < hitchance)
                {
                    HP--;
                    player.PrintlnBold(HP + "/" + MaxHP);
                }
                else
                {
                    player.PrintlnBold("^1MISS");
                }
                player.SetField("attackeddoor", 1);
                player.AfterDelay(1000, (e) => player.SetField("attackeddoor", 0));
            }
        }
    }

    public class PayDoor : BoxEntity
    {
        public Vector3 OpenOrigin { get; private set; }
        public int Size { get; private set; }
        public int Height { get; private set; }
        public bool IsClose { get; private set; }

        public PayDoor(Vector3 open, Vector3 close, Vector3 angle, int size, int height, int cost, int range) : base(BoxType.PayDoor, close, new Vector3(), range)
        {
            double num = ((size / 2) - 0.5) * -1.0;
            for (int i = 0; i < size; i++)
            {
                Entity ent1 = MapEdit.SpawnCrate(close + new Vector3(0f, 30f, 0f) * ((float)num), new Vector3(0f, 0f, 0f));
                ent1.Call("setmodel", "com_plasticcase_enemy");
                ent1.Call("enablelinkto");
                ent1.Call("linkto", Entity);
                for (int j = 1; j < height; j++)
                {
                    Entity ent2 = MapEdit.SpawnCrate((close + new Vector3(0f, 30f, 0f) * ((float)num)) - (new Vector3(70f, 0f, 0f) * j), new Vector3(0f, 0f, 0f));
                    ent2.Call("setmodel", "com_plasticcase_enemy");
                    ent2.Call("enablelinkto");
                    ent2.Call("linkto", Entity);
                }
                num++;
            }
            Entity.SetField("angles", angle);

            IsClose = true;
            OpenOrigin = open;
            Size = size;
            Height = height;
            Cost = cost;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (IsClose)
                    {
                        return "Press ^3[{+activate}] ^7to cleanup barriers. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (IsClose && player.GetTeam() == "allies")
                {
                    if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        player.PlayLocalSound(Sound.PaySound);
                        Open();
                    }
                    else
                    {
                        player.Println("^1Not enough cash for cleanup barriers. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        public void Open()
        {
            Entity.PlaySound(Sound.DoorSound);
            Entity.Call("moveto", OpenOrigin, 1);
            IsClose = false;
        }
    }

    public class Zipline : BoxEntity
    {
        public Vector3 Exit { get; private set; }
        public int MoveTime { get; private set; }
        public bool IsUsing { get; private set; }

        public Zipline(Vector3 enter, Vector3 exit, Vector3 angle, int movetime) : base(BoxType.Zipline, enter, angle, 50)
        {
            Exit = exit;
            IsUsing = false;
            MoveTime = movetime;
            Icon = "hudicon_neutral";
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");

            UsableText += player =>
            {
                if (!IsUsing)
                {
                    return "Press ^3[{+activate}] ^7to use zipline.";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (!IsUsing)
                {
                    Move(player);
                }
            };
        }

        private void Move(Entity player)
        {
            var start = Origin;
            IsUsing = true;

            Entity.Call("clonebrushmodeltoscriptmodel", MapEdit._nullCollision);
            player.Call("playerlinkto", Entity);
            Entity.Call("moveto", Exit, MoveTime);
            AfterDelay(MoveTime * 1000, e =>
            {
                if (player.Call<int>("islinked") != 0)
                {
                    player.Call("unlink");
                    player.Call("setorigin", Exit);
                }
                Entity.Call("moveto", start, 1);
            });
            AfterDelay(MoveTime * 1000 + 2000, e =>
            {
                IsUsing = false;
                Entity.Call("clonebrushmodeltoscriptmodel", MapEdit._airdropCollision);
            });
        }
    }

    public class Teleporter : BoxEntity
    {
        public Vector3 Exit { get; private set; }

        public Teleporter(Vector3 origin, Vector3 exit, Vector3 angle) : base(BoxType.Teleporter, origin, angle, 50)
        {
            Exit = exit;
            Laptop = MapEdit.CreateLaptop(Origin);
            Icon = "hudicon_neutral";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 300;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("scr_aiz_power") == 0 || Utility.GetDvar<int>("scr_aiz_power") == 2)
                    {
                        return "Requires Electricity";
                    }
                    if (player.GetField<int>("usingteleport") == 0)
                    {
                        if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                        {
                            return "Press ^3[{+activate}] ^7to use teleporter. [Cost: ^2$^610^7. You have ^2$^3" + player.GetCash() + "^7]";
                        }
                        return "Press ^3[{+activate}] ^7to use teleporter. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (Utility.GetDvar<int>("scr_aiz_power") != 1) return;
                if (player.GetTeam() == "allies" && player.GetField<int>("usingteleport") == 0)
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1 && player.GetCash() >= 10)
                    {
                        player.PayCash(10);
                        Teleport(player);

                    }
                    else if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        Teleport(player);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for Teleporter. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        private void Teleport(Entity player)
        {
            player.SetField("usingteleport", 1);
            Vector3 start = player.Origin;
            player.ShellShock("frag_grenade_mp", 3);
            player.AfterDelay(2000, e =>
            {
                player.Call("shellshock", "concussion_grenade_mp", 3);
                player.Call("setorigin", Exit);
                player.PlayLocalSound(Sound.TeleporterMusic);
            });
            player.AfterDelay(32000, e =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (player.Call<int>("islinked") != 0)
                    {
                        player.Call("unlink");
                    }
                    player.ShellShock("concussion_grenade_mp", 3);
                    player.Call("setorigin", start);
                    player.SetField("usingteleport", 0);
                }
            });
        }
    }

    public class Trampoline : BoxEntity
    {
        public int Height { get; private set; }

        public Trampoline(Vector3 origin, Vector3 angle, int height) : base(BoxType.Trampline, origin, angle, 50)
        {
            Height = height;
            Icon = "cardicon_tictacboom";
            Shader = Hud.CreateShader(origin, Icon);
            ObjectiveId = Hud.CreateObjective(origin, Icon);

            UsableText += player =>
            {
                return "Press ^3[{+gostand}] ^7to boost jump.";
            };

            OnInterval(100, e =>
            {
                foreach (var player in Utility.Players)
                {
                    if (player.IsAlive && Origin.DistanceTo(player.Origin) <= Range && player.Call<int>("IsOnGround") == 1)
                    {
                        DoJump(player);
                    }
                }

                return true;
            });
        }

        public void DoJump(Entity player)
        {
            if (!player.IsAlive) return;
            var vel = player.Call<Vector3>("getvelocity");
            player.Call("setvelocity", new Vector3(vel.X, vel.Y, Height));
        }
    }

    public class Power : BoxEntity
    {
        public Power(Vector3 origin, Vector3 angle) : base(BoxType.Power, origin, angle, 50)
        {
            if (MapEdit.HasPower)
            {
                Dispose();
                return;
            }

            MapEdit.HasPower = true;
            Icon = "cardicon_bulb";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 1000;

            Utility.SetDvar("scr_aiz_power", 0);

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("scr_aiz_power") == 0)
                    {
                        return "Press ^3[{+activate}] ^7to activate the electricity. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (Utility.GetDvar<int>("scr_aiz_power") != 0) return;
                if (player.GetTeam() == "allies")
                {
                    if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        OpenPower(player);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for activate the electricity. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        private void OpenPower(Entity player)
        {
            Utility.SetDvar("scr_aiz_power", 2);

            Vector3 origin2 = Origin;
            origin2.Z += 1000f;

            Entity.Call("clonebrushmodeltoscriptmodel", MapEdit._nullCollision);
            Entity.Call("moveto", origin2, 2.3f);
            AfterDelay(2300, e =>
            {
                Effects.PlayFx(Effects.empfx, origin2);
                Entity.PlaySoundAsMaster(Sound.BombExploedSound);
                Entity.PlaySoundAsMaster(Sound.PowerMusic);
                var messages = new List<string>
                {
                    player.Name,
                    "Activated Power!",
                };
                foreach (var item in Utility.Players)
                {
                    if (item.GetTeam() == "allies")
                    {
                        item.WelcomeMessage(messages, new Vector3(1, 1, 1), new Vector3(1, 0.5f, 0.3f), 1, 0.85f);
                    }
                }
                Utility.SetDvar("scr_aiz_power", 1);
            });
            AfterDelay(2400, e => Dispose());
        }
    }

    public class Ammo : BoxEntity
    {
        public Ammo(Vector3 origin, Vector3 angle) : base(BoxType.Ammo, origin, angle, 50)
        {
            Laptop = MapEdit.CreateLaptop(Origin);
            Icon = "waypoint_ammo_friendly";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 300;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                    {
                        return "Press ^3[{+activate}] ^7to buy ammo. [Cost: ^2$^6100^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                    return "Press ^3[{+activate}] ^7to buy ammo. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1 && player.GetCash() >= 100)
                    {
                        player.PayCash(100);
                        MaxAmmo(player);
                        player.PlayLocalSound(Sound.PaySound);
                    }
                    else if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        MaxAmmo(player);
                        player.PlayLocalSound(Sound.PaySound);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for Ammo. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        public static void MaxAmmo(Entity player)
        {
            player.Call("givemaxammo", Sharpshooter._firstWeapon.Code);
            player.Call("givemaxammo", Sharpshooter._secondeWeapon.Code);

            if (player.GetField<int>("perk_mulekick") == 1)
            {
                player.Call("givemaxammo", Sharpshooter._mulekickWeapon.Code);
            }

            if (!player.HasWeapon("trophy_mp"))
            {
                player.GiveWeapon("trophy_mp");
            }
            if (!player.HasWeapon("frag_grenade_mp"))
            {
                player.GiveWeapon("frag_grenade_mp");
            }
            player.Call("setweaponammoclip", "trophy_mp", 99);
            player.Call("setweaponammoclip", "flag_grenade_mp", 99);
            player.Call("givemaxammo", "trophy_mp");
            player.Call("givemaxammo", "flag_grenade_mp");
        }
    }

    public class Gambler : BoxEntity
    {
        public bool IsUsing { get; set; }

        public Gambler(Vector3 origin, Vector3 angle) : base(BoxType.Gambler, origin, angle, 50)
        {
            IsUsing = false;
            Laptop = MapEdit.CreateLaptop(Origin);
            Icon = "cardicon_8ball";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 500;

            UsableText += player =>
            {
                if (!IsUsing)
                {
                    if (player.GetTeam() == "allies" && (!player.HasField("isgambling") || player.GetField<int>("isgambling") == 0))
                    {
                        if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                        {
                            return "Press ^3[{+activate}] ^7to gamble. [Cost: ^2$^6100^7. You have ^2$^3" + player.GetCash() + "^7]";
                        }
                        return "Press ^3[{+activate}] ^7to gamble. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                }
                return "";
            };

            UsableThink += player =>
            {
                if (IsUsing) return;
                if (!player.IsAlive) return;
                if (player.GetTeam() == "allies" && (!player.HasField("isgambling") || player.GetField<int>("isgambling") == 0))
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1 && player.GetCash() >= 100)
                    {
                        player.PayCash(100);
                        Gamble(player, false);
                        player.PlayLocalSound(Sound.PaySound);
                    }
                    else if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        Gamble(player, false);
                        player.PlayLocalSound(Sound.PaySound);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for Gambler. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        private void Gamble(Entity player, bool regamble)
        {
            if (!regamble)
            {
                IsUsing = true;
                Laptop.Call("moveto", Laptop.Origin + new Vector3(0, 0, 30), 2);
                Laptop.AfterDelay(8000, e =>
                {
                    Laptop.Call("moveto", Laptop.Origin - new Vector3(0, 0, 30), 2);
                });
                player.AfterDelay(10000, ex => IsUsing = false);
            }
            player.SetField("isgambling", 1);

            player.PrintlnBold("^2Your result will show in 10 seconds");
            player.AfterDelay(1000, e => player.PrintlnBold("^29"));
            player.AfterDelay(1000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(2000, e => player.PrintlnBold("^28"));
            player.AfterDelay(2000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(3000, e => player.PrintlnBold("^27"));
            player.AfterDelay(3000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(4000, e => player.PrintlnBold("^26"));
            player.AfterDelay(4000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(5000, e => player.PrintlnBold("^25"));
            player.AfterDelay(5000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(6000, e => player.PrintlnBold("^24"));
            player.AfterDelay(6000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(7000, e => player.PrintlnBold("^23"));
            player.AfterDelay(7000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(8000, e => player.PrintlnBold("^22"));
            player.AfterDelay(8000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(9000, e => player.PrintlnBold("^21"));
            player.AfterDelay(9000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(10000, e => GambleThink(player));
        }

        private void GambleThink(Entity player)
        {
            if (!player.IsAlive || player.GetTeam()!="allies")
            {
                return;
            }
            switch (Utility.Random.Next(26))
            {
                case 0:
                    PrintGambleInfo(player, "You win nothing.", GambleType.Bad);
                    break;
                case 1:
                    PrintGambleInfo(player, "You win $500.", GambleType.Good);
                    player.WinCash(500);
                    break;
                case 2:
                    PrintGambleInfo(player, "You win $1000.", GambleType.Good);
                    player.Notify("money", Laptop.Origin);
                    player.WinCash(1000);
                    break;
                case 3:
                    PrintGambleInfo(player, "You win $2000.", GambleType.Good);
                    player.Notify("money", Laptop.Origin);
                    player.WinCash(2000);
                    break;
                case 4:
                    PrintGambleInfo(player, "You lose $500.", GambleType.Bad);
                    player.Notify("money", player.Call<Vector3>("gettagorigin", "j_spine4"));
                    player.PayCash(500);
                    break;
                case 5:
                    PrintGambleInfo(player, "You lose all cash.", GambleType.Bad);
                    player.Notify("money", player.Call<Vector3>("gettagorigin", "j_spine4"));
                    player.ClearCash();
                    break;
                case 6:
                    PrintGambleInfo(player, "You win $10000.", GambleType.Excellent);
                    player.Notify("money", Laptop.Origin);
                    player.WinCash(10000);
                    break;
                case 7:
                    PrintGambleInfo(player, "You win $5000.", GambleType.Good);
                    player.Notify("money", Laptop.Origin);
                    player.WinCash(5000);
                    break;
                case 8:
                    player.Notify("burned_out");
                    Utility.Println(player.Name + " gambled - ^3Burned Out.");
                    break;
                case 9:
                    player.Notify("killing_time");
                    Utility.Println(player.Name + " gambled - ^3Killing Time.");
                    break;
                case 10:
                    PrintGambleInfo(player, "You live or die in 5 second.", GambleType.Bad);
                    player.AfterDelay(1000, ex => player.PrintlnBold("^15"));
                    player.AfterDelay(1000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
                    player.AfterDelay(2000, ex => player.PrintlnBold("^14"));
                    player.AfterDelay(2000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
                    player.AfterDelay(3000, ex => player.PrintlnBold("^13"));
                    player.AfterDelay(3000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
                    player.AfterDelay(4000, ex => player.PrintlnBold("^12"));
                    player.AfterDelay(4000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
                    player.AfterDelay(5000, ex => player.PrintlnBold("^11"));
                    player.AfterDelay(5000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
                    player.AfterDelay(6000, ex =>
                    {
                        switch (Utility.Random.Next(2))
                        {
                            case 0:
                                PrintGambleInfo(player, "You live!", GambleType.Good);
                                break;
                            case 1:
                                PrintGambleInfo(player, "You die!", GambleType.Bad);
                                player.Notify("self_exploed");
                                break;
                        }
                    });
                    break;
                case 11:
                    PrintGambleInfo(player, "Gambler Restart.", GambleType.Bad);
                    player.AfterDelay(1000, ex => Gamble(player, true));
                    return;
                case 12:
                    PrintGambleInfo(player, "Robbed all cash.", GambleType.Excellent);
                    int cash = 0;
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() == "allies" && item.IsAlive && item != player)
                        {
                            item.GamblerText("Player " + player.Name + " robbed you all cash.", new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                            item.Notify("money", item.Call<Vector3>("gettagorigin", "j_spine4"));
                            cash += item.GetCash();
                            item.ClearCash();
                        }
                    }
                    player.WinCash(cash);
                    break;
                case 13:
                    PrintGambleInfo(player, "Incantation", GambleType.Bad);
                    player.SetField("incantation", 1);
                    break;
                case 14:
                    PrintGambleInfo(player, "Give all human $500", GambleType.Excellent);
                    player.Notify("money", Laptop.Origin);
                    player.WinCash(500);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() == "allies" && item.IsAlive && item != player)
                        {
                            item.WinCash(500);
                            item.GamblerText("Player " + player.Name + " give you $500.", new Vector3(1, 1, 1), new Vector3(0, 1, 0), 1, 0.85f);
                        }
                    }
                    break;
                case 15:
                    PrintGambleInfo(player, "You infected.", GambleType.Bad);
                    player.Notify("self_exploed");
                    break;
                case 16:
                    PrintGambleInfo(player, "You lose all weapon.", GambleType.Bad);
                    player.TakeAllWeapons();
                    break;
                case 17:
                    PrintGambleInfo(player, "You win riotshield in your back.", GambleType.Good);
                    player.Call("attachshieldmodel", "weapon_riot_shield_mp", "tag_shield_back");
                    break;
                case 18:
                    player.Notify("double_points");
                    Utility.Println(player.Name + " gambled - ^2Double Points.");
                    break;
                case 19:
                    PrintGambleInfo(player, "You win a random Perk-a-Cola.", GambleType.Good);
                    player.GiveRandomPerkCola(false);
                    break;
                case 20:
                    PrintGambleInfo(player, "You lose all Perk-a-Cola.", GambleType.Terrible);
                    player.RemoveAllPerkCola();
                    break;
                case 21:
                    PrintGambleInfo(player, "Other humans die or you die in 5 second.", GambleType.Terrible);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() == "allies" && item.IsAlive)
                        {
                            if (player != item)
                            {
                                item.GamblerText("You die or " + player.Name + " die after 5 second.", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                            }

                            item.AfterDelay(1000, ex => item.PrintlnBold("^05"));
                            item.AfterDelay(1000, ex => item.PlayLocalSound("ui_mp_nukebomb_timer"));
                            item.AfterDelay(2000, ex => item.PrintlnBold("^04"));
                            item.AfterDelay(2000, ex => item.PlayLocalSound("ui_mp_nukebomb_timer"));
                            item.AfterDelay(3000, ex => item.PrintlnBold("^03"));
                            item.AfterDelay(3000, ex => item.PlayLocalSound("ui_mp_nukebomb_timer"));
                            item.AfterDelay(4000, ex => item.PrintlnBold("^02"));
                            item.AfterDelay(4000, ex => item.PlayLocalSound("ui_mp_nukebomb_timer"));
                            item.AfterDelay(5000, ex => item.PrintlnBold("^01"));
                            item.AfterDelay(5000, ex => item.PlayLocalSound("ui_mp_nukebomb_timer"));
                        }
                    }
                    player.AfterDelay(6000, ex =>
                    {
                        switch (Utility.Random.Next(3))
                        {
                            case 0:
                            case 1:
                                foreach (var item in Utility.Players)
                                {
                                    if (player != item && item.GetTeam() == "allies")
                                        item.GamblerText(player.Name + " die. You live!", new Vector3(1, 1, 1), new Vector3(0, 1, 0), 1, 0.85f);
                                }
                                PrintGambleInfo(player, "You die!", GambleType.Bad);
                                player.Notify("self_exploed");
                                break;
                            case 2:
                                foreach (var item in Utility.Players)
                                {
                                    if (player != item && item.GetTeam() == "allies")
                                    {
                                        item.GamblerText("You die! And " + player.Name + " live!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                                        item.Notify("self_exploed");
                                    }
                                }
                                PrintGambleInfo(player, "You live! And you win $5000!", GambleType.Excellent);
                                player.Notify("money", Laptop.Origin);
                                player.WinCash(5000);
                                break;
                        }
                    });
                    break;
                case 22:
                    PrintGambleInfo(player, "Respin Cycle in 5 second.", GambleType.Good);
                    AfterDelay(5000, e => Sharpshooter._cycleRemaining = 0);
                    break;
                case 23:
                    player.Notify("max_ammo");
                    Utility.Println(player.Name + " gambled - ^2Max Ammo.");
                    break;
                case 24:
                    PrintGambleInfo(player, "Death Machine.", GambleType.Good);
                    player.TakeAllWeapons();
                    player.GiveMaxAmmoWeapon("iw5_m60jugg_mp_eotechlmg_rof_camo08");
                    player.AfterDelay(300, e => player.SwitchToWeaponImmediate("iw5_m60jugg_mp_eotechlmg_rof_camo08"));
                    break;
                case 25:
                    PrintGambleInfo(player, "Give all humans a random Perk-a-Cola.", GambleType.Excellent);
                    foreach (var item in Utility.Players)
                    {
                        if (item.IsAlive && item.GetTeam() == "allies")
                        {
                            if (item != player)
                            {
                                item.GiveRandomPerkCola(true);
                            }
                            else
                            {
                                item.GiveRandomPerkCola(false);
                            }
                        }
                    }
                    break;
            }
            player.SetField("isgambling", 0);
        }

        private enum GambleType
        {
            Good,
            Bad,
            Excellent,
            Terrible,
        }

        private void PrintGambleInfo(Entity player, string text, GambleType type)
        {
            switch (type)
            {
                case GambleType.Good:
                    player.GamblerText(text, new Vector3(1, 1, 1), new Vector3(0, 1, 0), 1, 0.85f);
                    Utility.Println(player.Name + " gambled - ^2" + text);
                    break;
                case GambleType.Bad:
                    player.GamblerText(text, new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                    Utility.Println(player.Name + " gambled - ^1" + text);
                    break;
                case GambleType.Excellent:
                    player.GamblerText(text, new Vector3(1, 1, 1), new Vector3(1, 1, 0), 1, 0.85f);
                    Utility.Println(player.Name + " gambled - ^3" + text);
                    break;
                case GambleType.Terrible:
                    player.GamblerText(text, new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                    Utility.Println(player.Name + " gambled - ^0" + text);
                    break;
            }
        }
    }

    public class RandomAirstrike : BoxEntity
    {
        public RandomAirstrike(Vector3 origin, Vector3 angle) : base(BoxType.RandomAirstrike, origin, angle, 50)
        {
            Laptop = MapEdit.CreateLaptop(Origin);
            Icon = "cardicon_award_jets";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 1000;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                    {
                        return "Press ^3[{+activate}] ^7to buy random airstrike. [Cost: ^2$^6100^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                    return "Press ^3[{+activate}] ^7to buy random airstrike. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (player.GetTeam() == "allies")
                {
                    if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);

                    }
                    else
                    {
                        player.Println("^1Not enough cash for buy random airstrike. Need ^2$^3" + Cost);
                    }
                }
            };
        }
    }

    public class PerkColaMachine : BoxEntity
    {
        public PerkCola PerkCola { get; set; }

        public PerkColaMachine(Vector3 origin, Vector3 angle, PerkColaType perk) : base(BoxType.PerkColaMachine, origin, angle, 50)
        {
            if (MapEdit.spawnedPerkColas.Contains(perk))
            {
                Dispose();
                return;
            }
            MapEdit.spawnedPerkColas.Add(perk);

            PerkCola = new PerkCola(perk);
            Icon = PerkCola.Icon;
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = PerkCola.Pay;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if ((Utility.GetDvar<int>("scr_aiz_power") == 0 || Utility.GetDvar<int>("scr_aiz_power") == 2) && PerkCola.Type != PerkColaType.QUICK_REVIVE)
                    {
                        return "Requires Electricity";
                    }
                    return "Press ^3[{+activate}] ^7to buy " + PerkCola.FullName + ". [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (Utility.GetDvar<int>("scr_aiz_power") != 1 && PerkCola.Type != PerkColaType.QUICK_REVIVE) return;
                if (player.GetTeam() == "allies")
                {
                    if (player.GetCash() >= Cost)
                    {
                        if (player.PerkColasCount() >= 7)
                        {
                            player.Println("^1You already have 7 Perk-a-Colas.");
                            return;
                        }
                        if (player.HasPerkCola(PerkCola.Type))
                        {
                            player.Println("^1You have already bought " + PerkCola.FullName + ".");
                            return;
                        }
                        if (PerkCola.Type == PerkColaType.TOMBSTONE && !player.HasPerkCola(PerkColaType.QUICK_REVIVE))
                        {
                            player.Println("^1You need buy Quick Revive first.");
                            return;
                        }
                        player.PayCash(Cost);
                        PerkCola.GiveToPlayer(player, true);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for " + PerkCola.FullName + ". Need ^2$^3" + Cost);
                    }
                }
            };
        }
    }

    public class RandomPerkCola : BoxEntity
    {
        public RandomPerkCola(Vector3 origin, Vector3 angle) : base(BoxType.RandomPerkCola, origin, angle, 50)
        {
            Icon = "cardicon_tf141";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 1500;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("scr_aiz_power") == 0 || Utility.GetDvar<int>("scr_aiz_power") == 2)
                    {
                        return "Requires Electricity";
                    }
                    if (player.GetTeam() == "allies" && (!player.HasField("isgambling") || player.GetField<int>("isgambling") == 0))
                    {
                        if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                        {
                            return "Press ^3[{+activate}] ^7to use Wunderfizz Orb. [Cost: ^2$^6500^7. You have ^2$^3" + player.GetCash() + "^7]";
                        }
                        return "Press ^3[{+activate}] ^7to use Wunderfizz Orb. [Cost: ^2$^3" + Cost + "^7. You have ^2$^3" + player.GetCash() + "^7]";
                    }
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (Utility.GetDvar<int>("scr_aiz_power") != 1) return;
                if (player.HasField("isgambling") && player.GetField<int>("isgambling") == 1) return;
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1 && player.GetCash() >= 500)
                    {
                        if (player.PerkColasCount() >= 7)
                        {
                            player.Println("^1You already have 7 Perk-a-Colas.");
                            return;
                        }
                        player.PlayLocalSound(Sound.PaySound);
                        player.PayCash(500);
                        Gamble(player);
                    }
                    else if (player.GetCash() >= Cost)
                    {
                        if (player.PerkColasCount() >= 7)
                        {
                            player.Println("^1You already have 7 Perk-a-Colas.");
                            return;
                        }
                        player.PlayLocalSound(Sound.PaySound);
                        player.PayCash(Cost);
                        Gamble(player);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for Wunderfizz Orb. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        private void Gamble(Entity player)
        {
            player.SetField("isgambling", 1);

            player.PrintlnBold("^2Your result will show in 10 seconds");
            player.AfterDelay(1000, e => player.PrintlnBold("^29"));
            player.AfterDelay(1000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(2000, e => player.PrintlnBold("^28"));
            player.AfterDelay(2000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(3000, e => player.PrintlnBold("^27"));
            player.AfterDelay(3000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(4000, e => player.PrintlnBold("^26"));
            player.AfterDelay(4000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(5000, e => player.PrintlnBold("^25"));
            player.AfterDelay(5000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(6000, e => player.PrintlnBold("^24"));
            player.AfterDelay(6000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(7000, e => player.PrintlnBold("^23"));
            player.AfterDelay(7000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(8000, e => player.PrintlnBold("^22"));
            player.AfterDelay(8000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(9000, e => player.PrintlnBold("^21"));
            player.AfterDelay(9000, e => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            player.AfterDelay(10000, e => GambleThink(player));
        }

        private void GambleThink(Entity player)
        {
            if (!player.IsAlive || player.GetTeam() != "allies")
            {
                return;
            }
            player.SetField("isgambling", 0);
            switch (Utility.Random.Next(20))
            {
                case 19:
                    player.GamblerText("Oh no! Wunderfizz Orb is gone", new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                    Utility.Println(player.Name + " destroyed Wunderfizz Orb");
                    RandomPerkDestroy();
                    break;
                default:
                    player.GiveRandomPerkCola(true);
                    break;
            }
        }

        private void RandomPerkDestroy()
        {
            Vector3 origin2 = Origin;
            origin2.Z += 500f;

            Entity.Call("clonebrushmodeltoscriptmodel", MapEdit._nullCollision);
            Entity.Call("moveto", origin2, 1.5f);

            AfterDelay(1500, e =>
            {
                Effects.PlayFx(Effects.selfexploedfx, Origin);
                Dispose();
            });
        }
    }
}

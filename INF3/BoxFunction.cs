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
        GobbleGumMachine,
        PerkColaMachine,
        RandomPerkCola,
    }

    public abstract class BoxEntity : IUsable, IDisposable
    {
        private bool disposed = false;

        private bool isObjective = false;
        private int objectiveId = -1;

        public event Func<Entity, string> UsableText;
        public event Action<Entity> UsableThink;


        public Entity Entity { get; private set; }
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

        public string GetUsableText(Entity player)
        {
            return UsableText(player);
        }

        public void DoUsableFunc(Entity player)
        {
            UsableThink(player);
        }

        public void Dispose()
        {
            disposed = true;

            Laptop.Notify("stop_rotate");

            AfterDelay(100, e =>
            {
                Entity.Call("delete");
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
            });
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
                            return "Continuous press ^ 3[{+activate}] ^7to attack the door.";
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
            Entity.Call("moveto", CloseOrigin, 1);
            AfterDelay(1000, ent =>
            {
                State = DoorState.Close;
            });
        }

        public void Open()
        {
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
                        return "Press ^3[{+activate}] ^7to cleanup barriers. [Cost: ^2$^3" + Cost + "^7]";
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
            Cost = 500;

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
                            return "Press ^3[{+activate}] ^7to use teleporter. [Cost: ^2$^610^7]";
                        }
                        return "Press ^3[{+activate}] ^7to use teleporter. [Cost: ^2$^3" + Cost + "^7]";
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
            Cost = 700;

            Utility.SetDvar("scr_aiz_power", 0);

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("scr_aiz_power") == 0)
                    {
                        return "Press ^3[{+activate}] ^7to activate the electricity. [Cost: ^2$^3" + Cost + "^7]";
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
            Cost = 100;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                    {
                        return "Press ^3[{+activate}] ^7to buy ammo. [Cost: ^2$^610^7]";
                    }
                    return "Press ^3[{+activate}] ^7to buy ammo. [Cost: ^2$^3" + Cost + "^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (player.GetTeam() == "allies")
                {
                    if (Function.Call<int>("getdvarint", "bonus_fire_sale") == 1 && player.GetCash() >= 10)
                    {
                        player.PayCash(10);
                        MaxAmmo(player);
                        player.PlayLocalSound(Sound.AmmoCrateSound);
                    }
                    else if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        MaxAmmo(player);
                        player.PlayLocalSound(Sound.AmmoCrateSound);
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
                    if (player.GetTeam() == "allies")
                    {
                        if (Utility.GetDvar<int>("bonus_fire_sale") == 1)
                        {
                            return "Press ^3[{+activate}] ^7to gamble. [Cost: ^2$^610^7]";
                        }
                        return "Press ^3[{+activate}] ^7to gamble. [Cost: ^2$^3" + Cost + "^7]";
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
                    if (Function.Call<int>("getdvarint", "bonus_fire_sale") == 1 && player.GetCash() >= 10)
                    {
                        player.PayCash(10);
                        Gamble(player);
                    }
                    else if (player.GetCash() >= Cost)
                    {
                        player.PayCash(Cost);
                        Gamble(player);
                    }
                    else
                    {
                        player.Println("^1Not enough cash for Gambler. Need ^2$^3" + Cost);
                    }
                }
            };
        }

        private void Gamble(Entity player)
        {
            IsUsing = true;
            player.SetField("isgambling", 1);

            //if (player.GetField<int>("gobble_coagulant") == 1)
            //{
            //    Laptop.Call("moveto", Laptop.Origin + new Vector3(0, 0, 30), 1);
            //    Laptop.AfterDelay(4000, e =>
            //    {
            //        Laptop.Call("moveto", Laptop.Origin - new Vector3(0, 0, 30), 1);
            //    });

            //    player.AfterDelay(1000, ex => player.PrintlnBold("^55"));
            //    player.AfterDelay(1000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            //    player.AfterDelay(2000, ex => player.PrintlnBold("^54"));
            //    player.AfterDelay(2000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            //    player.AfterDelay(3000, ex => player.PrintlnBold("^53"));
            //    player.AfterDelay(3000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            //    player.AfterDelay(4000, ex => player.PrintlnBold("^52"));
            //    player.AfterDelay(4000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            //    player.AfterDelay(5000, ex => player.PrintlnBold("^51"));
            //    player.AfterDelay(5000, ex => player.PlayLocalSound("ui_mp_nukebomb_timer"));
            //    player.AfterDelay(6000, ex => GambleThink(player));
            //    player.AfterDelay(6000, ex => IsUsing = false);
            //}
            //else
            //{
            Laptop.Call("moveto", Laptop.Origin + new Vector3(0, 0, 30), 2);
            Laptop.AfterDelay(8000, e =>
            {
                Laptop.Call("moveto", Laptop.Origin - new Vector3(0, 0, 30), 2);
            });

            player.PrintlnBold("^210");
            player.PlayLocalSound("ui_mp_nukebomb_timer");
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
            player.AfterDelay(10000, ex => IsUsing = false);
            //}
        }

        private void GambleThink(Entity player)
        {
            //if (player.GetCurrentGobbleGum().Type == GobbleGumType.LuckyCrit)
            //{
            //    player.SetField("reset_gambler_ready", 1);
            //    player.AfterDelay(1000, e => player.SetField("reset_gambler_ready", 0));
            //}

            switch (Utility.Random.Next(22))
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
                    player.WinCash(1000);
                    break;
                case 3:
                    PrintGambleInfo(player, "You win $2000.", GambleType.Good);
                    player.WinCash(2000);
                    break;
                case 4:
                    PrintGambleInfo(player, "You lose $500.", GambleType.Bad);
                    player.PayCash(500);
                    break;
                case 5:
                    PrintGambleInfo(player, "You lose all money.", GambleType.Bad);
                    player.ClearCash();
                    break;
                case 6:
                    PrintGambleInfo(player, "You win $10000.", GambleType.Excellent);
                    player.WinCash(10000);
                    break;
                case 7:
                    PrintGambleInfo(player, "You win 10 Bonus Points.", GambleType.Good);
                    player.WinPoint(10);
                    break;
                case 8:
                    PrintGambleInfo(player, "You win 50 Bonus Points.", GambleType.Excellent);
                    player.WinPoint(50);
                    break;
                case 9:
                    PrintGambleInfo(player, "You lose all Bonus Points.", GambleType.Bad);
                    player.ClearPoint();
                    break;
                case 10:
                    PrintGambleInfo(player, "You live or die after 5 second.", GambleType.Bad);
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
                                //if (player.GetCurrentGobbleGum().Type == GobbleGumType.NoGamble)
                                //{
                                //    player.ActiveGobbleGum();
                                //}
                                //else
                                //{
                                PrintGambleInfo(player, "You die!", GambleType.Bad);
                                player.Notify("self_exploed");
                                //}
                                break;
                        }
                    });
                    break;
                case 11:
                    PrintGambleInfo(player, "Gambler Restart.", GambleType.Bad);
                    player.AfterDelay(1000, ex => Gamble(player));
                    return;
                case 12:
                    PrintGambleInfo(player, "Robbed all cash.", GambleType.Excellent);
                    int cash = 0;
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() == "allies" && item.IsAlive && item != player)
                        {
                            //if (item.GetCurrentGobbleGum().Type == GobbleGumType.Strongbox)
                            //{
                            //    item.ActiveGobbleGum();
                            //}
                            //else
                            //{
                            item.GamblerText("Player " + player.Name + " robbed you all cash.", new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                            cash += item.GetCash();
                            item.ClearCash();
                            //}
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
                    //if (player.GetCurrentGobbleGum().Type == GobbleGumType.NoGamble)
                    //{
                    //    player.ActiveGobbleGum();
                    //}
                    //else
                    //{
                    player.Notify("self_exploed");
                    //}
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
                    PrintGambleInfo(player, "Surprise!", GambleType.Terrible);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() == "allies" && item.IsAlive)
                        {
                            //if (item.GetCurrentGobbleGum().Type == GobbleGumType.Strongbox)
                            //{
                            //    item.ActiveGobbleGum();
                            //}
                            //else
                            //{
                            item.ClearCash();
                            item.ClearPoint();
                            if (player != item)
                            {
                                item.GamblerText("Surprise!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                            }
                            //}
                        }
                    }
                    break;
                case 19:
                    PrintGambleInfo(player, "You win a random Perk-a-Cola.", GambleType.Good);
                    player.GiveRandomPerkCola();
                    break;
                case 20:
                    PrintGambleInfo(player, "You lose all Perk-a-Cola.", GambleType.Terrible);
                    player.RemoveAllPerkCola();
                    break;
                case 21:
                    PrintGambleInfo(player, "Other humans die or you die after 5 second.", GambleType.Terrible);
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
                                //if (player.GetCurrentGobbleGum().Type == GobbleGumType.NoGamble)
                                //{
                                //    player.ActiveGobbleGum();
                                //}
                                //else
                                //{
                                PrintGambleInfo(player, "You die!", GambleType.Bad);
                                player.Notify("self_exploed");
                                //}
                                break;
                            case 2:
                                foreach (var item in Utility.Players)
                                {
                                    if (player != item && item.GetTeam() == "allies")
                                    {
                                        //if (item.GetCurrentGobbleGum().Type == GobbleGumType.NoGamble)
                                        //{
                                        //    item.ActiveGobbleGum();
                                        //}
                                        //else
                                        //{
                                        item.GamblerText("You die! And " + player.Name + " live!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                                        item.Notify("self_exploed");
                                        //}
                                    }
                                }
                                PrintGambleInfo(player, "You live! And you have all Perk-a-Cola!", GambleType.Excellent);
                                player.WinCash(1000);
                                //player.GiveAllPerkCola();
                                break;
                        }
                    });
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
            Cost = 10;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    return "Press ^3[{+activate}] ^7to buy random airstrike. [Cost: ^3" + Cost + " ^5bonus Points^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (player.GetTeam() == "allies")
                {
                    if (player.GetPoint() >= Cost)
                    {
                        player.PayPoint(Cost);

                    }
                    else
                    {
                        player.Println("^1Not enough ^5Bonus Points ^1for buy random airstrike. Need ^3" + Cost + " ^5Bonus Points");
                    }
                }
            };
        }
    }

    public class GobbleGumMachine : BoxEntity
    {
        public GobbleGumMachine(Vector3 origin, Vector3 angle) : base(BoxType.GobbleGumMachine, origin, angle, 50)
        {
            Laptop = MapEdit.CreateLaptop(Origin);
            Icon = "dpad_killstreak_ac130";
            Shader = Hud.CreateShader(Origin, Icon, "allies");
            ObjectiveId = Hud.CreateObjective(Origin, Icon, "allies");
            Cost = 5;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    return "Press ^3[{+activate}] ^7to buy a Gobble Gum. [Cost: ^3" + Cost + " ^5bonus Points^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (player.GetTeam() == "allies")
                {
                    if (player.GetPoint() >= Cost)
                    {
                        player.PayPoint(Cost);
                        //player.GiveGubbleGum();
                    }
                    else
                    {
                        player.Println("^1Not enough ^5Bonus Points ^1for buy Gobble Gum. Need ^3" + Cost + " ^5Bonus Points");
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
                    if (Utility.GetDvar<int>("scr_aiz_power") == 0 || Utility.GetDvar<int>("scr_aiz_power") == 2)
                    {
                        return "Requires Electricity";
                    }
                    return "Press ^3[{+activate}] ^7to buy " + PerkCola.FullName + ". [Cost: ^2$^3" + Cost + "^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (Utility.GetDvar<int>("scr_aiz_power") != 1) return;
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("bonus_fire_sale") == 1 && player.GetCash() >= 10)
                    {
                        //int max = player.GetField<int>("gobble_unquenchable") == 1 ? 6 : 5;

                        if (player.PerkColasCount() >= 5)
                        {
                            player.Println("^1You already have 5 Perk-a-Cola.");
                            return;
                        }
                        if (player.HasPerkCola(PerkCola.Type))
                        {
                            player.Println("^1You already have " + PerkCola.FullName + ".");
                            return;
                        }
                        player.PayCash(10);
                        PerkCola.GiveToPlayer(player, true);
                    }
                    else if (player.GetCash() >= Cost)
                    {
                        if (player.PerkColasCount() >= 5)
                        {
                            player.Println("^1You already have 5 Perk-a-Cola.");
                            return;
                        }
                        if (player.HasPerkCola(PerkCola.Type))
                        {
                            player.Println("^1You already have " + PerkCola.FullName + ".");
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
            Cost = 10;

            UsableText += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (Utility.GetDvar<int>("scr_aiz_power") == 0 || Utility.GetDvar<int>("scr_aiz_power") == 2)
                    {
                        return "Requires Electricity";
                    }
                    return "Press ^3[{+activate}] ^7to use Der Wunderfizz. [Cost: ^3" + Cost + " ^5Bonus Points^7]";
                }
                return "";
            };

            UsableThink += player =>
            {
                if (!player.IsAlive) return;
                if (Utility.GetDvar<int>("scr_aiz_power") != 1) return;
                if (player.GetTeam() == "allies")
                {
                    if (player.GetPoint() >= Cost)
                    {
                        if (player.PerkColasCount() >= 5)
                        {
                            player.Println("^1You already have 5 Perk-a-Cola.");
                            return;
                        }

                        player.PayPoint(Cost);
                        player.GiveRandomPerkCola();
                    }
                    else
                    {
                        player.Println("^1Not enough ^5Bonus Points ^1for Der Wunderfizz. Need ^3" + Cost + " ^5Bonus Points");
                    }
                }
            };

        }
    }
}

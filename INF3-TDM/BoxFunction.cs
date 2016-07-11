using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public enum BoxType
    {
        Teleporter,
        Trampline,
        Gambler,
        PerkColaMachine,
    }

    public abstract class BoxEntity : IDisposable
    {
        private bool disposedValue = false;

        private bool isObjective = false;
        private int objectiveId = -1;

        protected delegate string TriggerString(Entity player);
        protected delegate void TriggerUse(Entity player);

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
        protected TriggerString OnTriggerString { get; set; }
        protected TriggerUse OnTriggerUse { get; set; }
        public BoxEntity(BoxType type, Vector3 origin, Vector3 angle, int range)
        {
            Type = type;
            Range = range;

            Entity = MapEdit.SpawnCrate(origin, angle);
        }

        #region calls
        public void Call(string func, params Parameter[] parameters)
        {
            Entity.Call(func, parameters);
        }

        public void Call(int identifier, params Parameter[] parameters)
        {
            Entity.Call(identifier, parameters);
        }

        public T Call<T>(string func, params Parameter[] parameters)
        {
            return Entity.Call<T>(func, parameters);
        }

        public T Call<T>(int identifier, params Parameter[] parameters)
        {
            return Entity.Call<T>(identifier, parameters);
        }

        public T GetField<T>(string name)
        {
            return Entity.GetField<T>(name);
        }

        public void SetField(string name, Parameter value)
        {
            Entity.SetField(name, value);
        }
        #endregion

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

        public string UsableText(Entity player)
        {
            if (!disposedValue)
            {
                return (string)OnTriggerString.DynamicInvoke(player);
            }
            throw new AggregateException();
        }

        public void Usable(Entity player)
        {
            if (!disposedValue)
            {
                OnTriggerUse.DynamicInvoke(player);
            }
        }

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

                AfterDelay(100, e =>
                {
                    Call("delete");
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

        public void Dispose()
        {
            Dispose(true);
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
            Shader = Hud.CreateShader(Origin, Icon);
            ObjectiveId = Hud.CreateObjective(Origin, Icon);
            Cost = 500;

            OnTriggerString += player =>
            {
                if (player.GetTeam() == "allies")
                {
                    if (player.GetField<int>("usingteleport") == 0)
                    {
                        if ((player.GetTeam() == "allies" && Utility.GetDvar<int>("allies_fire_sale") == 1) || (player.GetTeam() == "axis" && Utility.GetDvar<int>("axis_fire_sale") == 1))
                        {
                            return "Press ^3[{+activate}] ^7to use teleporter. [Cost: ^2$^610^7]";
                        }
                        return "Press ^3[{+activate}] ^7to use teleporter. [Cost: ^2$^3" + Cost + "^7]";
                    }
                }
                return "";
            };

            OnTriggerUse += player =>
            {
                if (!player.IsAlive) return;
                if (player.GetField<int>("usingteleport") == 0)
                {
                    if ((player.GetTeam() == "allies" && Utility.GetDvar<int>("allies_fire_sale") == 1) || (player.GetTeam() == "axis" && Utility.GetDvar<int>("axis_fire_sale") == 1) && player.GetCash() >= 10)
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

            OnTriggerString += player =>
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

    public class Gambler : BoxEntity
    {
        public bool IsUsing { get; set; }

        public Gambler(Vector3 origin, Vector3 angle) : base(BoxType.Gambler, origin, angle, 50)
        {
            IsUsing = false;
            Laptop = MapEdit.CreateLaptop(Origin);
            Icon = "cardicon_8ball";
            Shader = Hud.CreateShader(Origin, Icon);
            ObjectiveId = Hud.CreateObjective(Origin, Icon);
            Cost = 500;

            OnTriggerString += player =>
            {
                if (!IsUsing && player.GetField<int>("isgambling") == 0)
                {
                    if ((player.GetTeam() == "allies" && Utility.GetDvar<int>("allies_fire_sale") == 1) || (player.GetTeam() == "axis" && Utility.GetDvar<int>("axis_fire_sale") == 1))
                    {
                        return "Press ^3[{+activate}] ^7to gamble. [Cost: ^2$^610^7]";
                    }
                    return "Press ^3[{+activate}] ^7to gamble. [Cost: ^2$^3" + Cost + "^7]";
                }
                return "";
            };

            OnTriggerUse += player =>
            {
                if (IsUsing) return;
                if (!player.IsAlive) return;
                if ((!player.HasField("isgambling") || player.GetField<int>("isgambling") == 0))
                {
                    if ((player.GetTeam() == "allies" && Utility.GetDvar<int>("allies_fire_sale") == 1) || (player.GetTeam() == "axis" && Utility.GetDvar<int>("axis_fire_sale") == 1) && player.GetCash() >= 10)
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
        }

        private void GambleThink(Entity player)
        {
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
                    PrintGambleInfo(player, "Clean all enemys Perk-a-Cola.", GambleType.Excellent);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() != player.GetTeam() && item.IsAlive)
                        {
                            item.ClearCash();
                            item.GamblerText("Enemy team take you all Perk-a-Cola!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                        }
                    }
                    break;
                case 8:
                    PrintGambleInfo(player, "Double Points for friendly.", GambleType.Good);
                    player.Notify("double_points");
                    break;
                case 9:
                    PrintGambleInfo(player, "KaBoom!", GambleType.Good);
                    player.Notify("nuke_drop");
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
                                player.Notify("self_exploed");
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
                        if (item.IsAlive && item != player)
                        {
                            item.GamblerText("Player " + player.Name + " robbed you all cash.", new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
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
                    PrintGambleInfo(player, "Give all friendly $500", GambleType.Excellent);
                    player.WinCash(500);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() == player.GetTeam() && item.IsAlive && item != player)
                        {
                            item.WinCash(500);
                            item.GamblerText("Player " + player.Name + " give you $500.", new Vector3(1, 1, 1), new Vector3(0, 1, 0), 1, 0.85f);
                        }
                    }
                    break;
                case 15:
                    PrintGambleInfo(player, "Incantation for one enemy.", GambleType.Good);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() != player.GetTeam() && item.IsAlive)
                        {
                            item.SetField("incantation", 1);
                            break;
                        }
                    }
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
                        if (item.GetTeam() == player.GetTeam() && item.IsAlive)
                        {
                            item.ClearCash();
                            if (player != item)
                            {
                                item.GamblerText("Surprise!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                            }
                        }
                    }
                    break;
                case 19:
                    PrintGambleInfo(player, "Clean all enemys Cash.", GambleType.Excellent);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() != player.GetTeam() && item.IsAlive)
                        {
                            item.ClearCash();
                            item.GamblerText("Enemy team take you all cash!", new Vector3(0, 0, 0), new Vector3(1, 1, 1), 1, 0);
                        }
                    }
                    break;
                case 20:
                    PrintGambleInfo(player, "You lose all Perk-a-Cola.", GambleType.Terrible);
                    player.RemoveAllPerkCola();
                    break;
                case 21:
                    PrintGambleInfo(player, "Change a enemy to friendly.", GambleType.Good);
                    foreach (var item in Utility.Players)
                    {
                        if (item.GetTeam() != player.GetTeam() && item.IsAlive)
                        {
                            item.SetTeam(player.GetTeam());
                            item.GamblerText("You will change to " + player.GetTeam() + " by " + player.Name, new Vector3(1, 1, 1), new Vector3(1, 0, 0), 1, 0.85f);
                            break;
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
            Icon = PerkCola.GetIcon();
            ObjectiveId = Hud.CreateObjective(Origin, Icon);
            Cost = PerkCola.GetCost();

            string fullname = PerkCola.GetFullName();

            OnTriggerString += player =>
            {
                if ((player.GetTeam() == "allies" && Utility.GetDvar<int>("allies_fire_sale") == 1) || (player.GetTeam() == "axis" && Utility.GetDvar<int>("axis_fire_sale") == 1))
                {
                    return "Press ^3[{+activate}] ^7to buy " + fullname + ". [Cost: ^2$^610^7]";
                }
                return "Press ^3[{+activate}] ^7to buy " + fullname + ". [Cost: ^2$^3" + Cost + "^7]";
            };

            OnTriggerUse += player =>
            {
                if (!player.IsAlive) return;
                if ((player.GetTeam() == "allies" && Utility.GetDvar<int>("allies_fire_sale") == 1) || (player.GetTeam() == "axis" && Utility.GetDvar<int>("axis_fire_sale") == 1) && player.GetCash() >= 10)
                {
                    if (player.PerkColasCount() >= 4)
                    {
                        player.PrintlnBold("^1You already have 4 Perk-a-Cola.");
                        return;
                    }
                    if (player.HasPerkCola(PerkCola))
                    {
                        player.PrintlnBold("^1You already have " + fullname + ".");
                        return;
                    }
                    player.PayCash(10);
                    player.GivePerkCola(PerkCola);
                }
                else if (player.GetCash() >= Cost)
                {
                    if (player.PerkColasCount() >= 4)
                    {
                        player.PrintlnBold("^1You already have 4 Perk-a-Cola.");
                        return;
                    }
                    if (player.HasPerkCola(PerkCola))
                    {
                        player.PrintlnBold("^1You already have " + fullname + ".");
                        return;
                    }
                    player.PayCash(Cost);
                    player.GivePerkCola(PerkCola);
                }
                else
                {
                    player.Println("^1Not enough cash for " + fullname + ". Need ^2$^3" + Cost);
                }
            };
        }
    }
}

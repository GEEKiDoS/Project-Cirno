using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InfinityScript;

namespace INF3
{
    public class EasyMapEdit : BaseScript
    {
        private static string _currentPath;
        private Entity currenteditor;

        private Vector3 start;
        private Vector3 end;
        private Vector3 angles;
        private Parameter[] args;

        private bool creating = false;
        private string mapname;

        public EasyMapEdit()
        {
            mapname = Call<string>("getdvar", "mapname");

            if (!Directory.Exists("scripts\\inf3-maps\\" + mapname))
            {
                Directory.CreateDirectory("scripts\\inf3-maps\\" + mapname);
            }
            _currentPath = "scripts\\inf3-maps\\" + mapname + "\\" + mapname + ".txt";

            PlayerConnected += new Action<Entity>(player =>
            {
                player.Call("notifyonplayercommand", "fly", "+frag");
                player.OnNotify("fly", (ent) =>
                {
                    if (player.GetField<string>("sessionstate") != "spectator")
                    {
                        player.Call("allowspectateteam", "freelook", true);
                        player.SetField("sessionstate", "spectator");
                        player.Call("setcontents", 0);
                    }
                    else
                    {
                        player.Call("allowspectateteam", "freelook", false);
                        player.SetField("sessionstate", "playing");
                        player.Call("setcontents", 100);
                    }
                });
            });
        }
        public override void OnSay(Entity player, string name, string message)
        {
            if (message == "!crate")
            {
                player.Call("iprintlnbold", "crate set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "crate: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!ramp" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                player.Call("iprintlnbold", "ramp start set: " + start);
                return;
            }
            else if (message == "!ramp" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "ramp end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "ramp: " + start + ";" + end);
                return;
            }
            if (message == "!tp" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                player.Call("iprintlnbold", "elevator start set: " + start);
                return;
            }
            else if (message == "!tp" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "elevator end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "elevator: " + start + ";" + end);
                return;
            }
            if (message == "!htp" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                player.Call("iprintlnbold", "HiddenTP start set: " + start);
                return;
            }
            else if (message == "!htp" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "HiddenTP end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "HiddenTP: " + start + ";" + end);
                return;
            }
            if (message.StartsWith("!door") && !creating)
            {
                var split = message.Split(new char[] { ' ' }, 2);
                split = split[1].Split(new char[] { ',' });
                creating = true;
                currenteditor = player;
                start = player.Origin;
                angles = new Vector3(90, player.GetField<Vector3>("angles").Y, 0);
                args = new Parameter[] { split[0], split[1], split[2], split[3] };
                player.Call("iprintlnbold", "door start set: " + start);
                return;
            }
            else if (message.StartsWith("!door") && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "door end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "door: " + start + ";" + end + ";" + angles + ";" + args[0] + ";" + args[1] + ";" + args[2] + ";" + args[3]);
                return;
            }
            if (message.StartsWith("!paydoor") && !creating)
            {
                var split = message.Split(new char[] { ' ' }, 2);
                split = split[1].Split(new char[] { ',' });
                creating = true;
                currenteditor = player;
                start = player.Origin;
                angles = new Vector3(90, player.GetField<Vector3>("angles").Y, 0);
                args = new Parameter[] { split[0], split[1], split[2], split[3] };
                player.Call("iprintlnbold", "paydoor start set: " + start);
                return;
            }
            else if (message.StartsWith("!paydoor") && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "paydoor end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "paydoor: " + end + ";" + start + ";" + angles + ";" + args[0] + ";" + args[1] + ";" + args[2] + ";" + args[3]);
                return;
            }
            if (message == "!wall" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                player.Call("iprintlnbold", "wall start set: " + start);
                return;
            }
            else if (message == "!wall" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "wall end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "wall: " + end + ";" + start);
                return;
            }
            if (message == "!invwall" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                player.Call("iprintlnbold", "invwall start set: " + start);
                return;
            }
            else if (message == "!invwall" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "invwall end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "invwall: " + start + ";" + end);
                return;
            }
            if (message == "!floor" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                player.Call("iprintlnbold", "floor start set: " + start);
                return;
            }
            else if (message == "!floor" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "floor end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "floor: " + start + ";" + end);
                return;
            }
            if (message.StartsWith("!zipline") && !creating)
            {
                var split = message.Split(new char[] { ' ' }, 2);
                split = split[1].Split(new char[] { ',' });
                creating = true;
                currenteditor = player;
                start = player.Origin;
                angles = player.GetField<Vector3>("angles");
                args = new Parameter[] { split[0] };
                player.Call("iprintlnbold", "zipline start set: " + start);
                return;
            }
            else if (message.StartsWith("!zipline") && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "zipline end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "zipline: " + start + ";" + angles + ";" + end + ";" + args[0]);
                return;
            }
            if (message == "!teleporter" && !creating)
            {
                creating = true;
                currenteditor = player;
                start = player.Origin;
                angles = player.GetField<Vector3>("angles");
                player.Call("iprintlnbold", "teleporter start set: " + start);
                return;
            }
            else if (message == "!teleporter" && creating && currenteditor == player)
            {
                creating = false;
                end = player.Origin;
                player.Call("iprintlnbold", "teleporter end set: " + end);
                File.AppendAllText(_currentPath, Environment.NewLine + "teleporter: " + start + ";" + angles + ";" + end);
                return;
            }
            if (message.StartsWith("!trampoline"))
            {
                var split = message.Split(new char[] { ' ' }, 2);
                split = split[1].Split(new char[] { ',' });
                args = new Parameter[] { split[0] };
                player.Call("iprintlnbold", "trampoline set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "trampoline: " + player.Origin + ";" + player.GetField<Vector3>("angles") + ";" + args[0]);
                return;
            }
            if (message == "!power")
            {
                player.Call("iprintlnbold", "power set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "power: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!ammo")
            {
                player.Call("iprintlnbold", "ammo set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "ammo: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!gambler")
            {
                player.Call("iprintlnbold", "gambler set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "gambler: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!airstrike")
            {
                player.Call("iprintlnbold", "airstrike set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "airstrike: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message.StartsWith("!perk"))
            {
                var split = message.Split(new char[] { ' ' }, 2);
                split = split[1].Split(new char[] { ',' });
                int perk = 0;
                switch (split[0])
                {
                    case "revive":
                        perk = 0;
                        break;
                    case "speedcola":
                        perk = 1;
                        break;
                    case "juggernog":
                        perk = 2;
                        break;
                    case "staminup":
                        perk = 3;
                        break;
                    case "mulekick":
                        perk = 4;
                        break;
                    case "doubletap":
                        perk = 5;
                        break;
                    case "deadshot":
                        perk = 6;
                        break;
                    case "phd":
                        perk = 7;
                        break;
                    case "cherry":
                        perk = 8;
                        break;
                    case "widow":
                        perk = 9;
                        break;
                    case "vulture":
                        perk = 10;
                        break;
                }
                player.Call("iprintlnbold", split[0] + " set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "perk: " + player.Origin + ";" + player.GetField<Vector3>("angles") + ";" + perk);
                return;
            }
            if (message == "!randomperk")
            {
                player.Call("iprintlnbold", "randomperk set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "randomperk: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message=="!gobblegum")
            {
                player.Call("iprintlnbold", "gobblegum set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "gobblegum: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message.StartsWith("!model"))
            {
                var split = message.Split(new char[] { ' ' }, 2);
                split = split[1].Split(new char[] { ',' });
                args = new Parameter[] { split[0] };
                player.Call("iprintlnbold", "model set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "model: " + player.Origin + ";" + player.GetField<Vector3>("angles") + ";" + args[0]);
                return;
            }
            if (message == "!turret")
            {
                player.Call("iprintlnbold", "turret set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "turret: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!sentry")
            {
                player.Call("iprintlnbold", "sentry set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "sentry: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!gl")
            {
                player.Call("iprintlnbold", "gl set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "gl: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if (message == "!sam")
            {
                player.Call("iprintlnbold", "sam set: " + player.Origin);
                File.AppendAllText(_currentPath, Environment.NewLine + "sam: " + player.Origin + ";" + player.GetField<Vector3>("angles"));
                return;
            }
            if(message=="!undo")
            {
                var lines=File.ReadAllLines(_currentPath).ToList();
                lines.Remove(lines.Last());
                File.Delete(_currentPath);
                File.WriteAllLines(_currentPath, lines);
            }
            if (message == "!restart")
            {
                Utilities.ExecuteCommand("map_restart");
            }
        }
    }
}

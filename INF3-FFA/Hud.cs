using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public static class Hud
    {
        public static int CurObjID;

        private static readonly HudElem[] GambleTextHuds = new HudElem[18];
        private static readonly List<HudElem>[] PerkColaHuds = new List<HudElem>[18];

        public static void InitGambleTextHud(this Entity player)
        {
            GambleTextHuds[player.EntRef] = HudElem.NewClientHudElem(player);
        }

        public static void InitPerkHud(this Entity player)
        {
            PerkColaHuds[player.EntRef] = new List<HudElem>();
        }

        public static void AddPerkHud(this Entity player, HudElem hud)
        {
            PerkColaHuds[player.EntRef].Add(hud);
        }

        public static List<HudElem> GetPerkColaHud(this Entity player)
        {
            return PerkColaHuds[player.EntRef];
        }

        public static HudElem GetGambleTextHud(this Entity player)
        {
            return GambleTextHuds[player.EntRef];
        }
        public static void CreateCashHud(this Entity player)
        {
            HudElem hud = HudElem.CreateFontString(player, "hudbig", 1f);
            hud.SetPoint("right", "right", -50, 100);
            hud.HideWhenInMenu = true;
            player.OnInterval(100, delegate (Entity ent)
            {
                hud.SetText("^3$ ^7" + player.GetField<int>("aiz_cash"));
                return player.IsPlayer;
            });
        }

        public static void GamblerText(this Entity player, string text, Vector3 color, Vector3 glowColor, float intensity, float glowIntensity)
        {
            HudElem hud = GambleTextHuds[player.EntRef];

            hud.Call("destroy");

            hud = HudElem.CreateFontString(player, "hudbig", 2);
            var ent = hud.Entity;
            hud.SetPoint("CENTERMIDDLE", "CENTERMIDDLE", 0, 0);
            hud.SetText(text);
            hud.Color = color;
            hud.GlowColor = glowColor;
            hud.Alpha = 0;
            hud.GlowAlpha = glowIntensity;

            hud.ChangeFontScaleOverTime(0.25f, 0.75f);
            hud.Call("FadeOverTime", 0.25f);
            hud.Alpha = intensity;

            player.AfterDelay(250, e => player.Call("playLocalSound", "mp_bonus_end"));

            player.AfterDelay(3000, e =>
            {
                if (hud.Entity == ent)
                {
                    hud.ChangeFontScaleOverTime(0.25f, 2f);
                    hud.Call("FadeOverTime", 0.25f);
                    hud.Alpha = 0;
                }
            });

            player.AfterDelay(4000, e =>
            {
                if (hud.Entity == ent)
                {
                    hud.ChangeFontScaleOverTime(0.25f, 2f);
                    hud.Call("FadeOverTime", 0.25f);
                    hud.Alpha = 0;
                }
            });
        }

        public static void WelcomeMessage(this Entity player, List<string> messages, Vector3 color, Vector3 glowColor, float intensity, float glowIntensity)
        {
            var list = new List<HudElem>();

            foreach (var item in messages)
            {
                player.AfterDelay((messages.IndexOf(item) + 1) * 500, e =>
                {
                    var hud = HudElem.CreateFontString(player, "objective", 1.5f);
                    hud.SetPoint("TOPMIDDLE", "TOPMIDDLE", 0, 45 + messages.IndexOf(item) * 15);
                    hud.FontScale = 6;
                    hud.Color = color;
                    hud.SetText(item);
                    hud.Alpha = 0;
                    hud.GlowColor = glowColor;
                    hud.GlowAlpha = glowIntensity;

                    hud.ChangeFontScaleOverTime(0.2f, 1.5f);
                    hud.Call("fadeovertime", 0.2f);
                    hud.Alpha = intensity;

                    list.Add(hud);
                });
            }
            player.AfterDelay(messages.Count * 500 + 4000, e =>
            {
                foreach (var item in list)
                {
                    player.AfterDelay((list.IndexOf(item) + 1) * 500, en =>
                    {
                        item.ChangeFontScaleOverTime(0.2f, 4.5f);
                        item.Call("fadeovertime", 0.2f);
                        item.Alpha = 0;
                    });
                }
            });
            player.AfterDelay(60000, e =>
            {
                foreach (var item in list)
                {
                    item.Call("destroy");
                }
            });
        }

        public static HudElem PerkHudNoEffect(this Entity player, string shader)
        {
            int perksAmount = player.PerkColasCount() - 1;
            int MultiplyTimes = 28 * perksAmount;

            var hudshader = HudElem.NewClientHudElem(player);
            hudshader.AlignX = "center";
            hudshader.VertAlign = "middle";
            hudshader.AlignY = "middle";
            hudshader.HorzAlign = "center";
            hudshader.X = -410 + MultiplyTimes;
            hudshader.Y = 160;
            hudshader.Foreground = true;
            hudshader.SetShader(shader, 25, 25);
            hudshader.Alpha = 1;

            return hudshader;
        }

        public static HudElem PerkHud(this Entity player, string shader, Vector3 color, string text)
        {
            player.Call("setblurforplayer", 6, 0.5f);
            int perksAmount = player.PerkColasCount() - 1;
            int MultiplyTimes = 28 * perksAmount;

            var hudtext = HudElem.NewClientHudElem(player);
            hudtext.AlignX = "center";
            hudtext.VertAlign = "middle";
            hudtext.AlignY = "middle";
            hudtext.HorzAlign = "center";
            hudtext.Font = "objective";
            hudtext.FontScale = 1.5f;
            hudtext.X = 0;
            hudtext.Y = 0;
            hudtext.Foreground = true;
            hudtext.Color = color;
            hudtext.Alpha = 0;
            hudtext.SetText(text);

            var hudshader = HudElem.NewClientHudElem(player);
            hudshader.AlignX = "center";
            hudshader.VertAlign = "middle";
            hudshader.AlignY = "middle";
            hudshader.HorzAlign = "center";
            hudshader.X = 0;
            hudshader.Y = 0;
            hudshader.Foreground = true;
            hudshader.SetShader(shader, 25, 25);
            hudshader.Alpha = 1;

            player.AfterDelay(300, e =>
            {
                hudshader.Call("moveovertime", 0.5f);
                hudshader.X = -200;
            });
            player.AfterDelay(700, e =>
            {
                player.Call("setblurforplayer", 0, 0.3f);
                hudtext.Alpha = 1;
            });
            player.AfterDelay(3700, e =>
            {
                hudtext.Call("fadeovertime", 0.25f);
                hudtext.Alpha = 0;
                hudshader.Call("scaleovertime", 1, 25, 25);
                hudshader.Call("moveovertime", 1);
                hudshader.X = -410 + MultiplyTimes;
                hudshader.Y = 160;
            });
            player.AfterDelay(4700, e =>
            {
                hudtext.Call("destroy");

            });

            return hudshader;
        }

        public static void Credits(this Entity player)
        {
            HudElem credits = HudElem.CreateFontString(player, "hudbig", 1.0f);
            credits.SetPoint("CENTER", "BOTTOM", 0, -70);
            credits.Call("settext", "Project Cirno (INF3) for FFA");
            credits.Alpha = 0f;
            credits.SetField("glowcolor", new Vector3(1f, 0.5f, 1f));
            credits.GlowAlpha = 1f;

            HudElem credits2 = HudElem.CreateFontString(player, "hudbig", 0.6f);
            credits2.SetPoint("CENTER", "BOTTOM", 0, -90);
            credits2.Call("settext", "Vesion 1.0. Code in: https://github.com/A2ON");
            credits2.Alpha = 0f;
            credits2.SetField("glowcolor", new Vector3(1f, 0.5f, 1f));
            credits2.GlowAlpha = 1f;

            player.Call("notifyonplayercommand", "tab", "+scores");
            player.OnNotify("tab", entity =>
            {
                credits.Alpha = 1f;
                credits2.Alpha = 1f;
            });

            player.Call("notifyonplayercommand", "-tab", "-scores");
            player.OnNotify("-tab", entity =>
            {
                credits.Alpha = 0f;
                credits2.Alpha = 0f;
            });
        }

        public static void BonusDropTakeHud(Entity player, string text, string shader)
        {
            var hud = HudElem.NewHudElem();
            hud.HorzAlign = "center";
            hud.VertAlign = "middle";
            hud.AlignX = "center";
            hud.AlignY = "middle";
            hud.Font = "objective";
            hud.FontScale = 2;
            hud.Alpha = 1;
            hud.Color = new Vector3(1, 1, 1);
            hud.GlowColor = new Vector3(1f, 0.3f, 0.3f);
            hud.GlowAlpha = 0.85f;
            hud.X = 0;
            hud.Y = 140;

            hud.Call("moveovertime", 2);
            hud.Call("fadeovertime", 2);
            hud.Y = 80;
            hud.Alpha = 0;

            var icon = HudElem.NewTeamHudElem("allies");
            icon.HorzAlign = "center";
            icon.VertAlign = "middle";
            icon.AlignX = "center";
            icon.AlignY = "middle";
            icon.X = 0;
            icon.Y = 125;
            icon.Foreground = true;
            icon.SetShader(shader, 30, 30);
            icon.Alpha = 1;

            icon.Call("moveovertime", 2);
            icon.Call("fadeovertime", 2);
            icon.Y = 65;
            icon.Alpha = 0;

            player.AfterDelay(2000, e =>
            {
                hud.Call("destroy");
                icon.Call("destroy");
            });
        }

        public static HudElem BonusDropHud(string shader, float xpoint)
        {
            var icon = HudElem.NewHudElem();
            icon.HorzAlign = "center";
            icon.VertAlign = "middle";
            icon.AlignX = "center";
            icon.AlignY = "middle";
            icon.Foreground = true;
            icon.SetShader(shader, 30, 30);
            icon.Alpha = 1;
            icon.Y = 200;
            icon.X = xpoint;

            return icon;
        }

        public static void CreateFlagShader(Vector3 origin)
        {
            HudElem elem = HudElem.NewHudElem();
            elem.SetShader("waypoint_flag_friendly", 15, 15);
            elem.Alpha = 0.6f;
            elem.X = origin.X;
            elem.Y = origin.Y;
            elem.Z = origin.Z + 100f;
            elem.Call("SetWayPoint", 1, 1);
        }

        public static HudElem CreateShader(Vector3 origin, string shader, string team = "")
        {
            HudElem elem;
            if (team != "")
            {
                elem = HudElem.NewTeamHudElem(team);
            }
            else
            {
                elem = HudElem.NewHudElem();
            }
            elem.SetShader(shader, 15, 15);
            elem.Alpha = 0.6f;
            elem.X = origin.X;
            elem.Y = origin.Y;
            elem.Z = origin.Z + 50f;
            elem.Call("SetWayPoint", 1, 1);

            return elem;
        }

        public static int CreateObjective(Vector3 origin, string shader, string team = "none")
        {
            int num = 31 - CurObjID++;
            Function.SetEntRef(-1);
            Function.Call("objective_state", num, "active");
            Function.Call("objective_position", num, origin);
            Function.Call("objective_icon", num, shader);
            Function.Call("objective_team", num, team);

            return num;
        }
    }
}

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
        private static readonly HudElem[] TextPopupHuds = new HudElem[18];
        private static readonly HudElem[] TextPopup2Huds = new HudElem[18];
        private static readonly HudElem[] ScorePopupHuds = new HudElem[18];

        public static void InitGambleTextHud(this Entity player)
        {
            GambleTextHuds[player.EntRef] = GSCFunctions.NewClientHudElem(player);
        }

        public static void InitPerkHud(this Entity player)
        {
            PerkColaHuds[player.EntRef] = new List<HudElem>();
        }

        public static void InitTextPopup(this Entity player)
        {
            TextPopupHuds[player.EntRef] = GSCFunctions.NewClientHudElem(player);
        }

        public static void InitTextPopup2(this Entity player)
        {
            TextPopup2Huds[player.EntRef] = GSCFunctions.NewClientHudElem(player);
        }

        public static void InitScorePopup(this Entity player)
        {
            ScorePopupHuds[player.EntRef] = GSCFunctions.NewClientHudElem(player);
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

        public static HudElem GetTextPopupHud(this Entity player)
        {
            return TextPopupHuds[player.EntRef];
        }

        public static HudElem GetTextPopup2Hud(this Entity player)
        {
            return TextPopup2Huds[player.EntRef];
        }

        public static HudElem GetScorePopupHud(this Entity player)
        {
            return ScorePopupHuds[player.EntRef];
        }

        public static void GamblerText(this Entity player, string text, Vector3 color, Vector3 glowColor, float intensity, float glowIntensity)
        {
            HudElem hud = GambleTextHuds[player.EntRef];

            hud.Destroy();
            
            hud = HudElem.CreateFontString(player, HudElem.Fonts.HudBig, 2);
            var ent = hud.Entity;
            hud.SetPoint("CENTERMIDDLE", "CENTERMIDDLE", 0, 0);
            hud.SetText(text);
            hud.Color = color;
            hud.GlowColor = glowColor;
            hud.Alpha = 0;
            hud.GlowAlpha = glowIntensity;

            hud.ChangeFontScaleOverTime(0.75f);
            hud.FadeOverTime( 0.25f);
            hud.Alpha = intensity;

            BaseScript.AfterDelay(250, () => player.Call("playLocalSound", "mp_bonus_end"));

            BaseScript.AfterDelay(3000, () =>
            {
                if (hud.Entity == ent)
                {
                    hud.ChangeFontScaleOverTime(2f);
                    hud.FadeOverTime( 0.25f);
                    hud.Alpha = 0;
                }
            });

            BaseScript.AfterDelay(4000, () =>
            {
                if (hud.Entity == ent)
                {
                    hud.ChangeFontScaleOverTime(2f);
                    hud.FadeOverTime( 0.25f);
                    hud.Alpha = 0;
                }
            });
        }

        public static void WelcomeMessage(this Entity player, List<string> messages, Vector3 color, Vector3 glowColor, float intensity, float glowIntensity)
        {
            var list = new List<HudElem>();

            foreach (var item in messages)
            {
                BaseScript.AfterDelay((messages.IndexOf(item) + 1) * 500, () =>
                {
                    var hud = HudElem.CreateFontString(player, HudElem.Fonts.Objective, 1.5f);
                    hud.SetPoint("TOPMIDDLE", "TOPMIDDLE", 0, 45 + messages.IndexOf(item) * 15);
                    hud.FontScale = 6;
                    hud.Color = color;
                    hud.SetText(item);
                    hud.Alpha = 0;
                    hud.GlowColor = glowColor;
                    hud.GlowAlpha = glowIntensity;

                    hud.ChangeFontScaleOverTime( 1.5f);
                    hud.FadeOverTime(0.2f);
                    hud.Alpha = intensity;

                    list.Add(hud);
                });
            }
            BaseScript.AfterDelay(messages.Count * 500 + 4000, () =>
            {
                foreach (var item in list)
                {
                    BaseScript.AfterDelay((list.IndexOf(item) + 1) * 500, () =>
                    {
                        item.ChangeFontScaleOverTime(4.5f);
                        item.FadeOverTime( 0.2f);
                        item.Alpha = 0;
                        if (list.IndexOf(item) == list.Count - 1)
                        {
                            BaseScript.AfterDelay(1000, () =>
                            {
                                foreach (var item2 in list)
                                {
                                    item2.Destroy();
                                }
                            });
                        }
                    });
                }
            });
        }

        public static HudElem PerkHudNoEffect(this Entity player, PerkCola perk, bool playsound = false)
        {
            int perksAmount = player.PerkColasCount() - 1;
            int MultiplyTimes = 28 * perksAmount;

            var hudshader = GSCFunctions.NewClientHudElem(player);
            
            hudshader.AlignX = HudElem.XAlignments.Center;
            hudshader.VertAlign = HudElem.VertAlignments.Middle;
            hudshader.AlignY = HudElem.YAlignments.Middle;
            hudshader.HorzAlign = HudElem.HorzAlignments.Center;
            hudshader.X = -300 + MultiplyTimes;
            hudshader.Y = 180;
            hudshader.Foreground = true;
            hudshader.SetShader(perk.Icon, 25, 25);
            hudshader.FadeOverTime( 0.5f);
            hudshader.Alpha = 1;
            hudshader.SetField("perk", new Parameter(perk.Type));

            return hudshader;
        }

        public static HudElem PerkHud(this Entity player, PerkCola perk, bool playsound = false)
        {
            player.Call("setblurforplayer", 6, 0.5f);
            int perksAmount = player.PerkColasCount() - 1;
            int MultiplyTimes = 28 * perksAmount;

            var hudtext = GSCFunctions.NewClientHudElem(player);
            hudtext.AlignX = HudElem.XAlignments.Center;
            hudtext.VertAlign = HudElem.VertAlignments.Middle;
            hudtext.AlignY = HudElem.YAlignments.Middle;
            hudtext.HorzAlign = HudElem.HorzAlignments.Center;
            hudtext.Font = HudElem.Fonts.Objective;
            hudtext.FontScale = 1.5f;
            hudtext.X = 0;
            hudtext.Y = 0;
            hudtext.Foreground = true;
            hudtext.Color = perk.HudColor;
            hudtext.Alpha = 0;
            hudtext.SetText(perk.HudName);

            var hudshader = GSCFunctions.NewClientHudElem(player);
            hudshader.AlignX = HudElem.XAlignments.Center;
            hudshader.VertAlign = HudElem.VertAlignments.Middle;
            hudshader.AlignY = HudElem.YAlignments.Middle;
            hudshader.HorzAlign = HudElem.HorzAlignments.Center;
            hudshader.X = 0;
            hudshader.Y = 0;
            hudshader.Foreground = true;
            hudshader.SetShader(perk.Icon, 25, 25);
            hudshader.Alpha = 1;
            hudshader.SetField("perk", new Parameter(perk.Type));

            BaseScript.AfterDelay(300, () =>
            {
                hudshader.MoveOverTime( 0.5f);
                hudshader.X = -200;
            });
            BaseScript.AfterDelay(700, () =>
            {
                player.Call("setblurforplayer", 0, 0.3f);
                hudtext.Alpha = 1;
            });
            BaseScript.AfterDelay(3700, () =>
            {
                hudtext.FadeOverTime( 0.25f);
                hudtext.Alpha = 0;
                hudshader.ScaleOverTime( 1, 25, 25);
                hudshader.MoveOverTime( 1);
                hudshader.X = -300 + MultiplyTimes;
                hudshader.Y = 180;
            });
            BaseScript.AfterDelay(4700, () =>
            {
                hudtext.Destroy();

            });

            return hudshader;
        }

        public static void Credits(this Entity player)
        {
            HudElem credits = HudElem.CreateFontString(player, HudElem.Fonts.HudBig, 1.0f);
            credits.SetPoint("CENTER", "BOTTOM", 0, -70);
            credits.SetText( "Buffashion Infect");
            credits.Alpha = 0f;
            credits.SetField("glowcolor", new Vector3(1f, 0.5f, 1f));
            credits.GlowAlpha = 1f;

            HudElem credits2 = HudElem.CreateFontString(player, HudElem.Fonts.HudBig, 0.6f);
            credits2.SetPoint("CENTER", "BOTTOM", 0, -90);
            credits2.SetText( "Vesion 1.0 IS Beta 2. Code in: https://github.com/A2ON");
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

        [Obsolete]
        public static void TextPopup(this Entity player, string text)
        {
            HudElem hud = player.GetTextPopupHud();

            if (hud == null)
            {
                hud = GSCFunctions.NewClientHudElem(player);
            }
            hud.Destroy();

            hud = HudElem.CreateFontString(player, HudElem.Fonts.HudBig, 0.8f);
            hud.SetPoint("BOTTOMCENTER", "BOTTOMCENTER", 0, -65);
            hud.SetText(text);
            hud.Alpha = 0.85f;
            hud.GlowColor = new Vector3(0.3f, 0.9f, 0.9f);
            hud.GlowAlpha = 0.55f;
            hud.SetPulseFX( 100, 2100, 1000);
            hud.ChangeFontScaleOverTime( 0.75f);
            BaseScript.AfterDelay(100, () =>
            {
                hud.ChangeFontScaleOverTime(0.65f);
            });
        }

        [Obsolete]
        public static void TextPopup2(this Entity player, string text)
        {
            HudElem hud = player.GetTextPopup2Hud();

            if (hud == null)
            {
                hud = GSCFunctions.NewClientHudElem(player);
            }
            hud.Destroy();

            hud = HudElem.CreateFontString(player, HudElem.Fonts.HudBig, 0.8f);
            hud.SetPoint("BOTTOMCENTER", "BOTTOMCENTER", 0, -105);
            hud.SetText(text);
            hud.Alpha = 0.85f;
            hud.GlowColor = new Vector3(0.3f, 0.9f, 0.9f);
            hud.GlowAlpha = 0.55f;
            hud.SetPulseFX( 100, 3000, 1000);
            hud.ChangeFontScaleOverTime(0.75f);
            BaseScript.AfterDelay(100, () =>
            {
                hud.ChangeFontScaleOverTime(0.65f);
            });
        }

        private static void CreateRankHud(this Entity player)
        {
            var hud = HudElem.CreateFontString(player, HudElem.Fonts.HudBig, 1);
            hud.SetPoint("BOTTOMCENTER", "BOTTOMCENTER", 0, -80);
            hud.Alpha = 0;
            hud.Color = new Vector3(0.5f, 0.5f, 0.5f);

            ScorePopupHuds[player.EntRef] = hud;
        }

        [Obsolete]
        public static void ScorePopup(this Entity player, int amount, Vector3 hudcolor, float glowalpha)
        {
            if (amount == 0)
            {
                return;
            }
            if (player.GetScorePopupHud() != null)
            {
                var temphud = player.GetScorePopupHud();
                temphud.Destroy();
            }

            player.CreateRankHud();

            player.SetField("xpUpdateTotal", player.GetField<int>("xpUpdateTotal") + amount);

            var hud = player.GetScorePopupHud();

            if (player.GetField<int>("xpUpdateTotal") < 0)
            {
                hud.Color = new Vector3(25.5f, 25.5f, 3.6f);
                hud.GlowColor = new Vector3(0.9f, 0.3f, 0.3f);
                hud.GlowAlpha = 0.55f;
            }
            else
            {
                hud.Color = new Vector3(25.5f, 25.5f, 3.6f);
                hud.GlowColor = new Vector3(0.3f, 0.9f, 0.3f);
                hud.GlowAlpha = 0.55f;
            }

            hud.SetText(player.GetField<int>("xpUpdateTotal") > 0 ? "+" : "" + player.GetField<int>("xpUpdateTotal").ToString());
            hud.Alpha = 1;
            hud.SetPulseFX( 100, 3000, 1000);
            BaseScript.AfterDelay(3000, () =>
            {
                if (hud != null)
                {
                    player.SetField("xpUpdateTotal", 0);
                }
            });
        }

        public static void BonusDropTakeHud(Entity player, string text, string shader)
        {
            var hud = GSCFunctions.NewTeamHudElem("allies");
            hud.HorzAlign = HudElem.HorzAlignments.Center;
            hud.VertAlign = HudElem.VertAlignments.Middle;
            hud.AlignX = HudElem.XAlignments.Center;
            hud.AlignY = HudElem.YAlignments.Middle;
            hud.Font = HudElem.Fonts.Objective;
            hud.FontScale = 2;
            hud.Alpha = 1;
            hud.Color = new Vector3(1, 1, 1);
            hud.GlowColor = new Vector3(1f, 0.3f, 0.3f);
            hud.GlowAlpha = 0.85f;
            hud.X = 0;
            hud.Y = 140;

            hud.MoveOverTime( 2);
            hud.FadeOverTime( 2);
            hud.Y = 80;
            hud.Alpha = 0;

            var icon = GSCFunctions.NewTeamHudElem("allies");
            icon.HorzAlign = HudElem.HorzAlignments.Center;
            icon.VertAlign = HudElem.VertAlignments.Middle;
            icon.AlignX = HudElem.XAlignments.Center;
            icon.AlignY = HudElem.YAlignments.Middle;
            icon.X = 0;
            icon.Y = 125;
            icon.Foreground = true;
            icon.SetShader(shader, 30, 30);
            icon.Alpha = 1;

            icon.MoveOverTime( 2);
            icon.FadeOverTime( 2);
            icon.Y = 65;
            icon.Alpha = 0;

            BaseScript.AfterDelay(2000, () =>
            {
                hud.Destroy();
                icon.Destroy();
            });
        }

        public static HudElem BonusDropHud(string shader, float xpoint)
        {
            var icon = GSCFunctions.NewTeamHudElem("allies");
            icon.HorzAlign = HudElem.HorzAlignments.Center;
            icon.VertAlign = HudElem.VertAlignments.Middle;
            icon.AlignX = HudElem.XAlignments.Center;
            icon.AlignY = HudElem.YAlignments.Middle;
            icon.Foreground = true;
            icon.SetShader(shader, 30, 30);
            icon.Alpha = 1;
            icon.Y = 200;
            icon.X = xpoint;

            return icon;
        }

        public static void GobbleGumHud(this Entity player, string head, string text)
        {

        }

        public static void CreateFlagShader(Vector3 origin)
        {
            HudElem elem = GSCFunctions.NewHudElem();
            elem.SetShader("waypoint_flag_friendly", 15, 15);
            elem.Alpha = 0.6f;
            elem.X = origin.X;
            elem.Y = origin.Y;
            elem.Z = origin.Z + 100f;
            elem.SetWaypoint(true, true);
        }

        public static HudElem CreateShader(Vector3 origin, string shader, string team = "")
        {
            HudElem elem;
            if (team != "")
            {
                elem = GSCFunctions.NewTeamHudElem(team);
            }
            else
            {
                elem = GSCFunctions.NewHudElem();
            }
            elem.SetShader(shader, 15, 15);
            elem.Alpha = 0.6f;
            elem.X = origin.X;
            elem.Y = origin.Y;
            elem.Z = origin.Z + 50f;
            elem.SetWaypoint(true,true);

            return elem;
        }

        public static int CreateObjective(Vector3 origin, string shader, string team = "none")
        {
            int num = 31 - CurObjID++;
            Function.SetEntRef(-1);
            GSCFunctions.Objective_State( num, "active");
            GSCFunctions.Objective_Position( num, origin);
            GSCFunctions.Objective_Icon( num, shader);
            GSCFunctions.Objective_Team( num, team);

            return num;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    // CallWrapper for InfinityScript 1.5
    public static class CallWrapper
    {
        private static Dictionary<string, int> _functionMappings = new Dictionary<string, int>();
        private static Dictionary<string, int> _globalFunctionMappings = new Dictionary<string, int>();

        public static void AddMapping(string name, int value)
        {
            _functionMappings[name.ToLowerInvariant()] = value;
        }

        public static void AddGlobalMapping(string name, int value)
        {
            _globalFunctionMappings[name.ToLowerInvariant()] = value;
        }

        public static void AfterDelay(this Entity entity,int delay, Action<Entity> action)
        {
            BaseScript.AfterDelay(delay, () => action(entity));
        }

        public static void OnInterval(this Entity entity, int delay, Func<Entity, bool> action)
        {
            BaseScript.AfterDelay(delay, () => action(entity));
        }

        public static void Call(string func, params Parameter[] parameters)
        {
            var table = _globalFunctionMappings;

            if (!table.ContainsKey(func))
            {
                Log.Write(LogLevel.Warning, "no such function: {0}", func);
                return;
            }

            Function.SetEntRef(-1);
            Function.Call((ScriptNames.FunctionList)table[func], parameters);
        }

        public static TReturn Call<TReturn>(string func, params Parameter[] parameters)
        {
            var table = _globalFunctionMappings;

            if (!table.ContainsKey(func))
            {
                Log.Write(LogLevel.Warning, "no such function: {0}", func);
                return default(TReturn);
            }

            Function.SetEntRef(-1);
            Function.Call((ScriptNames.FunctionList)table[func], parameters);

            return (TReturn)Function.GetReturns();
        }

        public static void Call(this BaseScript script, string func, params Parameter[] parameters)
        {
            var table = _globalFunctionMappings;

            if (!table.ContainsKey(func))
            {
                Log.Write(LogLevel.Warning, "no such function: {0}", func);
                return;
            }

            Function.SetEntRef(-1);
            Function.Call((ScriptNames.FunctionList)table[func], parameters);
        }

        public static TReturn Call<TReturn>(this BaseScript script, string func, params Parameter[] parameters)
        {
            var table = _globalFunctionMappings;

            if (!table.ContainsKey(func))
            {
                Log.Write(LogLevel.Warning, "no such function: {0}", func);
                return default(TReturn);
            }

            Function.SetEntRef(-1);
            Function.Call((ScriptNames.FunctionList)table[func], parameters);

            return (TReturn)Function.GetReturns();
        }

        public static void Call(this Entity entity, string func, params Parameter[] parameters)
        {
            var table = _functionMappings;

            if (!table.ContainsKey(func))
            {
                Log.Write(LogLevel.Warning, "no such function: {0}", func);
                return;
            }

            Function.SetEntRef(entity.EntRef);
            Function.Call((ScriptNames.FunctionList)table[func], parameters);
        }

        public static TReturn Call<TReturn>(this Entity entity, string func, params Parameter[] parameters)
        {
            var table = _functionMappings;

            if (!table.ContainsKey(func))
            {
                Log.Write(LogLevel.Warning, "no such function: {0}", func);
                return default(TReturn);
            }

            Function.SetEntRef(entity.EntRef);
            Function.Call((ScriptNames.FunctionList)table[func], parameters);

            return (TReturn)Function.GetReturns();
        }

        public static void Initialize()
        {
            // playercmd #1
            AddMapping("getviewmodel", 33457);
            AddMapping("fragbuttonpressed", 33458);
            AddMapping("secondaryoffhandbuttonpressed", 33459);
            AddMapping("getcurrentweaponclipammo", 33460);
            AddMapping("setvelocity", 33461);
            AddMapping("getplayerviewheight", 33462);
            AddMapping("unknown", 33545);
            AddMapping("getnormalizedmovement", 33463);
            AddMapping("getnormalizedcameramovement", 33486);
            AddMapping("giveweapon", 33487);
            AddMapping("takeweapon", 33488);
            AddMapping("takeallweapons", 33489);
            AddMapping("getcurrentweapon", 33490);
            AddMapping("getcurrentprimaryweapon", 33491);
            AddMapping("getcurrentoffhand", 33492);
            AddMapping("hasweapon", 33493);
            AddMapping("switchtoweapon", 33494);
            AddMapping("switchtoweaponimmediate", 33495);
            AddMapping("switchtooffhand", 33496);
            AddMapping("givestartammo", 33522);
            AddMapping("givemaxammo", 33523);
            AddMapping("getfractionstartammo", 33524);
            AddMapping("getfractionmaxammo", 33525);
            AddMapping("isdualwielding", 33526);
            AddMapping("isreloading", 33527);
            AddMapping("isswitchingweapon", 33528);
            AddMapping("setorigin", 33529);
            AddMapping("getvelocity", 33530);
            AddMapping("setplayerangles", 33531);
            AddMapping("getplayerangles", 33532);
            AddMapping("usebuttonpressed", 33533);
            AddMapping("attackbuttonpressed", 33534);
            AddMapping("adsbuttonpressed", 33535);
            AddMapping("meleebuttonpressed", 33536);
            AddMapping("playerads", 33537);
            AddMapping("isonground", 33538);
            AddMapping("isusingturret", 33539);
            AddMapping("setviewmodel", 33540);
            AddMapping("setoffhandprimaryclass", 33541);
            AddMapping("getoffhandprimaryclass", 33542);
            AddMapping("setoffhandsecondaryclass", 33497);
            AddMapping("getoffhandsecondaryclass", 33498);
            AddMapping("beginlocationselection", 33499);
            AddMapping("endlocationselection", 33500);
            AddMapping("disableweapons", 33501);
            AddMapping("enableweapons", 33502);
            AddMapping("disableoffhandweapons", 33503);
            AddMapping("enableoffhandweapons", 33504);
            AddMapping("disableweaponswitch", 33505);
            AddMapping("enableweaponswitch", 33506);
            AddMapping("openpopupmenu", 33507);
            AddMapping("openpopupmenunomouse", 33508);
            AddMapping("closepopupmenu", 33509);
            AddMapping("openmenu", 33510);
            AddMapping("closemenu", 33511);
            AddMapping("freezecontrols", 33513);
            AddMapping("disableusability", 33514);
            AddMapping("enableusability", 33515);
            AddMapping("setwhizbyspreads", 33516);
            AddMapping("setwhizbyradii", 33517);
            AddMapping("setreverb", 33518);
            AddMapping("deactivatereverb", 33519);
            AddMapping("setvolmod", 33520);
            AddMapping("setchannelvolume", 33521);
            AddMapping("setchannelvolumes", 33464);
            AddMapping("deactivatechannelvolumes", 33465);
            AddMapping("playlocalsound", 33466);
            AddMapping("stoplocalsound", 33467);
            AddMapping("setweaponammoclip", 33468);
            AddMapping("setweaponammostock", 33469);
            AddMapping("getweaponammoclip", 33470);
            AddMapping("getweaponammostock", 33471);
            AddMapping("anyammoforweaponmodes", 33472);
            AddMapping("setclientdvar", 33473);
            AddMapping("setclientdvars", 33474);
            AddMapping("allowads", 33475);
            AddMapping("allowjump", 33476);
            AddMapping("allowsprint", 33477);
            AddMapping("setspreadoverride", 33478);
            AddMapping("resetspreadoverride", 33479);
            AddMapping("setaimspreadmovementscale", 33480);
            AddMapping("setactionslot", 33481);
            AddMapping("setviewkickscale", 33482);
            AddMapping("getviewkickscale", 33483);
            AddMapping("getweaponslistall", 33484);
            AddMapping("getweaponslistprimaries", 33485);
            AddMapping("getweaponslistoffhands", 33430);
            AddMapping("getweaponslistitems", 33431);
            AddMapping("getweaponslistexclusives", 33432);
            AddMapping("getweaponslist", 33433);
            AddMapping("canplayerplacesentry", 33434);
            AddMapping("canplayerplacetank", 33435);
            AddMapping("visionsetnakedforplayer", 33436);
            AddMapping("visionsetnightforplayer", 33437);
            AddMapping("visionsetmissilecamforplayer", 33438);
            AddMapping("visionsetthermalforplayer", 33439);
            AddMapping("visionsetpainforplayer", 33440);
            AddMapping("setblurforplayer", 33441);
            AddMapping("getplayerweaponmodel", 33442);
            AddMapping("getplayerknifemodel", 33443);
            AddMapping("updateplayermodelwithweapons", 33444);
            AddMapping("notifyonplayercommand", 33445);
            AddMapping("canmantle", 33446);
            AddMapping("forcemantle", 33447);
            AddMapping("ismantling", 33448);
            AddMapping("playfx", 33449);
            AddMapping("recoilscaleon", 33450);
            AddMapping("recoilscaleoff", 33451);
            AddMapping("weaponlockstart", 33452);
            AddMapping("weaponlockfinalize", 33453);
            AddMapping("weaponlockfree", 33454);
            AddMapping("weaponlocktargettooclose", 33455);
            AddMapping("weaponlocknoclearance", 33390);
            AddMapping("visionsyncwithplayer", 33391);
            AddMapping("showhudsplash", 33392);
            AddMapping("setperk", 33393);
            AddMapping("hasperk", 33394);
            AddMapping("clearperks", 33395);
            AddMapping("unsetperk", 33396);
            AddMapping("noclip", 33397);
            AddMapping("ufo", 33398);

            // playercmd #2
            AddMapping("pingplayer", 33308);
            AddMapping("buttonpressed", 33309);
            AddMapping("sayall", 33310);
            AddMapping("sayteam", 33311);
            AddMapping("showscoreboard", 33312);
            AddMapping("setspawnweapon", 33313);
            AddMapping("dropitem", 33314);
            AddMapping("dropscavengerbag", 33315);
            AddMapping("finishplayerdamage", 33340);
            AddMapping("suicide", 33341);
            AddMapping("closeingamemenu", 33342);
            AddMapping("iprintln", 33343);
            AddMapping("iprintlnbold", 33344);
            AddMapping("spawn", 33345);
            AddMapping("setentertime", 33346);
            AddMapping("cloneplayer", 33347);
            AddMapping("istalking", 33348);
            AddMapping("allowspectateteam", 33349);
            AddMapping("getguid", 33350);
            AddMapping("getxuid", 33382);
            AddMapping("ishost", 33383);
            AddMapping("getspectatingplayer", 33384);
            AddMapping("predictstreampos", 33385);
            AddMapping("updatescores", 33386);
            AddMapping("updatedmscores", 33387);
            AddMapping("setrank", 33388);
            AddMapping("setcardtitle", 33389);
            AddMapping("setcardicon", 33420);
            AddMapping("setcardnameplate", 33421);
            AddMapping("setcarddisplayslot", 33422);
            AddMapping("regweaponforfxremoval", 33423);
            AddMapping("laststandrevive", 33424);
            AddMapping("setspectatedefaults", 33425);
            AddMapping("getthirdpersoncrosshairoffset", 33426);
            AddMapping("disableweaponpickup", 33427);
            AddMapping("enableweaponpickup", 33428);

            // HECmd
            AddMapping("settext", 32950);
            AddMapping("clearalltextafterhudelem", 32951);
            AddMapping("setshader", 32952);
            AddMapping("settargetent", 32953);
            AddMapping("cleartargetent", 32954);
            AddMapping("settimer", 32955);
            AddMapping("settimerup", 32956);
            AddMapping("settimerstatic", 32957);
            AddMapping("settenthstimer", 32958);
            AddMapping("settenthstimerup", 32959);
            AddMapping("settenthstimerstatic", 32960);
            AddMapping("setclock", 32961);
            AddMapping("setclockup", 32962);
            AddMapping("setvalue", 32963);
            AddMapping("setwaypoint", 32964);
            AddMapping("rotatingicon", 32965);
            AddMapping("secondaryarrow", 32891);
            AddMapping("setwaypointiconoffscreenonly", 32892);
            AddMapping("fadeovertime", 32893);
            AddMapping("scaleovertime", 32894);
            AddMapping("moveovertime", 32895);
            AddMapping("reset", 32896);
            AddMapping("destroy", 32897);
            AddMapping("setpulsefx", 32898);
            AddMapping("setplayernamestring", 32899);
            AddMapping("fadeovertime2", 33547);
            AddMapping("scaleovertime2", 33548);
            AddMapping("changefontscaleovertime", 32900);

            // ScrCmd
            AddMapping("attach", 32791);
            AddMapping("attachshieldmodel", 32792);
            AddMapping("detach", 32804);
            AddMapping("detachshieldmodel", 32805);
            AddMapping("moveshieldmodel", 32806);
            AddMapping("detachall", 32807);
            AddMapping("getattachsize", 32808);
            AddMapping("getattachmodelname", 32809);
            AddMapping("getattachtagname", 32810);
            AddMapping("getattachignorecollision", 32835);
            AddMapping("hidepart", 32836);
            AddMapping("allinstances", 32837);
            AddMapping("hideallparts", 32838);
            AddMapping("showpart", 32839);
            AddMapping("showallparts", 32840);
            AddMapping("linkto", 32841);
            AddMapping("linktoblendtotag", 32842);
            AddMapping("unlink", 32843);
            AddMapping("islinked", 32867);
            AddMapping("enablelinkto", 32868);
            AddMapping("playerlinkto", 32885);
            AddMapping("playerlinktodelta", 32886);
            AddMapping("playerlinkweaponviewtodelta", 32887);
            AddMapping("playerlinktoabsolute", 32888);
            AddMapping("playerlinktoblend", 32889);
            AddMapping("playerlinkedoffsetenable", 32890);
            AddMapping("playerlinkedoffsetdisable", 32916);
            AddMapping("playerlinkedsetviewznear", 32917);
            AddMapping("playerlinkedsetusebaseangleforviewclamp", 32918);
            AddMapping("lerpviewangleclamp", 32919);
            AddMapping("setviewangleresistance", 32920);
            AddMapping("geteye", 32921);
            AddMapping("istouching", 32922);
            AddMapping("stoploopsound", 32923);
            AddMapping("stopsounds", 32924);
            AddMapping("playrumbleonentity", 32925);
            AddMapping("playrumblelooponentity", 32926);
            AddMapping("stoprumble", 32927);
            AddMapping("delete", 32928);
            AddMapping("setmodel", 32929);
            AddMapping("laseron", 32930);
            AddMapping("laseroff", 32931);
            AddMapping("laseraltviewon", 32932);
            AddMapping("laseraltviewoff", 32933);
            AddMapping("thermalvisionon", 32934);
            AddMapping("thermalvisionoff", 32935);
            AddMapping("unknown", 32803);
            AddMapping("unknown", 32768);
            AddMapping("thermalvisionfofoverlayon", 32936);
            AddMapping("thermalvisionfofoverlayoff", 32937);
            AddMapping("autospotoverlayon", 32938);
            AddMapping("autospotoverlayoff", 32939);
            AddMapping("setcontents", 32940);
            AddMapping("makeusable", 32941);
            AddMapping("makeunusable", 32942);
            AddMapping("setcursorhint", 32966);
            AddMapping("sethintstring", 32967);
            AddMapping("forceusehinton", 32968);
            AddMapping("forceusehintoff", 32969);
            AddMapping("makesoft", 32970);
            AddMapping("makehard", 32971);
            AddMapping("willneverchange", 32972);
            AddMapping("startfiring", 32973);
            AddMapping("stopfiring", 32974);
            AddMapping("isfiringturret", 32975);
            AddMapping("startbarrelspin", 32976);
            AddMapping("stopbarrelspin", 32977);
            AddMapping("getbarrelspinrate", 32978);
            AddMapping("remotecontrolturret", 32979);
            AddMapping("remotecontrolturretoff", 32980);
            AddMapping("shootturret", 32981);
            AddMapping("getturretowner", 32982);
            AddMapping("setsentryowner", 33006);
            AddMapping("setsentrycarrier", 33007);
            AddMapping("setturretminimapvisible", 33008);
            AddMapping("settargetentity", 33009);
            AddMapping("snaptotargetentity", 33010);
            AddMapping("cleartargetentity", 33011);
            AddMapping("getturrettarget", 33012);
            AddMapping("setplayerspread", 33013);
            AddMapping("setaispread", 33014);
            AddMapping("setsuppressiontime", 33015);
            AddMapping("setconvergencetime", 33049);
            AddMapping("setconvergenceheightpercent", 33050);
            AddMapping("setturretteam", 33051);
            AddMapping("maketurretsolid", 33052);
            AddMapping("maketurretoperable", 33053);
            AddMapping("maketurretinoperable", 33054);
            AddMapping("setturretaccuracy", 33082);
            AddMapping("setrightarc", 33083);
            AddMapping("setleftarc", 33084);
            AddMapping("settoparc", 33085);
            AddMapping("setbottomarc", 33086);
            AddMapping("setautorotationdelay", 33087);
            AddMapping("setdefaultdroppitch", 33088);
            AddMapping("restoredefaultdroppitch", 33089);
            AddMapping("turretfiredisable", 33090);
            AddMapping("turretfireenable", 33121);
            AddMapping("setturretmodechangewait", 33122);
            AddMapping("usetriggerrequirelookat", 33123);
            AddMapping("getstance", 33124);
            AddMapping("setstance", 33125);
            AddMapping("itemweaponsetammo", 33126);
            AddMapping("getammocount", 33127);
            AddMapping("gettagorigin", 33128);
            AddMapping("gettagangles", 33129);
            AddMapping("shellshock", 33130);
            AddMapping("stunplayer", 33131);
            AddMapping("stopshellshock", 33132);
            AddMapping("fadeoutshellshock", 33133);
            AddMapping("setdepthoffield", 33134);
            AddMapping("setviewmodeldepthoffield", 33135);
            AddMapping("setmotionblurmovescale", 33136);
            AddMapping("setmotionblurturnscale", 33168);
            AddMapping("setmotionblurzoomscale", 33169);
            AddMapping("viewkick", 33170);
            AddMapping("localtoworldcoords", 33171);
            AddMapping("getentitynumber", 33172);
            AddMapping("getentityvelocity", 33173);
            AddMapping("enablegrenadetouchdamage", 33174);
            AddMapping("disablegrenadetouchdamage", 33175);
            AddMapping("enableaimassist", 33176);
            AddMapping("disableaimassist", 33207);
            AddMapping("radiusdamage", 33208);
            AddMapping("detonate", 33209);
            AddMapping("damageconetrace", 33210);
            AddMapping("sightconetrace", 33211);
            AddMapping("settargetent", 33212);
            AddMapping("settargetpos", 33213);
            AddMapping("cleartarget", 33214);
            AddMapping("setflightmodedirect", 33215);
            AddMapping("setflightmodetop", 33216);
            AddMapping("getlightintensity", 33217);
            AddMapping("setlightintensity", 33218);
            AddMapping("isragdoll", 33219);
            AddMapping("setmovespeedscale", 33220);
            AddMapping("cameralinkto", 33221);
            AddMapping("cameraunlink", 33222);
            AddMapping("controlslinkto", 33251);
            AddMapping("controlsunlink", 33252);
            AddMapping("makevehiclesolidcapsule", 33253);
            AddMapping("makevehiclesolidsphere", 33254);
            AddMapping("remotecontrolvehicle", 33256);
            AddMapping("remotecontrolvehicleoff", 33257);
            AddMapping("isfiringvehicleturret", 33258);
            AddMapping("drivevehicleandcontrolturret", 33259);
            AddMapping("drivevehicleandcontrolturretoff", 33260);
            AddMapping("getplayersetting", 33261);
            AddMapping("getlocalplayerprofiledata", 33262);
            AddMapping("setlocalplayerprofiledata", 33263);
            AddMapping("remotecamerasoundscapeon", 33264);
            AddMapping("remotecamerasoundscapeoff", 33265);
            AddMapping("radarjamon", 33266);
            AddMapping("radarjamoff", 33267);
            AddMapping("setmotiontrackervisible", 33268);
            AddMapping("getmotiontrackervisible", 33269);
            AddMapping("circle", 33270);
            AddMapping("getpointinbounds", 33271);
            AddMapping("transfermarkstonewscriptmodel", 33272);
            AddMapping("setwatersheeting", 33273);
            AddMapping("setweaponhudiconoverride", 33274);
            AddMapping("getweaponhudiconoverride", 33275);
            AddMapping("setempjammed", 33276);
            AddMapping("playersetexpfog", 33277);
            AddMapping("isitemunlocked", 33278);
            AddMapping("getplayerdata", 33279);
            AddMapping("setplayerdata", 33306);

            // some entity (script_model) stuff
            AddMapping("moveto", 33399);
            AddMapping("movex", 33400);
            AddMapping("movey", 33401);
            AddMapping("movez", 33402);
            AddMapping("movegravity", 33403);
            AddMapping("moveslide", 33404);
            AddMapping("stopmoveslide", 33405);
            AddMapping("rotateto", 33406);
            AddMapping("rotatepitch", 33407);
            AddMapping("rotateyaw", 33408);
            AddMapping("rotateroll", 33409);
            AddMapping("addpitch", 33410);
            AddMapping("addyaw", 33411);
            AddMapping("addroll", 33412);
            AddMapping("vibrate", 33413);
            AddMapping("rotatevelocity", 33414);
            AddMapping("solid", 33415);
            AddMapping("notsolid", 33416);
            AddMapping("setcandamage", 33417);
            AddMapping("setcanradiusdamage", 33418);
            AddMapping("physicslaunchclient", 33419);
            AddMapping("physicslaunchserver", 33351);
            AddMapping("physicslaunchserveritem", 33352);
            AddMapping("clonebrushmodeltoscriptmodel", 33353);
            AddMapping("scriptmodelplayanim", 33354);
            AddMapping("scriptmodelclearanim", 33355);

            // varied ent/player script commands
            AddMapping("getorigin", 32910);
            AddMapping("useby", 32914);
            AddMapping("playsound", 32915);
            AddMapping("playsoundasmaster", 32878);
            AddMapping("playsoundtoteam", 32771);
            AddMapping("playsoundtoplayer", 32772);
            AddMapping("playloopsound", 32879);
            AddMapping("getnormalhealth", 32884);
            AddMapping("setnormalhealth", 32844);
            AddMapping("show", 32847);
            AddMapping("hide", 32848);
            AddMapping("playerhide", 32773);
            AddMapping("showtoplayer", 32774);
            AddMapping("enableplayeruse", 32775);
            AddMapping("disableplayeruse", 32776);
            AddMapping("makescrambler_unk", 33546);
            AddMapping("makeportableradar_unk", 32777);
            AddMapping("maketrophysystem_unk", 32778);
            AddMapping("makeunk", 32779);
            AddMapping("setmode", 32864);
            AddMapping("getmode", 32865);
            AddMapping("placespawnpoint", 32780);
            AddMapping("setteamfortrigger", 32781);
            AddMapping("clientclaimtrigger", 32782);
            AddMapping("clientreleasetrigger", 32783);
            AddMapping("releaseclaimedtrigger", 32784);
            AddMapping("isusingonlinedataoffline", 32785);
            AddMapping("getrestedtime", 32786);
            AddMapping("send73command_unk", 32787);
            AddMapping("sendleaderboards", 32800);
            AddMapping("isonladder", 32788);
            AddMapping("startragdoll", 32798);
            AddMapping("getcorpseanim", 32789);
            AddMapping("playerforcedeathanim", 32790);
            AddMapping("startac130", 33543);
            AddMapping("stopac130", 33544);

            // global stuff #1
            AddGlobalMapping("iprintln", 362);
            AddGlobalMapping("iprintlnbold", 363);
            AddGlobalMapping("logstring", 364);
            AddGlobalMapping("getent", 365);
            AddGlobalMapping("getentarray", 366);
            AddGlobalMapping("spawnplane", 367);
            AddGlobalMapping("spawnstruct", 368);
            AddGlobalMapping("spawnhelicopter", 369);
            AddGlobalMapping("isalive", 370);
            AddGlobalMapping("isspawner", 371);
            AddGlobalMapping("createattractorent", 372);
            AddGlobalMapping("createattractororigin", 373);
            AddGlobalMapping("createrepulsorent", 374);
            AddGlobalMapping("createrepulsororigin", 375);
            AddGlobalMapping("deleteattractor", 376);
            AddGlobalMapping("playsoundatpos", 377);
            AddGlobalMapping("newhudelem", 378);
            AddGlobalMapping("newclienthudelem", 379);
            AddGlobalMapping("newteamhudelem", 380);
            AddGlobalMapping("resettimeout", 381);
            AddGlobalMapping("precachefxteamthermal", 382);
            AddGlobalMapping("isplayer", 383);
            AddGlobalMapping("isplayernumber", 384);
            AddGlobalMapping("setsunlight", 57);
            AddGlobalMapping("resetsunlight", 58);
            AddGlobalMapping("setwinningplayer", 385);
            AddGlobalMapping("setwinningteam", 311);
            AddGlobalMapping("announcement", 312);
            AddGlobalMapping("clientannouncement", 313);
            AddGlobalMapping("getteamscore", 314);
            AddGlobalMapping("setteamscore", 315);
            AddGlobalMapping("setclientnamemode", 316);
            AddGlobalMapping("updateclientnames", 317);
            AddGlobalMapping("getteamplayersalive", 318);
            AddGlobalMapping("logprint", 319);
            AddGlobalMapping("worldentnumber", 320);
            AddGlobalMapping("obituary", 321);
            AddGlobalMapping("positionwouldtelefrag", 322);
            AddGlobalMapping("canspawn", 323);
            AddGlobalMapping("getstarttime", 324);
            AddGlobalMapping("precachestatusicon", 325);
            AddGlobalMapping("precacheminimapicon", 327);
            AddGlobalMapping("precachempanim", 328);
            AddGlobalMapping("restart", 329);
            AddGlobalMapping("exitlevel", 330);
            AddGlobalMapping("addtestclient", 331);
            AddGlobalMapping("makedvarserverinfo", 332);
            AddGlobalMapping("setarchive", 333);
            AddGlobalMapping("allclientsprint", 334);
            AddGlobalMapping("clientprint", 335);
            AddGlobalMapping("mapexists", 336);
            AddGlobalMapping("isvalidgametype", 337);
            AddGlobalMapping("matchend", 338);
            AddGlobalMapping("setplayerteamrank", 339);
            AddGlobalMapping("endparty", 340);
            AddGlobalMapping("setteamradar", 341);
            AddGlobalMapping("getteamradar", 342);
            AddGlobalMapping("setteamradarstrength", 343);
            AddGlobalMapping("getteamradarstrength", 344);
            AddGlobalMapping("getuavstrengthmin", 345);
            AddGlobalMapping("getuavstrengthmax", 262);
            AddGlobalMapping("getuavstrengthlevelneutral", 263);
            AddGlobalMapping("getuavstrengthlevelshowenemyfastsweep", 264);
            AddGlobalMapping("getuavstrengthlevelshowenemydirectional", 265);
            AddGlobalMapping("blockteamradar", 266);
            AddGlobalMapping("unblockteamradar", 267);
            AddGlobalMapping("isteamradarblocked", 268);
            AddGlobalMapping("getassignedteam", 269);
            AddGlobalMapping("setmatchdata", 270);
            AddGlobalMapping("getmatchdata", 271);
            AddGlobalMapping("sendmatchdata", 272);
            AddGlobalMapping("clearmatchdata", 273);
            AddGlobalMapping("setmatchdatadef", 274);
            AddGlobalMapping("setmatchclientip", 275);
            AddGlobalMapping("setmatchdataid", 276);
            AddGlobalMapping("setclientmatchdata", 277);
            AddGlobalMapping("getclientmatchdata", 278);
            AddGlobalMapping("setclientmatchdatadef", 279);
            AddGlobalMapping("sendclientmatchdata", 280);
            AddGlobalMapping("getbuildversion", 281);
            AddGlobalMapping("getbuildnumber", 282);
            AddGlobalMapping("getsystemtime", 283);
            AddGlobalMapping("getmatchrulesdata", 284);
            AddGlobalMapping("isusingmatchrulesdata", 285);
            AddGlobalMapping("kick", 286);
            AddGlobalMapping("issplitscreen", 287);
            AddGlobalMapping("setmapcenter", 288);
            AddGlobalMapping("setgameendtime", 289);
            AddGlobalMapping("visionsetnaked", 290);
            AddGlobalMapping("visionsetnight", 291);
            AddGlobalMapping("visionsetmissilecam", 292);
            AddGlobalMapping("visionsetthermal", 217);
            AddGlobalMapping("visionsetpain", 218);
            AddGlobalMapping("endlobby", 219);
            AddGlobalMapping("ambience", 220);
            AddGlobalMapping("getmapcustom", 221);
            AddGlobalMapping("updateskill", 222);
            AddGlobalMapping("spawnsighttrace", 223);

            // global stuff #2
            AddGlobalMapping("setprintchannel", 14);
            AddGlobalMapping("print", 15);
            AddGlobalMapping("println", 16);
            AddGlobalMapping("print3d", 17);
            AddGlobalMapping("line", 18);
            AddGlobalMapping("spawnturret", 19);
            AddGlobalMapping("canspawnturret", 20);
            AddGlobalMapping("assert", 21);
            AddGlobalMapping("assertex", 38);
            AddGlobalMapping("assertmsg", 39);
            AddGlobalMapping("isdefined", 40);
            AddGlobalMapping("isstring", 41);
            AddGlobalMapping("setdvar", 42);
            AddGlobalMapping("setdynamicdvar", 43);
            AddGlobalMapping("setdvarifuninitialized", 44);
            AddGlobalMapping("setdevdvar", 45);
            AddGlobalMapping("setdevdvarifuninitialized", 46);
            AddGlobalMapping("getdvar", 47);
            AddGlobalMapping("getdvarint", 48);
            AddGlobalMapping("getdvarfloat", 49);
            AddGlobalMapping("getdvarvector", 50);
            AddGlobalMapping("gettime", 51);
            AddGlobalMapping("getentbynum", 52);
            AddGlobalMapping("getweaponmodel", 53);
            AddGlobalMapping("getweaponhidetags", 81);
            AddGlobalMapping("getanimlength", 82);
            AddGlobalMapping("animhasnotetrack", 83);
            AddGlobalMapping("getnotetracktimes", 84);
            AddGlobalMapping("spawn", 85);
            AddGlobalMapping("spawnloopsound", 86);
            AddGlobalMapping("bullettrace", 87);
            AddGlobalMapping("bullettracepassed", 88);
            AddGlobalMapping("sighttracepassed", 116);
            AddGlobalMapping("physicstrace", 117);
            AddGlobalMapping("physicstracenormal", 118);
            AddGlobalMapping("playerphysicstrace", 119);
            AddGlobalMapping("getgroundposition", 120);
            AddGlobalMapping("getmovedelta", 121);
            AddGlobalMapping("getangledelta", 122);
            AddGlobalMapping("getnorthyaw", 123);
            AddGlobalMapping("setnorthyaw", 150);
            AddGlobalMapping("setslowmotion", 151);
            AddGlobalMapping("randomint", 152);
            AddGlobalMapping("randomfloat", 153);
            AddGlobalMapping("randomintrange", 154);
            AddGlobalMapping("randomfloatrange", 155);
            AddGlobalMapping("sin", 156);
            AddGlobalMapping("cos", 157);
            AddGlobalMapping("tan", 158);
            AddGlobalMapping("asin", 159);
            AddGlobalMapping("acos", 160);
            AddGlobalMapping("atan", 161);
            AddGlobalMapping("int", 162);
            AddGlobalMapping("float", 163);
            AddGlobalMapping("abs", 164);
            AddGlobalMapping("min", 165);
            AddGlobalMapping("max", 198);
            AddGlobalMapping("floor", 199);
            AddGlobalMapping("ceil", 200);
            AddGlobalMapping("exp", 201);
            AddGlobalMapping("log", 202);
            AddGlobalMapping("sqrt", 203);
            AddGlobalMapping("squared", 204);
            AddGlobalMapping("clamp", 205);
            AddGlobalMapping("angleclamp", 206);
            AddGlobalMapping("angleclamp180", 207);
            AddGlobalMapping("vectorfromlinetopoint", 208);
            AddGlobalMapping("pointonsegmentnearesttopoint", 209);
            AddGlobalMapping("distance", 210);
            AddGlobalMapping("distance2d", 211);
            AddGlobalMapping("distancesquared", 212);
            AddGlobalMapping("length", 213);
            AddGlobalMapping("lengthsquared", 214);
            AddGlobalMapping("closer", 215);
            AddGlobalMapping("vectordot", 216);
            AddGlobalMapping("vectornormalize", 246);
            AddGlobalMapping("vectortoangles", 247);
            AddGlobalMapping("vectortoyaw", 248);
            AddGlobalMapping("vectorlerp", 249);
            AddGlobalMapping("anglestoup", 250);
            AddGlobalMapping("anglestoright", 251);
            AddGlobalMapping("anglestoforward", 252);
            AddGlobalMapping("combineangles", 253);
            AddGlobalMapping("transformmove", 254);
            AddGlobalMapping("issubstr", 255);
            AddGlobalMapping("isendstr", 256);
            AddGlobalMapping("getsubstr", 257);
            AddGlobalMapping("tolower", 258);
            AddGlobalMapping("strtok", 259);
            AddGlobalMapping("stricmp", 260);
            AddGlobalMapping("ambientplay", 261);
            AddGlobalMapping("ambientstop", 293);
            AddGlobalMapping("precachemodel", 294);
            AddGlobalMapping("precacheshellshock", 295);
            AddGlobalMapping("precacheitem", 296);
            AddGlobalMapping("precacheshader", 297);
            AddGlobalMapping("precachestring", 298);
            AddGlobalMapping("precachemenu", 299);
            AddGlobalMapping("precacherumble", 300);
            AddGlobalMapping("precachelocationselector", 301);
            AddGlobalMapping("precacheleaderboards", 302);
            AddGlobalMapping("precacheheadicon", 326);
            AddGlobalMapping("loadfx", 303);
            AddGlobalMapping("playfx", 304);
            AddGlobalMapping("playfxontag", 305);
            AddGlobalMapping("stopfxontag", 306);
            AddGlobalMapping("playloopedfx", 307);
            AddGlobalMapping("spawnfx", 308);
            AddGlobalMapping("triggerfx", 309);
            AddGlobalMapping("playfxontagforclients", 310);
            AddGlobalMapping("physicsexplosionsphere", 346);
            AddGlobalMapping("physicsexplosioncylinder", 347);
            AddGlobalMapping("physicsjolt", 348);
            AddGlobalMapping("physicsjitter", 349);
            AddGlobalMapping("setexpfog", 350);
            AddGlobalMapping("isexplosivedamagemod", 351);
            AddGlobalMapping("radiusdamage", 352);
            AddGlobalMapping("setplayerignoreradiusdamage", 353);
            AddGlobalMapping("glassradiusdamage", 354);
            AddGlobalMapping("earthquake", 355);
            AddGlobalMapping("getnumparts", 356);
            AddGlobalMapping("getpartname", 386);
            AddGlobalMapping("weaponfiretime", 387);
            AddGlobalMapping("weaponclipsize", 388);
            AddGlobalMapping("weaponisauto", 389);
            AddGlobalMapping("weaponissemiauto", 390);
            AddGlobalMapping("weaponisboltaction", 391);
            AddGlobalMapping("weaponinheritsperks", 392);
            AddGlobalMapping("weaponburstcount", 393);
            AddGlobalMapping("weapontype", 394);
            AddGlobalMapping("weaponclass", 395);
            AddGlobalMapping("weaponinventorytype", 437);
            AddGlobalMapping("weaponstartammo", 438);
            AddGlobalMapping("weaponmaxammo", 439);
            AddGlobalMapping("weaponaltweaponname", 440);
            AddGlobalMapping("isweaponcliponly", 441);
            AddGlobalMapping("isweapondetonationtimed", 442);
            AddGlobalMapping("weaponhasthermalscope", 443);
            AddGlobalMapping("getvehiclenode", 444);
            AddGlobalMapping("getvehiclenodearray", 445);
            AddGlobalMapping("getallvehiclenodes", 446);
            AddGlobalMapping("getnumvehicles", 447);
            AddGlobalMapping("precachevehicle", 448);
            AddGlobalMapping("spawnvehicle", 449);
            AddGlobalMapping("getarray", 450);
            AddGlobalMapping("getspawnerarray", 408);
            AddGlobalMapping("playrumbleonposition", 409);
            AddGlobalMapping("playrumblelooponposition", 410);
            AddGlobalMapping("stopallrumbles", 411);
            AddGlobalMapping("soundexists", 412);
            AddGlobalMapping("openfile", 413);
            AddGlobalMapping("closefile", 414);
            AddGlobalMapping("fprintln", 415);
            AddGlobalMapping("fprintfields", 416);
            AddGlobalMapping("freadln", 417);
            AddGlobalMapping("fgetarg", 418);
            AddGlobalMapping("setminimap", 419);
            AddGlobalMapping("setthermalbodymaterial", 420);
            AddGlobalMapping("getarraykeys", 421);
            AddGlobalMapping("getfirstarraykey", 422);
            AddGlobalMapping("getnextarraykey", 396);
            AddGlobalMapping("sortbydistance", 397);
            AddGlobalMapping("tablelookup", 398);
            AddGlobalMapping("tablelookupbyrow", 399);
            AddGlobalMapping("tablelookupistring", 400);
            AddGlobalMapping("tablelookupistringbyrow", 401);
            AddGlobalMapping("tablelookuprownum", 402);
            AddGlobalMapping("getmissileowner", 403);
            AddGlobalMapping("magicbullet", 404);
            AddGlobalMapping("getweaponflashtagname", 405);
            AddGlobalMapping("averagepoint", 406);
            AddGlobalMapping("averagenormal", 407);
            AddGlobalMapping("getglass", 423);
            AddGlobalMapping("getglassarray", 424);
            AddGlobalMapping("getglassorigin", 425);
            AddGlobalMapping("isglassdestroyed", 426);
            AddGlobalMapping("destroyglass", 427);
            AddGlobalMapping("deleteglass", 428);
            AddGlobalMapping("getentchannelscount", 429);

            // objective
            AddGlobalMapping("objective_add", 431);
            AddGlobalMapping("objective_delete", 432);
            AddGlobalMapping("objective_state", 433);
            AddGlobalMapping("objective_icon", 434);
            AddGlobalMapping("objective_position", 435);
            AddGlobalMapping("objective_current", 436);
            AddGlobalMapping("objective_onentity", 357);
            AddGlobalMapping("objective_team", 358);
            AddGlobalMapping("objective_player", 359);
            AddGlobalMapping("objective_playerteam", 360);
            AddGlobalMapping("objective_playerenemyteam", 361);
        }
    }
}

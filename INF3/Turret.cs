using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public abstract class Turret
    {
        protected delegate string TurretHud(Entity player);
        protected delegate void TurretFunc(Entity player);

        protected Entity Entity { get; }
        protected TurretHud OnTurretString { get; set; }
        protected TurretFunc OnTurretFire { get; set; }

        public Turret(string weapon, string model, Vector3 origin, Vector3 angle, int toparc = 0, int bottomarc = 0, int leftarc = 0, int rightarc = 0)
        {
            Function.SetEntRef(-1);
            Entity = Function.Call<Entity>("spawnturret", "misc_turret", origin, weapon);
            Entity.Call("setmodel", model);
            Entity.SetField("angles", angle);
            if (toparc != 0)
            {
                Entity.Call("settoparc", toparc);
            }
            if (bottomarc != 0)
            {
                Entity.Call("setbottomarc", bottomarc);
            }
            if (leftarc != 0)
            {
                Entity.Call("setleftarc", bottomarc);
            }
            if (rightarc != 0)
            {
                Entity.Call("setrightarc", bottomarc);
            }
        }

        public string UsableText(Entity player)
        {
            return (string)OnTurretString.DynamicInvoke(player);
        }

        public void Usable(Entity player)
        {
            OnTurretFire.DynamicInvoke(player);
        }
    }

    public class OnGroundTurret : Turret
    {
        public OnGroundTurret(Vector3 origin, Vector3 angle) : base("sentry_minigun_mp", "weapon_minigun", origin, angle)
        {
            OnTurretString += new TurretHud(player =>
            {
                if (player.Call<int>("isusingturret") == 0 && player.Origin.DistanceTo(Entity.Origin) < 50)
                {
                    return "Press ^3[{+activate}] ^7to use.";
                }
                else if (player.Call<int>("isusingturret") == 1 && player.Origin.DistanceTo(Entity.Origin) < 50)
                {
                    return "Press ^3[{+activate}] ^7to drop.";
                }
                return "";
            });
        }
    }

    public class Sentry : Turret
    {
        public Sentry(Vector3 origin, Vector3 angle) : base("sentry_minigun_mp", "sentry_minigun_weak", origin, angle)
        {
            OnTurretString += new TurretHud(player =>
            {
                if (player.Call<int>("isusingturret") == 0 && player.Origin.DistanceTo(Entity.Origin) < 50)
                {
                    return "Press ^3[{+activate}] ^7to use.";
                }
                else if (player.Call<int>("isusingturret") == 1 && player.Origin.DistanceTo(Entity.Origin) < 50)
                {
                    return "Press ^3[{+activate}] ^7to drop.";
                }
                return "";
            });
        }
    }

    public class GL:Turret
    {
        private bool cooding = false;

        public GL(Vector3 origin, Vector3 angle) : base("sam_mp", "sentry_grenade_launcher", origin, angle)
        {
            OnTurretString += new TurretHud(player =>
            {
                if (player.Call<int>("isusingturret") == 0 && player.Origin.DistanceTo(Entity.Origin) < 50)
                {
                    return "Press ^3[{+activate}] ^7to use.";
                }
                else if (player.Call<int>("isusingturret") == 1 && player.Origin.DistanceTo(Entity.Origin) < 50)
                {
                    return "Press ^3[{+activate}] ^7to drop.";
                }
                return "";
            });

            OnTurretFire += new TurretFunc(player => 
            {
                if (!cooding && player.Call<int>("isusingturret") == 1 && player.Origin.DistanceTo(Entity.Origin) < 50 && player.Call<int>("attackbuttonpressed") == 1)
                {

                }
            });
        }
    }
}

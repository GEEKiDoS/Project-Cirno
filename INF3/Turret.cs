using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public enum TurretType
    {
        Turret,
        Sentry,
        GL,
        SAM
    }

    public abstract class Turret : IUsable
    {
        public virtual event Func<Entity, string> UsableText;
        public virtual event Action<Entity> UsableThink;

        public Entity Entity { get; }
        public Vector3 Origin
        {
            get
            {
                return Entity.Origin;
            }
        }
        public int Range { get; }

        public Turret(string weapon, string model, Vector3 origin, Vector3 angle, int toparc = 0, int bottomarc = 0, int leftarc = 0, int rightarc = 0)
        {
            Range = 50;

            Function.SetEntRef(-1);
            Entity = Function.Call<Entity>("spawnturret", "misc_turret", origin, weapon);
            Entity.Call("setmodel", model);
            Entity.SetField("angles", angle);
            Entity.Call("sethintstring", " ");
            Entity.Call("laseron");
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

            UsableText += player =>
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
            };

            MapEdit.usables.Add(this);
        }

        public virtual string GetUsableText(Entity player)
        {
            return UsableText(player);
        }

        public virtual void DoUsableFunc(Entity player)
        {
            UsableThink(player);
        }
    }

    public class OnGroundTurret : Turret
    {
        public OnGroundTurret(Vector3 origin, Vector3 angle) : base("sentry_minigun_mp", "weapon_minigun", origin, angle)
        {
            return;
        }
        public override void DoUsableFunc(Entity player)
        {
            return;
        }
    }

    public class Sentry : Turret
    {
        public Sentry(Vector3 origin, Vector3 angle) : base("sentry_minigun_mp", "sentry_minigun_weak", origin, angle)
        {
            return;
        }
        public override void DoUsableFunc(Entity player)
        {
            return;
        }
    }

    public class GL : Turret
    {
        private bool cooding = false;

        public GL(Vector3 origin, Vector3 angle) : base("sam_mp", "sentry_grenade_launcher", origin, angle, 30, 30, 60, 60)
        {
            UsableThink += player =>
            {
                try
                {
                    if (!cooding)
                    {
                        cooding = true;

                        Function.SetEntRef(-1);
                        Vector3 vector = Function.Call<Vector3>("anglestoforward", player.Call<Vector3>("getplayerangles"));
                        Vector3 dsa = new Vector3(vector.X * 1000000f, vector.Y * 1000000f, vector.Z * 1000000f);

                        Entity.AfterDelay(0, e => { if (player.Call<int>("isusingturret") == 1) { Function.SetEntRef(-1); Function.Call("magicbullet", "xm25_mp", Entity.Call<Vector3>("gettagorigin", "tag_laser"), dsa, player); } });

                        Entity.AfterDelay(1000, e => cooding = false);
                    }
                }
                catch (Exception)
                {
                    cooding = false;
                }
            };
        }
    }

    public class SAM : Turret
    {
        private bool cooding = false;

        public SAM(Vector3 origin, Vector3 angle) : base("sam_mp", "mp_sam_turret", origin, angle)
        {
            UsableThink += player =>
            {
                try
                {
                    if (!cooding)
                    {
                        cooding = true;

                        Vector3 le1 = Entity.Call<Vector3>("gettagorigin", "tag_le_missile1");
                        Vector3 le2 = Entity.Call<Vector3>("gettagorigin", "tag_le_missile2");
                        Vector3 ri1 = Entity.Call<Vector3>("gettagorigin", "tag_ri_missile1");
                        Vector3 ri2 = Entity.Call<Vector3>("gettagorigin", "tag_ri_missile2");

                        var dff = new Func<Entity, Vector3>(ent =>
                        {
                            Function.SetEntRef(-1);
                            Vector3 vector = Function.Call<Vector3>("anglestoforward", ent.Call<Vector3>("getplayerangles"));
                            Vector3 dsa = new Vector3(vector.X * 1000000f, vector.Y * 1000000f, vector.Z * 1000000f);

                            return dsa;
                        });

                        Entity.AfterDelay(0, e => { if (player.Call<int>("isusingturret") == 1) { Function.SetEntRef(-1); Function.Call("magicbullet", "sam_projectile_mp", le1, dff(player), player); } });
                        Entity.AfterDelay(250, e => { if (player.Call<int>("isusingturret") == 1) { Function.SetEntRef(-1); Function.Call("magicbullet", "sam_projectile_mp", ri1, dff(player), player); } });
                        Entity.AfterDelay(500, e => { if (player.Call<int>("isusingturret") == 1) { Function.SetEntRef(-1); Function.Call("magicbullet", "sam_projectile_mp", le2, dff(player), player); } });
                        Entity.AfterDelay(750, e => { if (player.Call<int>("isusingturret") == 1) { Function.SetEntRef(-1); Function.Call("magicbullet", "sam_projectile_mp", ri2, dff(player), player); } });

                        Entity.AfterDelay(5000, e => cooding = false);
                    }
                }
                catch (Exception)
                {
                    cooding = false;
                }
            };
        }
    }
}

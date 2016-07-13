using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class JetPack : BaseScript
    {
        public JetPack()
        {
            PlayerConnected += new Action<Entity>(player =>
            {
                player.SetField("readyjump", 1);
                player.Call("notifyonplayercommand", "jump", "+gostand");

                player.OnNotify("jump", ent =>
                {
                    if (player.GetField<int>("rtd_exo") == 1)
                    {
                        if (Ready(player) && player.Call<string>("getstance") == "stand" && player.GetField<int>("readyjump") == 1)
                        {
                            var vel = player.Call<Vector3>("getvelocity");

                            player.Call("setvelocity", new Vector3(vel.X, vel.Y, 600));
                            player.SetField("readyjump", 0);
                            OnInterval(100, () =>
                            {
                                if (player.Call<int>("IsOnGround") == 1)
                                {
                                    player.SetField("readyjump", 1);
                                    return false;
                                }
                                return true;
                            });
                        }
                    }
                });
            });
        }

        public bool Ready(Entity player)
        {
            if (player.IsPlayer && player.IsAlive && player.Call<int>("IsOnGround") == 1 && player.Call<int>("IsOnLadder") == 0 && player.Call<int>("IsMantling") == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

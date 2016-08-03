using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InfinityScript;

namespace INF3
{
    public class Sharpshooter : BaseScript
    {
        public static Weapon _firstWeapon;
        public static Weapon _mulekickWeapon;
        public static Weapon _secondeWeapon;

        private HudElem _cycleTimer;

        public static int _cycleRemaining = 30;

        public Sharpshooter()
        {
            UpdateWeapon();

            if (Utility.GetDvar<string>("g_gametype") == "infect")
            {
                OnInterval(100, () =>
                {
                    if (Call<int>("getteamscore", "axis") == 1)
                    {
                        SharpShooter_Tick();
                        return false;
                    }
                    return true;
                });
            }
            else
            {
                SharpShooter_Tick();
            }

            PlayerConnected += player =>
            {
                OnPlayerSpawned(player);
                player.SpawnedPlayer += () => OnPlayerSpawned(player);
            };
        }

        public void SharpShooter_Tick()
        {
            var _cycleTitle = HudElem.CreateServerFontString("objective", 1.4f);
            _cycleTitle.SetPoint("TOPLEFT", "TOPLEFT", 115, 5);
            _cycleTitle.HideWhenInMenu = true;
            _cycleTitle.SetText("Weapon Cycling: ");

            _cycleTimer = HudElem.CreateServerFontString("objective", 1.4f);
            _cycleTimer.SetPoint("TOPLEFT", "TOPLEFT", 255, 5);
            _cycleTimer.HideWhenInMenu = true;

            _cycleTimer.Call("settimer", _cycleRemaining - 1);

            OnInterval(1000, () =>
            {
                _cycleRemaining--;

                if (_cycleRemaining <= 0)
                {
                    _cycleRemaining = Utility.Random.Next(45, 90);
                    _cycleTimer.Call("settimer", _cycleRemaining);

                    UpdateWeapon();
                }

                return true;
            });
        }

        public void UpdateWeapon()
        {
            _firstWeapon = Weapon.GetRandomFirstWeapon();
            _secondeWeapon = Weapon.GetRandomSecondWeapon();
            _mulekickWeapon = Weapon.GetRandomFirstWeapon();

            while (_mulekickWeapon.BaseName == _firstWeapon.BaseName || _mulekickWeapon.Type == Weapon.WeaponType.Launcher || _mulekickWeapon.Type == Weapon.WeaponType.Killstreak || _mulekickWeapon.Type == Weapon.WeaponType.Special)
            {
                _mulekickWeapon = Weapon.GetRandomFirstWeapon();
            }

            foreach (var player in Players)
            {
                if (player.GetTeam() == "allies")
                {
                    player.TakeAllWeapons();

                    if (player.GetField<int>("perk_vulture") == 1)
                    {
                        player.GiveMaxAmmoWeapon(_firstWeapon.Code);
                        if (player.GetField<int>("perk_mulekick") == 1)
                        {
                            player.GiveMaxAmmoWeapon(_mulekickWeapon.Code);
                        }
                        player.GiveMaxAmmoWeapon(_secondeWeapon.Code);
                        player.GiveMaxAmmoWeapon("frag_grenade_mp");
                        player.GiveMaxAmmoWeapon("trophy_mp");
                        player.AfterDelay(100, ent =>
                        {
                            ent.SwitchToWeaponImmediate(_firstWeapon.Code);
                        });
                    }
                    else
                    {
                        if (_firstWeapon.Type == Weapon.WeaponType.Launcher || _firstWeapon.Type == Weapon.WeaponType.Killstreak)
                        {
                            player.GiveMaxAmmoWeapon(_firstWeapon.Code);
                        }
                        else
                        {
                            player.GiveWeapon(_firstWeapon.Code);
                        }
                        if (player.GetField<int>("perk_mulekick") == 1)
                        {
                            player.GiveWeapon(_mulekickWeapon.Code);
                        }
                        player.GiveMaxAmmoWeapon(_secondeWeapon.Code);
                        player.GiveMaxAmmoWeapon("frag_grenade_mp");
                        player.AfterDelay(100, ent =>
                        {
                            ent.SwitchToWeaponImmediate(_firstWeapon.Code);
                        });
                    }

                    player.GamblerText("Weapon Cycled", new Vector3(1, 1, 1), new Vector3(0.3f, 0.3f, 0.9f), 1, 0.85f);
                    player.PlayLocalSound(Sound.WeaponCycledSound);
                }
            }
        }

        public void OnPlayerSpawned(Entity player)
        {
            if (player.GetTeam() == "allies")
            {
                player.TakeAllWeapons();

                if (_firstWeapon.Type == Weapon.WeaponType.Launcher || _firstWeapon.Type == Weapon.WeaponType.Killstreak)
                {
                    player.GiveMaxAmmoWeapon(_firstWeapon.Code);
                }
                else
                {
                    player.GiveWeapon(_firstWeapon.Code);
                }
                player.GiveMaxAmmoWeapon(_secondeWeapon.Code);

                player.GiveWeapon("frag_grenade_mp");
                player.GiveWeapon("trophy_mp");

                player.AfterDelay(100, entity =>
                {
                    entity.SwitchToWeaponImmediate(_firstWeapon.Code);
                });

                player.OnInterval(100, e =>
                {
                    var weapon = player.CurrentWeapon;

                    if (player.HasField("perk_vulture") && player.GetField<int>("perk_vulture") == 1)
                    {
                        if (weapon.StartsWith("rpg") || weapon.StartsWith("iw5_smaw") || weapon.StartsWith("m320") || weapon.StartsWith("stinger") || weapon.StartsWith("javelin") || weapon.StartsWith("gl") || weapon.StartsWith("uav"))
                        {
                            player.Call("giveMaxAmmo", weapon);
                        }
                    }

                    if (player.GetTeam() == "axis")
                    {
                        return false;
                    }

                    return player.IsPlayer && player.IsAlive;
                });
            }
        }
    }
}

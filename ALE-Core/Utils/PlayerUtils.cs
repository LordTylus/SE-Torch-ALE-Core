using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;

namespace ALE_Core.Utils
{
    public class PlayerUtils {

        private static readonly string XBOX_ICON = "";
        private static readonly char PC_ICON = (char) 0xE030;

        public static bool isOnline(long playerId) {
            return MySession.Static.Players.IsPlayerOnline(playerId);
        }

        public static ulong GetSteamId(IMyPlayer player) {

            if (player == null)
                return 0L;

            return player.SteamUserId;
        }

        public static MyIdentity GetIdentityByNameOrId(string playerNameOrSteamId) {

            foreach (var identity in MySession.Static.Players.GetAllIdentities()) {

                if (identity.DisplayName == playerNameOrSteamId)
                    return identity;

                if (identity.DisplayName == PC_ICON + playerNameOrSteamId)
                    return identity;

                if (identity.DisplayName == XBOX_ICON + playerNameOrSteamId)
                    return identity;

                if (long.TryParse(playerNameOrSteamId, out long identityId)) 
                    if (identity.IdentityId == identityId)
                        return identity;

                if (ulong.TryParse(playerNameOrSteamId, out ulong steamId)) {

                    ulong id = MySession.Static.Players.TryGetSteamId(identity.IdentityId);
                    if (id == steamId)
                        return identity;
                }
            }

            return null;
        }

        public static MyIdentity GetIdentityByName(string playerName) {

            foreach (var identity in MySession.Static.Players.GetAllIdentities()) {

                if (identity.DisplayName == playerName)
                    return identity;

                if (identity.DisplayName == PC_ICON + playerName)
                    return identity;

                if (identity.DisplayName == XBOX_ICON + playerName)
                    return identity;
            }

            return null;
        }

        public static MyIdentity GetIdentityById(long playerId) {

            foreach (var identity in MySession.Static.Players.GetAllIdentities())
                if (identity.IdentityId == playerId)
                    return identity;

            return null;
        }

        public static string GetPlayerNameById(long playerId)
        {

            MyIdentity identity = GetIdentityById(playerId);

            if (identity != null)
                return identity.DisplayName;

            return "Nobody";
        }

        public static bool IsNpc(long playerId) {
            return MySession.Static.Players.IdentityIsNpc(playerId);
        }

        public static bool HasIdentity(long playerId) {
            return MySession.Static.Players.HasIdentity(playerId);
        }

        public static DateTime GetLastSeenDate(MyIdentity identity) {

            var lastLogoutTime = identity.LastLogoutTime;
            var lastLoginTime = identity.LastLoginTime;

            var date = lastLogoutTime;
            if (lastLoginTime > lastLogoutTime)
                date = lastLoginTime;

            return date;
        }
    }
}

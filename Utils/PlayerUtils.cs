using Sandbox.Game.World;
using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using VRage.Game.ModAPI;

namespace ALE_Core.Utils
{
    public class PlayerUtils
    {

        public static MyIdentity GetIdentityByName(string playerName) {

            foreach (var identity in MySession.Static.Players.GetAllIdentities())
                if (identity.DisplayName == playerName)
                    return identity;

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
    }
}

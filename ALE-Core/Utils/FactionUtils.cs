using Sandbox.Game.World;
using VRage.Game.ModAPI;

namespace ALE_Core.Utils
{
    public class FactionUtils
    {

        public static IMyFaction GetPlayerFaction(long playerId) {
            return MySession.Static.Factions.TryGetPlayerFaction(playerId);
        }

        public static bool HavePlayersSameFaction(long playerId1, long playerId2) {

            var faction1 = GetPlayerFaction(playerId1);
            var faction2 = GetPlayerFaction(playerId2);

            return faction1 == faction2;
        }

        public static string GetPlayerFactionTag(long playerId) {

            var faction = MySession.Static.Factions.TryGetPlayerFaction(playerId);

            if (faction == null)
                return "";

            return faction.Tag;
        }

        public static string GetFactionTagStringForPlayer(long playerId) {

            IMyFaction faction = GetPlayerFaction(playerId);

            string factionString = "";
            if (faction != null)
                factionString = "[" + faction.Tag + "]";

            return factionString;
        }

        public static IMyFaction GetIdentityByTag(string tag) {
            return MySession.Static.Factions.TryGetFactionByTag(tag);
        }

        public static IMyFaction GetIdentityById(long factionId) {
            return MySession.Static.Factions.TryGetFactionById(factionId);
        }
    }
}

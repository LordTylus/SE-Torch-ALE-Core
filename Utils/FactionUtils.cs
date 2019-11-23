using NLog;
using Sandbox.Game.Multiplayer;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Torch.Managers;
using VRage.Game.ModAPI;

namespace ALE_Core.Utils
{
    public class FactionUtils
    {
        Logger log = LogManager.GetCurrentClassLogger();

        public static string GetPlayerFactionTag(long playerId) {

            var faction = MySession.Static.Factions.TryGetPlayerFaction(playerId);

            if (faction == null)
                return "";

            return faction.Tag;
        }

        public static IMyFaction GetIdentityByTag(string tag) {
            return MySession.Static.Factions.TryGetFactionByTag(tag);
        }

        public static IMyFaction GetIdentityById(long factionId) {
            return MySession.Static.Factions.TryGetFactionById(factionId);
        }
    }
}

using NLog;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Torch.Commands;
using VRage.Game;
using VRage.Groups;

namespace ALE_Core.Utils {

    public class GridUtils {

        public static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly MyDefinitionId OXYGEN_DEFINITION = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Oxygen");
        private static readonly MyDefinitionId HYDROGEN_DEFINITION = MyDefinitionId.Parse("MyObjectBuilder_GasProperties/Hydrogen");

        private static string WildCardToRegular(string value) {
            return "^" + Regex.Escape(value).Replace("\\?", ".").Replace("\\*", ".*") + "$";
        }

        public static bool MatchesGridNameOrIdWithWildcard(MyCubeGrid grid, string nameOrId) {

            if (nameOrId == null)
                return true;

            string gridName = grid.DisplayName;

            var regex = WildCardToRegular(nameOrId);

            if (Regex.IsMatch(gridName, regex))
                return true;

            string entityIdAsString = grid.EntityId + "";

            if (Regex.IsMatch(entityIdAsString, regex))
                return true;

            return false;
        }

        public static bool MatchesGridNameOrId(MyCubeGrid grid, string nameOrId) {

            if (nameOrId == null)
                return true;

            string gridName = grid.DisplayName;

            if (gridName.Equals(nameOrId))
                return true;

            string entityIdAsString = grid.EntityId + "";

            if (entityIdAsString.Equals(nameOrId))
                return true;

            return false;
        }

        public static MyCubeGrid GetBiggestGridInGroup(IEnumerable<MyCubeGrid> grids) {

            MyCubeGrid biggestGrid = null;

            foreach (var grid in grids) {

                if (grid.Physics == null)
                    continue;

                if (biggestGrid == null || biggestGrid.BlocksCount < grid.BlocksCount)
                    biggestGrid = grid;
            }

            return biggestGrid;
        }

        public static bool Repair(MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group, CommandContext Context) {

            foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes) {

                MyCubeGrid grid = groupNodes.NodeData;

                var gridOwner = OwnershipUtils.GetOwner(grid);

                HashSet<MySlimBlock> blocks = grid.GetBlocks();
                foreach (MySlimBlock block in blocks) {

                    long owner = block.OwnerId;
                    if (owner == 0)
                        owner = gridOwner;

                    if (block.CurrentDamage > 0 || block.HasDeformation) {

                        block.ClearConstructionStockpile(null);
                        block.IncreaseMountLevel(block.MaxIntegrity, owner, null, 10000, true);

                        MyCubeBlock cubeBlock = block.FatBlock;

                        if (cubeBlock != null) {

                            grid.ChangeOwnerRequest(grid, cubeBlock, 0, MyOwnershipShareModeEnum.Faction);
                            if (owner != 0)
                                grid.ChangeOwnerRequest(grid, cubeBlock, owner, MyOwnershipShareModeEnum.Faction);
                        }
                    }
                }
            }

            return true;
        }

        public static bool Recharge(MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Group group, string blockpairName, float fillPercentage, CommandContext Context) {

            int changedBlocks = 0;
            int changedGrids = 0;

            foreach (MyGroups<MyCubeGrid, MyGridPhysicalGroupData>.Node groupNodes in group.Nodes) {

                MyCubeGrid grid = groupNodes.NodeData;
                bool gridChanged = false;

                HashSet<MySlimBlock> blocks = grid.GetBlocks();

                foreach (MySlimBlock block in blocks) {

                    MyCubeBlock cubeBlock = block.FatBlock;

                    if (cubeBlock != null) {

                        if (blockpairName == "battery" && cubeBlock is MyBatteryBlock battery) {

                            battery.CurrentStoredPower = battery.MaxStoredPower * fillPercentage;

                            changedBlocks++;
                            gridChanged = true;
                        }

                        if (blockpairName == "jumpdrive" && cubeBlock is MyJumpDrive jumpDrive) {

                            jumpDrive.CurrentStoredPower = jumpDrive.BlockDefinition.PowerNeededForJump * fillPercentage;

                            changedBlocks++;
                            gridChanged = true;
                        }

                        if ((blockpairName == "o2tank" || blockpairName == "tank") 
                            && cubeBlock is MyGasTank o2tank
                            && o2tank.BlockDefinition.StoredGasId == OXYGEN_DEFINITION) {

                            o2tank.ChangeFillRatioAmount(fillPercentage);

                            changedBlocks++;
                            gridChanged = true;
                        }

                        if ((blockpairName == "h2tank" || blockpairName == "tank")
                            && cubeBlock is MyGasTank h2tank
                            && h2tank.BlockDefinition.StoredGasId == HYDROGEN_DEFINITION) {

                            h2tank.ChangeFillRatioAmount(fillPercentage);

                            changedBlocks++;
                            gridChanged = true;
                        }
                    }
                }

                if (gridChanged)
                    changedGrids++;
            }

            Context.Respond(changedBlocks + " blocks on " + changedGrids + " grids recharged to "+(fillPercentage*100).ToString("0.0") +"%!");

            return true;
        }

        public static void TransferBlocksToBigOwner(HashSet<long> removedPlayers) {

            foreach (var entity in MyEntities.GetEntities()) {

                if (!(entity is MyCubeGrid grid))
                    continue;

                var newOwner = grid.BigOwners.FirstOrDefault();

                /* If new owner is nobody we share with all */
                var share = newOwner == 0 ? MyOwnershipShareModeEnum.All : MyOwnershipShareModeEnum.Faction;

                foreach (var block in grid.GetFatBlocks()) {

                    /* Nobody and players which werent deleted are ignored */
                    if (block.OwnerId == 0 || !removedPlayers.Contains(block.OwnerId))
                        continue;

                    grid.ChangeOwnerRequest(grid, block, 0, MyOwnershipShareModeEnum.Faction);
                    if (newOwner != 0)
                        grid.ChangeOwnerRequest(grid, block, newOwner, MyOwnershipShareModeEnum.Faction);
                }
            }
        }
    }
}

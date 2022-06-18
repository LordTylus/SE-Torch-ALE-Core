using Sandbox.ModAPI;
using System.Collections.Generic;
using VRage.ObjectBuilders;

namespace ALE_Core.EntitySpawner {
    
    public class EntitySpawner {

        /// <summary>
        /// Adds a collection of ObjectBuilders off game Thread in a parallel manner.
        /// Makes sure subgrids keep properly attached to their parents and things like
        /// Velocities are being reset to zero to not cause any issues later on.
        /// </summary>
        public static void SpawnInGridsPareallel(ICollection<MyObjectBuilder_EntityBase> grids) {

            /* 
             * Remap the OBs so that Subgrids are still phsically 
             * attached to their parents after adding with a new ID 
             */
            MyAPIGateway.Entities.RemapObjectBuilderCollection(grids);

            EntitySpawnCallback callback = new EntitySpawnCallback(grids.Count);

            foreach (var ObGrid in grids)
                MyAPIGateway.Entities.CreateFromObjectBuilderParallel(ObGrid, false, callback.Call);
        }

        /// <summary>
        /// Adds a collection of ObjectBuilders on game Thread in a sequential manner.
        /// Simulation Speed will suffer on huge grids.
        /// Makes sure subgrids keep properly attached to their parents and things like
        /// Velocities are being reset to zero to not cause any issues later on.
        /// </summary>
        public static void SpawnInGridsSequential(ICollection<MyObjectBuilder_EntityBase> grids) {

            /* 
             * Remap the OBs so that Subgrids are still phsically 
             * attached to their parents after adding with a new ID 
             */
            MyAPIGateway.Entities.RemapObjectBuilderCollection(grids);

            foreach (var ObGrid in grids)
                MyAPIGateway.Entities.CreateFromObjectBuilderAndAdd(ObGrid);
        }
    }
}

using Sandbox.Game.Entities;
using System.Collections.Generic;
using VRage.Game.Entity;
using VRage.ModAPI;
using VRageMath;

namespace ALE_Core.EntitySpawner {
    
    class MyEntityManager {

        /// <summary>
        /// This Method makes sure, that any entity that is NOT YET ADDED TO THE SCENE
        /// will be set up correctly. 
        /// 
        /// We will take care of things like Physics, Gravity and Speed so the grid 
        /// will not cause any trouble after adding to scene.
        /// </summary>
        public static void PrepareEntityForAdding(IMyEntity entity) {

            if (entity.Physics != null) {

                entity.Physics.Gravity = Vector3.Zero;
                entity.Physics.ClearSpeed();
                entity.Physics.Deactivate();
            }
        }

        /// <summary>
        /// This Method will make sure all Entities are being added to the scene. 
        /// Since usually the last Grid in the list is the biggest one and therefore the main grid,
        /// it will be added to the scene first. 
        /// </summary>
        public static void AddEntitiesToScene(ICollection<IMyEntity> entities) {

            var entityList = new List<MyEntity>();

            foreach (IMyEntity iEntity in entities)
                if (iEntity is MyEntity entity)
                    entityList.Add(entity);

            entityList.Reverse();

            foreach (MyEntity entity in entityList)
                AddSingleEntity(entity);
        }

        /// <summary>
        /// This Method will add a single entity to the scene, makes sure Gravity, 
        /// Physics etc. are properly enabled. 
        /// </summary>
        private static void AddSingleEntity(MyEntity entity) {

            bool hasPhysics = entity.Physics != null;

            if (hasPhysics) {

                entity.Physics.Activate();
                entity.Physics.Gravity = Vector3.Zero;
            }

            /*
             * Grids have to make sure their Disconnection Checks are paused 
             * for a frame, otherwise subgrids may fall apart. 
             */
            if (entity is MyCubeGrid grid) {

                grid.DetectDisconnectsAfterFrame();

                if (hasPhysics)
                    grid.Physics.DisableGravity = 2;
            }

            MyEntities.Add(entity, true);
        }
    }
}

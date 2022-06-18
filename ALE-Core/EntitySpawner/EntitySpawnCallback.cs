using System.Runtime.CompilerServices;
using VRage.Collections;
using VRage.ModAPI;

namespace ALE_Core.EntitySpawner {

    public class EntitySpawnCallback {
        
        private int targetGridCount;

        private readonly MyConcurrentList<IMyEntity> currentGridsList = new MyConcurrentList<IMyEntity>();

        public EntitySpawnCallback(int targetGridCount) {
            this.targetGridCount = targetGridCount;
        }

        /// <summary>
        /// This Method will take the newly created entity, prepare it and put
        /// it into the list of Entities waiting to be added. 
        /// 
        /// If we spawn in 5 Entities, this Method will be called 5 times. On the 5th time,
        /// all Entities waiting in the List (including the 5th one) will be added to the 
        /// Scene.
        /// 
        /// It is important to have this Method Synchronized, as Spawning in Entities is an
        /// Asynchronous task. And not Synchronizing the callback may result in lost update
        /// issues when accessing things like the list or count of entities to be added.
        /// 
        /// In such case the the Entities will never be added. 
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Call(IMyEntity ent) {

            MyEntityManager.PrepareEntityForAdding(ent);
            currentGridsList.Add(ent);

            if (currentGridsList.Count < targetGridCount)
                return;

            MyEntityManager.AddEntitiesToScene(currentGridsList);
        }
    }
}

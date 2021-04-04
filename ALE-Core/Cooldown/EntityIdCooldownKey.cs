using System;
using System.Collections.Generic;
using System.Text;

namespace ALE_Core.Cooldown {
    
    public class EntityIdCooldownKey : ICooldownKey {

        public long EntityId { get; }

        public EntityIdCooldownKey(long EntityId) {
            this.EntityId = EntityId;
        }

        public override bool Equals(object obj) {
            return obj is EntityIdCooldownKey key &&
                   EntityId == key.EntityId;
        }

        public override int GetHashCode() {
            return -1619204625 + EntityId.GetHashCode();
        }
    }
}

using System;

namespace PrototypeSystem {
    [Serializable]
    public abstract class BasePrototypeData : IPrototypeData {
        public string IDName { get; }
        public string BasedOn { get; }

        protected BasePrototypeData(string idName, string basedOn = null) {
            IDName = idName;
            BasedOn = basedOn;
        }
    }
}
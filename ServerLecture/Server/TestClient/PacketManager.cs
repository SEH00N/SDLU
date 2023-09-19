using System;

namespace TestClient
{
    public class PacketManager
    {
        private PacketManager instance = null;
        public PacketManager Instance {
            get {
                if (instance == null)
                    instance = new PacketManager();
                return instance;
            }
        }

        private PacketManager()
        {
            RegisterHandler();
        }

        private void RegisterHandler()
        {

        }
    }
}

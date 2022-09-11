using FishNet.Object;
namespace Players
{
    /// <summary>
    /// Instance of a player
    /// </summary>
    public class Player : NetworkBehaviour
    {
        public static Player Instance;
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!IsOwner)
                return;

            Instance = this;
        }
    }
}

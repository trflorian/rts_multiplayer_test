using Models;
using Unity.Netcode;
using UnityEngine;

namespace Buildings
{
    /// <summary>
    /// Building
    /// </summary>
    public class Building : NetworkBehaviour
    {
        private Renderer[] _allRenderers;
        
        private void Awake()
        {
            _allRenderers = GetComponentsInChildren<Renderer>();
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            Debug.Log("spawned, all renderers: " + _allRenderers.Length);
            foreach (var r in _allRenderers)
            {
                Debug.Log("changing color of renderer " + r);
                r.material.color = Constants.PlayerColors[OwnerClientId];
            }
        }
    }
}
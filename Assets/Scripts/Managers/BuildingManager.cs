using Buildings;
using Models;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class BuildingManager : NetworkBehaviour
    {
        [SerializeField] private Vector3[] spawnPoints;
        [SerializeField] private GameObject mainBuilding;

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            
            Debug.Log("spawning main building for local player");

            var clientId = NetworkManager.LocalClientId;
            var spawnPoint = spawnPoints[clientId];
            SpawnMainBuildingServerRpc(spawnPoint, clientId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SpawnMainBuildingServerRpc(Vector3 spawnPoint, ulong clientId)
        {
            Instantiate(mainBuilding, spawnPoint, Quaternion.identity).GetComponent<NetworkObject>()
                .SpawnAsPlayerObject(clientId);
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            SceneManager.LoadScene("LobbyScene");
        }
    }
}
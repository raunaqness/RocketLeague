using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        static public GameManager Instance;
        private GameObject instance;

        public GameObject WinnerUI;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]
        private GameObject playerPrefab;
        private GameObject ball;

        public GameObject Player1SpawnPosition;
        public GameObject Player2SpawnPosition;

        public GameObject BallSpawnTransform;

        private GameObject player1, player2;

        // Start Method

        // Update Method

        // Photon Methods

        //Helper Methods

    }
}
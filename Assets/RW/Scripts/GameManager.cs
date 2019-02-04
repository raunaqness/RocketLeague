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
        void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected) // 1
            {
                SceneManager.LoadScene("Launcher");
                return;
            }

            if (PlayerManager.LocalPlayerInstance == null)
            {
                if (PhotonNetwork.IsMasterClient) // 2
                {
                    Debug.Log("Instantiating Player 1");
                    // 3
                    player1 = PhotonNetwork.Instantiate("Car", Player1SpawnPosition.transform.position, Player1SpawnPosition.transform.rotation, 0);
                    // 4
                    ball = PhotonNetwork.Instantiate("Ball", BallSpawnTransform.transform.position, BallSpawnTransform.transform.rotation, 0);
                    ball.name = "Ball";
                }
                else // 5
                {
                    player2 = PhotonNetwork.Instantiate("Car", Player2SpawnPosition.transform.position, Player2SpawnPosition.transform.rotation, 0);

                }
            }
        }

        // Update Method
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        // Photon Methods
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel("Launcher");
            }
        }

        //Helper Methods

    }
}
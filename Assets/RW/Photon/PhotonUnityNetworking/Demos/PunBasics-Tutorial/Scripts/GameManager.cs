// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

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

        void Start()
        {
            Instance = this;

            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Launcher");
                return;
            }

            if (playerPrefab != null)
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("Instantiating Player 1");
                        player1 = PhotonNetwork.Instantiate(this.playerPrefab.name, Player1SpawnPosition.transform.position, Player1SpawnPosition.transform.rotation, 0);
                        ball = PhotonNetwork.Instantiate("Ball", BallSpawnTransform.transform.position, BallSpawnTransform.transform.rotation, 0);
                        ball.name = "Ball";
                    }
                    else
                    {
                        player2 = PhotonNetwork.Instantiate(this.playerPrefab.name, Player2SpawnPosition.transform.position, Player2SpawnPosition.transform.rotation, 0);
                        Debug.Log("Instantiating Player 2");
                    }
                }
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitApplication();
            }
        }

        // Photon Methods
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                LoadArena();
            }
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Player left room");
            SceneManager.LoadScene("Launcher");
        }

        //Helper Methods
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void DisableUI()
        {
            WinnerUI.SetActive(false);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            PhotonNetwork.LoadLevel("Launcher");
        }
    }
}
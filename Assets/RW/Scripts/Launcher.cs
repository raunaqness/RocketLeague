using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
    
        [SerializeField]
        private GameObject controlPanel;

        [SerializeField]
        private Text feedbackText;

        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        bool isConnecting;

        string gameVersion = "1";

        [Space(10)]
        [Header("Custom Variables")]
        public InputField PlayerName;
        public InputField RoomName;

        [Space(5)]
        public Text PlayerStatus;
        public Text ConnectionStatus;

        [Space(5)]
        public GameObject RoomJoinUI;
        public GameObject Button_LoadArena;
        public GameObject Button_JoinRoom;

        string _PlayerName = "";
        string _RoomName = "";

        // Start Method
        void Start()
        {
            // 1
            PlayerPrefs.DeleteAll();

            Debug.Log("Connecting to Photon Network");

            //2
            RoomJoinUI.SetActive(false);
            Button_LoadArena.SetActive(false);

            //3
            ConnectToPhoton();
        }

        void Awake()
        {
            //4 
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Helper Methods
        public void SetPlayerName(string s)
        {
            _PlayerName = s;
        }

        public void SetRoomName(string s)
        {
            _RoomName = s;
        }

        // Tutorial Methods
        void ConnectToPhoton()
        {
            ConnectionStatus.text = "Connecting...";
            PhotonNetwork.GameVersion = gameVersion; //1
            PhotonNetwork.ConnectUsingSettings(); //2
        }

        public void JoinRoom()
        {
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.LocalPlayer.NickName = _PlayerName; //1
                Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room" + RoomName.text);
                RoomOptions roomOptions = new RoomOptions(); //2
                TypedLobby typedLobby = new TypedLobby(_RoomName, LobbyType.Default); //3
                PhotonNetwork.JoinOrCreateRoom(_RoomName, roomOptions, typedLobby); //4
            }
        }

        public void LoadArena()
        {
            // 5
            if (PhotonNetwork.CurrentRoom.PlayerCount > 0)
            {
                PhotonNetwork.LoadLevel("MainArena");
            }
            else
            {
                PlayerStatus.text = "Minimum 2 Players required to Load Arena!";
            }
        }


        // Photon Methods

        public override void OnConnected()
        {
            // 1
            base.OnConnected();
            // 2
            ConnectionStatus.text = "Connected to Photon!";
            ConnectionStatus.color = Color.green;
            RoomJoinUI.SetActive(true);
            Button_LoadArena.SetActive(false);
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            // 3
            isConnecting = false;
            controlPanel.SetActive(true);
            Debug.LogError("Disconnected. Please check your Internet connection.");
        }

        public override void OnJoinedRoom()
        {

            // 4
            if (PhotonNetwork.IsMasterClient)
            {
                Button_LoadArena.SetActive(true);
                Button_JoinRoom.SetActive(false);
                PlayerStatus.text = "Your are Lobby Leader";
            }
            else
            {
                PlayerStatus.text = "Connected to Lobby";
            }
        }

    } 
}
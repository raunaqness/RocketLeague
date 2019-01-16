// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to connect, and join/create room automatically
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
	#pragma warning disable 649

    /// <summary>
    /// Launch manager. Connect, join a random room or create one if none or all full.
    /// </summary>
	public class Launcher : MonoBehaviourPunCallbacks
    {

		#region Private Serializable Fields

		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		[SerializeField]
		private GameObject controlPanel;

		[Tooltip("The Ui Text to inform the user about the connection progress")]
		[SerializeField]
		private Text feedbackText;

		[Tooltip("The maximum number of players per room")]
		[SerializeField]
		private byte maxPlayersPerRoom = 2;

        [Tooltip("The UI Loader Anime")]
        [SerializeField]
        private LoaderAnime loaderAnime;

		#endregion

		#region Private Fields

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
        public string _PlayerName = "";
        public string _RoomName = "";

        [Space(5)]
        public GameObject RoomJoinUI;
        public GameObject Button_LoadArena;
        public GameObject Button_JoinRoom;

        void Start(){
            Debug.Log("Connecting to Photon Network");
            RoomJoinUI.SetActive(false);
            Button_LoadArena.SetActive(false);
            ConnectToPhoton();
        }

        void Awake()
		{
			if (loaderAnime==null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> loaderAnime Reference.",this);
			}

			PhotonNetwork.AutomaticallySyncScene = true;

		}

		public void Connect()
		{

            if (PhotonNetwork.IsConnected){

                PhotonNetwork.LocalPlayer.NickName = _PlayerName;
                Debug.Log("PhotonNetwork.IsConnected! | Trying to Create/Join Room" + RoomName.text);
                RoomOptions roomOptions = new RoomOptions();
                TypedLobby typedLobby = new TypedLobby(_RoomName, LobbyType.Default);
                PhotonNetwork.JoinOrCreateRoom(_RoomName, roomOptions, typedLobby);

            }else{

            }
		}

        void ConnectToPhoton(){
            ConnectionStatus.text = "Connecting...";
            PhotonNetwork.GameVersion = this.gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void SetPlayerName(string s)
        {
            _PlayerName = s;
        }

        public void SetRoomName(string s)
        {
            _RoomName = s;
        }

        void LogFeedback(string message)
		{
			if (feedbackText == null) {
				return;
			}

			feedbackText.text += System.Environment.NewLine+message;
		}

        #endregion


        #region MonoBehaviourPunCallbacks CallBacks

        public override void OnConnected()
        {
            base.OnConnected();
            ConnectionStatus.text = "Connected to Photon!";
            ConnectionStatus.color = Color.green;
            RoomJoinUI.SetActive(true);
            Button_LoadArena.SetActive(false);

        }

        public void JoinGGRoom()
        {
            PhotonNetwork.JoinRoom("Ggroom");
        }

        public override void OnConnectedToMaster()
		{
			if (isConnecting)
			{
				LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
				Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

            }
		}

		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
			Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom});
		}


		public override void OnDisconnected(DisconnectCause cause)
		{
			LogFeedback("<Color=Red>OnDisconnected</Color> "+cause);
            loaderAnime.StopLoaderAnimation();

			isConnecting = false;
			controlPanel.SetActive(true);

		}

		public override void OnJoinedRoom()
		{

            HowManyPlayersInLobby();

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

        public void LoadArena()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                PhotonNetwork.LoadLevel("MainArena");
            }
            else
            {
                PlayerStatus.text = "Minimum 2 Players required to Load Arena!";
            }
        }

        public void HowManyPlayersInLobby()
        {

            Debug.Log("Total players in Lobby : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());
            Debug.Log("Room Info | Name " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Room Info | Info " + PhotonNetwork.CurrentRoom.ToString());

        }

        #endregion

    }
}
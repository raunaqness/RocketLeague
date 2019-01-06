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
		/// <summary>
		/// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon, 
		/// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
		/// Typically this is used for the OnConnectedToMaster() callback.
		/// </summary>
		bool isConnecting;

		/// <summary>
		/// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
		/// </summary>
		string gameVersion = "1";

        [Space(10)]
        [Header("Custom Variables")]
        public InputField PlayerName;
        public InputField RoomName;

        [Space(5)]
        public Text PlayersInLobby;
        public Text ConnectionStatus;

        [Space(5)]
        public string _PlayerName = "";
        public string _RoomName = "";

        [Space(5)]
        public GameObject RoomJoinUI;
        public GameObject LoadArenaButton;

        void Start(){
            Debug.Log("Connecting to Photon Network");
            RoomJoinUI.SetActive(false);
            ConnectToPhoton();
        }

        void Awake()
		{
			if (loaderAnime==null)
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> loaderAnime Reference.",this);
			}

			// #Critical
			// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
			PhotonNetwork.AutomaticallySyncScene = true;

		}

		public void Connect()
		{
			//feedbackText.text = "";
            //isConnecting = true;

			//controlPanel.SetActive(false);

		 //   if (loaderAnime!=null){
			//	loaderAnime.StartLoaderAnimation();
			//}



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
			// we do not assume there is a feedbackText defined.
			if (feedbackText == null) {
				return;
			}

			// add new messages as a new line and at the bottom of the log.
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
            LoadArenaButton.SetActive(false);


            //Debug.Log("Total players in Lobby : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());
            //Debug.Log("Room Info | Name " + PhotonNetwork.CurrentRoom.Name);
            //Debug.Log("Room Info | Info " + PhotonNetwork.CurrentRoom.ToString());
        }

        public void JoinGGRoom()
        {
            PhotonNetwork.JoinRoom("Ggroom");
        }

        public override void OnConnectedToMaster()
		{
            // we don't want to do anything if we are not attempting to join a room. 
			// this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
			// we don't want to do anything.
			if (isConnecting)
			{
				LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
				Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");

                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                //Connect();
            }
		}

		/// <summary>
		/// Called when a JoinRandom() call failed. The parameter provides ErrorCode and message.
		/// </summary>
		/// <remarks>
		/// Most likely all rooms are full or no rooms are available. <br/>
		/// </remarks>
		public override void OnJoinRandomFailed(short returnCode, string message)
		{
			LogFeedback("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
			Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

			// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
			PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom});
		}


		/// <summary>
		/// Called after disconnecting from the Photon server.
		/// </summary>
		public override void OnDisconnected(DisconnectCause cause)
		{
			LogFeedback("<Color=Red>OnDisconnected</Color> "+cause);
			//Debug.LogError("PUN Basics Tutorial/Launcher:Disconnected");

			// #Critical: we failed to connect or got disconnected. There is not much we can do. Typically, a UI system should be in place to let the user attemp to connect again.
			loaderAnime.StopLoaderAnimation();

			isConnecting = false;
			controlPanel.SetActive(true);

		}

		/// <summary>
		/// Called when entering a room (by creating or joining it). Called on all clients (including the Master Client).
		/// </summary>
		/// <remarks>
		/// This method is commonly used to instantiate player characters.
		/// If a match has to be started "actively", you can call an [PunRPC](@ref PhotonView.RPC) triggered by a user's button-press or a timer.
		///
		/// When this is called, you can usually already access the existing players in the room via PhotonNetwork.PlayerList.
		/// Also, all custom properties should be already available as Room.customProperties. Check Room..PlayerCount to find out if
		/// enough players are in the room to start playing.
		/// </remarks>
		public override void OnJoinedRoom()
		{

            HowManyPlayersInLobby();
            if (PhotonNetwork.IsMasterClient)
            {
                LoadArenaButton.SetActive(true);
            }

            //PhotonNetwork.LoadLevel("MainArena");




            //         if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            //{
            //	Debug.Log("We load the 'Room for 1' ");

            //	// #Critical
            //	// Load the Room Level. 
            //	PhotonNetwork.LoadLevel("MainArena");

            //}
        }

        public void LoadArena()
        {
            PhotonNetwork.LoadLevel("MainArena");
        }

        public void HowManyPlayersInLobby()
        {

            Debug.Log("Total players in Lobby : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());
            Debug.Log("Room Info | Name " + PhotonNetwork.CurrentRoom.Name);
            Debug.Log("Room Info | Info " + PhotonNetwork.CurrentRoom.ToString());

            PlayersInLobby.text = "Players : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString();

        }

        #endregion

    }
}
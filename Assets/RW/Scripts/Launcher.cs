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

        // Helper Methods

        // Tutorial Methods

        // Photon Methods

    } 
}
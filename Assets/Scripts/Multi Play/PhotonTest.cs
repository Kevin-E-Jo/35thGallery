using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    private byte maxPlayersPerRoom = 0;
	bool isConnecting;

	/// <summary>
	/// 
	/// </summary>
	//private void Awake()
	private void Start()
	{
		Initialize();
	}

	public void Initialize()
    {
		DontDestroyOnLoad(this.gameObject);
		PhotonNetwork.AutomaticallySyncScene = true;
		Connect();
	}

	/// <summary>
    /// 
    /// </summary>
	public void Connect()
	{
#if UNITY_EDITOR
		//LocalRepository.Instance.UserProfile = new UserProfile();
		//LocalRepository.Instance.UserProfile.nickName;
#endif
		//PhotonNetwork.NickName = GameObject.Find("GameManager").GetComponent<GameManager>().state.nickName;

		isConnecting = true;
		if (PhotonNetwork.IsConnected)
		{
			Debug.Log("Connected...");
			//PhotonNetwork.JoinRandomRoom();
			PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = this.maxPlayersPerRoom }, null, null);
		}
		else
		{
			Debug.Log("Connecting...");
			PhotonNetwork.ConnectUsingSettings();
		}

		StartCoroutine(StartLoadingMonitoring());
	}
	
	private IEnumerator StartLoadingMonitoring()
	{
		while (PhotonNetwork._AsyncLevelLoadingOperation == null)
		{
			Debug.Log("LoadScene process didn't start");
			yield return new WaitForFixedUpdate();
		}

		while (PhotonNetwork._AsyncLevelLoadingOperation.progress < 0.99)
		{
			yield return new WaitForFixedUpdate();
			if(PhotonNetwork._AsyncLevelLoadingOperation!=null)
				ReactCommunicator.Instance.SendLoadScenePercent(PhotonNetwork._AsyncLevelLoadingOperation.progress);
			else
				break;
		}
		
		StartCoroutine(StartConnectionMonitoring());

		yield return null;
	}
	
	public IEnumerator StartConnectionMonitoring()
	{
		while (PhotonNetwork.IsConnected)
		{
			yield return new WaitForFixedUpdate();
		}
		
		Debug.Log("PhotonNetwork disconnected");
		ReactCommunicator.Instance.SendDisconnectSign();

		yield return null;
	}
	
	/// <summary>
    /// 
    /// </summary>
	public override void OnConnectedToMaster()
	{
		if(isConnecting){

			Debug.Log("OnConnectedToMaster");
			//PhotonNetwork.JoinRandomRoom();
			PhotonNetwork.JoinOrCreateRoom("test", new RoomOptions { MaxPlayers = this.maxPlayersPerRoom }, null, null);
		}
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
		Debug.Log("<Color=Green>OnJoinedRoom</Color> with " + PhotonNetwork.CurrentRoom.PlayerCount + " Player(s)");
		Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running.");

		// #Critical: We only load if we are the first player, else we rely on  PhotonNetwork.AutomaticallySyncScene to sync our instance scene.
		if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
		{
			Debug.Log("We load the 'Room for 1' ");

			// #Critical
			// Load the Room Level. 
			//PhotonNetwork.LoadLevel("PunBasics-Room for 1");
			//GameEvents.Instance.RequestLoadingSetActive(false);
			PhotonNetwork.LoadLevel("3D_GalleryC");
			//StartCoroutine(StartLoadingMonitoring(PhotonNetwork.LoadLevel("3D_GalleryC")));
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
		Debug.Log("<Color=Red>OnJoinRandomFailed</Color>: Next -> Create a new Room");
		Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

		// #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
		PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayersPerRoom });
	}

}

using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Chat.Demo;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ChatTest : MonoBehaviour, IChatClientListener
{
    private ChatClient chatClient;
    private PhotonChatGUI chatGUI;
    protected internal ChatAppSettings chatAppSettings;
    private string selectedChannelName;

    public string UserName { get; set; }

    public void Initialize()
    {
#if PHOTON_UNITY_NETWORKING
        this.chatAppSettings = PhotonNetwork.PhotonServerSettings.AppSettings.GetChatSettings();
        this.chatGUI = GameObject.FindGameObjectWithTag("PhotonChatGUI").GetComponent<PhotonChatGUI>();
        this.chatGUI.Initialize(this);
        Connect();
#endif
    } 

    void Update()
    {
        if (this.chatClient != null)
        {
            // make sure to call this regularly! it limits effort internally, so calling often is ok!
            this.chatClient.Service(); 
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Connect()
    {
        this.chatClient = new ChatClient(this);
#if !UNITY_WEBGL
            this.chatClient.UseBackgroundWorkerForSending = true;
#endif
        /* User 고유 번호 */
        //UserName = gameObject.GetInstanceID().ToString();

        string userNickname = LocalRepository.Instance.UserProfile.nickName;

        if (8 < userNickname.Length)
            userNickname = userNickname.Substring(0, 8).PadRight(10, '.');

        UserName = userNickname;

        this.chatClient.AuthValues = new Photon.Chat.AuthenticationValues(this.UserName);
        this.chatClient.ConnectUsingSettings(this.chatAppSettings);
        Debug.Log("Connecting as: " + this.UserName);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log(state.ToString());
    }

    public void OnConnected()
    {
        this.chatClient.Subscribe("tnmeta");
        // You can set your online state (without a mesage).
        this.chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void ShowChannel(string channelName)
    {
        if (string.IsNullOrEmpty(channelName))
        {
            return;
        }

        ChatChannel channel = null;
        bool found = this.chatClient.TryGetChannel(channelName, out channel);

        chatGUI.CurrentChannelText.text = channel.ToStringMessages();

        if (!found)
        {
            Debug.Log("ShowChannel failed to find channel: " + channelName);
            return;
        }
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        Debug.Log("OnGetMessages");
        if (channelName.Equals(this.selectedChannelName))
        {
            // update text
            this.ShowChannel(channelName);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        foreach (string channel in channels)
        {
            //this.chatClient.PublishMessage(channel, "says 'hi'."); // you don't HAVE to send a msg on join but you could.
        }

        selectedChannelName = channels[0];
        Debug.Log("OnSubscribed: " + string.Join(", ", channels));
        this.ShowChannel(channels[0]);
    }

    public void OnClickSend()
    {
        //this.SendChatMessage(this.InputFieldChat.text);
        this.SendChatMessage("message Test");
    }

    public int TestLength = 2048;
    private byte[] testBytes = new byte[2048];

    public void SendChatMessage(string inputLine)
    {
        if (string.IsNullOrEmpty(inputLine))
        {
            return;
        }

        this.chatClient.PublishMessage(this.selectedChannelName, inputLine);        
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}

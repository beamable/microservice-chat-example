using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable;
using Beamable.Api.Autogenerated.Models;
using Beamable.Common.Models;
using Beamable.Server.Clients;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatRoom : MonoBehaviour
{
    private string _roomName;
    private BeamContext _beamContext;
    
    private BackendRoomServiceClient _backendService;
    private List<Message> _activeGroupChatMessages;
    
    [SerializeField] 
    private TMP_Text roomNameText;
    [SerializeField]
    private TMP_Text chatLogText;
    [SerializeField]
    private TMP_InputField messageInput;

    private async void Start()
    {
        _beamContext = await BeamContext.Default.Instance;

        await _beamContext.Accounts.OnReady;
        
        _backendService = new BackendRoomServiceClient();

        _roomName = PlayerPrefs.GetString("SelectedRoomName", string.Empty);
        roomNameText.text = _roomName;

        await JoinRoom(_beamContext.PlayerId, _roomName);

        _beamContext.Api.NotificationService.Subscribe(_roomName, HandleNotification);
        
        LoadChatHistory(); 
    }

    private async Task JoinRoom(long gamerTag, string roomName)
    {
        var response = await _backendService.JoinRoom(gamerTag, roomName);
        if (!response.data)
        {
            Debug.LogError($"Error joining room: {response.errorMessage}");
        }
    }

    private async void LoadChatHistory()
    {
        chatLogText.text = "";
        var response = await _backendService.GetRoomHistory(_roomName);
        if (!string.IsNullOrEmpty(response.errorMessage))
        {
            Debug.LogError($"Error loading chat history: {response.errorMessage}");
            return;
        }

        foreach (var message in response.data)
        {
            var username = await _backendService.GetPlayerAvatarName(message.senderGamerTag);
            string roomMessage = $"{username.data}: {message.content}";
            chatLogText.text += $"{roomMessage}\n";
        }
    }

    public async void SendMessage()
    {
        var messageText = messageInput.text;
        if (string.IsNullOrEmpty(messageText))
            return;

        if (messageText.StartsWith("/add "))
        {
            var username = messageText.Substring(5);
            await AddUserToRoom(username);
        }
        else
        {
            var response = await _backendService.SendMessage(_beamContext.PlayerId, _roomName, messageText);
            if (!response.data)
            {
                Debug.LogError($"Error sending message: {response.errorMessage}");
            }
        }

        messageInput.text = "";
    }

    private async Task AddUserToRoom(string username)
    {
        var playerDataResponse = await _backendService.GetPlayerDataByUsername(username);
        if (playerDataResponse.data.gamerTag > 0)
        {
            var joinRoomResponse = await _backendService.JoinRoom(playerDataResponse.data.gamerTag, _roomName);
            if (joinRoomResponse.data)
            {
                chatLogText.text += $"{username} has been added to the room.\n";
            }
            else
            {
                chatLogText.text += $"Failed to add {username} to the room: {joinRoomResponse.errorMessage}\n";
            }
        }
        else
        {
            chatLogText.text += $"Not a registered user: {username}\n";
        }
    }

    public async void ForgetRoom()
    {
        var response = await _backendService.LeaveRoom(_beamContext.PlayerId, _roomName);
        if (!response.data)
        {
            Debug.LogError($"Error leaving room: {response.errorMessage}");
        }
        else
        {
            SceneManager.LoadScene("ChatRooms");
        }
    }
    
    private async void HandleNotification(object payload)
    {
        try
        {
            if (payload is Beamable.Serialization.SmallerJSON.ArrayDict arrayDict)
            {
                if (arrayDict.TryGetValue("stringValue", out var jsonString))
                {
                    var message = JsonUtility.FromJson<MessageData>(jsonString.ToString());
                    Debug.Log($"Message content: {message.senderGamerTag} {message.content}");
                    await UpdateChatLog(message);
                }
                else
                {
                    Debug.LogError("No 'stringValue' found in payload");
                }
            }
            else
            {
                Debug.LogError("Payload is not an ArrayDict");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error parsing payload: {e.Message}");
        }
    }

    private async Task UpdateChatLog(MessageData message)
    {
        var usernameResponse = await _backendService.GetPlayerAvatarName(message.senderGamerTag);
        var roomMessage = $"{usernameResponse.data}: {message.content}";
        chatLogText.text += $"{roomMessage}\n";
    }
}

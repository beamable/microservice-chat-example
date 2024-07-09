using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable;
using Beamable.Common.Models;
using Beamable.Server.Clients;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlockedUsersManager : MonoBehaviour
{
    private BeamContext _beamContext;
    private BackendRoomServiceClient _backendService;

    [SerializeField] private GameObject blockedUserItemPrefab;
    [SerializeField] private Transform blockedUsersContainer;

    private async void Start()
    {
        _beamContext = await BeamContext.Default.Instance;
        await _beamContext.Accounts.OnReady;
        _backendService = new BackendRoomServiceClient();

        await LoadBlockedUsers();
    }

    private async Task LoadBlockedUsers()
    {
        var response = await _backendService.GetBlockedUsers(_beamContext.PlayerId);
        if (response.data != null && response.data.Count > 0)
        {
            foreach (var blockedUserTag in response.data)
            {
                var playerDataResponse = await _backendService.GetPlayerAvatarName(blockedUserTag);
                if (!string.IsNullOrEmpty(playerDataResponse.data))
                {
                    var blockedUserItem = Instantiate(blockedUserItemPrefab, blockedUsersContainer);
                    var usernameText = blockedUserItem.transform.Find("UsernameText").GetComponent<TMP_Text>();
                    var unblockButton = blockedUserItem.transform.Find("UnblockButton").GetComponent<Button>();

                    usernameText.text = playerDataResponse.data;
                    unblockButton.onClick.AddListener(() => UnblockUser(blockedUserTag, blockedUserItem));
                }
            }
        }
        else
        {
            Debug.LogError($"Error loading blocked users: {response.errorMessage}");
        }
    }

    private async void UnblockUser(long blockedGamerTag, GameObject blockedUserItem)
    {
        var response = await _backendService.UnblockUser(_beamContext.PlayerId, blockedGamerTag);
        if (response.data)
        {
            Destroy(blockedUserItem);
        }
        else
        {
            Debug.LogError($"Error unblocking user: {response.errorMessage}");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("PreviousScene"); // Replace "PreviousScene" with the actual name of the previous scene
    }
}

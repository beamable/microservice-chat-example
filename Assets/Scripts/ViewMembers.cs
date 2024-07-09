using System.Collections.Generic;
using System.Threading.Tasks;
using Beamable;
using Beamable.Server.Clients;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ViewMembers : MonoBehaviour
{
    private string _roomName;
    private BeamContext _beamContext;
    private BackendRoomServiceClient _backendService;

    [SerializeField] private GameObject memberItemPrefab;
    [SerializeField] private Transform membersContainer;
    [SerializeField] private TMP_Text roomTitle;

    private async void Start()
    {
        _beamContext = await BeamContext.Default.Instance;
        await _beamContext.Accounts.OnReady;
        _backendService = new BackendRoomServiceClient();

        _roomName = PlayerPrefs.GetString("SelectedRoomName", string.Empty);
        roomTitle.text = $"{_roomName}'s members";
        await LoadRoomMembers();
    }

    private async Task LoadRoomMembers()
    {
        var response = await _backendService.GetRoomMembers(_roomName);
        if (response.data.Count > 0)
        {
            foreach (var member in response.data)
            {
                var memberItem = Instantiate(memberItemPrefab, membersContainer);
                var usernameText = memberItem.transform.Find("UsernameText").GetComponent<TMP_Text>();
                var kickButton = memberItem.transform.Find("KickButton").GetComponent<Button>();

                usernameText.text = member.avatarName;

                if (member.gamerTag == _beamContext.PlayerId)
                {
                    // Disable or hide the kick button for the current user
                    kickButton.gameObject.SetActive(false);
                }
                else
                {
                    kickButton.onClick.AddListener(() => KickMember(member.gamerTag, memberItem));
                }
            }
        }
        else
        {
            Debug.LogError($"Error loading room members: {response.errorMessage}");
        }
    }

    private async void KickMember(long gamerTag, GameObject memberItem)
    {
        var response = await _backendService.KickMember(gamerTag, _roomName);
        if (response.data)
        {
            Destroy(memberItem);
        }
        else
        {
            Debug.LogError($"Error kicking member: {response.errorMessage}");
        }
    }

    public void GoBack()
    {
        SceneManager.LoadScene("ChatRoom");
    }
}

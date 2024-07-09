using Beamable;
using Beamable.Server.Clients;
using TMPro;
using UnityEngine;

public class SaveUsername : MonoBehaviour
{
    private BeamContext _beamContext;
    private BackendRoomServiceClient _backendService;

    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_Text messageText;

    private async void Start()
    {
        _beamContext = await BeamContext.Default.Instance;
        _backendService = new BackendRoomServiceClient();

    }

    public async void OnSubmitButtonClicked()
    {
        var username = usernameInput.text;
        if (string.IsNullOrEmpty(username))
        {
            messageText.text = "Username cannot be empty.";
            return;
        }

        var gamerTag = _beamContext.PlayerId;
        var response = await _backendService.SetPlayerAvatarName(gamerTag, username);
        messageText.text = response.data ? "Username registered!" : response.errorMessage;
    }
}

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Beamable;
    using Beamable.Server.Clients;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    public class ChatRooms: MonoBehaviour
    {
        private BeamContext _beamContext;

        [SerializeField]
        private Button roomButtonPrefab;
        [SerializeField]
        private Transform roomListContent;
        [SerializeField]
        private TMP_InputField chatNameInput;

        private BackendRoomServiceClient _backendService;
        private List<string> _chatRooms;

        protected async void Start()
        {
            await SetupBeamable();

            _backendService = new BackendRoomServiceClient();

            await GetUserRooms();
        }

        private async Task SetupBeamable()
        {
            _beamContext = await BeamContext.Default.Instance;
        }

        private async Task GetUserRooms()
        {
            var response = await _backendService.GetUserRooms(_beamContext.PlayerId);
            if (!string.IsNullOrEmpty(response.errorMessage))
            {
                Debug.LogError($"Error loading user rooms: {response.errorMessage}");
                return;
            }
            Debug.Log(response.data.Count);
            _chatRooms = response.data;
            DisplayRooms(_chatRooms);
        }

        private void DisplayRooms(List<string> rooms)
        {
            // Clear existing buttons
            foreach (Transform child in roomListContent)
            {
                Destroy(child.gameObject);
            }

            if (rooms.Count == 0)
            {
                Debug.Log(rooms.Count);
                // Display a message indicating no rooms are available
                var messageText = Instantiate(roomButtonPrefab, roomListContent);
                messageText.GetComponentInChildren<TextMeshProUGUI>().text = "No rooms available";
                return;
            }

            var count = 1;
            foreach (var roomName in rooms)
            {
                Debug.Log(roomName);
                var button = Instantiate(roomButtonPrefab, roomListContent);
                button.GetComponentInChildren<TextMeshProUGUI>().text = $"{count}. {roomName}";
                button.onClick.AddListener(() => OnGroupClick(roomName));
                count++;
            }
        }

        private void OnGroupClick(string roomName)
        {
            NavigateToRoom(roomName);
        }
        
        public void StartChatButton()
        {
            var roomName = chatNameInput.text;
            NavigateToRoom(roomName);
        }

        private void NavigateToRoom(string roomName)
        {
            PlayerPrefs.SetString("SelectedRoomName", roomName);
            SceneManager.LoadScene("ChatRoom");
        }
    }
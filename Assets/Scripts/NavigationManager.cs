using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }
    
    public void LoadChatRoomsScene()
    {
        SceneManager.LoadScene("ChatRooms");
    }

    public void LoadBlockedPlayers()
    {
        SceneManager.LoadScene("BlockedPlayers");
    }
}
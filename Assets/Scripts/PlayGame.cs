using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    public void PlayGameButton()
    {
        SceneManager.LoadScene("GamePlay");
    }
}

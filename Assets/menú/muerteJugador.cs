using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private GameObject deathPanel;

    private void Start()
    {
        if (deathPanel != null)
            deathPanel.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true);

        // Opcional: pausa el juego
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
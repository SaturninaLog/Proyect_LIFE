using UnityEngine;
using UnityEngine.SceneManagement;

public class InteraccionBotones : MonoBehaviour
{
    // --- PARA CARGAR ESCENAS ---
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    // --- PARA ABRIR Y CERRAR PANELES ---

    //Activa el panel que se coloque en el inspector
    public void AbrirPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    //desactiva el panel que se coloque en el inspector
    public void CerrarPanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    // Alterna el estado del panel (si está activo lo cierra, si no lo abre)
    public void TogglePanel(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
    }
}

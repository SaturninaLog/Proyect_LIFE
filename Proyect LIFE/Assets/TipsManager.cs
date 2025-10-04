using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI tipText; // Cambia a Text si no usas TMP
    public Button prevButton;
    public Button nextButton;

    [Header("Tips")]
    [TextArea]
    public string[] tips; // Aquí escribes los tips en el inspector

    private int currentIndex = 0;

    void Start()
    {
        if (tips.Length > 0)
            tipText.text = tips[currentIndex];

        prevButton.onClick.AddListener(PreviousTip);
        nextButton.onClick.AddListener(NextTip);
    }

    void PreviousTip()
    {
        currentIndex--;
        if (currentIndex < 0)
            currentIndex = tips.Length - 1; // Vuelve al último

        tipText.text = tips[currentIndex];
    }

    void NextTip()
    {
        currentIndex++;
        if (currentIndex >= tips.Length)
            currentIndex = 0; // Vuelve al primero

        tipText.text = tips[currentIndex];
    }
}

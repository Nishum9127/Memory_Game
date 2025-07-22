using UnityEngine;
using UnityEngine.UI;

public class GridSizeSelector : MonoBehaviour
{
    [Header("Grid Size Buttons")]
    public Button button2x2;
    public Button button2x3;
    public Button button4x4;
    public Button button5x6;

    void Start()
    {
        button2x2.onClick.AddListener(() => GameManager.Instance.OnGridSizeSelected(2, 2));
        button2x3.onClick.AddListener(() => GameManager.Instance.OnGridSizeSelected(2, 3));
        button4x4.onClick.AddListener(() => GameManager.Instance.OnGridSizeSelected(4, 4));
        button5x6.onClick.AddListener(() => GameManager.Instance.OnGridSizeSelected(5, 6));
    }
}

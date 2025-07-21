// GameManager.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject cardPrefab;
    public GridLayoutGroup grid;
    public TextMeshProUGUI scoreText;

    [Header("Card Images")]
    public List<Sprite> cardImages; // <-- animal images (fronts)
    public Sprite backImage;        // <-- default back image

    private List<Card> cards = new List<Card>();
    private Card firstCard, secondCard;
    private int score = 0;
    private bool canFlip = true;

    void Awake() => Instance = this;

    void Start()
    {
        LoadGame();
    }

    public void GenerateBoard(int rows, int cols)
    {
        int total = rows * cols;
        var imageList = new List<Sprite>();
        for (int i = 0; i < total / 2; i++)
        {
            imageList.Add(cardImages[i]);
            imageList.Add(cardImages[i]); // Add pair
        }

        imageList = imageList.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < total; i++)
        {
            var cardObj = Instantiate(cardPrefab, grid.transform);
            var card = cardObj.GetComponent<Card>();
            card.Init(imageList[i]);  // pass only the sprite
            cards.Add(card);
        }

        AdjustCardSize(rows, cols);
    }
    private void AdjustCardSize(int rows, int cols)
    {
        // Get the size of the parent container
        RectTransform rt = grid.GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        // Add spacing margin if needed
        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        // Calculate the available size per cell
        float cellWidth = (width - ((cols - 1) * spacingX)) / cols;
        float cellHeight = (height - ((rows - 1) * spacingY)) / rows;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }

    public void CardFlipped(Card card)
    {
        if (!canFlip) return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;

            // Don't allow more flips until checked
            canFlip = false;

            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f); // Short delay to show both flipped cards

        if (firstCard.id == secondCard.id)
        {
            firstCard.isMatched = true;
            secondCard.isMatched = true;

            // Optional: Play match sound
        }
        else
        {
            firstCard.HideCard();
            secondCard.HideCard();

            // Optional: Play mismatch sound
        }

        // Reset
        firstCard = null;
        secondCard = null;
        canFlip = true;
    }
    void SaveGame()
    {
        SaveManager.SaveScore(score);
    }

    void LoadGame()
    {
        score = SaveManager.LoadScore();
        UpdateScore();
        GenerateBoard(2, 3); // default
    }

    void UpdateScore() => scoreText.text = $"Score: {score}";
}

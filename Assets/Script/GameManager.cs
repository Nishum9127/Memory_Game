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
    public List<Sprite> cardImages;
    public Sprite backImage;

    private List<Card> cards = new List<Card>();
    public Card firstCard, secondCard;
    private int score = 0;

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
            imageList.Add(cardImages[i]);
        }

        imageList = imageList.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < total; i++)
        {
            var cardObj = Instantiate(cardPrefab, grid.transform);
            var card = cardObj.GetComponent<Card>();
            card.Init(imageList[i]);
            cards.Add(card);
        }

        AdjustCardSize(rows, cols);
    }

    private void AdjustCardSize(int rows, int cols)
    {
        RectTransform rt = grid.GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        float spacingX = grid.spacing.x;
        float spacingY = grid.spacing.y;

        float cellWidth = (width - ((cols - 1) * spacingX)) / cols;
        float cellHeight = (height - ((rows - 1) * spacingY)) / rows;

        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }

    public void CardFlipped(Card card)
    {
        if (card.isMatched || card == firstCard || card == secondCard)
            return;

        if (firstCard != null && secondCard != null)
        {
            if (firstCard.id != secondCard.id)
            {
                firstCard.HideCard();
                secondCard.HideCard();
            }

            firstCard = null;
            secondCard = null;
        }

        card.FlipCard();

        if (firstCard == null)
        {
            firstCard = card;
        }
        else if (secondCard == null)
        {
            secondCard = card;

            if (firstCard.id == secondCard.id)
            {
                firstCard.isMatched = true;
                secondCard.isMatched = true;

                firstCard.LockCard();
                secondCard.LockCard();

                score += 10;
                UpdateScore();
                SaveGame();
                // Reset match pair for next round
                firstCard = null;
                secondCard = null;
            }
            else StartCoroutine(HideAfterDelay(firstCard, secondCard));
        }
    }
    private IEnumerator HideAfterDelay(Card c1, Card c2)
    {
        yield return new WaitForSeconds(1f);

        c1.HideCard();
        c2.HideCard();
    }

    void SaveGame()
    {
        SaveManager.SaveScore(score);
    }

    void LoadGame()
    {
        score = SaveManager.LoadScore();
        UpdateScore();
        GenerateBoard(2, 3);
    }
    public void OnGridSizeSelected(int rows, int cols)
    {
        // Clear old cards
        foreach (Transform child in grid.transform)
            Destroy(child.gameObject);

        cards.Clear();

        // Generate new
        GenerateBoard(rows, cols);
    }

    void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }
}

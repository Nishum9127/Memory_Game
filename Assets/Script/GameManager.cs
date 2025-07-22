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
    public GameObject gameCompletePanel;

    [Header("Card Images")]
    public List<Sprite> cardImages;
    public Sprite backImage;

    private List<Card> cards = new List<Card>();
    Card firstCard, secondCard;
    private int score = 0;
    public int maxMoves = 6;
    private int remainingMoves;

    public TextMeshProUGUI movesText;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOverPanel;
    public GameObject startPanel;

    public Button restart;
    public Button start;
    private Dictionary<string, int> gridMoves = new Dictionary<string, int>()
    {
        { "2x2", 3 },
        { "2x3", 6 },
        { "4x4", 20 },
        { "5x6", 30 }
    };
    private string currentGridKey = "2x3";

    void Awake() => Instance = this;

    void Start()
    {
        LoadGame();
        restart.onClick.AddListener(RestartGame);
        start.onClick.AddListener(StartGame);
    }
    public void StartGame()
    {
        SoundManager.Instance.PlayBtnClickSound();
        startPanel.SetActive(false);
        OnGridSizeSelected(2, 3);
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
        if (card.isMatched)
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

            remainingMoves--;
            UpdateMoves();

            if (firstCard.id == secondCard.id)
            {
                SoundManager.Instance.PlayMatchSound();
                firstCard.isMatched = true;
                secondCard.isMatched = true;

                firstCard.LockCard();
                secondCard.LockCard();

                score += 10;
                UpdateScore();
                SaveGame();

                firstCard = null;
                secondCard = null;
                if (cards.All(c => c.isMatched))
                    CheckGameComplete();
            }
            else StartCoroutine(HideAfterDelay(firstCard, secondCard));

            if (remainingMoves <= 0 && !cards.All(c => c.isMatched))
            {
                GameOver();
            }
        }
    }
    void GameOver()
    {
        SoundManager.Instance.PlayGameOverSound();

        int totalMoves = gridMoves[currentGridKey];
        int totalMovesUsed = totalMoves - remainingMoves;

        int bestRemaining = PlayerPrefs.GetInt($"BestMoves_{currentGridKey}", -1);
        int bestMovesUsed = (bestRemaining != -1) ? (totalMoves - bestRemaining) : -1;

        if (bestMovesUsed != -1)
            gameOverText.text = $"Game Over\nYour Moves: {totalMovesUsed}\nBest: {bestMovesUsed} Moves";
        else
            gameOverText.text = $"Game Over\nYour Moves: {totalMovesUsed}\nNo Record";

        gameOverPanel.SetActive(true);
    }

    private IEnumerator HideAfterDelay(Card c1, Card c2)
    {
        SoundManager.Instance.PlayMismatchSound();

        yield return new WaitForSeconds(1f);
        c1.HideCard();
        c2.HideCard();
    }

    void SaveGame()
    {
        string key = $"BestMoves_{currentGridKey}";
        int prevBest = PlayerPrefs.GetInt(key, int.MaxValue);

        int totalMovesUsed = gridMoves[currentGridKey] - remainingMoves;

        if (totalMovesUsed < prevBest)
            PlayerPrefs.SetInt(key, totalMovesUsed);
    }
    void LoadGame()
    {
        int defaultRows = 2;
        int defaultCols = 3;
        currentGridKey = $"{defaultRows}x{defaultCols}";

        if (gridMoves.ContainsKey(currentGridKey))
            remainingMoves = gridMoves[currentGridKey];
        else
            remainingMoves = 15;

        UpdateScore();
        UpdateMoves();
        cards.Clear();

        foreach (Transform child in grid.transform)
            Destroy(child.gameObject);

        GenerateBoard(defaultRows, defaultCols);
    }

    private void CheckGameComplete()
    {
        if (cards.All(c => c.isMatched))
        {
            SoundManager.Instance.PlayGameWinSound();
            ShowGameCompleteUI();
        }
    }

    private void ShowGameCompleteUI()
    {
        gameCompletePanel.SetActive(true);
    }

    public void OnGridSizeSelected(int rows, int cols)
    {
        SoundManager.Instance.PlayGridCreationSound();
        foreach (Transform child in grid.transform)
            Destroy(child.gameObject);

        currentGridKey = $"{rows}x{cols}";

        if (gridMoves.ContainsKey(currentGridKey))
            remainingMoves = gridMoves[currentGridKey];
        else
            remainingMoves = 15;

        UpdateMoves();
        cards.Clear();

        GenerateBoard(rows, cols);
    }

    void UpdateMoves()
    {
        movesText.text = $"Moves: {remainingMoves}";
    }
    public void RestartGame()
    {
        SoundManager.Instance.PlayBtnClickSound();

        foreach (var card in cards)
        {
            Destroy(card.gameObject);
        }
        cards.Clear();

        firstCard = null;
        secondCard = null;

        score = 0;

        int rows = int.Parse(currentGridKey.Split('x')[0]);
        int cols = int.Parse(currentGridKey.Split('x')[1]);

        remainingMoves = gridMoves[currentGridKey];

        UpdateScore();
        UpdateMoves();

        gameOverPanel.SetActive(false);
        gameCompletePanel.SetActive(false);

        GenerateBoard(rows, cols);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ResetAllBests();
        }
    }
    public void ResetAllBests()
    {
        foreach (var key in gridMoves.Keys)
        {
            PlayerPrefs.DeleteKey($"BestMoves_{key}");
        }
    }

    void UpdateScore()
    {
        scoreText.text = $"Score: {score}";
    }
}

// Card.cs
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id; // To match with another card
    public Sprite frontSprite;
    public Image frontImage, backImage;
    public bool isFlipped = false;
    public bool isMatched = false;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);

        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);

    }

    public void Init(Sprite front)
    {
        frontImage.sprite = front;
        id = front.GetInstanceID();

        frontImage.gameObject.SetActive(false); // ✅ start hidden
        backImage.gameObject.SetActive(true);   // ✅ start visible

        isFlipped = false;
        isMatched = false;
    }

    public void OnClick()
    {
        if (isMatched || isFlipped) return;
        Debug.Log("C C " + id);
        FlipCard();
        GameManager.Instance.CardFlipped(this);
    }

    public void FlipCard()
    {
        Debug.Log("Flipping card: " + id);

        // Use animation or scale tween
        isFlipped = true;
        frontImage.gameObject.SetActive(true);   // ✅ show front
        backImage.gameObject.SetActive(false);   // ✅ hide back
        // Play flip sound
    }

    public void HideCard()
    {
        isFlipped = false;

        frontImage.gameObject.SetActive(false);  // ✅ hide front
        backImage.gameObject.SetActive(true);    // ✅ show back
    }

    public void LockCard()
    {
        frontImage.raycastTarget = false;
        backImage.raycastTarget = false;
        GetComponent<Button>().interactable = false;
    }

}

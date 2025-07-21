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
    }

    public void Init(Sprite front)
    {
        this.frontSprite = front;
        frontImage.sprite = front;
        id = front.GetInstanceID();
    }

    public void OnClick()
    {
        if (isMatched || isFlipped) return;
        Debug.Log("C C "+ id);
        FlipCard();
        GameManager.Instance.CardFlipped(this);
    }

    public void FlipCard()
    {
        // Use animation or scale tween
        isFlipped = !isFlipped;
        frontImage.gameObject.SetActive(isFlipped);
        backImage.gameObject.SetActive(!isFlipped);
        // Play flip sound
    }

    public void HideCard()
    {
        isFlipped = false;
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }
}

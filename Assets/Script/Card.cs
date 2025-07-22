using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    public Sprite frontSprite;
    public Image frontImage, backImage;
    public bool isFlipped = false;
    public bool isMatched = false;
    Animator animator;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        animator = GetComponent<Animator>();

        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);

    }

    public void Init(Sprite front)
    {
        frontImage.sprite = front;
        id = front.GetInstanceID();

        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);

        isFlipped = false;
        isMatched = false;
    }

    public void OnClick()
    {
        if (isMatched || isFlipped) return;
        GameManager.Instance.CardFlipped(this);
    }

    //public void FlipCard()
    //{
    //    SoundManager.Instance.PlayFlipSound();
    //    Debug.Log("Flipping card: " + id);

    //    // Use animation or scale tween
    //    isFlipped = true;
    //    frontImage.gameObject.SetActive(true);
    //    backImage.gameObject.SetActive(false);
    //    // Play flip sound
    //}

    public void FlipCard()
    {
        if (isFlipped) return;
        isFlipped = true;
        if (this != null)
        {
            StartCoroutine(FlipRoutine(true));
        }
        SoundManager.Instance.PlayFlipSound();
    }

    public void HideCard()
    {
        if (!isFlipped) return;
        isFlipped = false;
        if (this != null)
        {
            StartCoroutine(FlipRoutine(false));
        }
    }


    private IEnumerator FlipRoutine(bool showFront)
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime * 5f)
        {
            transform.localScale = new Vector3(i, 1, 1);
            yield return null;
        }

        frontImage.gameObject.SetActive(showFront);
        backImage.gameObject.SetActive(!showFront);

        for (float i = 0; i <= 1; i += Time.deltaTime * 5f)
        {
            transform.localScale = new Vector3(i, 1, 1);
            yield return null;
        }

        transform.localScale = Vector3.one;
    }
    //public void HideCard()
    //{
    //    isFlipped = false;

    //    frontImage.gameObject.SetActive(false);
    //    backImage.gameObject.SetActive(true);
    //}

    public void LockCard()
    {
        frontImage.raycastTarget = false;
        backImage.raycastTarget = false;
        GetComponent<Button>().interactable = false;
    }
}

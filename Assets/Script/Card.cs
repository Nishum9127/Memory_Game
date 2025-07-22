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
        Debug.Log("C C " + id);
        //FlipCard();
        GameManager.Instance.CardFlipped(this);
    }

    //public void FlipCard()
    //{
    //    SoundManager.Instance.PlayFlipSound();
    //    Debug.Log("Flipping card: " + id);

    //    // Use animation or scale tween
    //    isFlipped = true;
    //    frontImage.gameObject.SetActive(true);   // ✅ show front
    //    backImage.gameObject.SetActive(false);   // ✅ hide back
    //    // Play flip sound
    //}

    public void FlipCard()
    {
        if (isFlipped) return;
        isFlipped = true;
        StartCoroutine(FlipRoutine(true));
        SoundManager.Instance.PlayFlipSound();
    }

    public void HideCard()
    {
        if (!isFlipped) return;
        isFlipped = false;
        StartCoroutine(FlipRoutine(false));
    }


    private IEnumerator FlipRoutine(bool showFront)
    {
        // Step 1: Shrink X to 0
        for (float i = 1; i >= 0; i -= Time.deltaTime * 5f)
        {
            transform.localScale = new Vector3(i, 1, 1);
            yield return null;
        }

        // Step 2: Switch Image
        frontImage.gameObject.SetActive(showFront);
        backImage.gameObject.SetActive(!showFront);

        // Step 3: Expand X to 1
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

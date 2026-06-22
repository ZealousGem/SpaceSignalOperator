using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AnimateButton : ButtonBase
{
    [Header("Animation")]
     private Vector3 hoverScale = new Vector3(1.01f, 1.01f, 1.01f);
     private Vector3 pressedScale = new Vector3(0.95f, 0.95f, 0.95f);
    [SerializeField] private float duration = 0.12f;

    private Image ButtonImage;

    private TMP_Text ButtonText;

    private Vector3 _baseScale;
    private Coroutine _routine;

    private void Awake()
    {
        hoverScale = new Vector3(hoverScale.x * transform.localScale.x, hoverScale.y * transform.localScale.y, hoverScale.z * transform.localScale.z);
        pressedScale = new Vector3(pressedScale.x * transform.localScale.x, pressedScale.y * transform.localScale.y, pressedScale.z * transform.localScale.z);
        _baseScale = transform.localScale;

        ButtonImage = gameObject.GetComponent<Image>();
        ButtonText = gameObject.GetComponentInChildren<TMP_Text>();

        SetButtonAplha(false);
    }

    private void OnDisable()
    {
        if (_routine != null)
        {
            StopCoroutine(_routine);
            _routine = null;
        }
        transform.localScale = _baseScale;
    }

    public override void Entered()
    {
        StartScaleTo(hoverScale);
    }

    public override void Left()
    {
        StartScaleTo(_baseScale);
    }

    public override void Clicked()
    {
        StartScaleTo(pressedScale);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        SetButtonAplha(true);
        Entered();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        SetButtonAplha(false);
        Left();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Clicked();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        Entered();
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        Left();
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        Clicked();
    }

    private void StartScaleTo(Vector3 target)
    {
        if (!gameObject.activeInHierarchy) return;
        
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(ScaleTo(target));
    }

    private void SetButtonAplha(bool state)
    {
        if(ButtonImage == null || ButtonText == null) return;

        Color curColor = ButtonImage.color;

        if (state)
        {
            curColor.a = 100f;
            ButtonImage.color = curColor;
            ButtonText.color = Color.black;
        }

        else
        {
            curColor.a = 0f;
            ButtonImage.color = curColor;
            ButtonText.color = Color.white; 
        }


    }

    private IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / Mathf.Max(0.0001f, duration);
            transform.localScale = Vector3.Lerp(start, target, t);
            yield return null;
        }
        transform.localScale = target;
        _routine = null;

        // If it was pressed, return to hover/base.
        if (target == pressedScale)
        {
            // after click, go back to hoverScale (if currently hovered/selected) is tricky.
            // simplest: return to base scale.
            _routine = StartCoroutine(ScaleTo(_baseScale));
        }
    }
}


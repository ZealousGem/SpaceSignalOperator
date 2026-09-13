using UnityEngine;
using UnityEngine.EventSystems;

public class ScaleButton : AnimateButton
{
    protected override void Awake()
    {
        hoverScale = new Vector3(hoverScale.x * transform.localScale.x, hoverScale.y * transform.localScale.y, hoverScale.z * transform.localScale.z);
        pressedScale = new Vector3(pressedScale.x * transform.localScale.x, pressedScale.y * transform.localScale.y, pressedScale.z * transform.localScale.z);
        _baseScale = transform.localScale;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Entered();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
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

}

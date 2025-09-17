using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

/*对话选项配置器*/
public class TalkOption : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("选项文字")]
    [SerializeField] private TMP_Text text_option;
    [Header("颜色-进入")]
    [SerializeField] private Color color_enter;
    [Header("颜色-离开")]
    [SerializeField] private Color color_exit;

    private UnityAction action_click;

    private void OnEnable()
    {
        if (text_option == null) text_option = this.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void OnDestroy()
    {
        action_click = null;
    }

    public void SetOption(string val, UnityAction action)
    {
        text_option.text = val;
        action_click += action;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        action_click?.Invoke();
        action_click = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text_option.color = color_enter;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text_option.color = color_exit;
    }

}

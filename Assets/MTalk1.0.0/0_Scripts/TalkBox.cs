using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

/*对话框配置器*/
public class TalkBox : MonoBehaviour, IPointerClickHandler
{
    [Header("对话人昵称")]
    [SerializeField] private TMP_Text text_name;
    [Header("对话人内容")]
    [SerializeField] private TMP_Text text_value;
    [Header("选项载体")]
    [SerializeField] private Transform option_carrier;

    public Transform Option_carrier { get => option_carrier; }

    private void Start()
    {
        text_name = this.transform.GetChild(1).GetComponent<TMP_Text>();
        text_value = this.transform.GetChild(2).GetComponent<TMP_Text>();
        option_carrier = this.transform.GetChild(3).transform;
    }

    void OnDisable()
    {
        ClearAllOption();
    }

    public void SetTalkBoxName(string name)
    {
        text_name.text = name;
    }

    public void SetTalkBoxInfo(string value)
    {
        text_value.text = value;
    }

    public void ClearAllOption()
    {
        if (option_carrier.childCount == 0) return;
        for (int clearIndex = 0; clearIndex < option_carrier.childCount; clearIndex++)
        {
            Destroy(option_carrier.GetChild(clearIndex).gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.FireEvent("AudioManager", new object[] { "click_update", 1.0f, 0.052f, false });
        if (GameSystem.EXIST_SELECT_OPTION) return;
        GameSystem.PLAYER_TALK_CONTINUE_SIGN = true;
    }

}

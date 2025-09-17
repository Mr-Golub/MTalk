using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using System.Threading;
using System;

/*对话对接器*/
public class TalkHandler : MonoBehaviour, IEventListener
{
    [Header("NPC ID")]
    [SerializeField] private string npc_id;
    [Header("玩家当前数据串")]
    [SerializeField] private string player_cur_str;
    [Header("当前对话列表")]
    [SerializeField] private List<string> cur_value_list;
    [Header("当前选择列表")]
    [SerializeField] private List<string> cur_select_list;
    [Header("对话框")]
    [SerializeField] private Transform talk_obj;
    [Header("选项")]
    [SerializeField] private Transform talk_option;
    private TalkBoxData tempData;

    private CancellationTokenSource tokenCTS;

    private string condition_cur_key = "common";  //TODO:这里需要通过TalkManager的EventManager来读取存档获取进度
    private int cur_value_index = 0;

    void Start()
    {
        EventManager.RegisterEvent("NPC_" + npc_id, this);
        _ = Init();
    }

    private async UniTaskVoid Init()
    {
        await UniTask.WaitUntil(() => GameSystem.CONFIG_TALK_SUCCESS_SIGN == true);
        if (tempData == null)
        {
            cur_value_list.Clear();
            //talk_obj = UIManager.instance.Panel_talk;
            tempData = TalkManager.instance.Npc_talk_data_dic[npc_id];
        }
        //更新玩家记录串  TODO:这里需要设计存档API
        player_cur_str = condition_cur_key;

        Debug.Log($"{npc_id}->{GameSystem.CONFIG_TALK_SUCCESS_SIGN}");
    }

    public void ShowTalk()
    {
        tokenCTS = new CancellationTokenSource();
        GameSystem.PLAYER_TALK_END_SIGN = false;
        StartTalk(tokenCTS.Token).Forget();
    }

    public async UniTaskVoid StartTalk(CancellationToken token)
    {
        while (!GameSystem.PLAYER_TALK_END_SIGN)
        {
            talk_obj.gameObject.SetActive(true);
            talk_obj.GetComponent<TalkBox>().SetTalkBoxName(tempData.Npc_talk_name[player_cur_str]);

            //获取对话内容
            var talkValue = ResoCondition(npc_id, player_cur_str, cur_value_index);

            //装配信息
            talk_obj.GetComponent<TalkBox>().SetTalkBoxInfo(talkValue);

            //获取选择内容
            var sourceSelection = ResoSelection(player_cur_str);

            //解析装配选择项
            ResoAndCreateOption(sourceSelection);

            await UniTask.WaitUntil(() => GameSystem.PLAYER_TALK_CONTINUE_SIGN == true);
            GameSystem.PLAYER_TALK_CONTINUE_SIGN = false;
            //判断如果就一条对话，这里直接清空对话列表
            cur_value_list.Clear();
            cur_select_list.Clear();
        }
        talk_obj.gameObject.SetActive(false);
        tokenCTS?.Cancel();
        tokenCTS?.Dispose();
    }

    //根据条件串获取对话内容列表
    private string ResoCondition(string id, string key, int index)
    {
        cur_value_list = new List<string>(tempData.Npc_value[key]);
        return cur_value_list[index];
    }

    //获取选择内容
    private string ResoSelection(string key)
    {
        cur_select_list = new List<string>(tempData.Npc_select[key]);
        return cur_select_list[0];
    }

    //解析装配选择项
    private void ResoAndCreateOption(string str)
    {
        string[] command = str.Split("#");

        if (command[0].Equals("1")) //无选择+接条件
        {
            player_cur_str += command[1];
        }
        else if (command[0].Equals("3")) //无选择+回溯
        {
            player_cur_str = command[1];
            GameSystem.PLAYER_TALK_END_SIGN = true;
        }
        else if (command[0].Equals("4")) //有选择
        {
            GameSystem.EXIST_SELECT_OPTION = true;
            string[] optionArray = command[1].Split(";");

            //TODO:这里如果有UIManager，可以使用UIManager来获取组件
            //var optionCarrierObj = UIManager.instance.Panel_talk.GetComponent<TalkBox>().Option_carrier.transform;

            var optionCarrierObj = talk_obj.GetComponent<TalkBox>().Option_carrier.transform;

            foreach (var item in optionArray)
            {
                var option = item.Split("*"); //0-选项文字  1-附加指令

                //TODO:这里如果有UIManager，可以使用UIManager来获取组件
                //var optionObj = Instantiate(UIManager.instance.Panel_option, optionCarrierObj);

                var optionObj = Instantiate(talk_option, optionCarrierObj);

                optionObj.GetComponent<TalkOption>().SetOption(option[0], () =>
                {   
                    player_cur_str += option[1];
                    talk_obj.GetComponent<TalkBox>().ClearAllOption();
                    GameSystem.PLAYER_TALK_CONTINUE_SIGN = true;
                    GameSystem.EXIST_SELECT_OPTION = false;
                });
            }
        }
        else if (command[0].Equals("5") && command[1].Equals("stay")) //停留
        {
            GameSystem.PLAYER_TALK_END_SIGN = true;
        }
        else if (command[0].Equals("6")) //情况获得奖励+接条件
        {
            var option = command[1].Split("*");
            player_cur_str += option[0];
            //TODO EventManager呼叫增加奖励
        }
        else if (command[0].Equals("shop")) //商店
        {
            var option = command[1];
            //TODO EventManager呼叫指定商店
        }
    }

    public void OnEvent(string eventName, params object[] data)
    {
        var option = (string)data[0];
        switch (option)
        {
            case "talk":
                {
                    ShowTalk();
                    break;
                }
            case "add":
                {

                    break;
                }
            case "reset":
                {

                    break;
                }
            case "kill":
                {

                    break;
                }
            case "activity":
                {

                    break;
                }
        }
    }

}

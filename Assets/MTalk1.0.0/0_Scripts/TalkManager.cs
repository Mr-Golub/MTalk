using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*对话管理器*/
public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;

    [Header("NPC编号")]
    [SerializeField] private List<string> npc_id_list = new List<string>();
    [Header("NPC对话数据集")]
    [SerializeField] private Dictionary<string, TalkBoxData> npc_talk_data_dic = new Dictionary<string, TalkBoxData>();

    [Header("ID数量")]
    [SerializeField] private int id_count;
    [Header("NPC数据量")]
    [SerializeField] private int talk_count;

    public Dictionary<string, TalkBoxData> Npc_talk_data_dic { get => npc_talk_data_dic; set => npc_talk_data_dic = value; }

    void OnEnable()
    {
        instance = this;
    }

    public void AddData(JsonData jsonData)
    {
        //检测是否存在ID
        if (CheckID(jsonData.ID))
        {
            //存在ID
            //检测是否存在CONDITION
            var res = npc_talk_data_dic[jsonData.ID].Npc_value.ContainsKey(jsonData.CONDITION);
            if (res)
            {
                //存在
                npc_talk_data_dic[jsonData.ID].Npc_talk_name.Add(jsonData.CONDITION, jsonData.NAME);
                npc_talk_data_dic[jsonData.ID].Npc_value[jsonData.CONDITION].Add(jsonData.VALUE);
                npc_talk_data_dic[jsonData.ID].Npc_select[jsonData.CONDITION].Add(jsonData.SELECT);
                talk_count++;
            }
            else
            {
                //不存在
                npc_talk_data_dic[jsonData.ID].Npc_talk_name.Add(jsonData.CONDITION, jsonData.NAME);
                var valueList = new List<string>();
                valueList.Add(jsonData.VALUE);
                npc_talk_data_dic[jsonData.ID].Npc_value.Add(jsonData.CONDITION, valueList);
                var selectList = new List<string>();
                selectList.Add(jsonData.SELECT);
                npc_talk_data_dic[jsonData.ID].Npc_select.Add(jsonData.CONDITION, selectList);
                talk_count++;
            }
        }
        else
        {
            //不存在ID
            npc_id_list.Add(jsonData.ID);

            var talkBoxData = ScriptableObject.CreateInstance<TalkBoxData>();
            talkBoxData.Npc_id = jsonData.ID;
            talkBoxData.Npc_name = jsonData.NAME;

            talkBoxData.Npc_talk_name.Add(jsonData.CONDITION, jsonData.NAME);

            var valueList = new List<string>();
            valueList.Add(jsonData.VALUE);
            talkBoxData.Npc_value.Add(jsonData.CONDITION, valueList);

            var selectList = new List<string>();
            selectList.Add(jsonData.SELECT);
            talkBoxData.Npc_select.Add(jsonData.CONDITION, selectList);

            npc_talk_data_dic.Add(jsonData.ID, talkBoxData);
            id_count++;
            talk_count++;
        }
    }

    private bool CheckID(string id)
    {
        bool res = false;
        foreach (var item in npc_id_list)
        {
            if (item.Equals(id))
            {
                res = true;
                break;
            }
        }
        return res;
    }

}

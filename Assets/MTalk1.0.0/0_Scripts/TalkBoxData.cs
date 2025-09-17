using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTalkBox", menuName = "Data/TalkBox")]
public class TalkBoxData : ScriptableObject
{
    [Header("编号")]
    [SerializeField] private string npc_id;
    [Header("昵称")]
    [SerializeField] private string npc_name;
    [Header("对话昵称集")]
    [SerializeField] private Dictionary<string, string> npc_talk_name = new Dictionary<string, string>();
    [Header("内容集")]
    //TODO:根据表结构，这里Dic的value类型其实可以不用List<>，但是已经写好了，再改的话，其他要改的地方有点多，避免BUG，因为游戏体量也没有很大，如无必要，就不要动了。
    [SerializeField] private Dictionary<string, List<string>> npc_value = new Dictionary<string, List<string>>(); 
    [Header("选择集")]
    //TODO:根据表结构，这里Dic的value类型其实可以不用List<>，但是已经写好了，再改的话，其他要改的地方有点多，避免BUG，因为游戏体量也没有很大，如无必要，就不要动了。
    [SerializeField] private Dictionary<string, List<string>> npc_select = new Dictionary<string, List<string>>();

    public string Npc_id { get => npc_id; set => npc_id = value; }
    public string Npc_name { get => npc_name; set => npc_name = value; }
    public Dictionary<string, string> Npc_talk_name { get => npc_talk_name; set => npc_talk_name = value; }
    public Dictionary<string, List<string>> Npc_value { get => npc_value; set => npc_value = value; }
    public Dictionary<string, List<string>> Npc_select { get => npc_select; set => npc_select = value; }
}

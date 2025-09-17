using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

/*对话Jason适配器*/
public class TalkAdapter : MonoBehaviour
{
    [Header("Json数据集")]
    [SerializeField] private JsonDataArray json_data_array;

    void OnEnable()
    {
        var textText = Resources.Load<TextAsset>("TestTalk");
        json_data_array = JsonUtility.FromJson<JsonDataArray>(textText.text);
    }

    void Start()
    {
        _ = ConfigData();
    }

    private async UniTaskVoid ConfigData()
    {
        foreach (var item in json_data_array.JSON_DATA_ARRAY)
        {
            TalkManager.instance.AddData(item);
            await UniTask.Delay(100);
        }
        GameSystem.CONFIG_TALK_SUCCESS_SIGN = true;
    }

}

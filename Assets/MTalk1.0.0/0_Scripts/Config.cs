/*using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

*//*对战角色*//*
public interface BattleRole
{

}

*//*技能类型*//*
public enum ACTION_TYPE
{
    WU_LI,   //物理系
    MO_FA,   //魔法系
    BUFF,    //增益系
    DEBUFF   //负增益系
}

*//*玩家操作类型*//*
public enum PLAYER_OPERATE_TYPE
{
    NULL,
    COMMON_ATTACK,
    MAGIC_ATTACK,
    USE_ITEM,
    DEFENCE,
    RUN
}

*//*道具类型*//*
public enum PROP_TYPE
{
    YI_FU,
    XIANG_LIAN,
    WU_QI,
    MAO_ZI,
    JIE_ZHI,
    SHOU_ZHUO,
    XUE_ZI,
    YAO_DAI,
    LING_BAO,
    YAO_PIN,
}

*//*玩家普通状态*//*
public enum ROLE_COMMON_TYPE
{
    IDLE,
    RUN
}

*//*角色站队类型*//*
public enum Team_Type
{
    Hostile,
    Friendly
}

*//*战斗行动类型*//*
public enum ROLE_ACTION_TYPE
{
    IDLE,
    ATTACK,
    MAGIC,
    HURT,
    RUN,
    DIE
}

*//*战斗角色回调*//*
public enum Battle_CallBack
{
    CALLBACK_AFTER_ATTACK,       //物理攻击后

    CALLBACK_AFTER_USE_PROP,     //使用道具后

    CALLBACK_AFTER_MAGIC,        //使用魔法后

    CALLBACK_AFTER_DEFENCE,      //防御后

    CALLBACK_AFTER_RUN,          //逃跑后

    CALLBACK_AFTER_DIE,          //死亡后

    CALLBACK_AFTER_HURT,         //受伤后
}

*//*日常角色动作帧*//*
[System.Serializable]
public struct CommonActionSprite
{
    [Header("上")]
    [SerializeField] public Sprite[] U;
    [Header("右上")]
    [SerializeField] public Sprite[] RU;
    [Header("右")]
    [SerializeField] public Sprite[] R;
    [Header("右下")]
    [SerializeField] public Sprite[] RD;
    [Header("下")]
    [SerializeField] public Sprite[] D;
    [Header("左下")]
    [SerializeField] public Sprite[] LD;
    [Header("左")]
    [SerializeField] public Sprite[] L;
    [Header("左上")]
    [SerializeField] public Sprite[] LU;
}

*//*配置*//*
public static class Config
{
    public static BaseDataBox PLAYER_BASE_PARAM = new BaseDataBox
    {
        hp_max = 100f,
        hp_cur = 100f,
        mp_max = 100f,
        mp_cur = 100f,
        speed_cur = 2f,
        attack_cur = 2f,
        physics_defence_cur = 2f,
        magic_defence_cur = 2f,
        avoid_cur = 2f,
        level = 0,
        allow_be_attack_cur = true,
        is_dead = false
    };

    public static Battle_CallBack battle_callback;

    //初始化角色回调集
    #region
    *//*    public static Dictionary<Battle_CallBack, UnityAction> InitBattleCallBackDic()
        {
            var newDic = new Dictionary<Battle_CallBack, UnityAction>();
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_ATTACK, () => { });
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_MAGIC, () => { });
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_USE_PROP, () => { });
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_DEFENCE, () => { });
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_RUN, () => { });
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_HURT, () => { });
            newDic.Add(Battle_CallBack.CALLBACK_AFTER_DIE, () => { });

            return newDic;
        }*//*
    #endregion

    *//*根据系统时间生成非线性随机数*//*
    #region
    public static bool GetRandomBool()
    {
        int[] nums = { 10, 5, 15, 20, 30, 5, 5, 10 };
        int t = 0;
        int r = UnityEngine.Random.Range(0, 101);
        for (int i = 0; i < nums.Length; i++)
        {
            t += nums[i];
            if (r < t) return (nums[i] > 10 && nums[i] <= 30) ? true : false;
        }
        return false;
    }
    #endregion

    *//*根据时间戳加盐生成唯一5位整数*//*
    #region
    public static int GenerateUnique5DigitCode()
    {
        // 时间戳 + 微秒部分提高精度
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(); // 精确到毫秒

        // 加盐以增加唯一性（可选）
        string input = timestamp.ToString() + UnityEngine.Random.Range(0, 1000).ToString();

        // 使用 SHA256 生成哈希
        byte[] hash;
        using (SHA256 sha = SHA256.Create())
        {
            hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        }

        // 取前4个字节转换为 int
        int raw = BitConverter.ToInt32(hash, 0);
        raw = Mathf.Abs(raw); // 取正数

        // 映射到 5 位数：范围 10000 ~ 99999
        int result = 10000 + (raw % 90000); // 保证永远是5位

        return result;
    }
    #endregion

    *//*根据时间戳生成唯一5位字母+数字字符串*//*
    #region
    public static string GenerateUnique5CharString()
    {
        // 获取当前毫秒时间戳，并加随机数以增加唯一性
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        string input = timestamp.ToString() + UnityEngine.Random.Range(0, 1000).ToString();

        // 使用SHA256生成哈希
        byte[] hash;
        using (SHA256 sha = SHA256.Create())
        {
            hash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));
        }

        // 取前4个字节生成一个大整数
        int hashInt = BitConverter.ToInt32(hash, 0);
        hashInt = Mathf.Abs(hashInt); // 转正数

        // 映射到 base36 字符串（使用 0-9A-Z）
        return ToBase36(hashInt).PadLeft(5, '0').Substring(0, 5).ToUpper();
    }

    // 十进制转36进制字符串（0-9A-Z）
    private static string ToBase36(int value)
    {
        const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string result = "";
        do
        {
            result = chars[value % 36] + result;
            value /= 36;
        } while (value > 0);

        return result;
    }
    #endregion

    *//*倒放音源*//*
    #region
    *//*    public static AudioClip CreateReversedAudioClip(AudioClip clip)
        {
            int samples = clip.samples * clip.channels;
            float[] data = new float[samples];
            clip.GetData(data, 0);
            // 反转音频数据
            float[] reversedData = new float[samples];
            int channels = clip.channels;
            for (int i = 0; i < clip.samples; i++)
            {
                for (int c = 0; c < channels; c++)
                {
                    reversedData[(i * channels) + c] = data[((clip.samples - 1 - i) * channels) + c];
                }
            }
            // 创建新 AudioClip
            AudioClip reversedClip = AudioClip.Create(
                clip.name + "_Reversed",
                clip.samples,
                clip.channels,
                clip.frequency,
                false
            );
            reversedClip.SetData(reversedData, 0);
            return reversedClip;
        }*//*
    #endregion

}

*//*系统参数*//*
public static class GameSystem
{
    //选中道具
    public static Transform SELECT_PROP;
    //旧格子
    public static Transform OLD_GRID;
    //目标格子
    public static Transform TARGET_GRID;
    //格子所在面板
    public static Transform BOX;
    //是否在面板内
    public static bool IS_STAY_IN_BOX = false;

    //是否在装备面板内
    public static bool IS_STAT_IN_PERSON_PROP = false;

    //移动道具标志
    public static bool MOVE_PROP_SIGN = false;

    //有选择题标志
    public static bool EXIST_SELECT_OPTION = false;
    //玩家对话结束标志
    public static bool PLAYER_TALK_END_SIGN = false;
    //玩家对话继续标志
    public static bool PLAYER_TALK_CONTINUE_SIGN = false;

    //读取加载对话配置成功标志
    public static bool CONFIG_TALK_SUCCESS_SIGN = false;

    //地图允许玩家移动标志
    public static bool MAP_ALLOW_PLAYER_MOVE_SIGN = false;
    //玩家跑步标志
    public static bool IS_PLAYER_FAST_RUN = false;

    //对战每回合倒计时上限
    public static int SYSTEM_BATTLE_COUNT = 30;
    //对战回合下限
    public static int SYSTEM_BATTLE_ROUND_MIN = 1;
    //对战回合上限
    public static int SYSTEM_BATTLE_ROUND_MAX = 99;
    //伤害残影触发阈值
    public static float Hurt_Shadow_Limit = 2.0f;
    //战斗结束标志
    public static bool IS_BATTLE_END = true;
    //战斗继续标志
    public static bool IS_BATTLE_RESUME = false;
    //执行行动前Buff结束标志
    public static bool IS_BUFF_BEFORE_ACTION_END = false;
    //允许执行行动前Buff标志
    public static bool ALLOW_BUFF_BEFORE_ACTION_SIGN = false;
    //执行行动后Buff结束标志
    public static bool IS_BUFF_AFTER_ACTION_END = false;
    //允许执行行动后Buff标志
    public static bool ALLOW_BUFF_AFTER_ACTION_SIGN = false;
    //每个角色Buff执行结束
    public static bool PER_ROLE_BUFF_RESUME = false;
    //每回合角色攻击结束标志
    public static bool IS_PER_ROLE_ATTACK_FINISHED = false;

    //开始小回合标志
    public static bool IS_SMALL_ACTION_START_SIGN = false;
    //小回合角色攻击结束标志
    public static bool IS_SMALL_PER_ROLE_ATTACK_FINISHED = false;
    //统计当前小回合数
    public static int CUR_SMALL_ROUND_COUNT = 0;
    //小回合数上限
    public static int MAX_SMALL_ROUND_COUNT = 5;

    //战斗角色选中名称颜色
    public static Color SELECT_COLOR = new Color32(0xFC, 0xFF, 0x52, 0xFF);
    //战斗角色正常名称颜色
    public static Color COMMON_COLOR = new Color32(0x40, 0xCB, 0x67, 0xFF);
    //玩家选中的目标角色数据
    public static BattleRoleChecker PLAYER_SELECT_ROLE;
    //目标怪物集合
    public static BattleRoleChecker[] PLAYER_SELECT_ROLE_ARRAY;

    //玩家操作-普通攻击
    public static bool PLAYER_OPTION_COMMON_ATTACK = false;
    //玩家操作-法术攻击
    public static bool PLAYER_OPTION_MAGIC_ATTACK = false;
    //玩家操作-使用道具
    public static bool PLAYER_OPTION_USE_ITEM = false;
    //玩家操作-防御
    public static bool PLAYER_OPTION_DEFENCE = false;
    //玩家操作-逃跑
    public static bool PLAYER_OPTION_RUN = false;

    //玩家已经选定标志
    public static bool PLAYER_SELECTED = false;
    //玩家选择操作
    public static PLAYER_OPERATE_TYPE PLAYER_SELECTED_OPERATE_TYPE = PLAYER_OPERATE_TYPE.NULL;





    public static string LOAD_BOOK_SKILL_ICON_PATH = "/Icon/Skill/";











    //随机选择多个【非死亡】目标，主选目标排第一
    public static void GetRandomRandomMultiplyRole(BattleRoleChecker[] checkers, int num)
    {
        num = num > checkers.Length ? checkers.Length : num;
        var targetArray = checkers.Where(x => x.Unique_identity_id == PLAYER_SELECT_ROLE.Unique_identity_id).ToArray();
        var otherArray = checkers.Where(x => !x.Base_data_box.is_dead && x.Unique_identity_id != PLAYER_SELECT_ROLE.Unique_identity_id).ToArray();
        otherArray = otherArray.OrderBy(x => UnityEngine.Random.value).Take(num - 1).ToArray();
        PLAYER_SELECT_ROLE_ARRAY = targetArray.Concat(otherArray).ToArray();
    }

    //创建音效实例
    public static void CreateAudioObj(Transform obj, AudioClip clip, float pith, float volume, bool loop)
    {
        var audioObj = GameObject.Instantiate(obj);
        audioObj.GetComponent<AudioObj>().PlayAudio(clip, pith, volume, loop);
    }

    public static bool IsUIOverlap(RectTransform a, RectTransform b)
    {
        Rect rectA = GetWorldRect(a);
        Rect rectB = GetWorldRect(b);
        return rectA.Overlaps(rectB);
    }

    private static Rect GetWorldRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners); // 获取世界四角坐标
        Vector2 size = corners[2] - corners[0];
        return new Rect(corners[0], size);
    }
}

public static class UniTaskHelper
{
    public static UniTask WaitUntilChanged<TTarget, TValue>(
        this TTarget target,
        Func<TTarget, TValue> selector)
        where TTarget : class
    {
        return UniTask.WaitUntilValueChanged<TTarget, TValue>(
            target,
            selector
        );
    }
}

//背包道具存储数据盒子
[System.Serializable]
public struct ItemD
{
    public string item_id;
    public int item_index;
    //public Transform item_obj;
    public int item_count;
}

//角色类型
[System.Serializable]
public enum RoleType
{
    Biology,
    Undead,
    Elf
}

//Buff执行时机
[System.Serializable]
public enum BuffOperateTimeType
{
    ACTION_BEFORE,
    ACTION_AFTER
}

*//*装备属性数据盒*//*
[System.Serializable]
public struct EquipmentDataBox
{
    [Header("血量上限")]
    public float hp_max;
    [Header("血量")]
    public float hp;
    [Header("魔力上限")]
    public float mp_max;
    [Header("魔力")]
    public float mp;
    [Header("速度")]
    public float speed;
    [Header("攻击")]
    public float attack;
    [Header("防御")]
    public float physics_defencer;
    [Header("魔御")]
    public float magic_defence;
    [Header("敏捷")]
    public float avoid;
    [Header("等级")]
    public int level;
}

*//*玩家数据盒*//*
[System.Serializable]
public struct PlayerDataBox
{
    [Header("ID")]
    public string id;
    [Header("名称")]
    public string name;
    [Header("类型")]
    public RoleType type;
    [Header("血量上限")]
    public float hp_max;
    [Header("血量")]
    public float hp;
    [Header("魔力上限")]
    public float mp_max;
    [Header("魔力")]
    public float mp;
    [Header("速度")]
    public float speed;
    [Header("攻击")]
    public float attack;
    [Header("防御")]
    public float physics_defencer;
    [Header("魔御")]
    public float magic_defence;
    [Header("敏捷")]
    public float avoid;
    [Header("等级")]
    public int level;
    [Header("经验")]
    public float exp;
    [Header("允许被攻击")]
    public bool allow_be_attack;
    [Header("死亡")]
    public bool is_dead;
}

*//*基础数据盒*//*
[System.Serializable]
public struct BaseDataBox
{
    [Header("血量上限")]
    public float hp_max;
    [Header("血量")]
    public float hp_cur;
    [Header("魔力上限")]
    public float mp_max;
    [Header("魔力")]
    public float mp_cur;
    [Header("速度")]
    public float speed_cur;
    [Header("攻击")]
    public float attack_cur;
    [Header("防御")]
    public float physics_defence_cur;
    [Header("魔御")]
    public float magic_defence_cur;
    [Header("敏捷")]
    public float avoid_cur;
    [Header("经验")]
    public float level;
    [Header("允许被攻击")]
    public bool allow_be_attack_cur;
    [Header("死亡")]
    public bool is_dead;
}

[System.Serializable]
public struct RoleBaseData
{
    [Header("ID")]
    public string id;
    [Header("名称")]
    public string name;
    [Header("类型")]
    public RoleType type;
    [Header("描述")]
    [TextArea()]
    public string des;
    [Header("血量")]
    public float hp;
    [Header("魔力")]
    public float mp;
    [Header("速度")]
    public float speed;
    [Header("攻击")]
    public float attack;
    [Header("防御")]
    public float physics_defence;
    [Header("魔御")]
    public float magic_defence;
    [Header("敏捷")]
    public float avoid;
    [Header("经验")]
    public float exp;
    //持有物品编号集
    [Header("允许被攻击")]
    public bool allow_be_attack;
}

[System.Serializable]
public struct RoleActionGroup
{
    [Header("帧图")]
    public Sprite[] action_image;
    [Header("音效")]
    public AudioClip sound;
}

[System.Serializable]
public struct RoleActionGroupList
{
    [Header("站立")]
    public RoleActionGroup idle;
    [Header("攻击")]
    public RoleActionGroup attack;
    [Header("施法")]
    public RoleActionGroup magic;
    [Header("死亡")]
    public RoleActionGroup die;
    [Header("受伤")]
    public RoleActionGroup hurt;
    [Header("移动")]
    public RoleActionGroup run;
}

[System.Serializable]
public struct BuffInfoBox
{
    [Header("Buff ID")]
    public string buff_id;
    [Header("Buff 名称")]
    public string buff_name;
    [Header("Buff执行时机")]
    public BuffOperateTimeType buff_time_type;
    [Header("Buff实例")]
    public Transform buff_obj;
    [Header("Buff持续回合")]
    public int buff_dur;
}

[System.Serializable]
public struct BuffCheckBox
{
    [Header("Check Buff ID")]
    public string check_buff_id;
    [Header("Check Target Role ID")]
    public string check_target_role_id;
    [Header("Check Buff名称")]
    public string check_buff_name;
    [Header("Check Buff对象")]
    public Transform check_buff_obj;
    [Header("Check Buff设置回合")]
    public int check_buff_set_dur;
    [Header("Check Buff存留回合")]
    public int check_buff_dur;
}



//RBE适配json结构
#region
[System.Serializable]
public struct RBEJsonData
{
    [Header("ID")]
    public string ID;
    [Header("名称")]
    public string NAME;
    [Header("图标")]
    public string ICON;
    [Header("编号")]
    public string NO;
    [Header("冲突")]
    public string COLLISION;
    [Header("被覆盖几率")]
    public float REPLACE;
    [Header("触发几率")]
    public float ODDS;
    [Header("固定值")]
    public float FIXED;
    [Header("倍率")]
    public float MULTIPLE;
    [Header("次数")]
    public float COUNT;
    [Header("描述")]
    [TextArea]
    public string DES;
}

[System.Serializable]
public struct RBEJsonDataArray
{
    public RBEJsonData[] RBE_JSON_DATA_ARRAY;
}
#endregion

[System.Serializable]
public class RBEDataBox
{
    private RBEJsonData rbe_json_data;
    private int index;

    public RBEJsonData Rbe_json_data { get => rbe_json_data; set => rbe_json_data = value; }
    public int Index { get => index; set => index = value; }
}


*//*打书部分参数*//*
public static class BookSystem
{
    //打书上限
    public static int MAX_BOOK_COUNT = 12;
    //当前扩书几率
    public static float CUR_ADD_BOOK_ODD = -1;
    //当前书覆盖几率
    public static float CUR_BOOK_REPLACE_ODD = -1;

    //计算扩书几率
    public static bool ComputeSuccessExpandBook(int curBookNum, int maxBookNum)
    {
        var randomNum = UnityEngine.Random.Range(1, maxBookNum + 1);
        curBookNum = curBookNum >= maxBookNum ? maxBookNum : curBookNum;
        //Debug.Log($"{randomNum}--{curBookNum}");
        return randomNum > curBookNum ? true : false;
    }

    public static bool CheckIsCollision(string curBookNo = "", string targetBookNoStr = "")
    {
        bool res = false;
        if (curBookNo.Equals("") || targetBookNoStr.Equals("")) return false;
        if (targetBookNoStr.Equals("-1")) return false;
        var targetBookCollisionNoArray = targetBookNoStr.Split(",");
        foreach (var item in targetBookCollisionNoArray)
        {
            if (curBookNo.Equals(item))
            {
                res = true;
                break;
            }
        }
        return res;
    }

    public static bool CheckBookInBagExist(string id = "", List<RBEDataBox> array = null)
    {
        bool res = false;
        if (id.Equals("") || array == null || array.Count == 0) return false;
        foreach (var item in array)
        {
            if (item.Rbe_json_data.ID.Equals(id))
            {
                //Debug.Log($"------------------> {item.Rbe_json_data.ID}");
                res = true;
                break;
            }
        }
        return res;
    }

    public static List<RBEDataBox> ReplaceBookDataInBagByIndex(List<RBEDataBox> list, RBEDataBox dataBox, int index)
    {
        for (int findBoxIndex = 0; findBoxIndex < list.Count; findBoxIndex++)
        {
            if (list[findBoxIndex].Index == index)
            {
                dataBox.Index = index;
                list[findBoxIndex] = dataBox;
                break;
            }
        }
        return list;
    }

    public static List<RBEDataBox> OrderAndAllBackMove(List<RBEDataBox> list, RBEDataBox dataBox, int index)
    {
        //升序排序
        list = list.OrderBy(x => x.Index).ToList();
        foreach (var item in list)
        {
            if (item.Index >= index)
            {
                item.Index = ((item.Index + 1) % BookSystem.MAX_BOOK_COUNT);
            }
        }
        dataBox.Index = index;
        list.Add(dataBox);
        //再次升序排序
        list = list.OrderBy(x => x.Index).ToList();
        return list;
    }
}*/



//对话适配json结构
#region
[System.Serializable]
public struct JsonData
{
    public string ID;
    public string NAME;
    public string CONDITION;
    public string VALUE;
    public string SELECT;
}

[System.Serializable]
public struct JsonDataArray
{
    public JsonData[] JSON_DATA_ARRAY;
}
#endregion

public static class GameSystem
{
    //有选择题标志
    public static bool EXIST_SELECT_OPTION = false;
    //玩家对话结束标志
    public static bool PLAYER_TALK_END_SIGN = false;
    //玩家对话继续标志
    public static bool PLAYER_TALK_CONTINUE_SIGN = false;

    //读取加载对话配置成功标志
    public static bool CONFIG_TALK_SUCCESS_SIGN = false;
}
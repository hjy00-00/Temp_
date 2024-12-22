using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "New/quest")]
public class Quest : ScriptableObject {
    public string questName;
    public QUEST_RANK questRank;
    public QUEST_TYPE questType;
    [SerializeField] [TextArea] string questInfo;
    public int[] hintCondition;
    public string[] hintInfo;

    public Item[] prizeItems;
    public int[] prizeItemNum;
    public int prizeExp;
    public enum QUEST_RANK { F, E, D, C, B, A, S, SS, SSS }
    public enum QUEST_TYPE { SINGLE, MULTI, CHAIN }

    public string QuestInfo { get { return questInfo; } }
    /*********************************************************************************/
}
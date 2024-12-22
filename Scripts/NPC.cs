using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "New/npc")]
public class NPC : ScriptableObject {
    public string npcName;
    public NPC_TYPE npcType;
    [SerializeField] QUEST_STATE questState;

    public Dialogue[] basics;

    public string[] choice;
    bool acceptance = false;

    public Dialogue[] accept;
    public Dialogue[] refuse;

    public Dialogue[] ing;

    public Dialogue[] clear;

    public Dialogue[] clearAfter;

    public Item[] shopItems;


    GUIManager guiManager;

    public enum NPC_TYPE { NONE, SHOP, GUILD, QUEST_SINGLE, QUEST_MULTI, ETC }
    public enum QUEST_STATE { BASICS, CHOICE, ACCEPT, REFUSE, SELECT_1, SELECT_2, SELECT_3, QUEST_ING, QUEST_CLEAR, CLEAR_AFTER }
    /****************************************/
    public NPC_TYPE NPCType { get { return npcType; } }
    public QUEST_STATE QuestState { get { return questState; } set { questState = value; } }

    /*********************************************************************************/
    public Dialogue[] GetDialogue() {
        if(guiManager == null)
            guiManager = GameManager.GetInstance().GUIManager;

        Dialogue[] dialogues = null;

        switch(questState) {
            case QUEST_STATE.BASICS:
                dialogues = basics;
                break;
            case QUEST_STATE.CHOICE:
                switch(npcType) {
                    case NPC_TYPE.SHOP:
                    case NPC_TYPE.GUILD:
                        guiManager.SetDynamicButton(choice);
                        return null;
                    case NPC_TYPE.QUEST_SINGLE:
                    case NPC_TYPE.QUEST_MULTI:
                        guiManager.SetStaticButton();
                        return null;
                    default:
                        Debug.LogError(npcType + " -> CHOICE");
                        break;
                }
                break;
            case QUEST_STATE.ACCEPT:
                dialogues = accept;
                break;
            case QUEST_STATE.REFUSE:
                dialogues = refuse;
                break;
            case QUEST_STATE.SELECT_1:
                break;
            case QUEST_STATE.SELECT_2:
                break;
            case QUEST_STATE.SELECT_3:
                break;
            case QUEST_STATE.QUEST_ING:
                dialogues = ing;
                break;
            case QUEST_STATE.QUEST_CLEAR:
                dialogues = clear;
                break;
            case QUEST_STATE.CLEAR_AFTER:
                dialogues = clearAfter;
                break;
            default:
                break;
        }
        return dialogues;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    [SerializeField] List<Quest> list_GuildQuest;
    [SerializeField] List<Quest> list_ClearQuest;
    [SerializeField] List<Quest> list_PlayerQuest;

    [SerializeField] GameObject[] m_objLockImage;

    Player player;
    Inventory inventory;
    GUIQuestSearch guiQuestSearch;

    const int F = 0, E = 1, D = 2, C = 3, B = 4, A = 5, S = 6, SS = 7, SSS = 8;
    /*********************************************************************************/
    void Start() {
        player = GameManager.GetInstance().Player;
        inventory = GameManager.GetInstance().Inventory;
        guiQuestSearch = GameManager.GetInstance().GUIManager.GUIQuestSearch;
    }
    /*********************************************************************************/
    public void AddQuest(Quest _quest) {
        // 중복 체크
        for(int i = list_GuildQuest.Count - 1; i >= 0 ; i--) {
            if(list_GuildQuest[i] == _quest)
                return;
        }
        // 길드 목록에서 삭제 & 플레이어 목록에 추가
        list_GuildQuest.Remove(_quest);
        list_PlayerQuest.Add(_quest);
    }
    public void ClearQuest(Quest _clearquest) {
        list_PlayerQuest.Remove(_clearquest);
        ClearPrize(_clearquest);
        // 중복 체크
        for(int i = 0; i < list_ClearQuest.Count; i++) {
            if(list_ClearQuest[i] == _clearquest)
                return;
        }
        // 클리어 목록에 추가
        for(int i = 0; i <= list_ClearQuest.Count; i++) {
            if(list_ClearQuest[i] == null) {
                list_ClearQuest[i] = _clearquest;
                list_ClearQuest.Add(null);
                break;
            }
        }
    }
    void ClearPrize(Quest _clearquest) {
        for(int i = 0; i < _clearquest.prizeItems.Length; i++) {
            inventory.AcquireItem(_clearquest.prizeItems[i], _clearquest.prizeItemNum[i]);
        }
        player.GetExp(_clearquest.prizeExp);
    }

    public void PlayerRankCheck() {
        QuestRankAllLock();
        if(player.PlayerRank != Player.PLAYER_RANK.F) {
            int num = (int)player.PlayerRank - 1;
            Debug.Log(player.PlayerRank + " - " + num);
            QuestRankUnLock(num);
        }
        
        switch(player.PlayerRank) {
            case Player.PLAYER_RANK.F:
                break;
            case Player.PLAYER_RANK.E:
                break;
            case Player.PLAYER_RANK.D:
                break;
            case Player.PLAYER_RANK.C:
                break;
            case Player.PLAYER_RANK.B:
                break;
            case Player.PLAYER_RANK.A:
                break;
            case Player.PLAYER_RANK.S:
                break;
            case Player.PLAYER_RANK.SS:
                break;
            case Player.PLAYER_RANK.SSS:
                break;
        }
    }
    void QuestRankAllLock() {
        for(int i = 0; i < m_objLockImage.Length; i++)
            m_objLockImage[i].gameObject.SetActive(true);
    }
    public void QuestRankUnLock(int _unLockNum) {
        for(int i = 0; i <= _unLockNum; i++)
            m_objLockImage[i].gameObject.SetActive(false);
    }

    Quest.QUEST_RANK QuestRankCheck(int _rank) {
        switch(_rank) {
            case F:
                return Quest.QUEST_RANK.F;
            case E:
                return Quest.QUEST_RANK.E;
            case D:
                return Quest.QUEST_RANK.D;
            case C:
                return Quest.QUEST_RANK.C;
            case B:
                return Quest.QUEST_RANK.B;
            case A:
                return Quest.QUEST_RANK.A;
            case S:
                return Quest.QUEST_RANK.S;
            case SS:
                return Quest.QUEST_RANK.SS;
            case SSS:
                return Quest.QUEST_RANK.SSS;
            default:
                Debug.LogError("rank : " + _rank + " / [0:F] [1:E] [2:D] [3:C] [4:B] [5:A] [6:S] [7:SS] [8:SSS]");
                return Quest.QUEST_RANK.F;
        }
    }
    public void Button_OpenQuestList(int _rank) {
        Quest.QUEST_RANK QuestRank = QuestRankCheck(_rank);
        guiQuestSearch.InitializeQuest();

        for(int i = 0; i < list_GuildQuest.Count; i++) {
            Debug.Log(i + " / " + list_GuildQuest.Count);
            Debug.Log("quest : " + list_GuildQuest[i]);
            if(list_GuildQuest[i] != null) {
                if(list_GuildQuest[i].questRank == QuestRank) {
                    guiQuestSearch.AcquireQuest(list_GuildQuest[i]);
                }
            }
        }
        guiQuestSearch.SetQuestInfo();
    }
}
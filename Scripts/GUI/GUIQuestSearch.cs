using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIQuestSearch : MonoBehaviour {
    [SerializeField] Text m_textQuestName;
    [SerializeField] Text m_textQuestInfo;
    [SerializeField] Text m_textQuestHint;
    [SerializeField] Text m_textPlayerRank;

    [SerializeField] Image[] m_imagePrizeItems;
    [SerializeField] Text[] m_textPrizeItems;
    [SerializeField] Text m_textPrizeExp;

    [SerializeField] GameObject go_QuestBase;
    [SerializeField] GameObject go_PrevButton;
    [SerializeField] GameObject go_NextButton;
    [SerializeField] GameObject go_AcceptButton;
    [SerializeField] GameObject go_NoQuestBase;

    [SerializeField] List<Quest> quests = new List<Quest>();
    int questCount = 0;

    QuestManager questManager;
    /********************************************************************************/
    private void Start() {
        questManager = GameManager.GetInstance().QuestManager;
    }
    /********************************************************************************/
    public void InitializeQuest() {
        questCount = 0;

        ResetInfo();

        for(int i = 0; i < quests.Count; i++)
            quests[i] = null;

        go_NoQuestBase.SetActive(false);
        go_PrevButton.SetActive(false);
        go_NextButton.SetActive(false);
        go_AcceptButton.SetActive(false);
        go_QuestBase.SetActive(false);
    }
    void ResetInfo() {
        m_textQuestName.text = null;
        m_textQuestInfo.text = null;
        m_textQuestHint.text = null;
        m_textPlayerRank.text = null;

        for(int i = 0; i < m_imagePrizeItems.Length; i++) {
            m_imagePrizeItems[i].sprite = null;
            m_textPrizeItems[i].text = null;
            m_imagePrizeItems[i].gameObject.SetActive(false);
            m_textPrizeItems[i].gameObject.SetActive(false);
        }
        m_textPrizeExp.text = null;
    }

    public void AcquireQuest(Quest _quest) {
        // 중복 체크
        for(int i = 0; i < quests.Count; i++) {
            if(quests[i] == _quest)
                return;
        }
        // 추가
        for(int i = 0; i <= quests.Count; i++) {
            Debug.Log("GUI " + i + " / " + quests.Count);
            if(quests[i] == null) {
                quests[i] = _quest;
                quests.Add(null);
                break;
            }
        }
    }

    public void SetQuestInfo() {
        go_QuestBase.SetActive(true);
        go_AcceptButton.SetActive(true);
        if(quests[0] == null) {
            go_NoQuestBase.SetActive(true);
            return;
        }

        m_textQuestName.text = quests[questCount].questName;
        m_textQuestInfo.text = quests[questCount].QuestInfo;
        m_textQuestHint.text = null;

        Player player = GameManager.GetInstance().Player;
        for(int i = 0; i < quests[questCount].hintCondition.Length; i++) {
            if(player.PlayerStatus.nInt >= quests[questCount].hintCondition[i]) {
                if(i == 0)
                    m_textQuestHint.text = quests[questCount].hintInfo[i];
                else
                    m_textQuestHint.text += "\n" + quests[questCount].hintInfo[i];

            }
            else
                break;
        }

        switch(quests[questCount].questRank) {
            case Quest.QUEST_RANK.F:
                m_textPlayerRank.text = "F";
                break;
            case Quest.QUEST_RANK.E:
                m_textPlayerRank.text = "E";
                break;
            case Quest.QUEST_RANK.D:
                m_textPlayerRank.text = "D";
                break;
            case Quest.QUEST_RANK.C:
                m_textPlayerRank.text = "C";
                break;
            case Quest.QUEST_RANK.B:
                m_textPlayerRank.text = "B";
                break;
            case Quest.QUEST_RANK.A:
                m_textPlayerRank.text = "A";
                break;
            case Quest.QUEST_RANK.S:
                m_textPlayerRank.text = "S";
                break;
            case Quest.QUEST_RANK.SS:
                m_textPlayerRank.text = "SS";
                break;
            case Quest.QUEST_RANK.SSS:
                m_textPlayerRank.text = "SSS";
                break;
        }

        for(int i = 0; i < quests[questCount].prizeItems.Length; i++) {
            if(m_imagePrizeItems.Length >= i) {
                if(quests[questCount].prizeItems[i] != null) {
                    m_imagePrizeItems[i].sprite = quests[questCount].prizeItems[i].itemImage;
                    m_textPrizeItems[i].text = quests[questCount].prizeItems[i].itemName + " : " + quests[questCount].prizeItemNum;
                    m_imagePrizeItems[i].gameObject.SetActive(true);
                    m_textPrizeItems[i].gameObject.SetActive(true);
                }
            }
            else {
                Debug.LogError("포상 아이템 수가 5개 이상일 경우 (확장 필요)");
            }
        }
        
        m_textPrizeExp.text = "획득 경험치 : " + quests[questCount].prizeExp;
        
        if(questCount == 0) {
            go_PrevButton.SetActive(false);
            if(questCount == quests.Count) {
                go_NextButton.SetActive(false);
            }
            else {
                go_NextButton.SetActive(true);
            }
        }
        else if(questCount >= quests.Count - 1) {
            questCount = quests.Count;
            go_NextButton.SetActive(false);
        }
        else {
            go_PrevButton.SetActive(true);
            go_NextButton.SetActive(true);
        }
        if(quests[questCount + 1] == null)
            go_NextButton.SetActive(false);
    }
    public void Button_Next() {
        questCount++;
        ResetInfo();
        SetQuestInfo();
    }
    public void Button_Previous() {
        questCount--;
        ResetInfo();
        SetQuestInfo();
    }
    public void Button_Accept() {
        Debug.Log("수락 버튼");
        questManager.AddQuest(quests[questCount]);
        quests.Remove(quests[questCount]);

        InitializeQuest();
    }
}

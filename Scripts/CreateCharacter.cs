using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour {
    public enum E_CREATE_STATE { NONE = -1, SKILL, NAME, STATUS, CUSTOMIZING }
    [SerializeField] GameObject[] m_arrCreateScenes;
    [SerializeField] E_CREATE_STATE m_eCreateState;

    GUIManager m_cGUIManager;
    Player m_cPlayer;
    PlayerController m_cController;
    SkillList m_cSkillList;

    [SerializeField] StartingActiveSkill m_cStartingActiveSkill;
    [SerializeField] GameObject[] m_arrCard;
    [SerializeField] Skill[] m_arrSkill;
    [SerializeField] Text[] m_arrSkillName;
    [SerializeField] Image[] m_arrSkillImage;
    [SerializeField] Text[] m_arrSkillInfo;

    [SerializeField] Image m_imageSkill;
    [SerializeField] Text m_textSkillName;
    [SerializeField] Text m_textSkillInfo;
    [SerializeField] Text m_textName;
    [SerializeField] Text m_textStatus;
    [SerializeField] InputField m_inputName;
    [SerializeField] GameObject go_RandomSlotsParent;
    GUISlot[] m_cRandomSlots;
    [SerializeField] GameObject go_StatusSlotsParent;
    GUISlot[] m_cStatusSlots;
    [SerializeField] GameObject go_GoToCustomizingButton;

    [SerializeField] Item m_cItemStatus;

    Skill getSkill;
    /********************************************************************************/
    void Start() {
        m_cGUIManager = GameManager.GetInstance().GUIManager;
        m_cPlayer = GameManager.GetInstance().Player;
        m_cController = GameManager.GetInstance().PlayerController;
        m_cSkillList = GameManager.GetInstance().SkillList;
        m_cRandomSlots = go_RandomSlotsParent.GetComponentsInChildren<GUISlot>();
        m_cStatusSlots = go_StatusSlotsParent.GetComponentsInChildren<GUISlot>();

        go_GoToCustomizingButton.SetActive(false);
        SetCreateState(E_CREATE_STATE.NONE);
    }
    /********************************************************************************/
    public void InitializeScenes() {
        for(int i = 0; i < m_arrCreateScenes.Length; i++)
            m_arrCreateScenes[i].SetActive(false);
    }
    public void SetCreateState(E_CREATE_STATE state) {
        switch(state) {
            case E_CREATE_STATE.NONE:
                InitializeScenes();
                m_textName.text = m_inputName.text;
                m_cPlayer.Name = m_inputName.text;
                string status = 
                    "HP:? / ?\n" +
                    "MP:? / ?\n" +
                    "EXP :? / ?\n" +
                    "\n" +
                    "ATK :?\n" +
                    "M_ATK :?\n" +
                    "DEF :?\n" +
                    "M_DEF :?\n" +
                    "Critical :?\n" +
                    "\n" + "\n" +
                    "Str[근력] :? / ?\n" +
                    "Con[건강] :? / ?\n" +
                    "Int[마력] :? / ?\n" +
                    "Agi[민첩] :? / ?\n" +
                    "Luk[행운] :? / ?\n" +
                    "Wis[지혜] :? / ? ";
                m_textStatus.text = status;

                for(int i = 0; i < m_cStatusSlots.Length; i++)
                    m_cStatusSlots[i].ClearSlot();
                break;
            case E_CREATE_STATE.SKILL:
                m_arrCreateScenes[0].SetActive(true);
                for(int i = 0; i < m_arrCard.Length; i++) {
                    m_arrCard[i].gameObject.SetActive(true);
                    m_arrSkill[i] = m_cStartingActiveSkill.ShuffleSkill();
                    m_arrSkillName[i].text = m_arrSkill[i].skillName;
                    m_arrSkillInfo[i].text = m_cGUIManager.SkillInfo(m_arrSkill[i]);
                    m_arrSkillImage[i].sprite = m_arrSkill[i].skillImage;
                }
                break;
            case E_CREATE_STATE.NAME:
                m_arrCreateScenes[0].SetActive(false);
                m_arrCreateScenes[1].SetActive(true);
                m_textName.text = null;
                m_cPlayer.Name = null;
                RequirementsCheck();
                break;
            case E_CREATE_STATE.STATUS:
                m_arrCreateScenes[2].SetActive(true);
                m_cPlayer.Level = 1;
                m_cPlayer.Money = 100;
                m_cPlayer.LevelUpPoint = 0;
                m_cPlayer.PlayerStatus = new Status(0, 0, 0, 0, 0, 10);

                for(int i = 0; i < m_cRandomSlots.Length; i++) {
                    m_cRandomSlots[i].ClearSlot();
                    m_cRandomSlots[i].AddItem(m_cItemStatus);
                }

                int max = 24;
                for(int i = 0; i < max; i++) {
                    int num = GameManager.GetInstance().RandomNum(1, m_cRandomSlots.Length);
                    m_cRandomSlots[num - 1].SetSlotCount(1);
                }
                break;
            case E_CREATE_STATE.CUSTOMIZING:
                Debug.Log("CUSTOMIZING");
                m_arrCreateScenes[3].SetActive(true);
                break;
        }
        m_eCreateState = state;
    }
    /****************************************/
    // 1. 스킬 선택
    public void Button_SelectCard(int idx) {
        getSkill = m_arrSkill[idx];

        m_imageSkill.sprite = m_arrSkill[idx].skillImage;
        m_textSkillName.text = m_arrSkill[idx].skillName;
        m_textSkillInfo.text = m_arrSkill[idx].SkillInfo;

        SetCreateState(E_CREATE_STATE.NAME);
    }
    /****************************************/
    // 1. 이름 설정
    public void Event_SetPlayerName() {
        m_textName.text = m_inputName.text;
        m_cPlayer.Name = m_inputName.text;
        RequirementsCheck();

        SetCreateState(E_CREATE_STATE.STATUS);
    }
    // 2. 스텟 설정
    public void SetPlayerStatus() {
        // 1.str 2.con 3.int 4.agi 5.luk 6.wis
        m_cPlayer.PlayerStatus = new Status(0, 0, 0, 0, 0, 10);
        for(int i = 0; i < m_cStatusSlots.Length; i++) {
            int stats = i + 1;
            m_cPlayer.UpStats(stats, m_cStatusSlots[i].count);
        }
        m_textStatus.text = m_cPlayer.ToStatus();
        m_cPlayer.UsePotin(m_cPlayer.PlayerStatus.nMaxHP, m_cPlayer.PlayerStatus.nMaxMP);
        m_textStatus.text = m_cPlayer.ToStatus();

        RequirementsCheck();
    }
    void RequirementsCheck() {
        // 조건 1. 이름 설정
        if(m_cPlayer.Name == "") {
            go_GoToCustomizingButton.SetActive(false);
            return;
        }
        // 조건 2. 슬롯 모두 활성화
        for(int i = 0; i < m_cStatusSlots.Length; i++) {
            if(m_cStatusSlots[i].count == 0) {
                go_GoToCustomizingButton.SetActive(false);
                return;
            }
        }
        go_GoToCustomizingButton.SetActive(true);
    }
    public void Button_GoToCustomizing() {
        SetCreateState(E_CREATE_STATE.CUSTOMIZING);
    }
    /****************************************/
    // 3. 캐릭터 커스텀 설정

    /*
     
    null [ 미구현 ]
     
     */

    public void Button_GoToPlayGame() {
        m_cSkillList.AcquireSkill(getSkill);
        m_cController.Life = true;

        SetCreateState(E_CREATE_STATE.NONE);
        GameManager.GetInstance().SetState(GameManager.E_STATE.PLAY);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {
    public enum E_LOBBY_STATE { MAIN, CHARACTER_SELECT, CHARACTER_CREATE, SHOP }
    [SerializeField] E_LOBBY_STATE m_eLobbyState;
    [SerializeField] List<GameObject> m_listLobbyScenes;
    public enum E_GUI_STATE { NONE = -1, INVENTORY, DYNAMIC_INVENTORY, LEVELUP, DIALOGUE, SHOP, QUEST_SEARCH, QUEST_LIST, SKILL_LIST }
    [SerializeField] E_GUI_STATE m_eGUIState;

    [SerializeField] bool m_bGUIActivated = false;

    [SerializeField] GameObject go_Crosshair;
    [SerializeField] GameObject go_PopUpBase;
    [SerializeField] GameObject go_PlayerInfoBase;
    [SerializeField] GameObject go_InventoryBase;
    [SerializeField] GameObject go_ItemInfoBase;
    [SerializeField] Text text_Money;

    [SerializeField] GameObject go_QuestSearchBase;

    [SerializeField] GameObject go_DynamicInventoryBase;
    [SerializeField] Text text_DynamicName;
    [SerializeField] GameObject go_QuestList;
    [SerializeField] GameObject go_SkillList;



    [SerializeField] GameObject[] go_StaticButton;
    [SerializeField] GameObject[] go_DynamicButton;
    [SerializeField] Text[] text_DynamicButton;


    [SerializeField] GUIStatus m_cGUIStatus;
    [SerializeField] GUIItemInfo m_cGUIItemInfo;
    [SerializeField] GUIDialogue m_cGUIDialogue;
    [SerializeField] QuestManager m_cQuestManager;
    [SerializeField] GUIQuestSearch m_cGUIQuestSearch;
    [SerializeField] GUIQuestList m_cGUIQuestList;
    [SerializeField] GUIBox m_cGUIBox;

    //[SerializeField] GUIQuest m_cGUIQuest;
    //[SerializeField] GUIText m_cGUIText;

    [SerializeField] LevelUpManager m_cLevelUpManager;

    Player m_cPlayer;
    EquipmentSlot m_cEquipmentSlot;
    DynamicInventory m_cDynamicInventory;
    DropInventory m_DropInventory;
    NPC m_cTargetNPC;
    GameObject m_objTargetInventory;
    bool m_bDialogueTyping = false;
    LevelUp m_cLevelUp;
    /****************************************/
    public bool GUIActivated { get { return m_bGUIActivated; } set { m_bGUIActivated = value; } }
    public GameObject ItemInfoBase { get { return go_ItemInfoBase; } set { go_ItemInfoBase = value; } }


    public GUIItemInfo GUIItemInfo { get { return m_cGUIItemInfo; } }
    public GUIDialogue GUIDialogue { get { return m_cGUIDialogue; } }
    public E_GUI_STATE PopUp { get { return m_eGUIState; } }

    public GUIQuestSearch GUIQuestSearch { get { return m_cGUIQuestSearch; } }

    public DynamicInventory DynamicInventory { get { return m_cDynamicInventory; } set { m_cDynamicInventory = value; } }
    public DropInventory DropInventory { get { return m_DropInventory; } set { m_DropInventory = value; } }
    public NPC TargetNPC { get { return m_cTargetNPC; } set { m_cTargetNPC = value; } }
    public GameObject TargetInventory { get { return m_objTargetInventory; } set { m_objTargetInventory = value; } }
    public bool Typing { get { return m_bDialogueTyping; } }

    /********************************************************************************/
    void Start() {
        m_cPlayer = GameManager.GetInstance().Player;
        m_cEquipmentSlot = GameManager.GetInstance().EquipmentSlot;

        m_DropInventory = GameManager.GetInstance().DropInventory;
        //m_cDynamicInventory = GameManager.GetInstance().DynamicInventory;

        m_cLevelUp = GameManager.GetInstance().LevelUp;

        ShowLobbyScenes(0);
    }

    void FixedUpdate() {
        //카메라 이동        
        if(!m_bGUIActivated) {
            PlayerController playerController = GameManager.GetInstance().PlayerController;
            playerController.CameraRotation();
            playerController.CharacterRotation();
        }
    }
    void Update() {
        if(m_bDialogueTyping)
            UpdateDialogueState();
    }
    /********************************************************************************/
    public void ShowLobbyScenes(int _idx) {
        for(int i = 0; i < m_listLobbyScenes.Count; i++) {
            if(i == _idx)
                m_listLobbyScenes[i].SetActive(true);
            else
                m_listLobbyScenes[i].SetActive(false);
        }
    }
    public void SetGUIPopup(E_GUI_STATE _state) {
        AllClosePopUp();

        m_eGUIState = _state;
        m_bGUIActivated = !m_bGUIActivated;

        if(m_bGUIActivated)
            OnPopUp();
        else
            OffPopUp();
    }
    void OnPopUp() {
        go_Crosshair.SetActive(false);
        go_PopUpBase.SetActive(true);

        switch(m_eGUIState) {
            case E_GUI_STATE.NONE:
                AllClosePopUp();
                break;
            case E_GUI_STATE.INVENTORY:
                go_InventoryBase.SetActive(true);
                go_PlayerInfoBase.SetActive(true);
                m_cGUIItemInfo.ClearInfo();
                StatusUpdate();
                MoneyUpdate();
                break;
            case E_GUI_STATE.DYNAMIC_INVENTORY:
                Debug.Log("DYNAMIC_INVENTORY open");
                if (m_cTargetNPC.npcType == NPC.NPC_TYPE.SHOP)
                {
                    m_cDynamicInventory.SetDynamicInventory(m_cTargetNPC, "상점", 48);
                }

                go_InventoryBase.SetActive(true);
                go_DynamicInventoryBase.SetActive(true);
                break;
            case E_GUI_STATE.LEVELUP:
                m_cLevelUpManager.Shuffle = true;
                m_cLevelUpManager.ShowCards();
                break;
            case E_GUI_STATE.DIALOGUE:
                switch(TargetNPC.QuestState) {
                    case NPC.QUEST_STATE.CHOICE:
                        Debug.Log("!");
                        if(TargetNPC.npcType == NPC.NPC_TYPE.SHOP || TargetNPC.npcType == NPC.NPC_TYPE.GUILD) {
                            for(int i = 0; i < go_DynamicButton.Length; i++) {
                                go_DynamicButton[i].SetActive(true);
                                text_DynamicButton[i].text = TargetNPC.choice[i];
                            }
                        }
                        else {
                            for(int i = 0; i < go_StaticButton.Length; i++)
                                go_StaticButton[i].SetActive(true);
                        }
                        break;
                    case NPC.QUEST_STATE.ACCEPT:
                        break;
                    case NPC.QUEST_STATE.REFUSE:
                        break;
                    case NPC.QUEST_STATE.QUEST_ING:
                        break;
                    case NPC.QUEST_STATE.QUEST_CLEAR:
                        break;
                    case NPC.QUEST_STATE.CLEAR_AFTER:
                        break;
                    default:
                        break;
                }
                break;
            case E_GUI_STATE.SHOP:
                go_InventoryBase.SetActive(true);
                go_DynamicInventoryBase.SetActive(true);
                m_cDynamicInventory.SetDynamicInventory(m_cTargetNPC, "상점", 48);
                MoneyUpdate();
                for(int i = 0; i < m_cTargetNPC.shopItems.Length; i++) {
                    if(m_cTargetNPC.shopItems[i] != null) {
                        m_cDynamicInventory.AddItem(m_cTargetNPC.shopItems[i]);
                    }
                }

                break;
            case E_GUI_STATE.QUEST_SEARCH:
                go_QuestSearchBase.SetActive(true);
                m_cGUIQuestSearch.InitializeQuest();
                m_cQuestManager.PlayerRankCheck();
                break;
            //case E_GUI_STATE.QUEST:
            //NPC npc = m_objTargetNPC.GetComponent<NPC>();
            //npc.SetQuest = m_cTargetQuest;
            //if(npc.State == NPC.E_QUEST_STATE.NONE)
            //    npc.SetState(NPC.E_QUEST_STATE.NORMAL);
            //else if(npc.State == NPC.E_QUEST_STATE.QUEST_ING)
            //    npc.SetState(NPC.E_QUEST_STATE.QUEST_CHECK);
            //else
            //    npc.SetState(npc.State);
            //break;
            case E_GUI_STATE.QUEST_LIST:
                go_QuestList.SetActive(true);
                break;
            case E_GUI_STATE.SKILL_LIST:
                go_SkillList.SetActive(true);
                break;
            default:
                Debug.LogError("PopUp Null");
                break;
        }
    }
    public void OffPopUp() {
        go_Crosshair.SetActive(true);
        go_PopUpBase.SetActive(false);

        switch(m_eGUIState) {
            case E_GUI_STATE.INVENTORY:
                go_InventoryBase.SetActive(false);
                go_PlayerInfoBase.SetActive(false);
                m_eGUIState = E_GUI_STATE.NONE;
                break;
            case E_GUI_STATE.DYNAMIC_INVENTORY:
                Debug.Log("동적 인벤 닫기");

                SetDynamicInventory();

                go_InventoryBase.SetActive(false);
                go_DynamicInventoryBase.SetActive(false);
                //m_cDropInventory.BoxDespawn();
                m_eGUIState = E_GUI_STATE.NONE;
                break;
            case E_GUI_STATE.LEVELUP:
                m_cLevelUpManager.Shuffle = false;
                m_eGUIState = E_GUI_STATE.NONE;
                m_cLevelUpManager.CloseCard();
                break;
            case E_GUI_STATE.DIALOGUE:
                m_cGUIDialogue.OnOff(false);

                // 대화 후 NPC State
                switch(TargetNPC.QuestState) {
                    case NPC.QUEST_STATE.BASICS:
                        if(TargetNPC.npcType != NPC.NPC_TYPE.NONE) {
                            TargetNPC.QuestState = NPC.QUEST_STATE.CHOICE;

                            SetDialogue();
                        }
                        break;
                    case NPC.QUEST_STATE.CHOICE:
                        TargetNPC.QuestState = NPC.QUEST_STATE.BASICS;
                        break;
                    case NPC.QUEST_STATE.ACCEPT:
                        TargetNPC.QuestState = NPC.QUEST_STATE.BASICS;
                        break;
                    case NPC.QUEST_STATE.REFUSE:
                        TargetNPC.QuestState = NPC.QUEST_STATE.BASICS;
                        break;
                    case NPC.QUEST_STATE.QUEST_ING:

                        break;
                    case NPC.QUEST_STATE.QUEST_CLEAR:

                        break;
                    case NPC.QUEST_STATE.CLEAR_AFTER:

                        break;
                    default:
                        break;
                }
                break;
            case E_GUI_STATE.SHOP:
                go_InventoryBase.SetActive(false);
                go_DynamicInventoryBase.SetActive(false);
                m_cDynamicInventory.SetDynamicInventory(null, "");

                m_eGUIState = E_GUI_STATE.NONE;
                break;
            case E_GUI_STATE.QUEST_SEARCH:
                go_QuestSearchBase.SetActive(false);
                




                break;
            //case E_GUI_STATE.QUEST:
            //NPC npc = m_objTargetNPC.GetComponent<NPC>();
            //if(npc.State == NPC.E_QUEST_STATE.QUEST)
            //    npc.SetState(NPC.E_QUEST_STATE.NONE);
            //npc.Button.SetActive(false);
            //SetGUIPopup(E_GUI_STATE.NONE);
            //break;
            case E_GUI_STATE.QUEST_LIST:
                go_QuestList.SetActive(false);
                m_eGUIState = E_GUI_STATE.NONE;
                break;
            case E_GUI_STATE.SKILL_LIST:
                go_SkillList.SetActive(false);
                m_eGUIState = E_GUI_STATE.NONE;
                break;
            default:
                Debug.LogError("PopUp Null");
                break;
        }
    }
    public void AllClosePopUp() {
        go_Crosshair.SetActive(true);
        go_PopUpBase.SetActive(false);

        go_InventoryBase.SetActive(false);
        go_PlayerInfoBase.SetActive(false);

        SetDynamicInventory();

        go_InventoryBase.SetActive(false);
        go_DynamicInventoryBase.SetActive(false);

        m_cLevelUpManager.Shuffle = false;
        m_cLevelUpManager.CloseCard();

        m_cDynamicInventory.SetDynamicInventory(null, "");

        go_QuestSearchBase.SetActive(false);

        //go_QuestList.SetActive(false);

        go_SkillList.SetActive(false);

        //go_SettingBase.SetActive(false);


        for(int i = 0; i < go_StaticButton.Length; i++)
            go_StaticButton[i].SetActive(false);

        SetDynamicButton();

        m_eGUIState = E_GUI_STATE.NONE;
    }
    /****************************************/
    public void SetDialogue() {
        if(TargetNPC != null) {
            Debug.Log("NPC.QuestState : " + TargetNPC.QuestState);
            switch(TargetNPC.QuestState) {
                case NPC.QUEST_STATE.BASICS:

                    break;
                case NPC.QUEST_STATE.CHOICE:
                    m_bDialogueTyping = true;

                    break;
                case NPC.QUEST_STATE.ACCEPT:
                    break;
                case NPC.QUEST_STATE.REFUSE:
                    break;
                case NPC.QUEST_STATE.QUEST_ING:
                    break;
                case NPC.QUEST_STATE.QUEST_CLEAR:
                    break;
                case NPC.QUEST_STATE.CLEAR_AFTER:
                    break;
                default:
                    Debug.Log("default");
                    break;
            }
            if(TargetNPC.GetDialogue() != null)
                m_cGUIDialogue.ShowDialogue(TargetNPC.GetDialogue());
        }

    }
    void UpdateDialogueState() {
        if(TargetNPC.QuestState == NPC.QUEST_STATE.CHOICE) {
            switch(TargetNPC.NPCType) {
                case NPC.NPC_TYPE.SHOP:
                case NPC.NPC_TYPE.GUILD:
                    if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
                        ButtonDynamic(1);
                    }
                    if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
                        ButtonDynamic(2);
                    }
                    if(Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) {
                        ButtonDynamic(3);
                    }
                    if(Input.GetKeyDown(KeyCode.Escape)) {
                        Debug.Log("ESC");
                        TargetNPC.QuestState = NPC.QUEST_STATE.BASICS;
                        m_bDialogueTyping = false;
                    }
                    break;
                case NPC.NPC_TYPE.QUEST_SINGLE:
                case NPC.NPC_TYPE.QUEST_MULTI:
                    if(Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
                        Button_Yes();
                    }
                    if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
                        Button_No();
                    }
                    if(Input.GetKeyDown(KeyCode.Escape)) {
                        Debug.Log("ESC");
                        TargetNPC.QuestState = NPC.QUEST_STATE.BASICS;
                        m_bDialogueTyping = false;
                    }
                    break;
            }
        }
    }
    /****************************************/
    public void SetStaticButton() {
        for(int i = 0; i < go_StaticButton.Length; i++)
            go_StaticButton[i].SetActive(true);
    }
    public void SetDynamicButton(string[] _choice = null) {
        for(int i = 0; i < go_DynamicButton.Length; i++) {
            text_DynamicButton[i].text = null;
            go_DynamicButton[i].SetActive(false);            
        }
        if(go_DynamicButton.Length < text_DynamicButton.Length) {
            Debug.LogError("오브젝트 부족\n" + go_DynamicButton.Length + " / " + text_DynamicButton.Length);
            return;        
        }
        if(_choice != null) {
            for(int i = 0; i < _choice.Length; i++) {
                int num = i + 1;
                Debug.Log(_choice[i]);
                if(_choice[i] != null) {
                    text_DynamicButton[i].text = "[" + num + "] " + _choice[i];
                    go_DynamicButton[i].SetActive(true);
                }
            }
        }
    }

    public void Button_Yes() {
        //m_bGUIActivated = false;
        Debug.Log("Yes Button 1 : " + TargetNPC.QuestState);
        TargetNPC.QuestState = NPC.QUEST_STATE.ACCEPT;
        Debug.Log("Yes Button 2 : " + TargetNPC.QuestState);
        SetDialogue();
        //SetGUIPopup(E_GUI_STATE.DIALOGUE);
        m_bDialogueTyping = false;
    }
    public void Button_No() {
        //m_bGUIActivated = false;

        TargetNPC.QuestState = NPC.QUEST_STATE.ACCEPT;
        SetDialogue();
        //SetGUIPopup(E_GUI_STATE.DIALOGUE);
        m_bDialogueTyping = false;
    }

    public void ButtonDynamic(int _num) {
        switch(TargetNPC.npcType) {
            case NPC.NPC_TYPE.SHOP:
                switch(_num) {
                    case 1:
                        // 구매 & 판매
                        m_bGUIActivated = false;

                        TargetNPC.QuestState = NPC.QUEST_STATE.SELECT_1;
                        SetGUIPopup(E_GUI_STATE.SHOP);
                        m_bDialogueTyping = false;
                        break;
                    case 2:
                        // 아이템 요청
                        Debug.Log(_num + " / 요청");
                        m_bGUIActivated = false;

                        TargetNPC.QuestState = NPC.QUEST_STATE.SELECT_2;

                        m_bDialogueTyping = false;
                        break;
                }
                break;
            case NPC.NPC_TYPE.GUILD:
                switch(_num) {
                    case 1:
                        // 퀘스트 탐색
                        m_bGUIActivated = false;

                        TargetNPC.QuestState = NPC.QUEST_STATE.SELECT_1;
                        SetGUIPopup(E_GUI_STATE.QUEST_SEARCH);
                        m_bDialogueTyping = false;
                        break;
                    case 2:
                        // 퀘스트 완료
                        m_bGUIActivated = false;
                        Debug.Log("퀘스트 완료");
                        TargetNPC.QuestState = NPC.QUEST_STATE.SELECT_2;

                        m_bDialogueTyping = false;
                        break;
                    case 3:
                        // 랭크 업 or 랭크 업 조건 확인
                        m_bGUIActivated = false;
                        Debug.Log("랭크 업 or 랭크 업 조건 확인 - 준비중");
                        TargetNPC.QuestState = NPC.QUEST_STATE.SELECT_3;

                        m_bDialogueTyping = false;
                        break;
                }
                break;
        }
    }
    /****************************************/
    void SetDynamicInventory()
    {
        if (m_DropInventory != null)
        {
            if (m_DropInventory.DropItems != null)
            {
                for (int i = 0; i < m_cDynamicInventory.GetSlots.Length; i++)
                {
                    if (m_cDynamicInventory.GetSlots[i].item != null)
                    {
                        m_DropInventory.DropItems[i] = m_cDynamicInventory.GetSlots[i].item;
                        m_DropInventory.DropItemCount[i] = m_cDynamicInventory.GetSlots[i].count;
                    }
                    else
                    {
                        if (m_eGUIState == E_GUI_STATE.DYNAMIC_INVENTORY)
                        {
                            if (m_DropInventory.DropItems.Length - 1 < i) { break; }
                            m_DropInventory.DropItems[i] = null;
                            m_DropInventory.DropItemCount[i] = 0;
                        }
                    }
                }
            }
        }
    }
    /****************************************/
    public void StatusUpdate() {
        m_cEquipmentSlot.SlotsCheck();
        m_cGUIStatus.Initialize(m_cPlayer);
    }
    public void MoneyUpdate() {
        text_Money.text = m_cPlayer.Money.ToString();
    }
    /****************************************/

    // 레벨업 시 선택한 카드 내용 획득
    [SerializeField] Text m_textUpStatus;

    public void Button_SelectCard(int _idx) {
        m_cPlayer.LevelUpPoint--;
        switch(_idx) {
            case 0:
                // 돈 획득
                m_cPlayer.GetMoney(m_cLevelUpManager.GetSelectCard(_idx).GetMoney);
                OffPopUp();
                break;
            case 1:
            case 2:
                // 스텟 획득
                m_textUpStatus.text = string.Format(m_cLevelUpManager.GetSelectCard(_idx).GetUpSatusName + " + " + "<color=green>" + m_cLevelUpManager.GetSelectCard(_idx).Number + "</color>");
                m_cPlayer.UpStats(m_cLevelUpManager.GetSelectCard(_idx).GetStatus);
                // 코루틴 적용 필요 (올라간 스텟 확인용)
                StartCoroutine(UpStatusInfo());

                m_bGUIActivated = false;
                OffPopUp();
                break;
            case 3:
                // 스킬 획득

                OffPopUp();
                break;
            case 4:
                // 장비 획득

                OffPopUp();
                break;
        }
        m_cPlayer.UsePotin(m_cPlayer.PlayerStatus.nMaxHP, m_cPlayer.PlayerStatus.nMaxMP);
    }    
    IEnumerator UpStatusInfo() {
        m_textUpStatus.gameObject.SetActive(true);
        for(int time = 0; time <= 60; time++) {
            if(time >= 60) {
                m_textUpStatus.gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
    }
    /****************************************/
    public string SkillInfo(Skill _skill) {
        string info = null;

        // 보유 중이면 현재 레벨 + 1 출력, 그 외 New 출력
        if(false) {

        }
        else
            info += "New" + "\n" + "\n";
        info += _skill.SkillInfo;

        return info;
    }
}
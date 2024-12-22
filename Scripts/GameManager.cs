using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public enum E_STATE { NONE = -1, TITLE, LOBBY, PLAY, GAMEOVER, SETTING }
    
    [SerializeField] List<GameObject> m_listScenes;
    [SerializeField] E_STATE m_eState;


    [SerializeField] Camera camera_Main;
    [SerializeField] Transform trans_CamAxis;

    [SerializeField] ItemManager m_cItemManager;
    [SerializeField] SkillManager m_cSkillManager;
    [SerializeField] LevelUpManager m_cLevelupManager;
    [SerializeField] GUIManager m_cGUIManager;
    [SerializeField] QuestManager m_cQuestManager;

    [SerializeField] CreateCharacter m_cCreateCharacter;


    [SerializeField] ActionController m_cActionController;
    [SerializeField] PlayerController m_cPlayerController;
    [SerializeField] Player m_cPlayer;
    [SerializeField] Inventory m_cInventory;
    [SerializeField] EquipmentSlot m_cEquipmentSlot;
    [SerializeField] DynamicInventory m_cDynamicInventory;
    [SerializeField] SkillList m_cSkillList;
    [SerializeField] LevelUp m_cLevelUp;

    [SerializeField] DropItem m_cDropItem;
    [SerializeField] DropInventory m_cDropInventory;
    //[SerializeField] DropInventory m_cDropInventory;
    [SerializeField] Spawn m_cSpawn;

    [SerializeField] GameObject go_CreateCharacterPopUp;
    [SerializeField] GameObject go_CreateButton;

    //Drop m_cDrop;
    GameObject go_TargetNPC;
    //Quest m_cTargetQuest;

    /****************************************/
    public E_STATE GetState { get { return m_eState; } }

    public Camera MainCamera { get { return camera_Main; } }

    public ItemManager ItemManager { get { return m_cItemManager; } }
    public SkillManager SkillManager { get { return m_cSkillManager; } }
    public LevelUpManager LevelUpManager { get { return m_cLevelupManager; } }
    public GUIManager GUIManager { get { return m_cGUIManager; } }
    public QuestManager QuestManager { get { return m_cQuestManager; } }
    public CreateCharacter CreateCharacter { get { return m_cCreateCharacter; } }

    public ActionController ActionController { get { return m_cActionController; } }
    public PlayerController PlayerController { get { return m_cPlayerController; } }
    public Player Player { get { return m_cPlayer; } }
    public Inventory Inventory { get { return m_cInventory; } }
    public EquipmentSlot EquipmentSlot { get { return m_cEquipmentSlot; } }
    public DynamicInventory DynamicInventory { get { return m_cDynamicInventory; } }
    public SkillList SkillList { get { return m_cSkillList; } }
    public LevelUp LevelUp { get { return m_cLevelUp; } }
    public DropItem DropItem { get { return m_cDropItem; } }
    public DropInventory DropInventory { get { return m_cDropInventory; } }
    //public DropInventory DropInventory { get { return m_cDropInventory; } }
    public Spawn Spawn { get { return m_cSpawn; } }

    //public Drop Drop { get { return m_cDrop; } set { m_cDrop = value; } }

    public GameObject TargetNPC { get { return go_TargetNPC; } set { go_TargetNPC = value; } }
    //public Quest TargetQuest { get { return m_cTargetQuest; } set { m_cTargetQuest = value; } }

    [SerializeField] GameObject go_TempSpawnPos;
    [SerializeField] Inventory cTempInven;

    /********************************************************************************/
    //싱글톤기법: 객체는 원래 1개만 생성되로록 규칙이 정해져있어야하지만,
    //인스펙터사용등 편리함을 이유로 유니티에서 느슨한 규칙을 적용한것임.
    static GameManager m_cInstance;

    static public GameManager GetInstance() {
        return m_cInstance;
    }
    /********************************************************************************/
    void Awake() {
        m_cInstance = this;

        SetState(E_STATE.TITLE);
        //SetState(E_GUI_STATE.PLAY);
        //m_cMonsterManager.InitMonsters();
        //m_cMonster.ItemSetig();

        m_cGUIManager.DynamicInventory = m_cDynamicInventory;
    }
    void Start() {
        m_cGUIManager.ItemInfoBase.SetActive(false);
        m_cGUIManager.AllClosePopUp();
    }
    void Update() {
        UpdateGUIState();
        // 플레이로 이동
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for (int i = 1; i < 6; i++)
            {
                m_cPlayer.UpStats(i, 10);
            }
            SetState(E_STATE.PLAY);
        }
    }
    /********************************************************************************/


    void ShowScenes(int _idx) {
        for(int i = 0; i < m_listScenes.Count; i++) {
            if(i == _idx)
                m_listScenes[i].SetActive(true);
            else
                m_listScenes[i].SetActive(false);
        }
    }

    public void SetState(E_STATE _state) {
        switch(_state) {
            case E_STATE.TITLE:
                Time.timeScale = 0;
                break;
            case E_STATE.LOBBY:
                Time.timeScale = 0;
                go_CreateButton.SetActive(true);
                break;
            case E_STATE.PLAY:
                Time.timeScale = 1;
                break;
            case E_STATE.GAMEOVER:
                Time.timeScale = 0;
                break;
        }
        ShowScenes((int)_state);
        m_eState = _state;
    }
    void UpdateGUIState() {
        switch(m_eState) {
            case E_STATE.TITLE:
                // 마우스 왼쪽, 휠, 오른쪽 클릭 시
                if(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(2)) {
                    SetState(E_STATE.LOBBY);
                }
                break;
            case E_STATE.LOBBY:
                

                break;
            case E_STATE.PLAY:

                // 퀘스트 리스트
                //if(Input.GetKeyDown(KeyCode.Q)) {
                //    GameManager.GetInstance().GetInstance().SetGUIPopup(E_GUI_POPUP.QUEST_LIST);
                //    SetGUIPopup(E_GUI_POPUP.QUEST_LIST);
                //    //m_cQuestList.SetQuestGUI();
                //}

                // 팝업 off
                if(Input.GetKeyDown(KeyCode.Escape)) {
                    m_cGUIManager.GUIActivated = false;
                    m_cGUIManager.AllClosePopUp();
                }

                // 임시 스폰 박스
                if(Input.GetKeyDown(KeyCode.Alpha0)) {
                    m_cSpawn.SpawnInven(go_TempSpawnPos);
                }
                // 임시 즉사 키
                if (Input.GetKeyDown(KeyCode.Alpha9))
                {
                    m_cPlayer.Damage(99999);
                }
                break;
            case E_STATE.GAMEOVER:
                //
                break;
        }
    }

    public void EventGUIState(int _idx) {
        SetState((E_STATE)_idx);
    }
    public void EventGUIGameQuit() {
        Application.Quit();
    }

    /********************************************************************************/
    // 취소 버튼 (팝업 이동) - GUIManagerr 로 이동?
    public void Button_Lobby(int _idx) {
        switch(_idx) {
            case 0: // MAIN
                m_cCreateCharacter.SetCreateState(CreateCharacter.E_CREATE_STATE.NONE);
                break;
            case 1: // CHARACTER_CREATE
                m_cCreateCharacter.InitializeScenes();
                m_cCreateCharacter.SetCreateState(CreateCharacter.E_CREATE_STATE.SKILL);
                break;
        }
        m_cGUIManager.ShowLobbyScenes(_idx);
    }


    public void OpenBox() {
        //m_cdropinventory.setdynamicslot(m_cdrop);

        //m_cdrop.boxclosed.setactive(false);
        //m_cdrop.boxbody.setactive(true);
        //m_cdrop.boxcap.setactive(true);
    }
    /********************************************************************************/

    // 모바일용 조이스틱 설정 (미구현)
    [SerializeField] GameObject m_guiJoystic;
    [SerializeField] GameObject m_guiJoystickOn;
    [SerializeField] GameObject m_guiJoystickOff;
    public void ShowJoystick() {
        m_guiJoystickOn.SetActive(false);
        m_guiJoystickOff.SetActive(true);
        m_guiJoystic.SetActive(true);
    }
    public void CloseJoystick() {
        m_guiJoystickOn.SetActive(true);
        m_guiJoystickOff.SetActive(false);
        m_guiJoystic.SetActive(false);
    }
    /****************************************/
    public int RandomNum(int _start, int _end) {
        int num = 0;
        num = Random.Range(_start, _end + 1);
        return num;
    }

    public int DamageSum(Skill _skill) {
        float Return = 0;
        if(_skill.skillAttackType == Skill.SKILL_ATTACK_TYPE.PHYSICAL) {
            int Str = m_cPlayer.PlayerStatus.nStr;      // + 장비 스텟

            Return = Str;
            Return *= _skill.x;
            return (int)Return;
        }
        else if(_skill.skillAttackType == Skill.SKILL_ATTACK_TYPE.MAGIC) {
            int Int = m_cPlayer.PlayerStatus.nInt;      // + 장비 스텟
            Debug.Log("Dam : Int" + Int);
            Return = Int;
            Return *= _skill.x;
            return (int)Return;
        }
        else if(_skill.skillAttackType == Skill.SKILL_ATTACK_TYPE.MIX) {
            Debug.Log("물+마 공격");
            return 0;
        }
        else {
            Debug.Log("물리 공격도 마법 공격도 혼합 공격도 아닌 경우 ?");
            return 0;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUp : MonoBehaviour {
    public enum E_CARD { RANDOM_MONEY, STATS_1, STATS_2, RANDOM_SKILL, RANDOM_EQUIPMENT }

    bool m_bShuffle;
    int m_nName;
    int m_nNumber;

    string upSatusName;
    int money;
    Status status;
    Skill skill;
    Item equipment;

    [SerializeField] E_CARD m_eCARD;
    [SerializeField] Text m_textCardName;
    [SerializeField] Text m_textCardNum;
    [SerializeField] Image m_imgIcon;

    GUIManager m_cGUIManager;
    SkillManager m_cSkillManager;


    public bool Shuffle { get { return m_bShuffle; } set { m_bShuffle = value; } }
    public int Number { get { return m_nNumber; } }

    public string GetUpSatusName { get { return upSatusName; } }
    public int GetMoney { get { return money; } }
    public Status GetStatus { get { return status; } }
    public Skill GetSkill { get { return skill; } }
    public Item GetEquipment { get { return equipment; } }
    /*********************************************************************************/
    private void Start() {
        m_cGUIManager = GameManager.GetInstance().GUIManager;
        m_cSkillManager = GameManager.GetInstance().SkillManager;
    }
    /*********************************************************************************/
    public void InitCard(int _card) {
        m_nName = -1;
        m_nNumber = -1;

        money = 0;
        status = new Status();
        skill = null;
        equipment = null;

        if(m_bShuffle == true) {
            switch(_card) {
                case 0:
                    SetCard(E_CARD.RANDOM_MONEY);
                    break;
                case 1:
                    SetCard(E_CARD.STATS_1);
                    break;
                case 2:
                    SetCard(E_CARD.STATS_2);
                    break;
                case 3:
                    SetCard(E_CARD.RANDOM_SKILL);
                    break;
                case 4:
                    SetCard(E_CARD.RANDOM_EQUIPMENT);
                    break;
            }
            m_bShuffle = false;
        }
    }

    //  m_nName
    //  [1:HP] [2:MP]
    //  [   2 ~ 10  ] 
    //  [3:STR] [4:CON] [5:INT] [6:AGI] [7:LUK] [8:WIS]
    //  [                  1 ~ 5                    ]
    public void SetCard(E_CARD _card) {
        switch(_card) {
            case E_CARD.RANDOM_MONEY:
                int _money = GameManager.GetInstance().Player.Level;
                _money = _money * 1000;
                m_nNumber = GameManager.GetInstance().RandomNum(_money / 2, _money);

                money = m_nNumber;
                break;
            case E_CARD.STATS_1:
                m_nName = GameManager.GetInstance().RandomNum(1, 2);
                m_nNumber = GameManager.GetInstance().RandomNum(2, 10);

                switch(m_nName) {
                    case 1:
                        upSatusName = "HP";
                        status = new Status(0, m_nNumber);
                        break;
                    case 2:
                        upSatusName = "MP";
                        status = new Status(0, 0, 0, m_nNumber);
                        break;
                }
                break;
            case E_CARD.STATS_2:
                m_nName = GameManager.GetInstance().RandomNum(3, 8);
                m_nNumber = GameManager.GetInstance().RandomNum(1, 5);
                upSatusName = null;
                switch(m_nName) {
                    case 3:
                        upSatusName = "STR";
                        status = new Status(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, m_nNumber);
                        break;
                    case 4:
                        upSatusName = "CON";
                        status = new Status(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, m_nNumber);
                        break;
                    case 5:
                        upSatusName = "INT";
                        status = new Status(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, m_nNumber);
                        break;
                    case 6:
                        upSatusName = "AGI";
                        status = new Status(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, m_nNumber);
                        break;
                    case 7:
                        upSatusName = "LUK";
                        status = new Status(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, m_nNumber);
                        break;
                    case 8:
                        upSatusName = "WIS";
                        status = new Status(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, m_nNumber);
                        break;
                }
                break;
            case E_CARD.RANDOM_SKILL:
                //m_cSkillManager.GetSkill(RandomNum(min, max));
                //가져온 스킬 이름
                //가져온 스킬 적용
                break;
            case E_CARD.RANDOM_EQUIPMENT:
                //m_cEquipmentManager.GetEquipment(RandomNum(min, max));
                //가져온 장비 이름
                //m_nName = 
                //가져온 장비 획득
                //m_nNumber = 
                break;
        }
        PrintCard();
    }
    public void PrintCard() {
        switch(m_eCARD) {
            case E_CARD.RANDOM_MONEY:
                m_textCardName.text = string.Format("Random Money");
                m_textCardNum.text = string.Format("{0}", m_nNumber);
                break;
            case E_CARD.STATS_1:
            case E_CARD.STATS_2:
                m_textCardName.text = string.Format(upSatusName);
                m_textCardNum.text = string.Format("{0}", m_nNumber);
                break;

            case E_CARD.RANDOM_SKILL:
                m_textCardName.text = string.Format("스킬 이름");
                m_textCardNum.text = string.Format("");
                break;
            case E_CARD.RANDOM_EQUIPMENT:
                m_textCardName.text = string.Format("장비 이름");
                m_textCardNum.text = string.Format("");
                break;
        }
    }
}
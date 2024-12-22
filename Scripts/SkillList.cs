using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillList : MonoBehaviour {
    const int ALL = 0, ACTIVE = 1, PASSIVE = 2, BUFF = 3, CURE = 4,
        FIRE = 1, AQUA = 2, ELEC = 3, SOIL = 4, WIND = 5, LIGHT = 6, DARK = 7;

    [SerializeField] GameObject go_SkillListSlotPrefab;
    [SerializeField] GameObject go_SkillNmaePrefab;

    [SerializeField] List<GUISlot> list_SkillSlot;
    [SerializeField] List<GameObject> list_SkillNameList;

    [SerializeField] GridLayoutGroup m_gridLayoutContentSkillSlot;
    [SerializeField] GridLayoutGroup m_gridLayoutContentSkillName;

    [SerializeField] GameObject go_SkillAttribute;

    [SerializeField] Text text_SkillName;
    [SerializeField] Image image_SkillImage;
    [SerializeField] Text text_SkillInfo;
    [SerializeField] Text text_SkillStatus;

    public List<GUISlot> SkillSlotList { get { return list_SkillSlot; } }
    public List<GameObject> SkillNameList { get { return list_SkillNameList; } }
    /************************************************************************************/
    public void AcquireSkill(Skill _skill, int _lv = 1) {
        int idx = 0;

        // 기존 스킬
        for(int i = 0; i < list_SkillSlot.Count; i++) {
            if(list_SkillSlot[i] != null) {
                if(list_SkillSlot[i].skill != null) {
                    if(list_SkillSlot[i].skill.skillName == _skill.skillName) {
                        list_SkillSlot[i].SetSlotCount(_lv);
                        return;
                    }
                }
            }
            idx = i;
        }

        // 새로운 스킬
        GameObject skillSlot = Instantiate(go_SkillListSlotPrefab, m_gridLayoutContentSkillSlot.transform);

        SkillInfoButton infoButton = skillSlot.GetComponent<SkillInfoButton>();
        infoButton.ButtonIdx = idx;

        GUISlot newSkill = skillSlot.gameObject.GetComponent<GUISlot>();
        list_SkillSlot.Add(newSkill);
        list_SkillSlot[idx].AddSkill(_skill);

        GameObject skillName = Instantiate(go_SkillNmaePrefab, m_gridLayoutContentSkillName.transform);
        Text text = skillName.gameObject.GetComponent<Text>();
        text.text = string.Format(_skill.skillName);
        list_SkillNameList.Add(skillName);
    }

    public void Button_SkillType (int _idx) {
        go_SkillAttribute.SetActive(false);
        switch(_idx) {
            case ALL:
            case ACTIVE:
                go_SkillAttribute.SetActive(true);
                break;
        }
                ShowSkill(_idx);        
    }
    public void Button_SkillAttribute (int _idx) {
        ShowSkill(ACTIVE, _idx);
    }
    void ShowSkill(int _type, int _attribute = ALL) {

        for(int i = 0; i < list_SkillSlot.Count; i++) {
            Debug.Log(list_SkillSlot[i].skill + " : " + list_SkillSlot[i].skill.skillType);
            bool flag = false;
            switch(_type) {
                case ALL:
                    flag = true;
                    break;
                case ACTIVE:
                    if(list_SkillSlot[i].skill.skillType == Skill.SKILL_TYPE.ACTIVE)
                        flag = true;
                    Debug.Log(ACTIVE + " / " + flag);
                    break;
                case PASSIVE:
                    if(list_SkillSlot[i].skill.skillType == Skill.SKILL_TYPE.PASSIVE)
                        flag = true;
                    Debug.Log(PASSIVE + " / " + flag);
                    break;
                case BUFF:
                    if(list_SkillSlot[i].skill.skillType == Skill.SKILL_TYPE.BUFF)
                        flag = true;
                    Debug.Log(BUFF + " / " + flag);
                    break;
                case CURE:
                    if(list_SkillSlot[i].skill.skillType == Skill.SKILL_TYPE.CURE)
                        flag = true;
                    Debug.Log(CURE + " / " + flag);
                    break;
            }
            list_SkillSlot[i].gameObject.SetActive(flag);
            list_SkillNameList[i].gameObject.SetActive(flag);
        }

        switch(_type) {
            case PASSIVE:
            case BUFF:
            case CURE:
                return;
        }

        for(int i = 0; i < list_SkillSlot.Count; i++) {
            bool flag = false;
            switch(_attribute) {
                case ALL:
                    flag = true;
                    break;
                case FIRE:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.FIRE)
                        flag = true;
                    break;
                case AQUA:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.AQUA)
                        flag = true;
                    break;
                case ELEC:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.ELEC)
                        flag = true;
                    break;
                case SOIL:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.SOIL)
                        flag = true;
                    break;
                case WIND:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.WIND)
                        flag = true;
                    break;
                case LIGHT:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.LIGHT)
                        flag = true;
                    break;
                case DARK:
                    if(list_SkillSlot[i].skill.skillAttribute == Skill.SKILL_ATTRIBUTE.DARK)
                        flag = true;
                    break;
            }
            list_SkillSlot[i].gameObject.SetActive(flag);
            list_SkillNameList[i].gameObject.SetActive(flag);
        }
    }

    public void ShowSkillInfo(int _idx) {
        text_SkillName.text = string.Format(list_SkillSlot[_idx].skill.skillName);
        image_SkillImage.sprite = list_SkillSlot[_idx].skill.skillImage;
        text_SkillInfo.text = string.Format(list_SkillSlot[_idx].skill.SkillInfo);

        if(list_SkillSlot[_idx].skill.skillAttackType == Skill.SKILL_ATTACK_TYPE.PHYSICAL)
            text_SkillStatus.text = string.Format("물리 공격 (Physical)\n");
        else if(list_SkillSlot[_idx].skill.skillAttackType == Skill.SKILL_ATTACK_TYPE.MAGIC)
            text_SkillStatus.text = string.Format("마법 공격 (Magic)\n");
        else
            text_SkillStatus.text = string.Format("물리 + 마법 공격 (Mix)\n");


        if(list_SkillSlot[_idx].skill.useHp > 0)
            text_SkillStatus.text += string.Format("소모 HP      : {0}\n", list_SkillSlot[_idx].skill.useHp);
        else if(list_SkillSlot[_idx].skill.useMp > 0)
            text_SkillStatus.text += string.Format("소모 MP      : {0}\n", list_SkillSlot[_idx].skill.useMp);
        else
            text_SkillStatus.text += string.Format("소모  X\n");

        text_SkillStatus.text += string.Format(
            "배율 ( X )      : {0}\n" +
            "투사체 속도 : {1}\n" +
            "투사체 거리 : {2}\n" +
            "공격 딜레이 : {3}s",
            list_SkillSlot[_idx].skill.x,
            list_SkillSlot[_idx].skill.Speed,
            list_SkillSlot[_idx].skill.Dist,
            list_SkillSlot[_idx].skill.Delay);

    }
}
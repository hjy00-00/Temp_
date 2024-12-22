using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoButton : MonoBehaviour {
    int m_nIdx;

    public int ButtonIdx { get { return m_nIdx; } set { m_nIdx = value; } }
    /************************************************************************************/
    public void Button_ShowSkillInfo() {
        SkillList skillList = GameManager.GetInstance().SkillList;
        skillList.ShowSkillInfo(m_nIdx);
    }
}
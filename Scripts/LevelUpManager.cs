using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpManager : MonoBehaviour {
    [SerializeField] List<GameObject> m_listCard;
    [SerializeField] List<LevelUp> m_listLevelup;

    bool m_bShuffle;


    /****************************************/
    public bool Shuffle { get { return m_bShuffle; } set { m_bShuffle = value; } }
    /********************************************************************************/
    public LevelUp GetSelectCard(int _idx) {
        return m_listLevelup[_idx];
    }

    public void ShowCards() {
        this.gameObject.SetActive(true);
        for (int i = 0; i < m_listCard.Count; i++) {
            m_listCard[i].SetActive(true);
            if (m_bShuffle == true) {
                m_listLevelup[i].Shuffle = true;
                m_listLevelup[i].InitCard(i);
            }
        }
        m_bShuffle = false;
    }
    
    public void CloseCard() {
        for (int i = 0; i < m_listCard.Count; i++) {
            m_listCard[i].SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicInventory : MonoBehaviour {
    [SerializeField] GameObject go_SlotsParent;

    [SerializeField] GUISlot[] m_Slots;

    [SerializeField] Text text_Name;

    public GameObject DynamicInventoryParent { get { return go_SlotsParent; } set { go_SlotsParent = value; } }
    public GUISlot[] GetSlots { get { return m_Slots; } }
    /************************************************************************************/
    void Start() {
        m_Slots = go_SlotsParent.GetComponentsInChildren<GUISlot>();
    }
    /************************************************************************************/
    void Initialize() {
        text_Name.text = null;
        for(int i = 0; i < m_Slots.Length; i++) {
            m_Slots[i].gameObject.SetActive(true);
            if(m_Slots[i].item != null) {
                m_Slots[i].ClearSlot();
            }
        }
    }
    public void SetDynamicInventory(NPC _npc = null, string _type = null, int _size = 0) {
        Initialize();
        if(_npc == null) { Debug.LogWarning("TargetNPC : NULL"); return; }
        if(_size > 48) { Debug.LogWarning("동적 인벤토리 보다 큰 크기를 요구 (" + _size + "/48)"); return; }
        else if (_size < 0) { Debug.LogWarning("0 이하의 값을 입력 " + _size); return; }

        text_Name.text = _type + " (" + _npc.npcName + ")";

        for(int i = 0; i < m_Slots.Length; i++) {
            if(i >= _size)
                m_Slots[i].gameObject.SetActive(false);
        }
    }

    public void AddItem(Item _item, int _count = 1) {
        for (int i = 0; i < m_Slots.Length; i++)
        {
            if (m_Slots[i].item == null)
            {
                m_Slots[i].AddItem(_item, _count);
                break;
            }
        }
    }
}
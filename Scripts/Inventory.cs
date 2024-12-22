using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    [SerializeField] GameObject go_SlotsParent;

    GUISlot[] slots;
    DropItem m_cDropItem;

    public GUISlot[] GetSlots { get { return slots; } }
    /************************************************************************************/
    void Start() {
        slots = go_SlotsParent.GetComponentsInChildren<GUISlot>();
        m_cDropItem = GameManager.GetInstance().DropItem;
    }
    /************************************************************************************/
    public void AcquireItem(Item _item, int _count = 1) {
        if(Item.ITEM_TYPE.EQUIPMENT != _item.itemType) {
            int addNum = 0;
            for(int i = 0; i < slots.Length; i++) {
                if(slots[i].item != null) {
                    if(slots[i].item.itemName == _item.itemName) {
                        addNum = slots[i].item.itemMaxCount - slots[i].count; // (최대치와 비교하여) 남은 갯수 저장
                        if(addNum >= _count) {
                            slots[i].SetSlotCount(_count);
                            return;
                        }
                        else {
                            slots[i].SetSlotCount(addNum);
                            _count = _count - addNum;
                        }
                    }
                }
            }
        }
        for(int i = 0; i < slots.Length; i++) {
            if(slots[i].item == null) {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
        for(int i = 0; i < _count; i++)
            m_cDropItem.Drop(_item);
    }
}
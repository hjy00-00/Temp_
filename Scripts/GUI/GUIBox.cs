using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIBox : MonoBehaviour {
    [SerializeField] List<GUISlot> items;


    /********************************************************************************/

    public void AddItem(GUISlot _item, int _count) {
        int idx = 0;

        // 기존 스킬
        for(int i = 0; i < items.Count; i++) {
            if(items[i] != null) {
                if(items[i].item != null) {
                    if(items[i].item.itemName == _item.item.itemName) {
                        items[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
            idx = i;
        }

        // 새로운 스킬

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot : MonoBehaviour {
    [SerializeField] GameObject go_EquipmentSlotsParent;
    [SerializeField] GameObject go_AccessoriesSlotsParent;

    GUISlot[] slotsEquipment;
    GUISlot[] slotsAccessories;
    Status statusEquipment = new Status();

    [SerializeField] GameObject[] go_BG_Equipment;
    [SerializeField] GameObject[] go_BG_Accessories;
    public Status StatusEquipment { get { return statusEquipment; } }
    /************************************************************************************/
    private void Start() {
        slotsEquipment = go_EquipmentSlotsParent.GetComponentsInChildren<GUISlot>();
        slotsAccessories = go_AccessoriesSlotsParent.GetComponentsInChildren<GUISlot>();
    }
    /************************************************************************************/
    public void SlotsCheck() {
        statusEquipment = new Status();
        for(int i = 0; i < slotsEquipment.Length; i++) {
            if(slotsEquipment[i].item != null) {
                statusEquipment += slotsEquipment[i].item.itemStatus;
                go_BG_Equipment[i].SetActive(false);
            }
            else
                go_BG_Equipment[i].SetActive(true);
        }
        for(int i = 0; i < slotsAccessories.Length; i++) {
            if(slotsAccessories[i].item != null) {
                statusEquipment += slotsAccessories[i].item.itemStatus;
                go_BG_Accessories[i].SetActive(false);
            }
            else
                go_BG_Accessories[i].SetActive(true);
        }
    }

    public Item Put_On(GUISlot _slot) {
        Item tempItem = null;
        switch(_slot.item.itemPart) {
            case Item.ITEM_PART.HELMET:
            case Item.ITEM_PART.ARMOR:
            case Item.ITEM_PART.GLOVE:
            case Item.ITEM_PART.BOOTS:
            case Item.ITEM_PART.WEAPON:
                tempItem = slotsEquipment[(int)_slot.item.itemPart].item;

                slotsEquipment[(int)_slot.item.itemPart].ClearSlot();
                slotsEquipment[(int)_slot.item.itemPart].AddItem(_slot.item);
                break;

            case Item.ITEM_PART.EARRING:
            case Item.ITEM_PART.NECKLACE:
                tempItem = slotsAccessories[(int)_slot.item.itemPart].item;
                break;
            case Item.ITEM_PART.RING:
                if(slotsEquipment[2].item == null)
                    tempItem = slotsEquipment[2].item;
                else
                    tempItem = slotsEquipment[3].item;


                break;
            case Item.ITEM_PART.BELT:
                tempItem = slotsEquipment[4].item;
                break;
        }
        SlotsCheck();
        return tempItem;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropInventory : MonoBehaviour {
    [SerializeField] string strName;
    [SerializeField] E_INVEN_TYPE eInvenType;
    [SerializeField] Item[] items;
    [SerializeField] int[] count;

    public enum E_INVEN_TYPE { BOX_A, BOX_B, BOX_C, PLAYER, MONSTER, BOX_KEEP_S, BOX_KEEP_M, BOX_KEEP_L }
    ItemManager cItemManager;
    Spawn cSpawn;
    DropItem cDropItem;


    public string InvenName { get { return strName; } }
    public E_INVEN_TYPE Type { get { return eInvenType; } }
    public Item[] DropItems { get { return items; } set { items = value; } }
    public int[] DropItemCount { get { return count; } set { count = value; } }
    /*********************************************************************************/
    private void Start() {
        cSpawn = GameManager.GetInstance().Spawn;
        cDropItem = GameManager.GetInstance().DropItem;
    }
    private void Update() {
        if(KeepBoxCheck())
            CheckEmpty();

    }
    /*********************************************************************************/
    // 상자 -> 상자 종류 별 드랍 아이템 정리 후 랜덤 드랍 (갯수 랜덤)
    // 플레이어 -> 기본 인벤토리 복사 소환
    // 몬스터 -> 몬스터 별 드랍 아이템 정리 후 랜덤 드랍 (갯수 랜덤)
    public void SetInven(E_INVEN_TYPE _type) {
        cItemManager = GameManager.GetInstance().ItemManager;

        eInvenType = _type;
        SetBox();

        switch(_type) {
            case E_INVEN_TYPE.BOX_A:
            case E_INVEN_TYPE.BOX_B:
            case E_INVEN_TYPE.BOX_C:
                cItemManager.RandomItem(this);
                break;
            case E_INVEN_TYPE.PLAYER:
            case E_INVEN_TYPE.MONSTER:
                SetPlayer();
                break;
            default:
                break;
        }
    }

    void SetBox() {
        switch(eInvenType) {
            case E_INVEN_TYPE.BOX_A:
                strName = "Box A";
                Debug.Log("Box A");
                items = new Item[6];
                count = new int[6];
                break;
            case E_INVEN_TYPE.BOX_B:
                strName = "Box B";
                items = new Item[12];
                count = new int[12];
                break;
            case E_INVEN_TYPE.BOX_C:
                strName = "Box C";
                items = new Item[18];
                count = new int[18];
                break;
            case E_INVEN_TYPE.PLAYER:
                items = new Item[30];
                count = new int[30];
                break;
            case E_INVEN_TYPE.MONSTER:
                items = new Item[6];
                count = new int[6];
                break;
            case E_INVEN_TYPE.BOX_KEEP_S:
                strName = "Box_KEEP_S";
                items = new Item[12];
                count = new int[12];
                break;
            case E_INVEN_TYPE.BOX_KEEP_M:
                strName = "Box_KEEP_M";
                items = new Item[18];
                count = new int[18];
                break;
            case E_INVEN_TYPE.BOX_KEEP_L:
                strName = "Box_KEEP_L";
                items = new Item[30];
                count = new int[30];
                break;
            default:
                items = new Item[6];
                count = new int[6];
                break;
        }
    }
    bool KeepBoxCheck() {
        switch(eInvenType) {
            case E_INVEN_TYPE.BOX_KEEP_S:
            case E_INVEN_TYPE.BOX_KEEP_M:
            case E_INVEN_TYPE.BOX_KEEP_L:
                SetBox();
                return false;
            default:
                return true;
        }

    }
    public void KeepItems(Item _item, int _count = 1) {
        if(Item.ITEM_TYPE.EQUIPMENT != _item.itemType) {
            int remaining = 0;
            for(int i = 0; i < items.Length; i++) {
                if(items[i] != null) {
                    if(items[i].itemName == _item.itemName) {
                        remaining = items[i].itemMaxCount - _count;
                        if(remaining >= _count) {
                            count[i] += _count;
                            return;
                        }
                        else {
                            count[i] += remaining;
                            _count = _count - remaining;
                        }
                    }
                }
            }
        }
        for(int i = 0; i < items.Length; i++) {
            if(items[i] == null) {
                items[i] = _item;
                count[i] = _count;
                return;
            }
        }
        for(int i = 0; i < _count; i++)
            cDropItem.Drop(_item);
    }

    void SetPlayer() {
        Player player;
        if(eInvenType == E_INVEN_TYPE.PLAYER) {
            // 플레이어 적용
            player = GameManager.GetInstance().Player;

            for(int i = 0; i < player.GetInventory.GetSlots.Length; i++) {
                if(player.GetInventory.GetSlots[i].item != null) {
                    items[i] = player.GetInventory.GetSlots[i].item;
                    count[i] = player.GetInventory.GetSlots[i].count;
                    player.GetInventory.GetSlots[i].ClearSlot();
                }
            }
        }
        else {
            // 몬스터 적용
            player = null;
        }
        strName = player.Name;

    }

    void CheckEmpty() {
        bool empty = true;     // true = 비어 있는 상태, false = 무언가 있는 상태
        for(int i = 0; i < items.Length; i++) {
            if(items[i] != null) {
                empty = false;
                break;
            }
        }

        if(empty == true) {
            if(this.transform.gameObject.name == "DropBox") {
                this.gameObject.SetActive(false);
                cSpawn.ReSpawnBox();
            }
            else {
                this.gameObject.SetActive(false);
            }
        }
    }
}
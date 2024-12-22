using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIItemInfo : MonoBehaviour {
    [SerializeField] GameObject go_ItemInfoBase;

    [SerializeField] GUISlot m_InfoSlot;
    GUISlot m_tempSlot;
    [SerializeField] Text text_ItemName;
    [SerializeField] Image image_ItemImage;
    [SerializeField] Text text_ItemInfo;
    [SerializeField] Text text_ItemStatus;
    [SerializeField] Text text_ItemPrice;

    [SerializeField] InputField inputText_Count;
    [SerializeField] GameObject go_InputField;

    [SerializeField] GameObject go_EquipmentOn;
    [SerializeField] GameObject go_EquipmentOff;
    [SerializeField] GameObject go_Use;
    [SerializeField] GameObject go_Buy;
    [SerializeField] GameObject go_Sell;
    [SerializeField] GameObject go_Divide;
    [SerializeField] GameObject go_Keep;
    [SerializeField] GameObject go_TakeOut;
    [SerializeField] GameObject go_Drop;

    int price = 0;
    GUIManager guiManager;
    Inventory inventory;
    Player player;
    private void Start() {
        inputText_Count.onEndEdit.AddListener(DivideInputNum);

        guiManager = GameManager.GetInstance().GUIManager;
        inventory = GameManager.GetInstance().Inventory;
        player = GameManager.GetInstance().Player;
    }
    /********************************************************************************/
    void SetColor(float _alpha) {
        Color color = image_ItemImage.color;
        color.a = _alpha;
        image_ItemImage.color = color;
    }
    public void ClearInfo() {
        if(m_InfoSlot.item != null) {
            inventory.AcquireItem(m_InfoSlot.item, m_InfoSlot.count);
        }
        m_tempSlot = null;

        text_ItemName.text = "";
        image_ItemImage.sprite = null;
        text_ItemInfo.text = "";
        text_ItemStatus.text = "";
        text_ItemPrice.text = "";
        price = 0;

        SetColor(0);
        ButtonReSet();

        go_InputField.SetActive(false);
        m_InfoSlot.ClearSlot();
        go_ItemInfoBase.SetActive(false);
    }
    void ButtonReSet() {
        go_EquipmentOn.SetActive(false);
        go_EquipmentOff.SetActive(false);
        go_Use.SetActive(false);
        go_Buy.SetActive(false);
        go_Sell.SetActive(false);
        go_Divide.SetActive(false);
        go_Keep.SetActive(false);
        go_TakeOut.SetActive(false);
        go_Drop.SetActive(false);
        go_InputField.SetActive(false);
    }

    public void ShowItemInfo(GUISlot _slot, string _state = "") {
        go_ItemInfoBase.SetActive(true);
        go_Divide.SetActive(false);

        if(m_InfoSlot.item != null) {
            inventory.AcquireItem(m_InfoSlot.item, m_InfoSlot.count);
            m_InfoSlot.ClearSlot();
        }

        text_ItemName.text = _slot.item.itemName;
        image_ItemImage.sprite = _slot.item.itemImage;
        text_ItemInfo.text = _slot.item.itemInfo;
        text_ItemStatus.text = _slot.item.ToStatus();

        price = 0;

        m_tempSlot = _slot;
        text_ItemPrice.text = "판매가 : ";
        price = _slot.item.itemPrice / 2;
        text_ItemPrice.text += price.ToString();

        ButtonReSet();
        switch(_state) {
            case "Equipment":
                // (해제) 버튼
                go_EquipmentOff.SetActive(true);
                break;
            case "DropInventory":
                // (꺼내기) 버튼
                go_TakeOut.SetActive(true);
                break;
            case "Shop":
                m_tempSlot = _slot;
                text_ItemPrice.text = "구매가 : ";
                price = _slot.item.itemPrice;
                text_ItemPrice.text += price.ToString();

                // (구매) 버튼
                go_Buy.SetActive(true);
                break;
            default:
                m_InfoSlot.AddItem(_slot.item, _slot.count);

                SlotPointCheck(_slot);
                break;
        }
        SetColor(1);
    }
    void SlotPointCheck(GUISlot _slot) {
        price = 0;
        switch(_slot.name) {
            // 인벤토리, 상자, 퀵슬롯
            case "Slot":
            case "Dynamic Slot":
            case "Quick Slot":
                text_ItemPrice.text = "판매가 : ";
                price = _slot.item.itemPrice / 2;

                // (나누기) 버튼
                go_Divide.SetActive(true);
                // (버리기) 버튼
                go_Drop.SetActive(true);

                if(_slot.item.itemType == Item.ITEM_TYPE.EQUIPMENT) {
                    go_Divide.SetActive(false);
                    if(_slot.name == "Slot" || _slot.name == "Quick Slot")
                        // (장착) 버튼 온
                        go_EquipmentOn.SetActive(true);
                }
                else if(_slot.item.itemType == Item.ITEM_TYPE.USED) {
                    // (사용) 버튼
                    go_Use.SetActive(true);
                }

                if(guiManager.TargetNPC != null) {
                    if(guiManager.TargetNPC.npcType == NPC.NPC_TYPE.SHOP) {
                        // (판매) 버튼
                        go_Sell.SetActive(true);


                        go_Use.SetActive(false);
                        go_EquipmentOn.SetActive(false);
                        go_EquipmentOff.SetActive(false);
                    }
                }
                // (보관) 버튼
                if(guiManager.PopUp == GUIManager.E_GUI_STATE.DYNAMIC_INVENTORY) {
                    if(_slot.name == "Dynamic Slot")
                        go_TakeOut.SetActive(true);
                    else
                        go_Keep.SetActive(true);
                }

                break;
            default:
                Debug.LogError("Slot : " + _slot.name);
                return;
        }
        text_ItemPrice.text += price.ToString();
        _slot.ClearSlot();
    }

    public void Button_PutOn() {
        Item item = GameManager.GetInstance().EquipmentSlot.Put_On(m_InfoSlot);
        if(item != null)
            GameManager.GetInstance().Inventory.AcquireItem(item);
        GameManager.GetInstance().GUIManager.StatusUpdate();
        m_InfoSlot.ClearSlot();
        ClearInfo();
    }
    public void Button_TakeOff() {
        GameManager.GetInstance().Inventory.AcquireItem(m_tempSlot.item);
        m_tempSlot.ClearSlot();
        GameManager.GetInstance().GUIManager.StatusUpdate();
        ClearInfo();
    }
    public void Button_Use() {
        switch(m_InfoSlot.item.itemPart) {
            case Item.ITEM_PART.POTION:
                GameManager.GetInstance().Player.UsePotin(m_InfoSlot.item.itemStatus.nHP, m_InfoSlot.item.itemStatus.nMP);
                break;
            case Item.ITEM_PART.FOOD:
                break;
            case Item.ITEM_PART.EXPENDABLES:
                break;
        }
        m_InfoSlot.SetSlotCount(-1);
        SlotCountCheck();
    }
    public void Button_Buy() {
        if(player.Money >= price) {
            player.Money -= price;
            inventory.AcquireItem(m_tempSlot.item, m_tempSlot.count);
            guiManager.MoneyUpdate();
        }
        else {
            int temp = price - player.Money;
            Debug.LogError("팝업 출력 (금액 부족 : " + temp + ")");
        }
    }
    public void Button_Sell() {
        player.Money += m_InfoSlot.count * price;
        m_InfoSlot.SetSlotCount(-m_InfoSlot.count);
        guiManager.MoneyUpdate();
        SlotCountCheck();
    }
    public void Button_Divide() {
        go_InputField.SetActive(true);
    }
    public void DivideInputNum(string _text) {
        int count = m_InfoSlot.count;
        count -= int.Parse(inputText_Count.text);

        if(count > 0) {
            inventory.AcquireItem(m_InfoSlot.item, count);
            m_InfoSlot.SetSlotCount(-count);
            SlotCountCheck();
        }
    }
    public void Button_Drop() {
        for(int i = 0; i < m_InfoSlot.count; i++) {
            GameManager.GetInstance().DropItem.Drop(m_InfoSlot.item);
            m_InfoSlot.SetSlotCount(-1);
        }
        SlotCountCheck();
    }
    public void Button_Keep() {
        // (클릭한 아이템이) 인벤토리 -> 
        if(guiManager.DropInventory != null) {
            guiManager.DropInventory.KeepItems(m_InfoSlot.item, m_InfoSlot.count);
        }
        m_InfoSlot.ClearSlot();
        SlotCountCheck();
    }
    public void Button_TakeOut() {
        // (클릭한 아이템이) 인벤토리 <- 
        player.GetInventory.AcquireItem(m_InfoSlot.item, m_InfoSlot.count);
        m_InfoSlot.ClearSlot();
        SlotCountCheck();
    }

    public void SlotCountCheck() {
        if(m_InfoSlot.count <= 0)
            ClearInfo();
    }
}
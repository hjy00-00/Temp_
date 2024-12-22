using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GUISlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler {
    public Item item;       //아이템
    public Skill skill;     //스킬
    public int count;       //갯수
    public Image image;     //아이콘

    [SerializeField] Text text_Count;
    [SerializeField] GameObject go_CountImage;

    GUIManager guiManager;
    EquipmentSlot m_cEquipmentSlot;
    Inventory m_cInventory;
    DropItem m_cDropItem;
    GUIItemInfo m_cGUIItemInfo;

    public Text CountTxt { get { return text_Count; } set { text_Count = value; } }
    /*********************************************************************************/
    private void Start() {
        guiManager = GameManager.GetInstance().GUIManager;
        m_cEquipmentSlot = GameManager.GetInstance().EquipmentSlot;
        m_cInventory = GameManager.GetInstance().Inventory;
        m_cDropItem = GameManager.GetInstance().DropItem;
        m_cGUIItemInfo = GameManager.GetInstance().GUIManager.GUIItemInfo;
    }
    /*********************************************************************************/
    public void SetColor(float _alpha) {
        Color color = image.color;
        color.a = _alpha;
        image.color = color;
    }

    public void AddItem(Item _item, int _count = 1) {
        item = _item;
        count = _count;

        if(image != null)
            image.sprite = item.itemImage;

        if(item.itemType != Item.ITEM_TYPE.EQUIPMENT) {
            go_CountImage.SetActive(true);
            text_Count.text = count.ToString();
        }
        else {
            text_Count.text = "0";
            go_CountImage.SetActive(false);
        }
        SetColor(1);
    }
    public void AddSkill(Skill _skill, int _lv = 1) {
        skill = _skill;
        count = _lv;
        image.sprite = skill.skillImage;

        go_CountImage.SetActive(true);
        text_Count.text = count.ToString();

        SetColor(1);
    }

    public void SetSlotCount(int _count) {
        count += _count;

        text_Count.text = count.ToString();
        if(skill != null) {
            if(skill.skillMaxLv <= count) {
                text_Count.text = "M";
                text_Count.text = count.ToString();
            }
        }

        if(count <= 0) {
            ClearSlot();
        }
    }

    public void ClearSlot() {
        item = null;
        skill = null;
        count = 0;
        image.sprite = null;
        SetColor(0);

        text_Count.text = "0";
        go_CountImage.SetActive(false);
    }

    /*********************************************************************************/

    public void OnPointerClick(PointerEventData eventData) {
        if(eventData.button == PointerEventData.InputButton.Right) {
            if(item != null) {
                // 팝업별 우클릭 상호작용
                switch(GameManager.GetInstance().GUIManager.PopUp) {
                    case GUIManager.E_GUI_STATE.INVENTORY:
                        // 장비 착용
                        if(item.itemType == Item.ITEM_TYPE.EQUIPMENT) {
                            // 미장착 아이템 우클릭 (장착)
                            if(SlotPartCheck(this) == Item.ITEM_PART.NONE) {
                                Item temp = m_cEquipmentSlot.Put_On(this);

                                this.ClearSlot();
                                if(temp != null)
                                    this.AddItem(temp);
                                GameManager.GetInstance().GUIManager.StatusUpdate();
                            }
                            // 장착 아이템 우클릭 (장착 해제)
                            else {
                                m_cInventory.AcquireItem(this.item);
                                this.ClearSlot();
                                m_cEquipmentSlot.SlotsCheck();
                                GameManager.GetInstance().GUIManager.StatusUpdate();
                            }

                        }
                        // 소모품 사용
                        else if(item.itemType == Item.ITEM_TYPE.USED) {
                            switch(item.itemPart) {
                                case Item.ITEM_PART.POTION:
                                    GameManager.GetInstance().Player.UsePotin(item.itemStatus.nHP, item.itemStatus.nMP);
                                    break;
                                case Item.ITEM_PART.FOOD:
                                    break;
                                case Item.ITEM_PART.EXPENDABLES:
                                    break;
                            }
                            SetSlotCount(-1);

                            if(item == null)
                                GameManager.GetInstance().GUIManager.GUIItemInfo.ClearInfo();
                        }
                        break;
                    case GUIManager.E_GUI_STATE.DYNAMIC_INVENTORY:
                        m_cInventory.AcquireItem(item, count);
                        this.ClearSlot();
                        break;
                    case GUIManager.E_GUI_STATE.LEVELUP:
                        break;
                    case GUIManager.E_GUI_STATE.SHOP:
                        Debug.Log("Shop !!");

                        break;
                    case GUIManager.E_GUI_STATE.DIALOGUE:
                        break;
                    case GUIManager.E_GUI_STATE.QUEST_LIST:
                        break;
                    case GUIManager.E_GUI_STATE.SKILL_LIST:
                        break;
                    default:
                        break;
                }
            }
            GameManager.GetInstance().GUIManager.GUIItemInfo.ClearInfo();
        }
        else if(eventData.button == PointerEventData.InputButton.Left) {
            if(item != null) {
                if(item.itemName != "Status") {
                    switch(this.gameObject.name) {
                        case "Slot_HELMET":
                        case "Slot_ARMOR":
                        case "Slot_GLOVE":
                        case "Slot_BOOTS":
                        case "Slot_WEAPON":

                        case "Slot_NECKLACE":
                        case "Slot_EARRING":
                        case "Slot_RING_1":
                        case "Slot_RING_2":
                        case "Slot_BELT":
                            guiManager.GUIItemInfo.ShowItemInfo(this, "Equipment");
                            break;
                        case "Dynamic Slot":
                            if(guiManager.PopUp == GUIManager.E_GUI_STATE.SHOP) {
                                guiManager.GUIItemInfo.ShowItemInfo(this, "Shop");
                            }
                            else if(guiManager.PopUp == GUIManager.E_GUI_STATE.DYNAMIC_INVENTORY) {
                                guiManager.GUIItemInfo.ShowItemInfo(this, guiManager.DropInventory.InvenName);
                            }
                            break;
                        default:
                            guiManager.GUIItemInfo.ShowItemInfo(this);

                            this.ClearSlot();
                            break;
                    }

                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if(item != null) {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSet();

            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData) {
        if(item != null) {
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData) {
        DragSlot.instance.SetColor(0);
    }

    public void OnDrop(PointerEventData eventData) {
        if(DragSlot.instance.dragSlot != null) {
            // 현재 팝업이 '상점'일 경우 드래그 종료
            if(guiManager.PopUp == GUIManager.E_GUI_STATE.SHOP) { return; }

            // 드래그 종료 시 마우스 위치의 오브젝트 이름 (GUISlot 이 붙은 오브젝트만 반응)
            switch(this.gameObject.name) {
                case "Slot":

                    break;
                case "Slot_HELMET":
                case "Slot_ARMOR":
                case "Slot_GLOVE":
                case "Slot_BOOTS":
                case "Slot_WEAPON":

                case "Slot_NECKLACE":
                case "Slot_EARRING":
                case "Slot_RING_1":
                case "Slot_RING_2":
                case "Slot_BELT":
                    // 장비슬롯에 드래그 종료 시 아이템이 장비가 아닌 경우 변경 X
                    if(!(DragSlot.instance.dragSlot.item.itemType == Item.ITEM_TYPE.EQUIPMENT)) { return; }
                    // 장비슬롯에 드래그 종료 시 장비칸 확인 후 맞지 않은 장비칸인 경우 변경 X
                    if(!(DragSlot.instance.dragSlot.item.itemPart == SlotPartCheck(this))) { return; }
                    break;
                case "BG":
                    // 밖의 공간으로 드래그 종료 (아이템 버리기)
                    PlayerController playerController = GameManager.GetInstance().PlayerController;

                    for(int i = 0; i < DragSlot.instance.dragSlot.count; i++) {
                        m_cDropItem.Drop(DragSlot.instance.dragSlot.item);
                    }

                    DragSlot.instance.dragSlot.ClearSlot();
                    return;
                default:
                    if("Status Slot" == this.gameObject.name) break;
                    Debug.LogWarning("this GameObject name : " + this.gameObject.name);
                    break;
            }

            // 기타 아이템 (갯수가 있는 아이템)
            if(item != null) {
                if(item.name == DragSlot.instance.dragSlot.item.name && item.itemName != "Status") {
                    m_cInventory.AcquireItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.count);
                    DragSlot.instance.dragSlot.ClearSlot();
                    return;
                }
            }
            ChangeSlot();
        }
    }

    void ChangeSlot() {
        if(this == DragSlot.instance.dragSlot) { Debug.Log("return"); return; }

        Item _item = item;
        int _count = count;
        bool _statusUpdate = false;

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.count);

        if(DragSlot.instance.dragSlot.item.itemType == Item.ITEM_TYPE.EQUIPMENT)
            _statusUpdate = true;

        if(_item != null)
            DragSlot.instance.dragSlot.AddItem(_item, _count);
        else
            DragSlot.instance.dragSlot.ClearSlot();

        if(_statusUpdate) {
            GameManager.GetInstance().GUIManager.StatusUpdate();
        }
        if(item != null) {
            if(item.itemName != "Status")
                m_cGUIItemInfo.SlotCountCheck();
            else if(item.itemName == "Status") {
                GameManager.GetInstance().CreateCharacter.SetPlayerStatus();
            }
        }
    }

    Item.ITEM_TYPE SlotTypeCheck(GUISlot _slot) {
        switch(_slot.item.itemType) {
            case Item.ITEM_TYPE.EQUIPMENT:
                return Item.ITEM_TYPE.EQUIPMENT;
            case Item.ITEM_TYPE.USED:
                return Item.ITEM_TYPE.USED;
            case Item.ITEM_TYPE.INGREDIENT:
                return Item.ITEM_TYPE.INGREDIENT;
            case Item.ITEM_TYPE.ETC:
                return Item.ITEM_TYPE.ETC;
            default:
                Debug.LogError(_slot.item + " : " + _slot.item.itemType);
                return Item.ITEM_TYPE.ETC;
        }
    }
    Item.ITEM_PART SlotPartCheck(GUISlot _slot) {
        switch(_slot.name) {
            case "Slot_HELMET":
                return Item.ITEM_PART.HELMET;
            case "Slot_ARMOR":
                return Item.ITEM_PART.ARMOR;
            case "Slot_GLOVE":
                return Item.ITEM_PART.GLOVE;
            case "Slot_BOOTS":
                return Item.ITEM_PART.BOOTS;
            case "Slot_WEAPON":
                return Item.ITEM_PART.WEAPON;

            case "Slot_EARRING":
                return Item.ITEM_PART.EARRING;
            case "Slot_NECKLACE":
                return Item.ITEM_PART.NECKLACE;
            case "Slot_RING_1":
                return Item.ITEM_PART.RING;
            case "Slot_RING_2":
                return Item.ITEM_PART.RING;
            case "Slot_BELT":
                return Item.ITEM_PART.BELT;
            default:
                Debug.Log("NONE");
                return Item.ITEM_PART.NONE;
        }
    }
}
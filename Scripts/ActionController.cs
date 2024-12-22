using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour {
    [SerializeField] float range;                    //습득 가능한 최대 거리

    // 접촉한 태그를 실행 (true)
    bool pickupActivated = false;
    bool talkActivated = false;
    bool inventoryActivated = false;

    RaycastHit hitInfo;                 //충돌체 정보 저장

    [SerializeField] public LayerMask layerMask;

    [SerializeField] Text actionText;

    GUIManager guiManager;
    Inventory playerInventory;
    DynamicInventory dynamicInventory;

    /*********************************************************************************/
    private void Start() {
        guiManager = GameManager.GetInstance().GUIManager;
        playerInventory = GameManager.GetInstance().Player.GetInventory;
        dynamicInventory = GameManager.GetInstance().DynamicInventory;
    }
    private void Update() {
        CheckTag();
        TryAction();
    }
    /*********************************************************************************/
    void TryAction() {
        if(Input.GetKeyDown(KeyCode.E)) {
            CheckTag();

            CanPickUp();
            CanTalk();
            CanOpenInven();
        }
    }
    /*********************************************************************************/
    void CheckTag() {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
            InfoAppear(hitInfo.transform.tag);
        else
            InfoDisapper();
    }
    /*********************************************************************************/
    // 충돌체 정보 출력 (아이템 레이어 일 경우) [아이템] [NPC]
    void InfoAppear(string _tag) {
        actionText.gameObject.SetActive(true);
        switch(_tag) {
            case "Item":
                pickupActivated = true;
                actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().m_Item.itemName + "획득 " + "<color=yellow>" + "(E)" + "</color>";
                break;
            case "NPC":
                talkActivated = true;
                actionText.text = hitInfo.transform.GetComponent<NPCPickUp>().m_NPC.npcName + "대화 " + "<color=yellow>" + "(E)" + "</color>";
                break;
            case "DropItems":
                inventoryActivated = true;
                actionText.text = hitInfo.transform.GetComponent<DropInventory>().InvenName + "열기 " + "<color=yellow>" + "(E)" + "</color>";
                break;
            default:
                Debug.LogError("아이템 레이어이지만 분류한 태그가 없음");
                break;
        }
    }
    // 충돌체 정보 삭제
    void InfoDisapper() {
        pickupActivated = false;
        talkActivated = false;
        inventoryActivated = false;
        actionText.text = "";
        actionText.gameObject.SetActive(false);
    }

    // 행동
    void CanPickUp() {
        if(pickupActivated) {
            if(hitInfo.transform != null && hitInfo.transform.tag == "Item") {
                playerInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().m_Item);
                hitInfo.transform.gameObject.SetActive(false);
                InfoDisapper();
            }
        }
    }
    void CanTalk() {
        if(talkActivated && GameManager.GetInstance().GUIManager.PopUp != GUIManager.E_GUI_STATE.DIALOGUE) {
            if(hitInfo.transform != null) {
                NPC npc = hitInfo.transform.GetComponent<NPCPickUp>().m_NPC;

                switch(npc.QuestState) {
                    case NPC.QUEST_STATE.CHOICE:
                    case NPC.QUEST_STATE.ACCEPT:
                    case NPC.QUEST_STATE.REFUSE:
                    case NPC.QUEST_STATE.SELECT_1:
                    case NPC.QUEST_STATE.SELECT_2:
                    case NPC.QUEST_STATE.SELECT_3:
                        npc.QuestState = NPC.QUEST_STATE.BASICS;
                        break;
                }
                guiManager.TargetNPC = npc;
                guiManager.SetDialogue();
            }
        }
    }
    void CanOpenInven() {
        if(inventoryActivated && hitInfo.transform != null) {
            DropInventory items = hitInfo.transform.GetComponent<DropInventory>();
            NPC npc = hitInfo.transform.GetComponent<NPCPickUp>().m_NPC;
            guiManager.DropInventory = items;
            guiManager.TargetNPC = npc;
            guiManager.TargetInventory = hitInfo.transform.gameObject;
            guiManager.SetGUIPopup(GUIManager.E_GUI_STATE.DYNAMIC_INVENTORY);
            dynamicInventory.SetDynamicInventory(npc, null, items.DropItems.Length);

            for(int i = 0; i < items.DropItems.Length; i++) {
                if(items.DropItems[i] != null) {
                    dynamicInventory.AddItem(items.DropItems[i], items.DropItemCount[i]);
                }
            }
        }
    }
    /*********************************************************************************/

}
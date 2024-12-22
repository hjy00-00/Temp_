using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour {
    static public DragSlot instance;

    public GUISlot dragSlot;

    [SerializeField] Image image_Item;
    [SerializeField] GameObject go_Count;
    [SerializeField] Text text_Count;
    [SerializeField] Text text_StatusCount;


    private void Start() {
        instance = this;
        SetColor(0);
    }

    public void DragSet() {
        image_Item.sprite = dragSlot.item.itemImage;
        SetColor(1);
    }

    public void SetColor(float _alpha) {
        Color color = image_Item.color;
        color.a = _alpha;
        image_Item.color = color;

        if(_alpha == 1) {
            if(dragSlot.item.itemType != Item.ITEM_TYPE.EQUIPMENT) {
                if(dragSlot.item.itemName == "Status") {
                    text_StatusCount.gameObject.SetActive(true);
                    text_StatusCount.text = dragSlot.CountTxt.text;
                }
                else {
                    text_StatusCount.gameObject.SetActive(true);
                    text_Count.text = dragSlot.CountTxt.text;
                }
            }
            else {
                go_Count.gameObject.SetActive(false);
            }

        }
        else {
            text_StatusCount.gameObject.SetActive(false);
            go_Count.gameObject.SetActive(false);
            text_Count.text = null;
            text_StatusCount.text = null;

        }
    }
}
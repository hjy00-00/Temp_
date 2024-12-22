using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour {
    public void Drop(Item _item) {
        string objPos = string.Format("{0}/", _item.itemType);

        if(_item.itemType == Item.ITEM_TYPE.EQUIPMENT || _item.itemType == Item.ITEM_TYPE.USED)
            objPos += string.Format("{0}/", _item.itemPart);

        objPos += _item.itemName + "_obj";

        GameObject prefab = Resources.Load("Item_Prefabs/" + objPos) as GameObject;
        GameObject dropItem = Instantiate(prefab) as GameObject;
        dropItem.name = _item.itemName;
        dropItem.transform.position = GameManager.GetInstance().PlayerController.DropPos.position;
    }
}
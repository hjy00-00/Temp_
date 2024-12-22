using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "New/item")]
public class Item : ScriptableObject {
	public string itemName;
	public ITEM_TYPE itemType;
	public ITEM_PART itemPart;
	public Sprite itemImage;
	public GameObject itemPrefab;
	public int itemMaxCount = 1;
	public int itemPrice;
	public Status itemStatus;
	[TextArea] public string itemInfo;


	public enum ITEM_TYPE { EQUIPMENT, USED, INGREDIENT, ETC }
	public enum ITEM_PART { NONE = -1, HELMET, ARMOR, GLOVE, BOOTS, WEAPON, EARRING, NECKLACE, RING, BELT, POTION, FOOD, EXPENDABLES };

	/********************************************************************************/

	public string ToStatus() {
		string info = "";
		switch(itemType) {
            case ITEM_TYPE.EQUIPMENT:
                info = string.Format(
				"HP:{0}\n" +
				"MP:{1}\n" +
				"Str[±Ù·Â]:{2}\n" +
				"Con[°Ç°­]:{3}\n" +
				"Int[¸¶·Â]:{4}\n" +
				"Agi[¹ÎÃ¸]:{5}\n" +
				"Cha[¸Å·Â]:{6}\n" +
				"Wis[ÁöÇý]:{7}",
				itemStatus.nHP,
				itemStatus.nMP,
				itemStatus.nStr,
				itemStatus.nCon,
				itemStatus.nInt,
				itemStatus.nAgi,
				itemStatus.nLuk,
				itemStatus.nWis);
				break;
            case ITEM_TYPE.USED:
                if(itemStatus.nHP > 0) {
					info += "HP '" + itemStatus.nHP + "' È¸º¹" + "\n";
                }
                if(itemStatus.nMP > 0) {
					info += "MP '" + itemStatus.nMP + "' È¸º¹" + "\n";
				}
                break;
            case ITEM_TYPE.INGREDIENT:
                break;
            case ITEM_TYPE.ETC:
                break;
            default:
                break;
        }
		return info;
	}
}
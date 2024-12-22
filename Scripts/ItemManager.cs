using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Status {
	// 1차 능력치
	public int nHP;
	public int nMaxHP;
	public int nMP;
	public int nMaxMP;
	public int nEXP;
	public int nMaxEXP;

	public int nATK;			// ((STR + 장비 STR) * 3) + 장비 ATK
	public int nM_ATK;			// ((INT + 장비 INT) * 5) + 장비 M_ATK
	public int nDEF;			// ((STR + 장비 STR) * 1) + 장비 DEF
	public int nM_DEF;          // ((WIS + 장비 WIS) * 3) + 장비 M_DEF
	public int nCritical;		//

	// 2차 능력치
	public int nStr;			// 근력	(물리 공격력, 소량의 물리 방어력)
	public int nCon;			// 건강	(최대 HP, [HP 회복 속도])
	public int nInt;			// 마력	(마법 공격력, [약점 간파], [퀘스트 난이도 하락 힌트 같은 것으로...])
	public int nAgi;			// 민첩	([이동 속도], [공격 속도])
	public int nLuk;			// 운	([카드 선택 시 보상 증가])
	public int nWis;			// 지혜	(마법 방어력, 최대 MP, [MP 회복력])

	// 부가 능력치
	public int nFame;			// 명성
	public int nNature;			// 성향

	// FTH[신앙]	FRD[친화력]
	/*
	public int nReg_LIGHT;		// 빛 저항
	public int nReg_DARK;       // 어둠 저항
	public int nReg_FIRE;       // 불 저항
	public int nReg_AQUA;       // 물 저항
	public int nReg_WIND;       // 바람 저항
	public int nReg_SOIL;       // 토양 저항
	public int nReg_ELEC;       // 전기 저항
	public int nREP;			// 마 저항
	*/
	public Status(
		int _hp = 0, int _maxhp = 0, int _mp = 0, int _maxmp = 0, int _exp = 0, int _maxexp = 10, int _atk = 0, int _Matk = 0, int _def = 0, int _Mdef = 0, int _critical = 0,
		int _str = 0, int _con = 0, int _int = 0, int _agi = 0, int _luk = 0, int _wis = 0,
		int _fame = 0, int _nature = 0) {
		
		nHP = _hp;
		nMaxHP = _maxhp;
		nMP = _mp;
		nMaxMP = _maxmp;
		nEXP = _exp;
		nMaxEXP = _maxexp;
		nATK = _atk;
		nM_ATK = _Matk;
		nDEF = _def;
		nM_DEF = _Mdef;
		nCritical = _critical;

		nStr = _str;
		nCon = _con;
		nInt = _int;
		nAgi = _agi;
		nLuk = _luk;
		nWis = _wis;

		nFame = _fame;
		nNature = _nature;
	}
	public static Status operator +(Status _status, int _value) {
		_status.nHP += _value;
		_status.nMaxHP += _value;
		_status.nMP += _value;
		_status.nMaxMP += _value;
		_status.nEXP += _value;
		_status.nMaxEXP += _value;
		_status.nATK += _value;
		_status.nM_ATK += _value;
		_status.nDEF += _value;
		_status.nM_DEF += _value;
		_status.nCritical += _value;

		_status.nStr += _value;
		_status.nCon += _value;
		_status.nAgi += _value;
		_status.nInt += _value;
		_status.nLuk += _value;
		_status.nWis += _value;

		_status.nFame += _value;
		_status.nNature += _value;
		return _status;
	}
	public static Status operator +(Status _status, Status _statusAdd) {
		Status sResult = new Status();
		sResult.nHP += _statusAdd.nHP;
		sResult.nMaxHP += _statusAdd.nMaxHP;
		sResult.nMP += _statusAdd.nMP;
		sResult.nMaxMP += _statusAdd.nMaxMP;
		sResult.nEXP += _statusAdd.nEXP;
		sResult.nMaxEXP += _statusAdd.nMaxEXP;
		sResult.nATK += _statusAdd.nATK;
		sResult.nM_ATK += _statusAdd.nM_ATK;
		sResult.nDEF += _statusAdd.nDEF;
		sResult.nM_DEF += _statusAdd.nM_DEF;
		sResult.nCritical += _statusAdd.nCritical;

		sResult.nStr += _statusAdd.nStr;
		sResult.nCon += _statusAdd.nCon;
		sResult.nInt += _statusAdd.nInt;
		sResult.nAgi += _statusAdd.nAgi;
		sResult.nLuk += _statusAdd.nLuk;
		sResult.nWis += _statusAdd.nWis;

		sResult.nFame += _statusAdd.nFame;
		sResult.nNature += _statusAdd.nNature;
		return sResult;
	}
	public static Status operator -(Status _status, Status _op) {
		Status sResult;
		sResult.nHP = _status.nHP - _op.nHP;
		sResult.nMaxHP = _status.nMaxHP - _op.nMaxHP;
		sResult.nMP = _status.nMP - _op.nMP;
		sResult.nMaxMP = _status.nMaxMP - _op.nMaxMP;
		sResult.nEXP = _status.nEXP - _op.nEXP;
		sResult.nMaxEXP = _status.nMaxEXP - _op.nMaxEXP;
		sResult.nATK = _status.nATK - _op.nATK;
		sResult.nM_ATK = _status.nM_ATK - _op.nM_ATK;
		sResult.nDEF = _status.nDEF - _op.nDEF;
		sResult.nM_DEF = _status.nM_DEF - _op.nM_DEF;
		sResult.nCritical = _status.nCritical - _op.nCritical;

		sResult.nStr = _status.nStr - _op.nStr;
		sResult.nCon = _status.nCon - _op.nCon;
		sResult.nInt = _status.nInt - _op.nInt;
		sResult.nAgi = _status.nAgi - _op.nAgi;
		sResult.nLuk = _status.nLuk - _op.nLuk;
		sResult.nWis = _status.nWis - _op.nWis;

		sResult.nFame = _status.nFame - _op.nFame;
		sResult.nNature = _status.nNature - _op.nNature;
		return sResult;
	}
}



public class ItemManager : MonoBehaviour {
	[SerializeField] List<Item> listItems;

	[SerializeField] Item[] arrHELMETs;
	[SerializeField] Item[] arrARMORs;
	[SerializeField] Item[] arrGLOVEs;
	[SerializeField] Item[] arrBOOTSs;
	[SerializeField] Item[] arrWEAPONs;
	[SerializeField] Item[] arrEARRINGs;
	[SerializeField] Item[] arrNECKLACEs;
	[SerializeField] Item[] arrRINGs;
	[SerializeField] Item[] arrBELTs;

	[SerializeField] Item[] arrPOTIONs;
	[SerializeField] Item[] arrFOODs;

	[SerializeField] Item[] arrEXPENDABLESs;

	/************************************************************************************/

	public List<Item> ListItems { get { return listItems; } }

	public Item[] GetARMORs { get { return arrARMORs; } }
	public Item[] GetHELMETs { get { return arrHELMETs; } }
	public Item[] GetGLOVEs { get { return arrGLOVEs; } }
	public Item[] GetBOOTSs { get { return arrBOOTSs; } }
	public Item[] GetWEAPONs { get { return arrWEAPONs; } }
	public Item[] GetEARRINGs { get { return arrEARRINGs; } }
	public Item[] GetNECKLACEs { get { return arrNECKLACEs; } }
	public Item[] GetRINGs { get { return arrRINGs; } }
	public Item[] GetBELTs { get { return arrBELTs; } }

	public Item[] GetPOTIONs { get { return arrPOTIONs; } }
	public Item[] GetFOODs { get { return arrFOODs; } }

	public Item[] GetEXPENDABLESs { get { return arrEXPENDABLESs; } }

	/************************************************************************************/

	public Item GetItem(int _idx) {
		return listItems[_idx];
	}
	public Item SearchItem(string _name) {
		int i = 0;
        do {
			if(listItems[i].itemName == _name) {
				return listItems[i];
			}
			else
				i++;
		} while(i >= listItems.Count);

		Debug.LogError("같은 이름의 아이템이 없음.");
		return null;
	}

	public void RandomItem(DropInventory _dropInven)
    {
		for (int i = 0; i < _dropInven.DropItems.Length; i++)
		{
			Item item = SetItem();
			_dropInven.DropItems[i] = item;
			_dropInven.DropItemCount[i] = SetItemCount(item.itemPart);
		}		
	}
	Item.ITEM_TYPE SetItemType(int _type)
    {
        switch (_type)
        {
            case 0: return Item.ITEM_TYPE.EQUIPMENT;
            case 1: return Item.ITEM_TYPE.USED;
            case 2: return Item.ITEM_TYPE.INGREDIENT;
			default: return Item.ITEM_TYPE.ETC;
		}
    }
	Item.ITEM_PART SetEquipmentPart(int _type)
    {
        switch (_type)
        {
			case 1: return Item.ITEM_PART.HELMET;
			case 2: return Item.ITEM_PART.ARMOR;
			case 3: return Item.ITEM_PART.GLOVE;
			case 4: return Item.ITEM_PART.BOOTS;
			case 5: return Item.ITEM_PART.WEAPON;
			case 6: return Item.ITEM_PART.EARRING;
			case 7: return Item.ITEM_PART.NECKLACE;
			case 8: return Item.ITEM_PART.RING;
			case 9: return Item.ITEM_PART.BELT;
			case 10: return Item.ITEM_PART.POTION;
			case 11: return Item.ITEM_PART.FOOD;
			default : return Item.ITEM_PART.EXPENDABLES;
        }
    }
	int SetItemCount(Item.ITEM_PART _part)
    {
		int num = 1;

		switch (_part)
        {
            case Item.ITEM_PART.HELMET:
            case Item.ITEM_PART.ARMOR:
            case Item.ITEM_PART.GLOVE:
            case Item.ITEM_PART.BOOTS:
            case Item.ITEM_PART.WEAPON:
            case Item.ITEM_PART.EARRING:
            case Item.ITEM_PART.NECKLACE:
            case Item.ITEM_PART.RING:
            case Item.ITEM_PART.BELT:
				break;
            case Item.ITEM_PART.POTION:
				num = Random.Range(1, 5 + 1);
				break;
			case Item.ITEM_PART.FOOD:
				num = Random.Range(1, 10 + 1);
				break;
			case Item.ITEM_PART.EXPENDABLES:
				num = Random.Range(1, 50 + 1);
				break;
		}
		return num;
	}
	Item SetItem()
    {
		Item.ITEM_TYPE tpye = SetItemType(Random.Range(0, listItems.Count + 1));
		Item.ITEM_PART part;
		tpye = Item.ITEM_TYPE.USED;
		if (tpye == Item.ITEM_TYPE.EQUIPMENT)
		{
			part = SetEquipmentPart(Random.Range(0, (int)Item.ITEM_PART.BELT + 1));
		}
		else
		{
			part = SetEquipmentPart(Random.Range((int)Item.ITEM_PART.POTION, (int)Item.ITEM_PART.EXPENDABLES + 1));

		}

		int itemNum = 0;
        switch (part)
        {
            case Item.ITEM_PART.HELMET:
				itemNum = Random.Range(0, arrHELMETs.Length);
				return arrHELMETs[itemNum];
            case Item.ITEM_PART.ARMOR:
				itemNum = Random.Range(0, arrARMORs.Length);
				return arrARMORs[itemNum];
			case Item.ITEM_PART.GLOVE:
				itemNum = Random.Range(0, arrGLOVEs.Length);
				return arrGLOVEs[itemNum];
			case Item.ITEM_PART.BOOTS:
				itemNum = Random.Range(0, arrBOOTSs.Length);
				return arrBOOTSs[itemNum];
			case Item.ITEM_PART.WEAPON:
				itemNum = Random.Range(0, arrWEAPONs.Length);
				return arrWEAPONs[itemNum];
			case Item.ITEM_PART.EARRING:
				itemNum = Random.Range(0, arrEARRINGs.Length);
				return arrEARRINGs[itemNum];
			case Item.ITEM_PART.NECKLACE:
				itemNum = Random.Range(0, arrNECKLACEs.Length);
				return arrNECKLACEs[itemNum];
			case Item.ITEM_PART.RING:
				itemNum = Random.Range(0, arrRINGs.Length);
				return arrRINGs[itemNum];
			case Item.ITEM_PART.BELT:
				itemNum = Random.Range(0, arrBELTs.Length);
				return arrBELTs[itemNum];
			case Item.ITEM_PART.POTION:
				itemNum = Random.Range(0, arrPOTIONs.Length);
				return arrPOTIONs[itemNum];
			case Item.ITEM_PART.FOOD:
				itemNum = Random.Range(0, arrFOODs.Length);
				return arrFOODs[itemNum];
			case Item.ITEM_PART.EXPENDABLES:
				itemNum = Random.Range(0, arrEXPENDABLESs.Length);
				return arrEXPENDABLESs[itemNum];
			default:
				Debug.LogError("아이템 메니저에 등록되지 않은 파트 : " + part);
				return null;
        }
    }
}
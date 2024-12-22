using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    [SerializeField] GameObject go_prefabPlayerInven;
    [SerializeField] GameObject go_prefabMobInven;
    [SerializeField] GameObject go_prefabBox;
    [SerializeField] NPC cInven;

    [SerializeField] int nReSpawnDelay;

    [SerializeField] DropInventory.E_INVEN_TYPE tempE_TYPE;

    /************************************************************************************/
    public void SpawnInven(GameObject _pos, Inventory _inventory = null, DropInventory.E_INVEN_TYPE _invenType = DropInventory.E_INVEN_TYPE.BOX_KEEP_S) {
        DropInventory items;
        switch(_invenType) {
            case DropInventory.E_INVEN_TYPE.PLAYER:
                go_prefabPlayerInven.transform.position = _pos.transform.position;
                go_prefabPlayerInven.SetActive(true);
                items = go_prefabPlayerInven.GetComponent<DropInventory>();

                items.SetInven(DropInventory.E_INVEN_TYPE.PLAYER);
                break;
            case DropInventory.E_INVEN_TYPE.MONSTER:
                go_prefabMobInven.SetActive(true);
                items = go_prefabMobInven.GetComponent<DropInventory>();

                //items.SetInven(DropInventory.E_INVEN_TYPE.MONSTER);
                break;
            default:
                go_prefabBox.gameObject.SetActive(true);
                items = go_prefabBox.GetComponent<DropInventory>();

                items.SetInven(RandomBoxType());
                break;
        }
    }

    public DropInventory.E_INVEN_TYPE RandomBoxType() {
        int chance = Random.Range(1, 101); // 1���� 100������ ���� ���� ����

        if(chance <= 10)
            return DropInventory.E_INVEN_TYPE.BOX_C; // 10% Ȯ��
        else if(chance <= 40)
            return DropInventory.E_INVEN_TYPE.BOX_B; // 30% Ȯ��
        else
            return DropInventory.E_INVEN_TYPE.BOX_A; // 60% Ȯ��
    }


    public void ReSpawnBox() {
        Debug.Log("ReSpawnBox");
        StartCoroutine(Delayed_Time());
    }
    IEnumerator Delayed_Time() {
        Debug.Log("���� " + nReSpawnDelay + "�ʷ� ����");
        yield return new WaitForSeconds(nReSpawnDelay);   // 10��(600��) ���
        SpawnInven(go_prefabBox);
    }
}
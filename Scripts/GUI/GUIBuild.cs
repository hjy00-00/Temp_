using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Build {
    public string CraftName;
    public GameObject CraftPrefab;
    public GameObject CraftPreviewPrefab;
}
public class GUIBuild : MonoBehaviour {
    bool m_bCraftPopup = false;
    [SerializeField] GameObject go_CraftBase;

    [SerializeField] List<GameObject> list_Select;
    [SerializeField] Build[] build_Fence;
    [SerializeField] Build[] build_Trap;
    int SelectMenu = -1;
    const int Fence = 0, Trap = 1, temp1 = 2, temp2 = 3, temp3 = 4;

    GameObject m_craftPreview;
    GameObject m_craftPrefab;

    [SerializeField]
    Transform m_trnsCrosshair;
    bool m_bCraftPreview = false;

    RaycastHit hitInfo;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float m_fRange;
    /****************************************/
    public bool CraftPopup { get { return m_bCraftPopup; } set { m_bCraftPopup = value; } }
    public bool CraftPreview { get { return m_bCraftPreview; } }
    /********************************************************************************/
    private void FixedUpdate() {
        if(m_bCraftPreview)
            PreviewPositionUpdate();
    }
    /********************************************************************************/
    public void ShowCraftPopup() {
        go_CraftBase.SetActive(true);
        for(int i = 0; i < list_Select.Count; i++) {
            list_Select[i].gameObject.SetActive(false);
        }
    }
    public void CloseCraftPopup() {
        go_CraftBase.SetActive(false);
    }

    public void ClickSelect(int _num) {
        for(int i = 0; i < list_Select.Count; i++) {
            list_Select[i].gameObject.SetActive(false);
        }
        list_Select[_num].gameObject.SetActive(true);
        SelectMenu = _num;
    }
    public void ClickSlot(int _num) {
        switch(SelectMenu) {
            case Fence:
                m_craftPreview = Instantiate(build_Fence[_num].CraftPreviewPrefab, m_trnsCrosshair.position + m_trnsCrosshair.forward, Quaternion.identity);
                m_craftPrefab = build_Fence[_num].CraftPrefab;
                break;
            case Trap:
                m_craftPreview = Instantiate(build_Trap[_num].CraftPreviewPrefab, m_trnsCrosshair.position + m_trnsCrosshair.forward, Quaternion.identity);
                m_craftPrefab = build_Trap[_num].CraftPrefab;
                break;
            case temp1:

                break;
            case temp2:

                break;
            case temp3:

                break;
        }
        CloseCraftPopup();
        m_bCraftPreview = true;
    }
    public void Cancel() {
        if(m_bCraftPreview)
            Destroy(m_craftPreview);

        SelectMenu = -1;
        m_bCraftPopup = false;
        m_bCraftPreview = false;
        m_craftPreview = null;
        m_craftPrefab = null;
        CloseCraftPopup();
    }
    public void PreviewPositionUpdate() {
        if(Physics.Raycast(m_trnsCrosshair.position, m_trnsCrosshair.forward, out hitInfo, m_fRange, layerMask)) {
            if(hitInfo.transform != null) {
                Vector3 vLocation = hitInfo.point;
                m_craftPreview.transform.position = vLocation;
            }
        }
    }
    public void Install() {
        if(m_bCraftPreview && m_craftPreview.GetComponent<PreviewObject>().IsInstall()) {
            Instantiate(m_craftPrefab, hitInfo.point, Quaternion.identity);
            Destroy(m_craftPreview);

            m_bCraftPopup = false;
            m_bCraftPreview = false;
            m_craftPreview = null;
            m_craftPrefab = null;
            CloseCraftPopup();
        }
    }
}
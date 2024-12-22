using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewObject : MonoBehaviour {
    List<Collider> list_Collider = new List<Collider>();

    [SerializeField] int m_nLayerGround;
    const int IGNORE_RAYCAST_LAYER = 2;

    [SerializeField] Material mat_Blue;
    [SerializeField] Material mat_Red;
    /********************************************************************************/
    void Update() {
        ChangeColor();
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.layer != m_nLayerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER) {
            list_Collider.Add(other);
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.layer != m_nLayerGround && other.gameObject.layer != IGNORE_RAYCAST_LAYER) {
            list_Collider.Remove(other);
        }
    }
    /********************************************************************************/
    public void ChangeColor() {
        if(list_Collider.Count > 0)
            SetColor(mat_Red);
        else
            SetColor(mat_Blue);
    }
    public void SetColor(Material _mat) {
        foreach(Transform transChild in this.transform) {
            var newMaterial = new Material[transChild.GetComponent<Renderer>().materials.Length];
            for(int i = 0; i < newMaterial.Length; i++) {
                newMaterial[i] = _mat;
            }
            transChild.GetComponent<Renderer>().materials = newMaterial;
        }
    }

    public bool IsInstall() {
        return list_Collider.Count == 0;
    }
}
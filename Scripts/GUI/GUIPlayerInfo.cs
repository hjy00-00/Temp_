using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIPlayerInfo : MonoBehaviour {
    [SerializeField] Text name;
    [SerializeField] Text lv;
    [SerializeField] Image barHP;
    [SerializeField] Image barMP;
    [SerializeField] Image barEXP;

    public void UpdataPlayerStatus(Player _player) {
        name.text = _player.Name;
        lv.text = string.Format("{0}", _player.Level);
        barHP.fillAmount = UpdateBars(_player.PlayerStatus.nHP, _player.PlayerStatus.nMaxHP);
        barMP.fillAmount = UpdateBars(_player.PlayerStatus.nMP, _player.PlayerStatus.nMaxMP);
        barEXP.fillAmount = UpdateBars(_player.PlayerStatus.nEXP, _player.PlayerStatus.nMaxEXP);
    }

    public float UpdateBars(float _cur, float _max) {
        return _cur / _max;
    }
}

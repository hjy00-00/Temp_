using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIWorldMonsterInfo : MonoBehaviour
{
    [SerializeField] Text name;
    [SerializeField] Image barHP;

    public void UpdataMonserStatus(Player _player)
    {
        name.text = _player.Name;
        barHP.fillAmount = UpdateBars(_player.PlayerStatus.nHP, _player.PlayerStatus.nMaxHP);
    }

    public float UpdateBars(float _cur, float _max)
    {
        return _cur / _max;
    }

}

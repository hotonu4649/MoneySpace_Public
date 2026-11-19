using UnityEngine;
using TMPro;

namespace RelatedMoney
{
    /// <summary>
    /// HaveMoneyViewクラスは、所持金の表示を担当するMonoBehaviourです。
    /// </summary>
    public class HaveMoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyText;
        
        /// <summary>
        /// 所持金を更新して表示します。
        /// </summary>
        /// <param name="money">更新する所持金</param>
        /// <returns></returns>
        public void UpdateMoneyDisplay(Money money)
        {
            _moneyText.text = $"{money}";
        }
    }
}
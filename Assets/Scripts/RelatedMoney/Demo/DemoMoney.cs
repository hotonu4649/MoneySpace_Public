using NaughtyAttributes;
using UnityEngine;
using VContainer;

namespace RelatedMoney.Demo
{
    /// <summary>
    /// DemoMoneyクラスは、所持金のデモンストレーションを行うためのMonoBehaviourです。
    /// </summary>
    public class DemoMoney : MonoBehaviour
    {
        private HaveMoneyStatus _haveMoneyStatus;
        
        [Inject]
        public void Construct(HaveMoneyStatus haveMoneyStatus)
        {
            _haveMoneyStatus = haveMoneyStatus;
        }
        
        [Button]
        public void AddMoney()
        {
            _haveMoneyStatus.AddMoney(new Money(5));
        }

        [Button]
        public void SubtractMoney()
        {
            _haveMoneyStatus.SubtractMoney(new Money(3));
        }
    }
}
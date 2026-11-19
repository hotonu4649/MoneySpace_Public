using UnityEngine;

namespace DynamicItems
{
    /// <summary>
    /// インターフェースは、スポーンされるオブジェクトのデータを定義するために使用されます。
    /// </summary>
    public interface ISpawnData
    {
        public GameObject Prefab { get; }
        //ほかにもいろいろな情報を追加することができる
    }
}
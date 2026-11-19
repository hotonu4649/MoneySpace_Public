using UnityEngine;

namespace Effects
{
    public interface ISpawnBeforeEffect
    {
        public abstract void BeforeEffect(Vector2 position, GameObject effect);
    }
}
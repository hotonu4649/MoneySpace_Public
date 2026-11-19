using UnityEngine;

namespace Effects
{
    public interface ISpawnAfterEffect
    {
        public abstract void AfterEffect(Vector2 position, GameObject effect);
    }
}
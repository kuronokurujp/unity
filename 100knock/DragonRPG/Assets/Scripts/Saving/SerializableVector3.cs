using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializableVector3
    {
        public float x, y, z;

        public SerializableVector3(Vector3 pos)
        {
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }

        public Vector3 ToVector()
        {
            return new Vector3(this.x, this.y, this.z);
        }
    }
}
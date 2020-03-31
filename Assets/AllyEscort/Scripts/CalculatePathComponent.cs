using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public abstract class CalculatePathComponent : MonoBehaviour
    {
        public abstract List<Vector3> GetPath(Vector3 start, Vector3 end);
    }

}
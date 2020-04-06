using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort
{
    public class MoveAimlesslyState : MoveToPointState
    {
        public Vector3 areaCenter = Vector3.zero;
        public Vector3 areaSize = new Vector3(5, 0, 5);
        public float waitTimer = 0.5f;
        private float _timer;

        internal override bool HandleInitialize()
        {
            CalculatePath(GetRandomPoint());
            _timer = 0;
            return true;
        }

        internal override void HandleNullPath()
        {
            CalculatePath(GetRandomPoint());
        }

        internal override void HandleEmptyPath()
        {
            _timer += Time.deltaTime;

            if (_timer > waitTimer)
            {
                _timer = 0;
                CalculatePath(GetRandomPoint());
            }
        }

        internal Vector3 GetRandomPoint()
        {
            Vector3 point = areaCenter;

            point.x += Random.Range(-areaSize.x, areaSize.x);
            point.y += Random.Range(-areaSize.y, areaSize.y);
            point.z += Random.Range(-areaSize.z, areaSize.z);

            return point;
        }
    }
}
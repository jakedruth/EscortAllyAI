using System.Collections;
using System.Collections.Generic;
using AllyEscort;
using UnityEngine;

namespace AllyEscort
{
    public class GoToPointState : BaseState
    {
        [Header("Variables")]
        public float maxSpeed;
        public float minSpeed;
        private float _speed;
        public float acceleration;
        public bool slowDownToTarget;
        public float slowDownRange;
        private List<Vector3> _path;

        public override void OnEnter()
        {
            _speed = 0;
        }

        public override void Update()
        {
            if (_path.Count > 0)
            {
                Vector3 delta = _path[0] - pos;
                Vector3 dir = delta.normalized;

                _speed = Mathf.MoveTowards(_speed, maxSpeed, acceleration * Time.deltaTime);

                // Check to see if should slow down to the target
                if (slowDownToTarget && _path.Count == 1 && delta.sqrMagnitude < slowDownRange * slowDownRange)
                {
                    float fractionWithinRange = delta.magnitude / slowDownRange;
                    _speed = fractionWithinRange * maxSpeed;
                }

                if (_speed < minSpeed)
                    _speed = minSpeed;

                Vector3 velocity = dir * _speed;
                pos += velocity * Time.deltaTime;

                transform.position = pos;

                if (delta.sqrMagnitude <= 0.01f)
                {
                    _path.RemoveAt(0);
                }
            }
        }

        public override void OnExit()
        { }

        public void SetPath(List<Vector3> path)
        {
            _path = path;
        }
    }
}

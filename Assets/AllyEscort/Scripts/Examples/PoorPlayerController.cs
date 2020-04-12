using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AllyEscort.Example
{
    /// <summary>
    /// This is intentionally a very bad player controller. The user should implement their own user input class.
    /// This class exist so the example scene gives a better use-case example.
    /// </summary>
    public class PoorPlayerController : MonoBehaviour
    {
        public float speed;

        // Update is called once per frame
        void Update()
        {
            Vector3 velocity = Vector3.zero;

            velocity.x += Input.GetAxis("Horizontal") * speed;
            velocity.z += Input.GetAxis("Vertical") * speed;

            transform.position += velocity * Time.deltaTime;
        }
    }
}
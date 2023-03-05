using System;
using UnityEngine;

namespace Tutorial
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private Vector3 offset;

        private Transform _target;


        private void Start()
        {
            _target = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            Vector3 newPos = _target.transform.position + offset;
            transform.position = Vector3.Lerp(transform.position, newPos, speed * Time.deltaTime);
        }
    }
}
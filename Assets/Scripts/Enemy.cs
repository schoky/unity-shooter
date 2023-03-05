using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Tutorial
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private Transform hitPoint;
        [SerializeField] private float radius;
        [SerializeField] private float reloadTime;
        [SerializeField] private int damage;
        
        private NavMeshAgent _agent;
        private Transform _target;
        private Animator _anim;
        private bool _canHit;
        private bool _isReloaded = true;


        private void Start()
        {
            _anim = GetComponent<Animator>();
            _agent = GetComponent<NavMeshAgent>();
            _target = FindObjectOfType<Player>().transform;
        }

        private void Update()
        {
            _agent.SetDestination(_target.position);

            _anim.SetBool("isWalk", _agent.velocity.magnitude > 0.1f);

            bool isDistance = Vector3.Distance(transform.position, _target.position) < _agent.stoppingDistance;
            _canHit = isDistance;

            if (_canHit && _isReloaded)
            {
                Collider[] colliders = Physics.OverlapSphere(hitPoint.position, radius);
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent(out Player player))
                    {
                        _anim.SetTrigger("hit");
                        player.GetDamage(damage);
                    }
                }
                
                StartCoroutine(Reload());
            }
        }

        IEnumerator Reload()
        {
            _isReloaded = false;
            yield return new WaitForSeconds(reloadTime);
            _isReloaded = true;
        }
    }
}
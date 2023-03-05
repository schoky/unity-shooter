using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tutorial
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float reloadTime;
        [SerializeField] private int startHealth;
        [SerializeField] private PlayerUI ui;
        [SerializeField] private Transform hitPoint;
        [SerializeField] private float hitRadius;

        private Rigidbody _rb;
        private Animator _anim;
        private bool _canHit = true;
        private int _health;


        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _anim = GetComponent<Animator>();

            _health = startHealth;
        }

        private void Update()
        {
            Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (moveInput.magnitude > 0.1f)
            {
                Quaternion rotation = Quaternion.LookRotation(moveInput);
                rotation.x = 0;
                rotation.z = 0;
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
            
            _anim.SetBool("isWalk", moveInput.magnitude > 0.1f);

            _rb.velocity = moveInput * speed;


            if (Input.GetMouseButton(0) && _canHit == true)
            {
                _anim.SetTrigger("axeHit");
                StartCoroutine(Reload());
            }
        }

        IEnumerator Reload()
        {
            _canHit = false;
            yield return new WaitForSeconds(reloadTime);
            _canHit = true;
        }

        public void GetDamage(int damage)
        {
            _health -= damage; // _health = _health - damage
            ui.SetHealth(_health);

            if (_health <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }

        private void Hit()
        {
            Collider[] colliders = Physics.OverlapSphere(hitPoint.position, hitRadius);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].TryGetComponent(out Tree tree) && tree.isRespawned)
                {
                    ui.TreeCount++;
                    tree.Destroy();
                }

                if (colliders[i].TryGetComponent<Enemy>(out _))
                {
                    Destroy(colliders[i].gameObject);
                }
            }
        }
    }
}
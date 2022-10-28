using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeletonController : MonoBehaviour
{
    private int routine;
    private float chronometer;
    [Header("Enemy")]
    private GameObject target;
    [SerializeField] private float enemySpeed = 1f;
    [SerializeField] private float enemyViewRange = 5f;
    [SerializeField] private float enemyRotation = 25f;
    [Range (0, 1f)] [SerializeField] private float attackDistance = 1f;
    private Animator animator;
    private Quaternion angle;
    private float degree;
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Enemy_behavior();
    }

    public void Enemy_behavior()
    {
        if (Vector3.Distance(transform.position, target.transform.position) > enemyViewRange)
        {
            animator.SetBool("run", false);
            chronometer += 1 * Time.deltaTime;
            if (chronometer >= 4)
            {
                routine = Random.Range(0, 2);
                chronometer = 0;
                print(routine);
            }

            switch (routine)
            {
                case 0:
                    animator.SetBool("walk", false);
                    break;
                case 1:
                    degree = Random.Range(0, 360);
                    angle = Quaternion.Euler(0, degree, 0);
                    routine++;
                    break;
                case 2:
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, angle, 0.5f);
                    transform.Translate(enemySpeed * Time.deltaTime * Vector3.forward);
                    animator.SetBool("walk", true);
                    break;

            }
        }
        else
        {
            if (Vector3.Distance(transform.position, target.transform.position) > attackDistance && attacking)
            {
                var lookPos = target.transform.position - transform.position;
                lookPos.y = 0;
                var rotation = Quaternion.LookRotation(lookPos);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, enemyRotation);
                animator.SetBool("walk", false);

                animator.SetBool("run", true);
                transform.Translate((enemySpeed + 0.7f) * Time.deltaTime * Vector3.forward);

                animator.SetBool("attack", false);
            }
            else
            {
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
                animator.SetBool("attack", true);
                attacking = true;
            }
            
        }
    }

    public void Finish_animator()
    {
        animator.SetBool("attack", false);
        attacking = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    private Movement movement;
    private Attributes attributes;
    private float horizontal_move;
    private Animator animator;
    private bool isCharge = false;
    private bool isClose = false;
    private GameObject particle_temp;
    public GameObject heart;
    public GameObject particle;
    public GameObject level_complete_object1;
    public GameObject level_complete_object2;
    public GameObject game_over_text;
    public Slider mental_slider;
    public GameObject instruction_panel;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        attributes = GetComponent<Attributes>();
        attributes.dumbell = 0;
        animator = GetComponent<Animator>();
        mental_slider.value = attributes.mental;
    }

    // Update is called once per frame
    void Update()
    {
        if (isClose)
        {
            if (attributes.health > 0 && !attributes.game_finish)
            {
                horizontal_move = Input.GetAxisRaw("Horizontal") * attributes.speed;
                animator.SetFloat("speed", Mathf.Abs(horizontal_move));

                if (Mathf.Abs(horizontal_move) > 0.01)
                {
                    FindObjectOfType<AudioManager>().play("run");
                }

                if (Input.GetButtonDown("Jump"))
                {
                    FindObjectOfType<AudioManager>().play("jump");
                    attributes.jump = true;
                    animator.SetBool("jump", true);
                }

                if (attributes.level_complete)
                {
                    level_complete_object1.SetActive(true);
                    level_complete_object2.SetActive(true);
                }

                if (!isCharge)
                {
                    attributes.mental -= (attributes.mental_reduce * Time.deltaTime);
                }
                else
                {
                    if(attributes.mental <= 1)
                    {
                        attributes.mental += (attributes.mental_charge * Time.deltaTime);
                        FindObjectOfType<AudioManager>().play("fill_mental");
                    }
                }
                mental_slider.value = attributes.mental;
            }

            if (attributes.mental <= 0)
            {
                Transform[] life = heart.GetComponentsInChildren<Transform>();
                Destroy(life[1].gameObject);
                attributes.health -= 1;
                animator.SetTrigger("get_hit");
                FindObjectOfType<AudioManager>().play("hit");
                if (attributes.health == 0)
                {
                    StartCoroutine(game_over());
                }
                else
                {
                    attributes.mental = 1f;
                }
            }
        }
    }

    void FixedUpdate()
    {
        movement.Move(horizontal_move * Time.fixedDeltaTime, attributes.jump);
        attributes.jump = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 7)
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }

        if(collision.tag == "Enemy")
        {
            if(attributes.health > 0)
            {
                Transform[] life = heart.GetComponentsInChildren<Transform>();
                Destroy(life[1].gameObject);
                attributes.health -= 1;
                animator.SetTrigger("get_hit");
                FindObjectOfType<AudioManager>().play("hit");
                if (attributes.health == 0)
                {
                    StartCoroutine(game_over());
                }
            }
        }
        if (collision.tag == "Dumbell")
        {
            Destroy(Instantiate(particle, collision.gameObject.transform.position, Quaternion.identity), 1f);
            Destroy(collision.gameObject);
            FindObjectOfType<AudioManager>().play("dumbell");
            attributes.dumbell += 1;
        }
        if(collision.tag == "FillMental")
        {
            particle_temp = Instantiate(particle, collision.transform.position, Quaternion.identity);
        }
        if (collision.tag == "NextLevel" && attributes.level_complete)
        {
            FindObjectOfType<AudioManager>().play("finish");
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "FillMental")
        {
            isCharge = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "FillMental")
        {
            isCharge = false;
            Destroy(particle_temp);
        }
    }

    private IEnumerator game_over()
    {
        animator.SetBool("isDeath", true);
        FindObjectOfType<AudioManager>().play("hit");
        horizontal_move = 0f;
        game_over_text.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }

    public void OnLand()
    {
        animator.SetBool("jump", false);
    }

    public void close_instruction()
    {
        FindObjectOfType<AudioManager>().play("ui_sound");
        Destroy(instruction_panel);
        isClose = true;
    }
}

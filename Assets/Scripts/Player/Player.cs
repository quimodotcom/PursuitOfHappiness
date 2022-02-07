using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField] float      speed = 2.0f;
    public float                jumpForce = 7.5f;

    [Header("Graphics")]
    private Animator            darkAnimator;
    [SerializeField] private Animator            lightAnimator;
    [SerializeField] private SpriteRenderer      darkSprite;
    [SerializeField] private SpriteRenderer      lightSprite;

    private Rigidbody2D         body2d;
    private Sensor_Player       groundSensor;
    private bool                grounded = false;
    private bool                combatIdle = false;
    private bool                isDead = false;
    private int                 health;
    private int                 startingHealth;
    private int                 happiness;
    private int                 totalHappiness;

    //SFX
    //Walking
    [Header("Audio Clips")]
    public AudioSource walk1;
    public AudioSource walk2;
    public AudioSource walk3;
    public AudioSource walk4;
    public AudioSource walk5;

    private bool stopped = false;
    
    float pickedNum;

    // Use this for initialization
    void Start () {
        pickedNum = 0f;
        Game.Instance.Player = this;

        darkAnimator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Player>();

        health = startingHealth;
        totalHappiness = FindObjectsOfType<Happiness>().Length;

        SpriteRendererAlpha(lightSprite, happiness / totalHappiness);
        SpriteRendererAlpha(darkSprite, 1 - (happiness / totalHappiness));
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!grounded && groundSensor.State()) {
            grounded = true;
            darkAnimator.SetBool("Grounded", grounded);
            lightAnimator.SetBool("Grounded", grounded);

        }

        //Check if character just started falling
        if(grounded && !groundSensor.State()) {
            grounded = false;
            darkAnimator.SetBool("Grounded", grounded);
            lightAnimator.SetBool("Grounded", grounded);
        }

        //Recover
        if (Input.GetKeyDown("e") && grounded)
        {
            KillPlayer(false);
            DecrementHappiness();
            isDead = !isDead;
        }

        // -- Handle input and movement --
        if (!stopped)
        {
            float inputX = Input.GetAxis("Horizontal");

            // Swap direction of sprite depending on walk direction
            if (inputX > 0)
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            else if (inputX < 0)
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

            // Move
            body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

            //Set AirSpeed in animator
            darkAnimator.SetFloat("AirSpeed", body2d.velocity.y);
            lightAnimator.SetFloat("AirSpeed", body2d.velocity.y);

            // -- Handle Animations --

            /*//Hurt
            else if (Input.GetKeyDown("q"))
                m_animator.SetTrigger("Hurt");*/

            //Attack
            if(Input.GetMouseButtonDown(0) && grounded) {
                darkAnimator.SetTrigger("Attack");
                lightAnimator.SetTrigger("Attack");
            }

            /*//Change between idle and combat idle
            else if (Input.GetKeyDown("f"))
                m_combatIdle = !m_combatIdle;*/

            //Jump
            else if (Input.GetKeyDown("space") && grounded)
            {
                darkAnimator.SetTrigger("Jump");
                lightAnimator.SetTrigger("Jump");
                grounded = false;
                darkAnimator.SetBool("Grounded", grounded);
                lightAnimator.SetBool("Grounded", grounded);
                body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
                groundSensor.Disable(0.2f);
            }

            //Run
            else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            {
                darkAnimator.SetInteger("AnimState", 2);
                lightAnimator.SetInteger("AnimState", 2);
            }

            //Combat Idle
            else if (combatIdle)
            {
                darkAnimator.SetInteger("AnimState", 1);
                lightAnimator.SetInteger("AnimState", 1);
            }

            //Idle
            else
            {
                darkAnimator.SetInteger("AnimState", 0);
                lightAnimator.SetInteger("AnimState", 0);
            }
        }
    }

    public void KillPlayer(bool status)
    {
        if(status)
        {
            stopped = true;
            darkAnimator.SetTrigger("Death");
            lightAnimator.SetTrigger("Death");
        }
        else
        {
            darkAnimator.SetTrigger("Recover");
            lightAnimator.SetTrigger("Recover");
            stopped = false;
        }
    }

    public void UpdateHealth(int _delta) {
        health += _delta;        
	}

    public void IncrementHappiness() {
        happiness++;

        SpriteRendererAlpha(lightSprite, happiness / (float)totalHappiness);
        SpriteRendererAlpha(darkSprite, 1 - (happiness / (float)totalHappiness));

        speed++;
	}

    public void DecrementHappiness()
    {
        happiness--;

        SpriteRendererAlpha(lightSprite, happiness / (float)totalHappiness);
        SpriteRendererAlpha(darkSprite, 1 - (happiness / (float)totalHappiness));

        if(speed < 4)
        {
            speed = 2;
        }
        else
        {
            speed--;
        }
    }

    private void SpriteRendererAlpha(SpriteRenderer _renderer, float _alpha) {
        Color tmp = _renderer.color;
        tmp.a = _alpha;
        _renderer.color = tmp;
    }

    void walkSFX()//Plays walking sound effect
    {
        pickedNum = Random.Range(1, 5);
        if(pickedNum == 1)
        {
            walk1.Play();
        }
        if (pickedNum == 2)
        {
            walk2.Play();
        }
        if (pickedNum == 3)
        {
            walk3.Play();
        }
        if (pickedNum == 4)
        {
            walk4.Play();
        }
        if (pickedNum == 5)
        {
            walk5.Play();
        }
    }
    public IEnumerator OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && darkAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "HeavyBandit_Attack" || darkAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "LightBandit_Attack")
        {
            yield return new WaitForSeconds(0.3f);
            Destroy(collision.gameObject);
        }
    }
}

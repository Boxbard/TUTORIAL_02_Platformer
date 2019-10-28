using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerCat : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public float jumpForce;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public Text countText;
    private int count;
    public Text winText;

    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public AudioClip MusicClipOne;
    public AudioClip MusicClipTwo;
    public AudioSource MusicSource;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        count = 0;
        winText.text = "";
        SetCountText();
        MusicSource.clip = MusicClipOne;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    //FixedUpdate is for physics
    //LateUpdate is the last to be called

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 3);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }

        if (rd2d.velocity.y < 0)
        {
            rd2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

       {
            if (health > numOfHearts)
            {
                health = numOfHearts;
            }
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < health)
                {
                    hearts[i].sprite = fullHeart;
                }
                else
                {
                    hearts[i].sprite = emptyHeart;
                }
                if (i < numOfHearts)
                {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
        }
    }

    
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.SetActive(false);
            health = health - 1;
            if (health <= 0)
            {
                winText.text = "You Lose!";
                Destroy(gameObject);
            }
        }
        if (count == 6)
        {
            transform.position = new Vector2(70.0f, 0.09999999f);
            health = 3;
        }
    }

    void SetCountText ()
    {
        countText.text = "Score: " + count.ToString();
        if (count >= 12)
        {
            winText.text = "You win! Game created by Casey Chung";
            MusicSource.clip = MusicClipTwo;
            MusicSource.Play();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground")
        {
            if(Input.GetKeyDown(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                rd2d.velocity = new Vector2(rd2d.velocity.x, jumpForce);
            }
        }
    }

}

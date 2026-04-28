using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class XianJing : MonoBehaviour
{
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            collision.transform.GetComponent<CapsuleCollider2D>().enabled = false;
            // ������ϵ�һ��
            collision.transform.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(collision.transform.GetComponent<Rigidbody2D>().linearVelocity.x, 7f);
            collision.transform.GetComponent<Player>().enabled = false;
            collision.transform.GetComponent<HeroAnimations>().PlayDie();

            Timer.Instance.PlayTimer(5, () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }
    }
}

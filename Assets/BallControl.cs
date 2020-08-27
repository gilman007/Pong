using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour {

	// Rigidbody 2D bola
    private Rigidbody2D rigidBody2D;
    private Animator anim;

    [SerializeField]private PlayerControl player1;
    [SerializeField]private PlayerControl player2;
 
    // Besarnya gaya awal yang diberikan untuk mendorong bola
    public float xInitialForce;
    public float yInitialForce;
    private int fireBallCount;
    private int fireBallRandom;

    // Titik asal lintasan bola saat ini
    private Vector2 trajectoryOrigin;

	// Use this for initialization
	void Start () {
		rigidBody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
     
        trajectoryOrigin = transform.position;
 
        // Mulai game
        RestartGame();
	}

	void PushBall()
    {

        // Menyimpan besar kecepatan bola
        float ballMagnitude = yInitialForce * yInitialForce + xInitialForce * xInitialForce;

        // Tentukan nilai komponen y dari gaya dorong antara -yInitialForce dan yInitialForce
        float yRandomInitialForce = Random.Range(-yInitialForce, yInitialForce);

        // Kalkulasi x baru setelah randomisasi komponen y
        float xForce = Mathf.Sqrt(ballMagnitude - yRandomInitialForce * yRandomInitialForce);

        // Tentukan nilai acak antara 0 (inklusif) dan 2 (eksklusif)
        float randomDirection = Random.Range(0, 2);

        // Jika nilainya di bawah 1, bola bergerak ke kiri. 
        // Jika tidak, bola bergerak ke kanan.
        if (randomDirection < 1.0f)
        {
            // Gunakan gaya untuk menggerakkan bola ini.
            rigidBody2D.AddForce(new Vector2(-xForce, yRandomInitialForce));
        }
        else
        {
            rigidBody2D.AddForce(new Vector2(xForce, yRandomInitialForce));
        }
    }

	void ResetBall()
    {
        // Reset posisi menjadi (0,0)
        transform.position = Vector2.zero;
 
        // Reset kecepatan menjadi (0,0)
        rigidBody2D.velocity = Vector2.zero;

        //Reset Fireball
        anim.SetBool("isFireBall",false);
        fireBallCount = 0;
        fireBallRandom = Random.Range(3,20);
        Debug.Log("Fireball Spawn : " + fireBallRandom);
    }

	void RestartGame()
    {
        // Kembalikan bola ke posisi semula
        ResetBall();
 
        // Setelah 2 detik, berikan gaya ke bola
        Invoke("PushBall", 2);
    }

    private void OnCollisionEnter2D(Collision2D col){
        fireBallCount += 1;
        Debug.Log(fireBallCount);
        //Debug.Log(col.gameObject.name);

        //Memunculkan Fire Ball
        if(fireBallRandom == fireBallCount){
            anim.SetBool("isFireBall", true);
        }

        if (fireBallCount > fireBallRandom){
            Debug.Log("test");
            if (col.gameObject.name == "Player1"){
                player2.SetScore(5);
            }
            else if (col.gameObject.name == "Player2"){
                player1.SetScore(5);
            }
        }
    }


    // Ketika bola beranjak dari sebuah tumbukan, rekam titik tumbukan tersebut
    private void OnCollisionExit2D(Collision2D collision)
    {
        trajectoryOrigin = transform.position;
    }

    // Untuk mengakses informasi titik asal lintasan
    public Vector2 TrajectoryOrigin
    {
        get { return trajectoryOrigin; }
    }
}

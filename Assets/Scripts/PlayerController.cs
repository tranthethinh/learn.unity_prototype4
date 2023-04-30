using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed = 100.0f;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    public bool hasPowerupGun = false;
    public bool hasPowerupJump = false;
    private float powerupStrength = 11.0f;
    public GameObject powerupIndicator;

    public Transform FirePoint;//place bullet appear
    public GameObject Bullet;
    private float time;

    public float hangTime;
    public float smashSpeed;
    public float explosionForce;
    public float explosionRadius;

    float floorY;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        List<GameObject> allEnemies = new List<GameObject>();
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        allEnemies.AddRange(GameObject.FindGameObjectsWithTag("S_enemy"));

        if (hasPowerupGun && Input.GetKey(KeyCode.F))
        {
            foreach (GameObject enemy in allEnemies)
            {
                time += Time.deltaTime;
                gameObject.transform.LookAt(enemy.transform);
                Vector3 shootEnemy = enemy.transform.position - FirePoint.transform.position;
                if (time >= 0.3f)
                {
                    GameObject bullet = Instantiate(Bullet, FirePoint.transform.position, FirePoint.transform.rotation);
                    bullet.GetComponent<Rigidbody>().AddForce(shootEnemy * 1000);
                    Destroy(bullet, 1f);
                    time = 0;
                }
            }

        }
        if (hasPowerupJump && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Smash());
        }
        allEnemies.Clear(); // frees the memory used by the list

        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, 0, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountDownRoutine());
        }
        if (other.CompareTag("PowerupGun"))
        {


            hasPowerupGun = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountDownRoutine());
        }
        if (other.CompareTag("PowerupJump"))
        {
            hasPowerupJump = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerupCountDownRoutine());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
            Debug.Log("Collided with " + collision.gameObject.name + "with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);

        }
        if (collision.gameObject.CompareTag("S_enemy"))
        {

            Vector3 awayFromEnemy = (transform.position - collision.gameObject.transform.position).normalized;
            Debug.Log("Collided with " + collision.gameObject.name + "with powerup set to " + hasPowerup);
            playerRb.AddForce(awayFromEnemy * powerupStrength*4, ForceMode.Impulse);

        }
    }
    IEnumerator PowerupCountDownRoutine()
    {
        yield return new WaitForSeconds(8);
        powerupIndicator.gameObject.SetActive(false);
        hasPowerup = false;
        hasPowerupGun = false;
        hasPowerupJump = false;
    }
    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>(); //Store the y position before taking off
        floorY = transform.position.y; //Calculate the amount of time we will go up
        float jumpTime = Time.time + hangTime;
        while (Time.time < jumpTime)
        { //move the player up while still keeping their xvelocity.
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        //Now move the player down
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
        }
        //Cycle through all enemies.
        for (int i = 0; i < enemies.Length; i++)
        {
            //Apply an explosion force that originates from our position.
            if (enemies[i] != null) enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);
        }
    }
}

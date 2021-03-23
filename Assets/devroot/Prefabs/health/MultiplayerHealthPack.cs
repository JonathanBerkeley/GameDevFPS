using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Multiplayer variant of health pack
public class MultiplayerHealthPack : MonoBehaviour
{
    public int respawnDelay = 10;
    public float spinSpeed = 200.0f;
    public int regenAmount = 100;
    public AudioClip[] packAudio;

    // Update is called once per frame
    void Update()
    {
        //Spinning animation
        transform.Rotate(0, 0, spinSpeed * Time.deltaTime, Space.Self);
    }

    //Detecting if a player touches healthpack
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //Increases the player's health that touches the health pack
            try
            {
                collision.gameObject.GetComponent<MultiplayerStats>().IncreaseHealth(regenAmount);
            } catch
            {
            }
            StartCoroutine(RespawnHealthPack());

        }
    }

    IEnumerator RespawnHealthPack()
    {
        //Play regenerate sound
        AudioSource.PlayClipAtPoint(packAudio[1], transform.position);

        //Hide under world
        transform.position = new Vector3(
            transform.position.x
            , transform.position.y - 1000
            , transform.position.z);

        //Wait for two seconds efficiently
        yield return new WaitForSeconds(respawnDelay);

        //Reset to position
        transform.position = new Vector3(
            transform.position.x
            , transform.position.y + 1000
            , transform.position.z);

        //Play respawn sound
        AudioSource.PlayClipAtPoint(packAudio[0], transform.position);
    }
}

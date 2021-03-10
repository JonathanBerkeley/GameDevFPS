using System.Collections;
using System.Collections.Generic;
using FlagTranslations;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject projectilePrefab;
    public GameObject gameLogic;
    public Text gameChat;
    public Text errorConnectMessage;

    private SpawnHandler spawnHandler;
    private Color preColor;

    private void Awake()
    {
        //Make this a singleton class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already in existence, destroying self");
            Destroy(this);
        }

        preColor = gameChat.color;
    }

    private void Start()
    {
        spawnHandler = gameLogic.GetComponent<SpawnHandler>();
    }


    //This is used to handle all code related to spawning players
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        GameObject[] rl = spawnHandler.GetRandomisedSpawns();
        //If this is data for the current client
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab
                , spawnHandler.GetRandomSpawn(rl).transform.position
                , _rotation);
            PlayerID.AssignNewID(_player);
        }
        // If it's data for other clients
        else
        {
            _player = Instantiate(playerPrefab
                , spawnHandler.GetRandomSpawn(rl).transform.position
                , _rotation);
            PlayerID.AssignNewID(_player);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void CreateProjectile(int _id, Vector3 _location, Quaternion _rotation)
    {
        //Debug.Log($"CreateProjectile was called! With data {_id} {_location} {_rotation}");
        GameObject projectile = Instantiate(projectilePrefab, _location, _rotation);

        //Gives rocket momentum
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.AddForce(projectile.transform.forward * MultiplayerLaunchProjectile.projectileSpeed, ForceMode.VelocityChange);
    }


    private List<string> _chatMessages = new List<string>();
    public void ReceiveChat(int _id, string _message)
    {
        StopAllCoroutines();
        gameChat.color = preColor;
        gameChat.text = "";
        _chatMessages.Add($"{players[_id].username}: {_message}\n");

        if (_chatMessages.Count > 7)
        {
            _chatMessages.RemoveAt(0);
        }

        for (int i = 0; i < _chatMessages.Count; ++i)
        {
            gameChat.text += _chatMessages[i];
        }

        StartCoroutine(ChatFadeOverTime());

        IEnumerator ChatFadeOverTime()
        {
            Color currentColor = preColor;
            Color fadedColor = preColor;
            fadedColor.a = 0;
            yield return new WaitForSeconds(5);


            while (currentColor.a > fadedColor.a)
            {
                currentColor.a -= 0.05f;
                yield return new WaitForSeconds(0.1f);
                gameChat.color = currentColor;
            }

        }
    }

    internal void ProcessServerMessage(ServerCodeTranslations _message)
    {
        switch (_message)
        {
            case ServerCodeTranslations.serverFull:
                errorConnectMessage.text = "Server full";
                break;
            case ServerCodeTranslations.invalidUsername:
                errorConnectMessage.text = "Username invalid";
                break;
            case ServerCodeTranslations.usernameTaken:
                errorConnectMessage.text = "Username taken";
                break;
            default:
                break;
        }
    }

    internal void ProcessClientMessage(ClientCodeTranslations _message)
    {
        switch (_message)
        {
            case ClientCodeTranslations.noError:
                errorConnectMessage.text = "";
                break;
            case ClientCodeTranslations.connectionRefused:
                errorConnectMessage.text = "Connection was refused";
                break;
            case ClientCodeTranslations.badForms:
                errorConnectMessage.text = "Forms unfilled";
                break;
            default:
                break;
        }
    }



    //Resets player list on disconnect
    public void ResetDictionary()
    {
        players = new Dictionary<int, PlayerManager>();
    }


}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class TimeCycle : MonoBehaviour
{
    public Light lght;
    [SerializeField] protected static int DayLenght = 3000;
    [SerializeField] protected static int NightLenght =3000;
    public List<GameObject> spawners;
    protected int GameTime = 0;
    protected bool isDay = false;
    protected bool fpd = true;
    int lightintensity;
    public GameObject sky1;
    public GameObject sky2;
    public float cloudSpeed;

    protected PlayerController player;

    //music
    /*public GameObject DaymusObj;
    public GameObject NightmusObj;
    public AudioSource DayM;
    public AudioSource NightM;
    public float vol = 0.2f;*/
    //////////////
    public GameObject Moon;
    float xrange = 14.0f / DayLenght;
    float yrange = 10.0f / DayLenght;
    Animator Moonanimator;

    public GameObject Totem1;
    public GameObject Totem2;
    public GameObject Totem3;
    public GameObject AnimTotem1;
    public GameObject AnimTotem2;
    public GameObject AnimTotem3;
    public GameObject ShadowTotem1;
    public GameObject ShadowTotem2;
    public GameObject ShadowTotem3;
    public enum NumOfDay
    {
        first, second, third
    }
    public NumOfDay NumDay;
    void Awake()
    {
        Moonanimator = Moon.GetComponent<Animator>();
        lght.intensity = 0;
        foreach (var spawner in spawners)
            spawner.SetActive(false);
        foreach (var line in GameStats.enemyOnScreen)
            foreach (var enemy in line)
                enemy.ReturnToBase();
    }
    // Start is called before the first frame update
    void Start()
    {


        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //NightM.volume = vol;
        //DayM.volume = 0;
    }

    //Раскомментировать для дебага - скип дня/ночи
    private void Update()
    { 
        if (Input.GetKeyDown(KeyCode.O))
        {
            GameTime = DayLenght + 1;
        }
    }

    void FixedUpdate()
    {

        GameTime += 1;

        if (isDay)
        {

            if (GameTime > DayLenght) //Change Day to Night
            {
                Moonanimator.SetInteger("IsDayInt", 0);
                isDay = false;
                GameTime = 0;
                if (GameStats.Encounter == 0)
                {
                    StartCoroutine(SetTotem1());
                }
                if (GameStats.Encounter == 1)
                {
                    StartCoroutine(SetTotem2());
                }
                if (GameStats.Encounter == 2)
                {
                    if(NumDay == NumOfDay.first)
                        PlayerPrefs.SetInt("Level", 2);
                    else if(NumDay == NumOfDay.second)
                        PlayerPrefs.SetInt("Level", 3);
                    PlayerPrefs.Save();
                    player.StartCoroutine(nameof(player.Dance));
                    StartCoroutine(SetTotem3());
                }
                foreach (var spawner in spawners)
                    spawner.SetActive(false);
                foreach (var line in GameStats.enemyOnScreen)
                    foreach (var enemy in line)
                        enemy.ReturnToBase();
                //Debug.Log("night");

                fpd = true;
                //StartCoroutine(SetNight());
                //StartCoroutine(moon.night());
            }
            if (fpd) // fpd - First Part Day (��������� ���������)
            {
                lght.intensity = 2 * ((float)GameTime / (float)DayLenght);
                if (GameTime > (DayLenght / 2))
                {
                    fpd = false;
                    //StartCoroutine(sun.night());

                }
            }
            if (!fpd) //��������� ���������
            {
                lght.intensity = 2 - 2 * ((float)GameTime / (float)DayLenght);
            }
        }
        else
        {

            if (GameTime > (NightLenght - (NightLenght / 10)))
            {

                //��� ���� ������� ����� �����, ������ �� ���, �� ����� ��� ����� �����������)

            }
            if (GameTime > NightLenght) // Change Night to Day
            {
                if (!player.GetIsBat() && !player.GetAtHome())
                {
                    //Debug.Log("turning");
                    player.StartCoroutine(nameof(player.TurnIntoBat));
                }

                isDay = true;
                Moonanimator.SetInteger("IsDayInt", 1);
                //Debug.Log("day");
                GameTime = 0;
                //GameStats.Encounter++;
                //StartCoroutine(sun.day());

                foreach (var spawner in spawners)
                {
                    spawner.SetActive(true);
                }
                //StartCoroutine(SetDay());
            }
        }
        // sky animations
        sky1.transform.position = new Vector3(sky1.transform.position.x + cloudSpeed, sky1.transform.position.y, sky1.transform.position.z);
        if (sky2.transform.position.x <= 0)
        {
            sky1.transform.position = new Vector3(0, sky1.transform.position.y, sky1.transform.position.z);
        }



        //Moon and Sun Control
        //Debug.Log(xrange);
        Moon.transform.position = new Vector3((float)(-7 + xrange * GameTime), (float)(10.0f*Mathf.Sin((Mathf.PI/3000)*(GameTime))), 0);
    }

    public bool GetIsDay()
    {
        return isDay;
    }

    /*private IEnumerator SetNight()
    {
        DayM.volume = vol;
        while (DayM.volume > 0.01f)
        {
            DayM.volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
        DayM.volume = 0.0f;
        NightM.volume = 0.0f;
        while (NightM.volume < vol)
        {
            NightM.volume += 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator SetDay()
    {

        NightM.volume = vol;
        while (NightM.volume > 0.01f)
        {
            NightM.volume -= 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
        NightM.volume = 0.0f;
        DayM.volume = 0.0f;
        while (DayM.volume < vol)
        {
            DayM.volume += 0.01f;
            yield return new WaitForSeconds(0.1f);
        }
    }*/
    private IEnumerator SetTotem1()
    {
        AnimTotem1.SetActive(true);
        yield return new WaitForSeconds(2);
        AnimTotem1.SetActive(false);
        //ShadowTotem1.SetActive(false);
        Totem1.SetActive(true);
        GameStats.Encounter++;
    }
    private IEnumerator SetTotem2()
    {
        AnimTotem2.SetActive(true);
        yield return new WaitForSeconds(2);
        AnimTotem2.SetActive(false);
        //ShadowTotem2.SetActive(false);
        Totem2.SetActive(true);
        GameStats.Encounter++;
    }
    private IEnumerator SetTotem3()
    {
        AnimTotem3.SetActive(true);
        yield return new WaitForSeconds(2);
        AnimTotem3.SetActive(false);
        //ShadowTotem3.SetActive(false);
        Totem3.SetActive(true);
        yield return new WaitForSeconds(1);
        GameStats.Encounter++;
    }
    

}

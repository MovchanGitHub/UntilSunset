using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CnControls;

public class PlayerController : MonoBehaviour
{
    public float xSpeed = 1.5f;
    public float ySpeed = 1.25f;

    //public float timeInvincible = 2.0f;

    public Rigidbody2D rigidbBody2D;
    Vector2 moveDelta;

    public Animator animator;

    public bool isBat;
    public bool isTurning;

    public bool atHome;

    public TimeCycle timeCycle;

    public GameObject Bat;

    private Resources HenchmanRes;
    private Resources resources;

    private int sotringOrderBase = 1000;
    private SortingGroup mySortingGroup;
    private int batOffset = 0;

    private float positionRendererTimer;
    private float positionRendererTimerMax = .1f;

    private Vector3 batSpawnPosition;
    public static int henchmanLine;

    public GameObject nimb;
    public bool isGod;

    private int coinAmount;
    private int woodAmount;
    private int stoneAmount;
    private int henchmanAmount;

    private AudioSource source;
    public AudioClip[] walksounds;
    public AudioClip CHypnosis;
    private bool isWalking;
    private bool soundIsPlaying;
    private bool onTheWay;
    public bool isTutorial;
    private bool[] sIsPlaying;
    private bool isFlapping;
    private bool isDancing;

    public bool isLeaving;

    private float batXSpeed = 10f;
    private float batYSpeed = 8f;

    //private int henchmanLine;
    protected virtual void Awake()
    {
        mySortingGroup = gameObject.GetComponent<SortingGroup>();
        rigidbBody2D = GetComponent<Rigidbody2D>();
        HenchmanRes = GameObject.Find("HenchmenText").GetComponent<Resources>();
        timeCycle = GameObject.Find("GameStatsObject").GetComponent<TimeCycle>();
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
        source = gameObject.GetComponent<AudioSource>();
        animator.SetFloat("LastVertical", -1);
    }

    protected virtual void Start()
    {
        isBat = false;
        atHome = true;
        isWalking = false;
        onTheWay = false;
        soundIsPlaying = false;
        sIsPlaying = new bool[] { false, false, false };
        isFlapping = false;
        isDancing = false;

#if UNITY_ANDROID
        batXSpeed = 6f;
        batYSpeed = 5f;
#endif

    //SetGodSettings();
}

    protected virtual void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.J))
            StartCoroutine(nameof(Dance));*/

        if (isTurning || isLeaving || isDancing) return;

        Turning();

        if (isBat || !timeCycle.GetIsDay()) return;

        SpawnBat();
    }

    protected virtual void FixedUpdate()
    {
        if (isTurning || isLeaving || isDancing) return;

        UpdateMotor();
        PlayWalkSound();
    }

    private void LateUpdate()
    {
        SetSortingLayer();
    }

    private void SetSortingLayer()
    {
        positionRendererTimer -= Time.deltaTime;
        if (positionRendererTimer <= 0f)
        {
            positionRendererTimer = positionRendererTimerMax;
            mySortingGroup.sortingOrder = (int)(sotringOrderBase - transform.position.y * 10 - batOffset);
        }
    }

    public void Turning()
    {
        if ((Input.GetButtonDown("Jump")|| PauseMenu.TurningClick) && (atHome || !timeCycle.GetIsDay()))
        {
            if (!isBat)
            {
                StartCoroutine(nameof(TurnIntoBat));
            }
            else
            {
                StartCoroutine(nameof(TurnIntoCharacter));
            }
        }
    }

    public IEnumerator TurnIntoBat()
    {
        while (isTurning)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (!isBat)
        {
            isTurning = true;
            isBat = true;
            animator.SetBool("IsBat", true);
            yield return new WaitForSeconds(0.5f);
            SetBatSettings();
        }
    }

    private IEnumerator TurnIntoCharacter()
    {
        while (isTurning)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (isBat)
        {
            isTurning = true;
            isBat = false;
            animator.SetBool("IsBat", false);
            yield return new WaitForSeconds(0.5f);
            SetCharacterSettings();
        }
    }

    private void SetBatSettings()
    {
        animator.Play("Bat");
        isTurning = false;
        isBat = true;
        xSpeed = batXSpeed;
        ySpeed = batYSpeed;
    }

    public void SetCharacterSettings()
    {
        animator.SetFloat("LastHorizontal", 0);
        animator.SetFloat("LastVertical", -1);
        animator.Play("Idle");
        isTurning = false;
        isBat = false;
        xSpeed = 1.5f;
        ySpeed = 1.25f;
    }

    public IEnumerator Dance()
    {
        isLeaving = false;
        isDancing = true;

        while (isTurning)
        {
            yield return new WaitForSeconds(0.1f);
        }

        if (isBat)
        {
            isTurning = true;
            isBat = false;
            animator.Play("ToCharacter");
            yield return new WaitForSeconds(0.5f);
            SetCharacterSettings();
        }

        animator.Play("Dancing");
    }

    public void ActivateCheat()
    {
        if (isGod)
            UnsetGodSettings();
        else
            SetGodSettings();
    }

    protected void SetGodSettings() 
    {
        isGod = true;
        nimb.SetActive(true);
        coinAmount = GameStats.Coins;
        woodAmount = GameStats.Wood;
        stoneAmount = GameStats.Stone;
        henchmanAmount = GameStats.Henchman;
        GameStats.Coins = 666;
        GameStats.Wood = 666;
        GameStats.Stone = 666;
        GameStats.Henchman = 666;
        resources.UpdateAll();
    }

    public void UnsetGodSettings()
    {
        isGod = false;
        nimb.SetActive(false);
        GameStats.Coins = coinAmount;
        GameStats.Wood = woodAmount;
        GameStats.Stone = stoneAmount;
        GameStats.Henchman = henchmanAmount;
        resources.UpdateAll();
    }

    private void UpdateMotor()
    {
        Vector3 movement = new Vector3(CnInputManager.GetAxisRaw("Horizontal"), CnInputManager.GetAxisRaw("Vertical"), 0f);
        float x = movement.x;// Input.GetAxisRaw("Horizontal");
        float y = movement.y;// Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0) isWalking = true;
        else isWalking = false;

        moveDelta = new Vector2(x * xSpeed, y * ySpeed);

        animator.SetFloat("Horizontal", x);
        animator.SetFloat("Vertical", y);
        animator.SetFloat("Speed", moveDelta.sqrMagnitude);

        if (x != 0 || y != 0)
        {
            animator.SetFloat("LastHorizontal", x);
            animator.SetFloat("LastVertical", y);
        }

        transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, 0);
    }

    public bool GetIsBat()
    {
        return isBat;
    }

    public bool GetIsDancing()
    {
        return isDancing;
    }

    public bool GetAtHome()
    {
        return atHome;
    }

    private void CalculateBatSpawnPosition()
    {
        batSpawnPosition = transform.position;
        batSpawnPosition.y = henchmanLine - 1;
    }

    private void GetLineForSpawnBat()
    {
        if (transform.position.y > -1.4 && transform.position.y < 2.5)
        {
            henchmanLine = (int)System.Math.Round(transform.position.y);
        }
        else henchmanLine = 0;
    }
    public void SpawnBat()
    {
        if ((Input.GetKeyDown(KeyCode.E) || PauseMenu.SpawnClick) && GameStats.Henchman >= 3)
        {
            GetLineForSpawnBat();
            if (GameStats.henchmanOnScreen[henchmanLine] == 0)
            {
                isTurning = true;
                animator.Play("InvokeHenchman");
                Invoke(nameof(SetCharacterSettings), 0.2f);
                CalculateBatSpawnPosition();
                Instantiate(Bat, batSpawnPosition, Quaternion.identity);
                GameStats.Henchman -= 3;
                HenchmanRes.UpdateHenchman();
                GameStats.henchmanOnScreen[henchmanLine] = 1;
            }
        }
    }

    public void EnterMainBuilding()
    {
        atHome = true;
    }

    public void ExitMainBuilding()
    {
        atHome = false;

        if (!isBat && timeCycle.GetIsDay())
        {
            StartCoroutine(nameof(TurnIntoBat));
        }
    }

    public void SubdueEnemy(EnemyCharacter enemy)

    {
        if (GameStats.Henchman >= 5 && !enemy.IsFriend() 
            && !isBat && timeCycle.GetIsDay() && !isTurning)
        {
            enemy.IsFriendMakeTrue();
            isTurning = true;
            animator.Play("Magic");
            GameObject.Find("ResSounds").GetComponent<AudioSource>().PlayOneShot(CHypnosis);
            StartCoroutine(ThunderZoneActivate(enemy));
        }
    }

    public void LoseSubduingBlood()
    {
        GameStats.Henchman -= 5;
        resources.UpdateHenchman();
    }

    private IEnumerator ThunderZoneActivate(EnemyCharacter enemy)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        if (enemy != null)
            enemy.BecomeFriend();
        SetCharacterSettings();
    }

    private void PlayWalkSound()
    {
        if (isWalking && !isBat && !isTurning)
        {//0 2.6
            isFlapping = false;

            if (transform.position.y > 0 && transform.position.y < 2.6)
                onTheWay = true;
            else onTheWay = false;

            if (atHome && !sIsPlaying[0])
            {
                source.clip = walksounds[0];
                sIsPlaying = new bool[] { false, false, false };
                sIsPlaying[0] = true;
                source.Play();
            }
            else if (onTheWay && !atHome && !sIsPlaying[2])
            {
                source.clip = walksounds[2];
                sIsPlaying = new bool[] { false, false, false };
                sIsPlaying[2] = true;
                source.Play();
            }
            else if (!onTheWay && !atHome && !sIsPlaying[1])
            {
                source.clip = walksounds[1];
                sIsPlaying = new bool[] { false, false, false };
                sIsPlaying[1] = true;
                source.Play();
            }

            if (!soundIsPlaying)
            {
                soundIsPlaying = true;
                source.Play();
            }
        }
        else if (isBat)
        {
            if (!isFlapping)
            {
                isFlapping = true;
                source.Stop();
                source.clip = walksounds[3];
                source.Play();
                sIsPlaying = new bool[] { false, false, false };
            }
        }
        else
        {
            isFlapping = false;
            source.Stop();
            soundIsPlaying = false;
            sIsPlaying = new bool[] { false, false, false };
        }
        
    }

    public void ReturnRight()
    {
        isLeaving = true;

        animator.SetFloat("Speed", 1);
        animator.SetFloat("Horizontal", 1);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("LastHorizontal", 1);
        animator.SetFloat("LastVertical", 0);

        StartCoroutine(GoRight());
    }

    protected virtual IEnumerator GoRight()
    {
        while (transform.position.x < -14.5 && isLeaving)
        {
            transform.Translate(isBat ? 0.04f : 0.02f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }

        isLeaving = false;
    }

    public void ReturnLeft()
    {
        isLeaving = true;

        animator.SetFloat("Speed", 1);
        animator.SetFloat("Horizontal", -1);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("LastHorizontal", -1);
        animator.SetFloat("LastVertical", 0);

        StartCoroutine(GoLeft());
    }

    protected virtual IEnumerator GoLeft()
    {
        while (transform.position.x > 14.5 && isLeaving)
        {
            transform.Translate(isBat ? -0.04f : -0.02f, 0, 0);
            yield return new WaitForSeconds(0.01f);
        }

        isLeaving = false;
    }

    public void SetOnTheWay(bool p) => onTheWay = p;

    public void PauseWalkSound()
    {
        source.loop = false;
        source.Pause();
    }

    public void ContinueWalkSound()
    {
        source.loop = true;
        source.Play();
    }
}

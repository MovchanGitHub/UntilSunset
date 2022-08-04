using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ResourceScript : MonoBehaviour
{
    private PlayerController pl;
    protected double DTime;
    public double DTimeMax;
    protected double DTimeSpriteMax;
    protected double DTimeSprite;
    protected int spInd;
    public int resLim;
    public int res;
    public Sprite[] sp;
    public GameObject resInd;
    private ResourceIndicator resIndComponent;
    protected SpriteRenderer resSp;
    protected AudioSource source;
    private AudioSource sRemove;
    public AudioClip collectSound;
    protected Resources resources;
    public AudioClip CRemove;
    private bool isRemoved = false;

    private void Awake()
    {
        pl = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
        resIndComponent = resInd.GetComponent<ResourceIndicator>();
        resSp = resInd.GetComponent<SpriteRenderer>();
        source = GetComponent<AudioSource>();
        sRemove = GameObject.Find("ResSounds").GetComponent<AudioSource>();
        resInd.SetActive(false);
#if UNITY_ANDROID
        SetPEValues(); // for Pocket Edition
#endif
    }

    protected virtual void Start()
    {
        DTime = DTimeMax;
        res = resLim;

        DTimeSpriteMax = resLim * DTimeMax / 32; // sp.Length = 29
        DTimeSprite = DTimeSpriteMax;
        spInd = 0;

        source.volume = 0.5f;
    }

    protected virtual void Update()
    {
        if (resIndComponent.isMousePressed && res > 0)
        {
            DTime -= Time.deltaTime;
            if (DTime <= 0)
                CollectItem();

            DTimeSprite -= Time.deltaTime;
            if (DTimeSprite <= 0)
                AdjustIndicator();
        }

        if (res == 0 && !isRemoved)
            ReadyToDie();
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            if (pl.GetIsBat() || pl.GetAtHome() || pl.GetIsDancing())
            {
                resIndComponent.isMousePressed = false;
                resInd.SetActive(false);
            }
            else
                resInd.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            resInd.SetActive(false);
            resIndComponent.isMousePressed = false;
        }
    }

    private void ReadyToDie()
    {
        resSp.sprite = sp[sp.Length - 1];
        isRemoved = true;
        Invoke(nameof(ObjectDie), 0.5f);
        sRemove.PlayOneShot(CRemove, 0.7f);
    }

    protected virtual void AdjustIndicator()
    {
        DTimeSprite = DTimeSpriteMax;
        spInd++;
        if (spInd < sp.Length - 1)
            resSp.sprite = sp[spInd];
    }

    protected virtual void CollectItem()
    {
        DecreaseResource();
        source.PlayOneShot(collectSound, 0.5f);
        DTime = DTimeMax;
    }

    protected virtual void DecreaseResource()
    {
        res--;
    }

    protected virtual void ObjectDie()
    {
        Destroy(gameObject);
    }

    protected void RenewResource()
    {
        isRemoved = false;
        res = resLim;
        spInd = 0;
        resSp.sprite = sp[spInd];
        DTimeSprite = DTimeSpriteMax;
        DTime = DTimeMax;
    }

    protected virtual void SetPEValues()
    {
        resLim *= 2;
        DTimeMax *= 0.65f;
        resInd.transform.localScale *= 1.85f;
    }
}

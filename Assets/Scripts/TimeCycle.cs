using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCycle : MonoBehaviour
{
    public Light lght;
    int DayLenght = 5000;
    int NightLenght = 5000;
    public GameObject spawner;
    public Text TimeTXT;
    public GameObject newwave;
    public newwave nw;
    int GameTime = 0;  
    bool isDay = true;
    bool fpd = true;
    int lightintensity;
    public GameObject sky1;
    public GameObject sky2;
    public float cloudSpeed;


    // Start is called before the first frame update
    void Start()
    {
        //spawner.SetActive(false);
        //newwave.SetActive(false);
    }

    void FixedUpdate()
    {
       
        GameTime += 1;
        TimeTXT.text = GameTime.ToString();
        
        if (isDay)
        {
            
            if (GameTime > DayLenght)
            {
                isDay = false;
                GameTime = 0;
                spawner.SetActive(false);
                fpd = true;
            }
            if (fpd)
            {
                lght.intensity = 2*((float)GameTime / (float)DayLenght) ;
                if (GameTime> (DayLenght / 2))
                {
                    fpd = false;
                    
                }
            }
            if(!fpd)
            {
                lght.intensity = 2 - 2*((float)GameTime / (float)DayLenght);
            }
        }
        else
        {
            if (GameTime > (NightLenght- (NightLenght / 10)))
            {
                nw.time = 200;
                newwave.SetActive(true);
                
            }
            if (GameTime > NightLenght)
            {
                isDay = true;
                GameTime = 0;
                spawner.SetActive(true);
            }
        }

        sky1.transform.position = new Vector3(sky1.transform.position.x + cloudSpeed, sky1.transform.position.y, sky1.transform.position.z);
        if (sky2.transform.position.x <= 0)
        {
            sky1.transform.position = new Vector3(0, sky1.transform.position.y, sky1.transform.position.z);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ShieldControol : MonoBehaviour
{
    public GameObject LeftNormalShield;
    public GameObject LeftCoolShield;
    public GameObject RightNormalShield;
    public GameObject RightCoolShield;
    private Image shieldl;
    private Image shieldr;
    public GameManager gameManager;

    public GameObject L1Shield;
    public GameObject L2Shield;
    public GameObject L3Shield;
    public GameObject R1Shield;
    public GameObject R2Shield;
    public GameObject R3Shield;

    public bool canUseShield = true; //add this to shield active trigger
    public float ShieldCoolDownTime = 1f;
    private float ShieldCoolDownTimerl = 0f ;
    private float ShieldCoolDownTimerr = 0f;



    void Start()
    {
        GetComponent<GameManager>();
        //Where is the shield cool down or refresh time?
        //ShieldCoolDownTime = gameManager.NewRoundTime;

        LeftNormalShield.SetActive(false);
        LeftCoolShield.SetActive(false);
        RightNormalShield.SetActive(false);
        RightCoolShield.SetActive(false);

        shieldl = LeftCoolShield.GetComponent<Image>();
        shieldr = RightCoolShield.GetComponent<Image>();
        shieldl.fillAmount = 0;
        shieldr.fillAmount = 0;

    }


    void Update()
    {
        if (L1Shield.activeSelf == true || L2Shield.activeSelf == true || L3Shield.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                canUseShield = false;
                ShieldCoolDownTimerl = 0;
            }
                // ShieldCoolDownTimer = 0;
                LeftNormalShield.SetActive(true);
            LeftCoolShield.SetActive(true);

            if(canUseShield == false)
            {
                ShieldCoolDownTimerl += Time.deltaTime;
                shieldl.fillAmount =  (ShieldCoolDownTime - ShieldCoolDownTimerl) / ShieldCoolDownTime;

                if (ShieldCoolDownTimerl >= ShieldCoolDownTime)
                {
                    canUseShield = true;
                    ShieldCoolDownTimerl = 0;
                }


            }

        }
        else
        {
            LeftNormalShield.SetActive(false);
            LeftCoolShield.SetActive(false);
        }

        if (R1Shield.activeSelf == true || R2Shield.activeSelf == true || R3Shield.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                canUseShield = false;
                ShieldCoolDownTimerl = 0;
            }
            // ShieldCoolDownTimer = 0;
            RightNormalShield.SetActive(true);
            RightCoolShield.SetActive(true);

            if (canUseShield == false)
            {
                ShieldCoolDownTimerr += Time.deltaTime;
                 shieldr.fillAmount = (ShieldCoolDownTime - ShieldCoolDownTimerr) / ShieldCoolDownTime;
                //shieldr.fillAmount = 0.5f;
                if (ShieldCoolDownTimerr >= ShieldCoolDownTime)
                {
                    canUseShield = true;
                    ShieldCoolDownTimerr = 0;
                }


            }
        }
        else
        {
            RightNormalShield.SetActive(false);
            RightCoolShield.SetActive(false);
        }
    }
}

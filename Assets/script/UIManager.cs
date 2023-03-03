using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Runtime.InteropServices;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public VirtualJoystick virtualJoystick;
    public GameObject ItemPanel;

    public Image ItemImage;
    public TMP_Text ItemName;
    public TMP_Text ItemDescription;
    public TMP_Text ItemPrice;

    public bool IsUIOpen = false;

    ProductUIRender productRender;

    public RawImage productRenderedImage;

    private string web_itemName = "";
    private string web_itemDescription ="";
    private string web_itemPrice ="";

    private bool mute = false;


    [DllImport("__Internal")]
    private static extern void CartTime(string id);


    [DllImport("__Internal")]
    private static extern void RequestItem(string id);

    string selectedItemID = "";

    private static UIManager main;
    public static UIManager Main
    {
        get
        {
            if (main == null)
            {
                main = FindObjectOfType<UIManager>();
            }
            return main;
        }
    }

    public void OnClick(string eventName)
    {
        if (eventName == "Close")
        {
            StartCoroutine(productRender.EnsureReturnToLayer());

            if (isDoneRecursiveLayer)
            {
                productRender.gameObject.SetActive(false);
                selectedItemID = "";
                web_itemName = "";
                web_itemDescription = "";
                web_itemPrice = "";
                ItemPanel.SetActive(false);
            }

        }

        if (eventName == "AddToCart")
        {
            AddToCart();
        }
    }
  

    public void AddToCart()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                    CartTime(selectedItemID);
#endif

        Debug.Log(selectedItemID + " Added to cart");
    }

    public void DisplayItemDiscription(Collider collider)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
                    RequestItem(collider.name);
#endif


        selectedItemID = collider.name;

        ItemPanel.SetActive(true);

        productRender.gameObject.SetActive(true);
        ProductUIRender.Main.MoveTowardsProduct(collider);
    }

    public void SetItemPrice(string value)
    {
        web_itemPrice = value;
    }

    //public void SetIsMobile(bool value)
    //{
    //    StoreManager.Main.IsRunningOnMobile = value;
    //    Debug.Log(StoreManager.Main.IsRunningOnMobile + " == unity value");
    //}

    //public void SetIsMobileString(string value)
    //{
    //    if (value == "true")
    //    {
    //        StoreManager.Main.IsRunningOnMobile = true;
    //    }
    //    else
    //    {
    //        StoreManager.Main.IsRunningOnMobile = false;
    //    }
    //    Debug.Log("Called this");
    //}

    public bool GetBool(string name)
    {
        if (name == "Mute")
        {
            return mute;
        }
            

        return false;
    }

    public bool Set(string name, bool value)
    {
        if (name == "Mute")
        {
            mute = value;

            if (AudioManager.Main == null)
            {
                Debug.Log("Audio Manager is missing");
                return mute;
            }
            if (mute) AudioManager.Main.Mute("BackgroundAudio"); else AudioManager.Main.Unmute("BackgroundAudio");
            return mute;
        }

        return false;
    }

    public void SetItemDescription(string value)
    {
        web_itemDescription = value;
    }

    public void SetItemName(string value)
    {
        web_itemName = value;
    }

    public bool isDisabled(string name)
    {
        return false;
    }

    void Awake()
    {
        if (productRender == null)
            productRender = FindObjectOfType<ProductUIRender>(true);

        if (productRender != null)
            productRender.gameObject.SetActive(false);
    }


    void Start()
    {
        ItemPanel.SetActive(false);

        StartCoroutine(LateStart());

        if (productRenderedImage == null)
        {
            productRenderedImage = GetComponentInChildren<RawImage>(true);
        }

        if (productRenderedImage != null)
        {
            productRenderedImage.gameObject.AddComponent<EventTrigger>();
            EventTrigger.Entry pointerDrag = new EventTrigger.Entry
            {
                eventID = EventTriggerType.Drag
            };
            pointerDrag.callback.AddListener((e) => RotateUIRenderedProduct((PointerEventData)e));
            productRenderedImage.gameObject.GetComponent<EventTrigger>().triggers.Add(pointerDrag);
        }
    }

    private void RotateUIRenderedProduct(PointerEventData e)
    {
        productRender.RotateCameraAroundTarget(e.delta);
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(1.8f);
        virtualJoystick.gameObject.SetActive(true);

        if (StoreManager.Main.IsRunningOnMobile)
        {
            virtualJoystick.gameObject.transform.localScale = new Vector3(5, 5, 5);
            RectTransform vJoyRect = virtualJoystick.gameObject.GetComponent<RectTransform>();
            vJoyRect.anchorMin = new Vector2(0.5f, 0);
            vJoyRect.anchorMax = new Vector2(0.5f, 0);
            vJoyRect.pivot = new Vector2(0, 0);
            vJoyRect.anchoredPosition = new Vector2(vJoyRect.anchoredPosition.x - 180, vJoyRect.anchoredPosition.y + 200);

            ItemPanel.transform.localScale = new Vector3(1.6f, 1.6f, 1.6f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (ItemPanel.activeInHierarchy)
        {
            IsUIOpen = true;
            ItemName.text = web_itemName;
            ItemDescription.text = web_itemDescription;
            ItemPrice.text = web_itemPrice;
        }
        else
        {
            IsUIOpen = false;
        }
    }

    public static bool isDoneRecursiveLayer = false;

    public static void SetLayerRecursively(Transform parent, int layer)
    {
        isDoneRecursiveLayer = false;
        parent.gameObject.layer = layer;

        for (int i = 0, count = parent.childCount; i < count; i++)
        {
            SetLayerRecursively(parent.GetChild(i), layer);
        }

        isDoneRecursiveLayer = true;
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class StoreManager : MonoBehaviour
{
    private static StoreManager main;

    public bool IsRunningOnMobile = false;
    public bool DeviceSupportGyro = false;

    public Quaternion GyroQuaternion;

    public static StoreManager Main
    {
        get
        {
            if (main == null)
            {
                main = FindObjectOfType<StoreManager>();
            }
            return main;
        }
    }


    //public void SetIsMobile (bool value)
    //{
    //    IsRunningOnMobile = value;
    //    Debug.Log(IsRunningOnMobile + " == unity value");
    //}

    public void SetIsMobileString (string value)
    {
        if (value == "true")
        {
            IsRunningOnMobile = true;
        }
        else
        {
            IsRunningOnMobile = false;
        }
    }

    public void SetDeviceSupportGyro(bool value)
    {
        DeviceSupportGyro = value;
    }

    public void GetDeviceZXYQuaternion(string value)
    {
        string[] rot = value.Split(';');

        SetNewRotation(float.Parse(rot[0]), float.Parse(rot[1]), float.Parse(rot[2]), float.Parse(rot[3]));
    }

    public void SetNewRotation(float w, float x, float y, float z)
    {
        //cam.transform.rotation = Quaternion.Inverse( new Quaternion(x, y, z, w));
        Quaternion q = new Quaternion(x, y, -z, -w);
        GyroQuaternion = q;
        //cam.transform.rotation = q;
        //cam.transform.Rotate(90f, 0f, 0f, Space.World);
    }
}

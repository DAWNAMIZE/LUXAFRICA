using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductUIRender : MonoBehaviour
{
    private static ProductUIRender main;
    public static ProductUIRender Main
    {
        get
        {
            if (main == null)
            {
                main = FindObjectOfType<ProductUIRender>();
            }
            return main;
        }
    }

    public Camera productCamera;

    int tempLayer;
    Collider tempCollider;

    private float rotationY;
    private float rotationX;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity;

    private float distanceFromProduct;

    private void Awake()
    {
        productCamera = GetComponentInChildren<Camera>();
    }


    public void MoveTowardsProduct(Collider collider)
    {
        //this.transform.position = collider.transform.position;
        tempCollider = collider;
        tempLayer = collider.transform.gameObject.layer;

        productCamera.transform.localEulerAngles = Vector3.zero;

        float cameraDistance = 2.0f; // Constant factor
        Vector3 objectSizes = collider.bounds.max - collider.bounds.min;
        float objectSize = Mathf.Max(objectSizes.x, objectSizes.y, objectSizes.z);
        float cameraView = 2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * productCamera.fieldOfView); // Visible height 1 meter in front
        float distance = cameraDistance * objectSize / cameraView; // Combined wanted distance from the object
        distance += 0.5f * objectSize; // Estimated offset from the center to the outside of the object

        productCamera.orthographicSize = (objectSize / 2) + .5f;

        distanceFromProduct = distance;
        productCamera.transform.position = collider.bounds.center - distance * productCamera.transform.forward;



        UIManager.SetLayerRecursively(collider.transform, 5);
    }

    public void OnDisable()
    {
        if (tempCollider != null)
             UIManager.SetLayerRecursively(tempCollider.transform, tempLayer);
    }


    public IEnumerator EnsureReturnToLayer()
    {
        UIManager.SetLayerRecursively(tempCollider.transform, tempLayer);

        yield return UIManager.isDoneRecursiveLayer;
    }

    public void RotateCameraAroundTarget(Vector2 drag)
    {
        rotationY += drag.x;
        rotationX += drag.y;

        rotationX = Mathf.Clamp(rotationX, -40, 40);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, 0.03f);

        productCamera.transform.localEulerAngles = currentRotation;


       productCamera.transform.position = tempCollider.bounds.center - productCamera.transform.forward * distanceFromProduct;



    }

}

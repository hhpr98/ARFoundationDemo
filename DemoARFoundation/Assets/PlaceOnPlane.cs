using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using UnityEngine.EventSystems;

public class PlaceOnPlane : MonoBehaviour
{
    private ARSessionOrigin sessionOrigin;
    private List<ARRaycastHit> hits;

    public GameObject model;
    public GameObject canvas;


    // Start is called before the first frame update
    void Start()
    {
        sessionOrigin = GetComponent<ARSessionOrigin>();
        hits = new List<ARRaycastHit>();

        model.SetActive(false);
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (sessionOrigin.Raycast(touch.position, hits, UnityEngine.Experimental.XR.TrackableType.PlaneWithinPolygon) && IsPointerOverUIObject(touch.position))
        {
            Pose pose = hits[0].pose;

            if (!model.activeInHirerarchy)
            {
                model.SetActive(true);
                model.transform.position = pose.position;
                model.transform.rotation = pose.rotation;

                canvas.SetActive(true);
            }
            else
            {
                model.transform.position = pose.position;
            }
        }    

    }

    bool IsPointerOverUIObject(Vector2 pos)
    {
        if (EventSystem.current == null)
        {
            return false;
        }
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(pos.x, pos.y);
        List<RayCastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0 ? true : false;
    }
}

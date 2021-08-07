using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PadAct : MonoBehaviour
{
    public Material Pad_untouch, Pad_touch;

    int padNumber;

    public int nextNoteNo;

    Camera cam;//主相機(Camera)
    LayerMask mask;//圖層遮罩
    public int status;//按鍵狀態(0 = 為=未按下, 1 = 剛按下的瞬間,2 = 按住中,3 = 左滑,4 = 右滑,5 = 上滑)
    int platform;//程式運行的平台

    float lastTouchY;
    bool touched;

    public bool Touchend;
    int lastStatus, TouchId;

    float lastMouseY;
    void Start()
    {
        TouchId = -1;
        Touchend = false;
        touched = false;

        nextNoteNo = -1;
        padNumber = transform.GetSiblingIndex();
        lastMouseY = 0;
        cam = Camera.main;
        mask = 1 << (LayerMask.NameToLayer("Pad")) | 1 << (LayerMask.NameToLayer("UI"));
        status = 0;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            platform = 0;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            platform = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (platform == 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (status == 1 || status == 2 || status == 5)
            {
                if (Physics.Raycast(ray, out RaycastHit H, Mathf.Infinity, mask.value))
                {
                    if (H.collider.name == this.name)
                    {
                        if (Input.GetMouseButton(0))
                        {
                            if (Input.mousePosition.y - lastMouseY >= 10)
                            {
                                status = 5;
                                lastMouseY = Input.mousePosition.y;
                            }
                            else
                            {
                                lastMouseY = Input.mousePosition.y;
                                status = 2;
                            }

                        }
                        else
                        {
                            status = 0;
                        }
                    }
                    else
                    {
                        int LeftPadNo = padNumber - 1;
                        int RightPadNo = padNumber + 1;

                        bool slide = false;

                        if (LeftPadNo >= 0)
                        {
                            GameObject LeftPad = transform.parent.gameObject.transform.GetChild(LeftPadNo).gameObject;
                            if (H.collider.name == LeftPad.name)
                            {
                                status = 3;
                                slide = true;
                                //Debug.Log(name + ":" + "LeftSlide!");
                            }
                        }
                        if (RightPadNo < transform.parent.childCount && !slide)
                        {

                            GameObject RightPad = transform.parent.gameObject.transform.GetChild(RightPadNo).gameObject;
                            if (H.collider.name == RightPad.name)
                            {
                                status = 4;
                                slide = true;
                                //Debug.Log(name + ":" + "RightSlide!");
                            }
                        }

                        if (!slide) status = 0;
                    }
                }
                else
                {
                    status = 0;
                }
            }
            else
            {
                if (Physics.Raycast(ray, out RaycastHit Hit, Mathf.Infinity, mask.value))
                {
                    if (Hit.collider.name == this.name)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            lastMouseY = Input.mousePosition.y;
                            status = 1;
                            //Debug.Log(MC.GetComponent<Conductor>().songPosition);
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            if (Input.mousePosition.y - lastMouseY >= 10)
                            {
                                status = 5;
                                lastMouseY = Input.mousePosition.y;
                            }
                            else
                            {
                                lastMouseY = Input.mousePosition.y;
                                status = 2;
                            }
                        }
                    }
                    else
                    {
                        status = 0;
                    }
                }
                else
                {
                    status = 0;
                }
            }


        }
        else if (platform == 1)
        {
            int TouchCount;
            TouchCount = Input.touchCount;
            if (TouchCount > 0)
            {
                for (int i = 0; i < TouchCount; i++)
                {
                    Touch t = Input.GetTouch(i);
                    Ray ray = cam.ScreenPointToRay(t.position);
                    if (Physics.Raycast(ray, out RaycastHit Hit, Mathf.Infinity, mask.value))
                    {
                        if (Hit.collider.name == name)
                        {
                            TouchId = t.fingerId;

                            if (t.phase == TouchPhase.Began)
                            {
                                lastTouchY = t.position.y;
                                status = 1;
                                touched = true;
                                break;
                            }
                            else if (t.phase == TouchPhase.Stationary)
                            {
                                lastTouchY = t.position.y;
                                status = 2;
                                touched = true;
                                break;
                            }
                            else if (t.phase == TouchPhase.Moved)
                            {
                                t = Input.GetTouch(i);
                                ray = cam.ScreenPointToRay(t.position);

                                if (Physics.Raycast(ray, out Hit, Mathf.Infinity, mask.value))
                                {
                                    if (Hit.collider.name == name)
                                    {
                                        if (t.position.y - lastTouchY >= 10)
                                        {
                                            lastTouchY = t.position.y;
                                            status = 5;
                                            touched = true;
                                            break;
                                        }
                                        else
                                        {
                                            status = 2;
                                            touched = true;
                                            break;
                                        }
                                    }
                                }
                            }
                            else if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
                            {
                                touched = false;
                            }
                        }
                    }
                }

                if (!touched)
                {
                    if (status == 1 || status == 2 || status == 5)
                    {
                        bool slide = false;

                        if (TouchId != -1)
                        {
                            Touch lastT = Input.GetTouch(TouchId);
                            Ray ray = cam.ScreenPointToRay(lastT.position);
                            if (Physics.Raycast(ray, out RaycastHit Hit, Mathf.Infinity, mask.value))
                            {
                                int LeftPadNo = padNumber - 1;
                                int RightPadNo = padNumber + 1;

                                if (LeftPadNo >= 0)
                                {
                                    GameObject LeftPad = transform.parent.gameObject.transform.GetChild(LeftPadNo).gameObject;
                                    if (Hit.collider.name == LeftPad.name)
                                    {
                                        status = 3;
                                        Debug.Log(name + " LeftSlide!");
                                        slide = true;
                                    }
                                }
                                if (RightPadNo < transform.parent.childCount)
                                {
                                    GameObject RightPad = transform.parent.gameObject.transform.GetChild(RightPadNo).gameObject;
                                    if (Hit.collider.name == RightPad.name)
                                    {
                                        status = 4;
                                        Debug.Log(name + "RightSlide!");
                                        slide = true;
                                    }
                                }
                            }
                        }
                        if (!slide) status = 0;
                        else TouchId = -1;
                    }
                    else
                    {
                        status = 0;
                    }
                }
                else
                {
                    touched = false;
                }
            }
            else
            {
                status = 0;
            }
        }
        if (status == 0 && lastStatus != 0) Touchend = true;
        else Touchend = false;

        lastStatus = status;



        switch (status)
        {
            case 0:
                gameObject.GetComponent<Renderer>().material = Pad_untouch;
                break;
            case 1:
            case 2:
            case 5:
                gameObject.GetComponent<Renderer>().material = Pad_touch;
                break;
        }
    }
}

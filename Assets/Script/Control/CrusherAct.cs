using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CrusherAct : MonoBehaviour
{
    public Material untouch, touch;

    public int nextNoteNo;

    Camera cam;//�D�۾�(Camera)
    LayerMask mask;//�ϼh�B�n
    public int status;//���䪬�A(0 = ��=�����U, 1 = ����U������,2 = ����,3 = ����,4 = �k��,5 =)
    int platform;//�{���B�檺���x


    float lastMouseX, lastTouchX;
    bool touched;

    public bool Touchend;
    int lastStatus;
    void Start()
    {
        touched = false;

        nextNoteNo = -1;
        cam = Camera.main;
        mask = 1 << (LayerMask.NameToLayer("Crusher"));

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

                if (Physics.Raycast(ray, out RaycastHit Hit, Mathf.Infinity, mask.value))
                {
                    if (Hit.collider.name == this.name)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            lastMouseX = Input.mousePosition.x;
                            status = 1;
                            //Debug.Log(MC.GetComponent<Conductor>().songPosition);
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            if (Input.mousePosition.x - lastMouseX <= -10)
                            {
                                status = 3;
                            }
                            else if (Input.mousePosition.x - lastMouseX >= 10)
                            {
                                status = 4;
                            }
                            else
                            {
                                status = 2;
                            }

                            lastMouseX = Input.mousePosition.x;

                            Vector3 m;
                            m.x = Input.mousePosition.x;
                            m.y = Input.mousePosition.y;
                            m.z = 10;
                            Vector3 pos = cam.ScreenToWorldPoint(m);

                            pos.y = 0;
                            pos.z = 0;

                            if (pos.x >= 7) pos.x = 7f;
                            else if (pos.x <= -7) pos.x = -7f;
                            transform.position = pos;


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
                else
                {
                    status = 0;
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
                                if (t.phase == TouchPhase.Began)
                                {
                                    lastTouchX = t.position.x;
                                    status = 1;
                                    touched = true;
                                    break;
                                }
                                else if (t.phase == TouchPhase.Stationary)
                                {
                                    lastTouchX = t.position.x;
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
                                            if (t.position.x - lastTouchX >= 10)
                                            {
                                                lastTouchX = t.position.x;
                                                status = 4;
                                            }
                                            else if (t.position.x - lastTouchX <= -10)
                                            {
                                                lastTouchX = t.position.x;
                                                status = 3;
                                            }
                                            else
                                            {
                                                status = 2;
                                            }

                                            Vector3 m;
                                            m.x = t.position.x;
                                            m.y = t.position.y;
                                            m.z = 10;
                                            Vector3 pos = cam.ScreenToWorldPoint(m);

                                            pos.y = 0;
                                            pos.z = 0;

                                            if (pos.x >= 7) pos.x = 7f;
                                            else if (pos.x <= -7) pos.x = -7f;
                                            transform.position = pos;

                                            touched = true;
                                            break;
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
                        status = 0;
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
                    gameObject.GetComponent<Renderer>().material = untouch;
                    break;
                case 1:
                case 2:
                    gameObject.GetComponent<Renderer>().material = touch;
                    break;
            }
        }
    
}

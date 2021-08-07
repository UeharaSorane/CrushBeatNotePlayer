using UnityEngine;

public class PadControl : MonoBehaviour
{
    // Start is called before the first frame update
    public int[] PadStatus;


    void Start()
    {
        PadStatus = new int[transform.childCount];

    }

    // Update is called once per frame
    void Update()
    {
        CheckPad();
    }

    void CheckPad()
    {
        for (int i = 0; i < PadStatus.Length; i++)
        {
            PadStatus[i] = transform.GetChild(i).gameObject.GetComponent<PadAct>().status;
        }
    }

    public void ResetPad(int id)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<PadAct>().nextNoteNo == id)
            {
                transform.GetChild(i).GetComponent<PadAct>().nextNoteNo = -1;
            }
        }
    }
}

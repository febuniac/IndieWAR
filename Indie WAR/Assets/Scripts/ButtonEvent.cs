using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.EventSystems;
using System.Linq;


public class ButtonEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public string tribe;
    public int nTerr;
    private bool toggleFlag;
    private List<int> red, blue, yellow, green, black;
    private bool disabled;
    private bool entered;
    private List<List<int>> playerColors;
    private System.Random temp;

    void Awake()
    {
        toggleFlag = false;
        disabled = false;
        entered = false;
    }

    void Start()
    {

        temp = new System.Random();
        //nTerr = temp.Next(1, 4);
        nTerr = 3;
        transform.GetChild(0).GetComponent<Text>().text = nTerr.ToString();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
        GetComponent<Button>().GetComponent<Image>().color = Color.green;


        red = new List<int> { 5, 6, 11 };
        yellow = new List<int> { 1, 3, 15 };
        blue = new List<int> { 12, 13, 14 };
        black = new List<int> { 7, 4, 10 };
        green = new List<int> { 2, 8, 9 };
        playerColors = new List<List<int>> { blue, yellow, red, black, green };

  

    }

    void Update()
    {
        transform.GetChild(0).GetComponent<Text>().text = nTerr.ToString();

        if (disabled)
        {
            GetComponent<Button>().GetComponent<Image>().color = Color.grey;
        }
        else
        {
            if (entered)
            {
                GetComponent<Button>().GetComponent<Image>().color = Color.white;
            }
            else if (!toggleFlag)
            {
                Transform p = transform.parent;
                if (p.gameObject.GetComponent<Graph>().black.Contains(GetN(transform.name)))
                {
                    GetComponent<Button>().GetComponent<Image>().color = Color.black;
                }
                else if (p.gameObject.GetComponent<Graph>().yellow.Contains(GetN(transform.name)))
                {
                    GetComponent<Button>().GetComponent<Image>().color = Color.yellow;
                }
                else if (p.gameObject.GetComponent<Graph>().blue.Contains(GetN(transform.name)))
                {
                    GetComponent<Button>().GetComponent<Image>().color = Color.magenta;
                }
                else if (p.gameObject.GetComponent<Graph>().green.Contains(GetN(transform.name)))
                {
                    GetComponent<Button>().GetComponent<Image>().color = Color.green;
                }
                else if (p.gameObject.GetComponent<Graph>().red.Contains(GetN(transform.name)))
                {
                    GetComponent<Button>().GetComponent<Image>().color = Color.red;
                }
            }
            else if (toggleFlag)
            {
                GetComponent<Button>().GetComponent<Image>().color = Color.white;
            }

        }
    }

    private void Disable ()
    {
        disabled = true;
        toggleFlag = true;
    }

    private void Enable ()
    {
        disabled = false;
        toggleFlag = false;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        entered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        entered = false;
    }

    public void Select()
    {
        int index = name.IndexOf('(') + 1;
        transform.parent.SendMessage("OnSelect", this);
    }

    public void Deselect()
    {
        int index = name.IndexOf('(') + 1;
        transform.parent.SendMessage("OnDeselect");
    }

    public void TaskOnClick()
    {
        toggleFlag = !toggleFlag;
        if (toggleFlag)
        {
            Select();
        }
        else
        {
            Deselect();
        }

    }

    private void Loses()
    {
        nTerr -= 1;
        transform.GetChild(0).GetComponent<Text>().text = nTerr.ToString();
    }

    int GetN(string name)
    {
        int index = name.IndexOf("(") + 1;
        return (int.Parse(name.Substring(index, name.Length - index - 1)));
    }

    void Lost(GameObject[] go)
    {
        int remInd = -1;
        int addInd = -1;
        nTerr += 1;
        for(int j=0; j < playerColors.Count ; j++)
        {
            foreach(int i in playerColors[j])
            {
                if (i == GetN(go[1].name))
                {
                    remInd = j;
                }
                else if (i == GetN(go[0].name)) {
                    addInd = j;
                }
            }
        }

        playerColors[addInd].Add(GetN(go[1].name));
        playerColors[remInd].RemoveAt(playerColors[remInd].IndexOf(GetN(go[1].name)));

    }

    void Won()
    {
        nTerr -= 1;
    }
}


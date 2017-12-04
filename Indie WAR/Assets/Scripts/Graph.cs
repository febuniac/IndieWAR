using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using System;
using Satsuma;

public class Graph : MonoBehaviour
{
    private bool isSelected = false;
    private int attackTo, attackFrom, attackFromN, attackToN;
    public List<int> neighbors, red, blue, yellow, green, black;
    GameObject attacker, defender;
    System.Random dice;
    private int round, currentPlayer;
    public Text roundText, playerText, infoText;
    public GameObject continueButton;
    private bool disappear, player;
    private Node[] nodes;
    private Arc[] arcs;
    CustomGraph g;
    double[] closenessess;
    private List<List<int>> playerColors;
    private List<string> playerNames;
    private bool continuation;
    private int[] obj;

    IDictionary<int, List<int>> graph;
    string[] temp;


    // Use this for initialization
    void Start()
    {
        graph = new Dictionary<int, List<int>>();
        graph.Add(1, new List<int> { 3, 11, 14, 13, 12, 6 });
        graph.Add(2, new List<int> { 9 });
        graph.Add(3, new List<int> { 10, 15, 9, 1, 11 });
        graph.Add(4, new List<int> { 7, 10, 3 });
        graph.Add(5, new List<int> { 11 });
        graph.Add(6, new List<int> { 11 });
        graph.Add(7, new List<int> { 4, 8, 10 });
        graph.Add(8, new List<int> { 9, 7, 10 });
        graph.Add(9, new List<int> { 2, 8, 3, 10 });
        graph.Add(10, new List<int> { 3, 7, 8, 9, 4, 15 });
        graph.Add(11, new List<int> { 1, 3, 5, 6, 15, 14 });
        graph.Add(12, new List<int> { 14, 1, 13 });
        graph.Add(13, new List<int> { 12, 1, 9 });
        graph.Add(14, new List<int> { 1, 11, 12 });
        graph.Add(15, new List<int> { 3, 10 });

        red = new List<int> { 5, 6, 11 };
        yellow = new List<int> { 1, 3, 15 };
        blue = new List<int> { 12, 13, 14 };
        black = new List<int> { 7, 4, 10 };
        green = new List<int> { 2, 8, 9 };
        playerNames = new List<string> { "blue", "yellow", "red", "black", "green" };
        temp = new string[] { "magenta", "amarelas", "vermelhas", "pretas", "verdes" };
        continuation = false;
        playerColors = new List<List<int>> { blue, yellow, red, black, green };

        g = new CustomGraph();
        arcs = new Arc[100];
        closenessess = new double[16];
        nodes = new Node[16];

        attackTo = -1;
        attackFrom = -1;

        round = 0;
        currentPlayer = 0;
        disappear = false;
        obj = new int[5];



        dice = new System.Random();
        player = true;
        for (int i = 0; i <5; i++)
        {
            obj[i] = dice.Next(0, 5);
        }
        StartCoroutine(objText());


        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = g.AddNode();
        }

        int c = 0;
        foreach (int i in graph.Keys)
        {
            foreach (int j in graph[i])
            {
                arcs[c] = g.AddArc(nodes[j], nodes[i], Directedness.Undirected);
            }
            c += 1;
        }

        c = 0;
        foreach (Node n in nodes)
        {
            closenessess[c] = Closeness(n, g);
            c += 1;
        }
    }

    IEnumerator objText()
    {
        infoText.text = String.Format("Você controla \n as tribos {0}", playerNames[0]);
        yield return new WaitForSeconds(3.4f);
        infoText.text = String.Format("Seu objetivo e destruir \n as tribos {0}", temp[obj[0]]);
        yield return new WaitForSeconds(3.4f);
        infoText.text = "";
    }

    double Closeness(Node a, CustomGraph g)
    {
        var dik = new Dijkstra(g, arc => 1, DijkstraMode.Sum);
        double avg_distance = 0;
        dik.AddSource(a);

        foreach (Node n in nodes)
        {
            if (n != a)
            {
                dik.RunUntilFixed(n);
                avg_distance += dik.GetDistance(n)/14;
            }
        }
        return avg_distance;
    }


    // Update is called once per frame
    void Update()
    {
        roundText.text = "Round: " + round;
        playerText.text = "Player: " + playerNames[currentPlayer];
        int c = CheckObj();
        if (c != -1)
        {
            foreach (Transform child in transform)
            {
                child.SendMessage("Enable");
                child.GetComponent<Button>().enabled = false;
            }
            infoText.text = String.Format("As tribos {0} ganharam!", playerNames[c]);
            currentPlayer = -11111;
            StopAllCoroutines();

        }

    }

    int CheckObj()
    {
        for (int i = 0; i<5; i++)
        {
            if (playerColors[obj[i]].Count == 0)
            {
                return (i);
            }
        }
        return -1;
    }


    IEnumerator WaitAndPlay()
    {
        if (continueButton.gameObject.activeSelf)
        {
            yield return new WaitUntil(() => continuation);
            continuation = false;

        }
        continueButton.SetActive(false);
        double temp = closenessess.Max();
        int index = Array.IndexOf(closenessess, temp);
        double maxC = 0;
        int tempNode = 0; ;
        Transform tempTerr = null;
        Transform tempAttack = null;

        infoText.text = "IA jogando...";

        foreach (Transform child in transform)
        {
            if (playerColors[currentPlayer].Contains(GetN(child.name)) && child.GetComponent<ButtonEvent>().nTerr > 1)
            {
                foreach (int i in graph[GetN(child.name)])
                {
                    if ((closenessess[i] > maxC) && !playerColors[currentPlayer].Contains(i))
                    {
                        tempTerr = child;
                        tempNode = i;
                        maxC = closenessess[i];
                    }
                }
            }
        }

        foreach (Transform child in transform)
        {
            if (GetN(child.name) == tempNode)
            {
                tempAttack = child;
            }
        }

        if (tempTerr == null || tempAttack == null)
        {
            currentPlayer += 1;
            StartCoroutine(WaitAndPlay());
        }else
        {

            //OnSelect(tempTerr.GetComponent<ButtonEvent>());
            tempTerr.GetComponent<ButtonEvent>().TaskOnClick();
            yield return new WaitForSeconds(1.2f);

            //OnSelect(tempAttack.GetComponent<ButtonEvent>());
            tempAttack.GetComponent<ButtonEvent>().TaskOnClick();

            yield return new WaitForSeconds(1.2f);

        }


    }


    void OnSelect(ButtonEvent terr)
    {
        GameObject go = terr.gameObject;
        int terrTemp = GetN(go.transform.name);
        int nTerr = terr.nTerr;



        foreach (Transform child in transform)
        {

            neighbors = graph[terrTemp];
            if (!neighbors.Contains(GetN(child.name)) && GetN(child.name) != GetN(go.name)) 
            {
                child.SendMessage("Disable");
                child.GetComponent<Button>().enabled = false;
            }
            else if (playerColors[currentPlayer].Contains(GetN(child.name)))
            {
                child.SendMessage("Disable");
            }
        }

        if (isSelected)
        {
            attackToN = nTerr;
            attackTo = terrTemp;
            defender = terr.gameObject;
            attack(attacker, defender);
            if (defender.GetComponent<ButtonEvent>().nTerr == 0)
            {
                defender.SendMessage("Lost", new GameObject[] { attacker, defender });
                attacker.SendMessage("Won");
                print("Won");
                Lost(attacker, defender);
            }
            isSelected = false;
            
            currentPlayer += 1;

            if (currentPlayer == 5)
            {
                currentPlayer = 0;
                StartCoroutine(PV());
                round += 1;
                foreach(Transform child in transform)
                {
                    if (dice.Next(1, 7) == 6)
                    {
                        child.GetComponent<ButtonEvent>().nTerr += 1;

                    }
                }
                StopCoroutine(WaitAndPlay());
            } else if (currentPlayer > 0)
            {
                StartCoroutine(WaitAndPlay());
            }
            return;
        }

        if (!isSelected)
        {
            attackFrom = terrTemp;
            attacker = terr.gameObject;
            attackFromN = nTerr;
            isSelected = true;
        }


    }

    IEnumerator TextAndPlay()
    {
        yield return new WaitUntil(() => continuation);
        continuation = false;
        yield return WaitAndPlay();
        
    }
    IEnumerator PV ()
    {
        infoText.text = "Sua vez!";
        yield return new WaitForSeconds(2);
        infoText.text = "";
        StopCoroutine(PV());
    }

    private void attack (GameObject a, GameObject d)
    {
        int attackDice = dice.Next(1, 7*a.GetComponent<ButtonEvent>().nTerr);
        int defendDice = dice.Next(1, 7 * d.GetComponent<ButtonEvent>().nTerr);

        if (attackDice > defendDice)
        {
            defender.SendMessage("Loses");
            if (currentPlayer == 0)
            {
                infoText.text = String.Format("Ganhou! {0} X {1}", attackDice, defendDice);
                continueButton.SetActive(true);

            }

        }
        else
        {
            if (currentPlayer == 0)
            {
                infoText.text = String.Format("Perdeu! {0} X {1}", attackDice, defendDice);
                continueButton.SetActive(true);
                defender.SendMessage("Loses");

            }
            else
            {
                attacker.SendMessage("Loses");
            }

        }
        OnDeselect();
        player = false;
    }


    void Continued()
    {
        continueButton.SetActive(false);
        continuation = true;
        infoText.text = "";
    }


    void OnDeselect()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Button>().enabled = true;
            child.SendMessage("Enable");

        }
        isSelected = false;
        attackFrom = -1;
        neighbors = null;

    }

    int GetN (string name)
    {
        int index = name.IndexOf("(") + 1;
        return (int.Parse(name.Substring(index, name.Length - index - 1)));
    }


    void Lost(GameObject a, GameObject d)
    {
        int remInd = -1;
        int addInd = -1;
        for (int j = 0; j < playerColors.Count; j++)
        {
            foreach (int i in playerColors[j])
            {
                if (i == GetN(d.name))
                {
                    remInd = j;
                }
                else if (i == GetN(a.name))
                {
                    addInd = j;
                }
            }
        }

        //a.GetComponent<ButtonEvent>().nTerr -= 1;
        //d.GetComponent<ButtonEvent>().nTerr += 1;


        playerColors[addInd].Add(GetN(d.name));
        playerColors[remInd].RemoveAt(playerColors[remInd].IndexOf(GetN(d.name)));

    }
}

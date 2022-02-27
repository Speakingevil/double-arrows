using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using KModkit;
using System;

public class DoubleArrowsScript : MonoBehaviour {
    public KMAudio Audio;
    public KMBombInfo bomb;
    public List<KMSelectable> buttons;
    public TextMesh disp;
    public Renderer[] buttonID;
    public Material[] greyout;

    private int[][] grid = new int[9][]
        {new int[9]{ 51, 80, 60, 79, 61,  9,  8, 49, 50},
         new int[9]{ 52, 77, 53, 78, 34, 35, 47, 48, 46},
         new int[9]{ 21, 75, 18, 74, 37, 36, 38, 39, 45},
         new int[9]{ 20, 76, 19, 73, 33, 72, 30, 42, 43},
         new int[9]{ 22, 23, 17, 16, 32, 15, 31, 40, 44},
         new int[9]{ 25, 24, 65, 66, 70, 71, 29, 41, 28},
         new int[9]{  3, 57, 64, 56, 63, 14, 12, 13,  2},
         new int[9]{ 26, 58, 59, 67, 69, 68,  7,  6, 27},
         new int[9]{  4, 81, 54, 55, 62, 10, 11,  5,  1},};
    private int[][] callib = new int[2][] { new int[4], new int[4]};
    private bool[] pressed = new bool[8];
    private string[] dir = new string[4] { "Left", "Up", "Right", "Down" };
    private int[] location = new int[2];
    private int pressCount;
    private bool freeMode;
    private bool ascend;

    //Logging
    static int moduleCounter = 1;
    int moduleID;
    bool moduleSolved;

    private void Awake()
    {
        moduleID = moduleCounter++;
        foreach (KMSelectable button in buttons)
        {
            int b = buttons.IndexOf(button);
            button.OnInteract += delegate () { ButtonPress(b); return false; };
        }
    }

    void Start ()
    {
        Reset();
	}

    private void ButtonPress(int b)
    {
        if(moduleSolved == false)
        {
            buttons[b].AddInteractionPunch(0.5f);
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, buttons[b].transform);
            if (b == 8)
            {
                Debug.LogFormat("[Double Arrows #{0}]Reset button pressed", moduleID);
                Reset();
            }
            else
            {
                if(b < 4)
                {
                    switch(callib[0][b])
                    {
                        case 0:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[1] = (location[1] + 8) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (((grid[location[0]][(location[1] + 8) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 8) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[1] = (location[1] + 8) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if(freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: invalid move to {1}", moduleID, grid[location[0]][(location[1] + 8) % 9], dir[b]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                        case 1:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[0] = (location[0] + 8) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (((grid[(location[0] + 8) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 8) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[0] = (location[0] + 8) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: invalid move to {1}", moduleID, grid[(location[0] + 8) % 9][location[1]], dir[b]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                        case 2:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[1] = (location[1] + 1) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (((grid[location[0]][(location[1] + 1) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 1) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[1] = (location[1] + 1) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: invalid move to {1}", moduleID, grid[location[0]][(location[1] + 1) % 9], dir[b]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                        case 3:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[0] = (location[0] + 1) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (((grid[(location[0] + 1) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 1) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[0] = (location[0] + 1) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Inner {2} pressed: invalid move to {1}", moduleID, grid[(location[0] + 1) % 9][location[1]], dir[b]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                    }
                }
                else
                {
                    switch(callib[1][b - 4])
                    {
                        case 0:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[1] = (location[1] + 7) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (((grid[location[0]][(location[1] + 7) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 7) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[1] = (location[1] + 7) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: invalid move to {1}", moduleID, grid[location[0]][(location[1] + 7) % 9], dir[b - 4]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                        case 1:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[0] = (location[0] + 7) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (((grid[(location[0] + 7) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 7) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[0] = (location[0] + 7) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: invalid move to {1}", moduleID, grid[(location[0] + 7) % 9][location[1]], dir[b - 4]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                        case 2:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[1] = (location[1] + 2) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (((grid[location[0]][(location[1] + 2) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 2) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[1] = (location[1] + 2) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: invalid move to {1}", moduleID, grid[location[0]][(location[1] + 2) % 9], dir[b - 4]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                        case 3:
                            if (freeMode == true && pressed[b] == false)
                            {
                                location[0] = (location[0] + 2) % 9;
                                pressed[b] = true;
                                buttonID[b].material = greyout[1];
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (((grid[(location[0] + 2) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 2) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false)) && freeMode == false)
                            {
                                location[0] = (location[0] + 2) % 9;
                                Debug.LogFormat("[Double Arrows #{0}]Outer {2} pressed: moving to {1}", moduleID, grid[location[0]][location[1]], dir[b - 4]);
                                pressCount++;
                            }
                            else if (freeMode == false)
                            {
                                Debug.LogFormat("[Double Arrows #{0}]Outer Down pressed: invalid move to {1}", moduleID, grid[(location[0] + 2) % 9][location[1]], dir[b - 4]);
                                GetComponent<KMBombModule>().HandleStrike();
                            }
                            break;
                    }
                }
                if(pressCount < 8 && freeMode == true)
                {
                    int digit = grid[location[0]][location[1]] % 9;
                    if (digit == 0)
                        digit = 9;
                    disp.text = digit.ToString();
                }
                else if(pressCount < 10)
                {
                    if (freeMode == true)
                    {
                        for(int i = 0; i < 8; i++)
                        {
                            buttonID[i].material = greyout[0];
                        }
                        Debug.LogFormat("[Double Arrows #{0}]All eight buttons have been pressed", moduleID);
                        ascend = bomb.GetBatteryCount() % 2 == 0;
                        pressCount = 0;
                        freeMode = false;
                    }
                    disp.text = string.Empty;
                }
                else
                {
                    moduleSolved = true;
                    disp.text = "GG";
                    Audio.PlaySoundAtTransform("InputCorrect", transform);
                    GetComponent<KMBombModule>().HandlePass();
                }
            }
        }
    }

    private void Reset()
    {
        freeMode = true;
        pressCount = 0;
        for(int i = 0; i < 8; i++)
        {
            pressed[i] = false;
            buttonID[i].material = greyout[0];
        }
        for(int i = 0; i < 2; i++)
        {
            location[i] = UnityEngine.Random.Range(0, 9);
            List<int> init = new List<int> { 0, 1, 2, 3 };
            for(int j = 4; j > 0; j--)
            {
                int rand = UnityEngine.Random.Range(0, j);
                callib[i][4 - j] = init[rand];
                Debug.LogFormat("[Double Arrows #{0}]The {1} {2} button moves {3} {4} {5}", moduleID, new string[] { "Inner", "Outer" }[i], dir[4 - j], i + 1, new string[] { "space", "spaces"}[i], dir[init[rand]]);
                init.RemoveAt(rand);
            }
        }
        string digits = grid[location[0]][location[1]].ToString();
        if(grid[location[0]][location[1]] < 10)
        {
            digits = "0" + digits;
        }
        disp.text = digits;
        Debug.LogFormat("[Double Arrows #{0}]The initial location was ({1}, {2}) = {3}", moduleID, location[1] + 1, location[0] + 1, digits);
    }

#pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} udlr [Presses inner buttons] | !{0} UDLR [Presses outer buttons] | !{0} reset [Presses black button]";
#pragma warning restore 414

    private IEnumerator ProcessTwitchCommand(string command)
    {
        if (Regex.IsMatch(command, @"^\s*reset\s*$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant))
        {
            yield return null;
            buttons[8].OnInteract();
	        yield break;
        }

            var buttonstring = Regex.Match(command, @"^\s*([lurdLURD, ]+)\s*$");
        if (!buttonstring.Success)
            yield break;

        yield return null;
        yield return buttonstring.Groups[1].Value.Select(ch => { var pos = "lurdLURD".IndexOf(ch); return pos == -1 ? null : buttons[pos]; }).Where(button => button != null).ToArray();
    }

    private IEnumerator TwitchHandleForcedSolve()
    {
        if (freeMode)
        {
            int[] order = { 0, 1, 2, 3, 4, 5, 6, 7 };
            order = order.Shuffle();
            for (int i = 0; i < 8; i++)
            {
                if (!pressed[order[i]])
                {
                    buttons[order[i]].OnInteract();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
        int start = pressCount;
        for (int i = start; i < 10; i++)
        {
            if ((grid[location[0]][(location[1] + 8) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 8) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[0], 0)].OnInteract();
            else if ((grid[(location[0] + 8) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 8) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[0], 1)].OnInteract();
            else if ((grid[location[0]][(location[1] + 1) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 1) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[0], 2)].OnInteract();
            else if ((grid[(location[0] + 1) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 1) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[0], 3)].OnInteract();
            else if ((grid[location[0]][(location[1] + 7) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 7) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[1], 0) + 4].OnInteract();
            else if ((grid[(location[0] + 7) % 9][location[1]] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[(location[0] + 7) % 9][location[1]] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[1], 1) + 4].OnInteract();
            else if ((grid[location[0]][(location[1] + 2) % 9] == grid[location[0]][location[1]] % 81 + 1 && ascend == true) || (grid[location[0]][(location[1] + 2) % 9] == (grid[location[0]][location[1]] + 79) % 81 + 1 && ascend == false))
                buttons[Array.IndexOf(callib[1], 2) + 4].OnInteract();
            else
                buttons[Array.IndexOf(callib[1], 3) + 4].OnInteract();
            yield return new WaitForSeconds(0.1f);
        }
    }
}

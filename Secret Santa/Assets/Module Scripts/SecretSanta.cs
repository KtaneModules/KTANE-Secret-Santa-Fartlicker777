using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Rnd = UnityEngine.Random;
using KModkit;

public class SecretSanta : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   public KMSelectable[] Gifts;
   public KMSelectable Sumbit;
   public GameObject[] ResidualHighlights;

   public GameObject[] Presents;
   public GameObject[] Ribbons;
   public GameObject[] WholePresent;
   public Material[] PColors;
   public Material[] RColors;

   public TextMesh[] PriceTags;

   IsRotating GetRotate = new IsRotating();

   int[] GiftChoice = new int[10];
   int[] GiftColorsToNumbers = { 12, 15, 22, 25, 18, 7 };
   string[] PresentColorNames = { "red", "orange", "yellow", "green", "blue", "purple" };
   string[] GiftNames = "Handball (10),Wine Glass,5L of Soda,Foreign Coins,Kickball,Live Chicken,Walkie Talkie,Cookbook,Shoebill,Pipe Bomb,Tortured Soul,Gold Marbles,Toy Piano,Marimba,Discord Nitro".Split(',');

   int[] GiftPrices = new int[6];

   int[] FinalPrices = new int[6];
   int Solution;

   bool[] CurrentSelected = new bool[6];
   bool Selected;
   bool IsRot;
   bool IsPlaying;

   KMSelectable GiftThatWillSound;
   string SoundThatWillPlay;

   static int ModuleIdCounter = 1;
   int ModuleId;
   private bool ModuleSolved;

   void Awake () {
      ModuleId = ModuleIdCounter++;

      foreach (KMSelectable Gift in Gifts) {
         Gift.OnInteract += delegate () { GiftPress(Gift); return false; };
      }

      Sumbit.OnInteract += delegate () { SumbitPress(); return false; };

   }

   void SumbitPress () {
      Sumbit.AddInteractionPunch();
      Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, Sumbit.transform);
      if (ModuleSolved) {
         return;
      }
      for (int i = 0; i < 6; i++) {
         if (CurrentSelected[i]) {
            if (Solution == i) {
               GetComponent<KMBombModule>().HandlePass();
               ModuleSolved = true;
               return;
            }
         }
      }
      GetComponent<KMBombModule>().HandleStrike();
   }

   void GiftPress (KMSelectable Gift) {
      for (int i = 0; i < 6; i++) {
         if (Gift == Gifts[i]) {
            IsPlaying = false;
            if (CurrentSelected[i]) {
               Selected = false;
               for (int j = 0; j < 6; j++) {
                  CurrentSelected[j] = false;
                  ResidualHighlights[j].SetActive(false);
               }
            }
            else {
               Selected = true;
               GiftThatWillSound = Gift;
               SoundThatWillPlay = GiftNames[GiftChoice[i]];
               for (int j = 0; j < 6; j++) {
                  CurrentSelected[j] = false;
                  ResidualHighlights[j].SetActive(false);
               }
               ResidualHighlights[i].SetActive(true);
               CurrentSelected[i] = true;
            }
         }
      }
   }

   void Update () {
      IsRot = GetRotate.GetIsRot();
      if (Selected && IsRot && !IsPlaying) {
         StartCoroutine(PlayGiftSound());
      }
   }

   IEnumerator PlayGiftSound () {
      IsPlaying = true;
      Audio.PlaySoundAtTransform(SoundThatWillPlay, GiftThatWillSound.transform);
      yield return new WaitForSecondsRealtime(.2f);
      IsPlaying = false;
   }

   void Start () {
      Calculate();
   }

   void Calculate () {

      ShuffleData ThisShuffle = new ShuffleData();
      GiftData ThisData = new GiftData();

      int[] GiftChoice = ThisShuffle.Shuf[0].GetGiftChoice();
      for (int i = 0; i < 6; i++) {
         Debug.LogFormat("[Secret Santa #{0}] Gift {1} is colored {2}.", ModuleId, i + 1, PresentColorNames[ThisShuffle.Shuf[0].GetGiftColors()[i]]);
         int temp = ThisData.Gift[GiftChoice[i]].GetRibbon();
         Debug.LogFormat("[Secret Santa #{0}] Its ribbon is colored {1}.", ModuleId, new string[] { "gold", "white", "bronze", "silver" }[temp]);
         Debug.LogFormat("[Secret Santa #{0}] Its internal gift is {1}.", ModuleId, GiftNames[GiftChoice[i]]);
         if (temp == 0) {
            temp = -3;
         }
         else if (temp == 2) {
            temp = 3;
         }
         else if (temp == 3) {
            temp = -1;
         }
         int val = ThisData.Gift[GiftChoice[i]].GetValue();
         Debug.LogFormat("[Secret Santa #{0}] Its starting value is {1}.", ModuleId, val);
         switch (GiftChoice[i] + temp) {
            case 0:
            case 4:
            case 8:
            case 9:
            case 13:
               val = RevDig(val);
               break;
            case 2:
               val = Avg(val);
               break;
            case 3:
               val += DigRoot(val);
               break;
            case 5:
               val += 20;
               break;
            case 6:
               val -= 10;
               break;
            case 7:
               val += 10;
               break;
            case 10:
               val -= 20;
               break;
            case 11:
               val -= DigRoot(val);
               break;
            case 12:
               val /= 5;
               break;
            case 14:
               val = 100 - val;
               break;
         }
         Debug.LogFormat("[Secret Santa #{0}] Its new value is {1}.", ModuleId, val);
         ThisData.Gift[GiftChoice[i]].SetValue(val);
         val = ThisData.Gift[GiftChoice[i]].GetValue();
         int T = GiftColorsToNumbers[ThisShuffle.Shuf[0].GetGiftColors()[i]] + DigRoot(val);

         if ((val < 45 && val - T >= 10) || (val >= 45 && val + T > 99)) {
            val -= T;
         }
         else {
            val += T;
         }

         Debug.LogFormat("[Secret Santa #{0}] Its final value is {1}.", ModuleId, val);
         ThisData.Gift[GiftChoice[i]].SetValue(val);
         GiftPrices[i] = ThisData.Gift[GiftChoice[i]].GetValue();
      }

      this.GiftChoice = ThisShuffle.Shuf[0].GetGiftChoice();

      for (int i = 0; i < 6; i++) {
         Presents[i].GetComponent<MeshRenderer>().material = PColors[ThisShuffle.Shuf[0].GetGiftColors()[i]];
         Ribbons[i].GetComponent<MeshRenderer>().material = RColors[ThisData.Gift[ThisShuffle.Shuf[0].GetGiftChoice()[i]].GetRibbon()];
      }

      foreach (GameObject Gift in WholePresent) {
         Gift.transform.Rotate(0, Rnd.Range(0, 90f), 0);
      }

      GeneratePrices();
   }

   void GeneratePrices () {
      BubbleSort(GiftPrices);

      bool CeilOrFloor = Rnd.Range(0, 2) == 0; //Chooses between top two or bottom two

      Debug.LogFormat("[Secret Santa #{0}] The correct value is {1}.", ModuleId, CeilOrFloor ? "above the ceiling price" : "beneath the floor price");

      for (int i = 0; i < 5; i++) {
         int temp = 0;
         do {
            temp = Rnd.Range(10, 99);
         } while (GiftPrices.Contains(temp) || (temp > GiftPrices[0] && temp < GiftPrices[1]) || (temp > GiftPrices[4] && temp < GiftPrices[5]) || FinalPrices.Contains(temp));
         FinalPrices[i] = temp;
      }

      if (CeilOrFloor) {
         FinalPrices[5] = Rnd.Range(GiftPrices[4] + 1, GiftPrices[5]);
      }
      else {
         FinalPrices[5] = Rnd.Range(GiftPrices[0] + 1, GiftPrices[1]);
      }

      Debug.LogFormat("[Secret Santa #{0}] The solution price is ${1}.", ModuleId, FinalPrices[5]);

      FinalPrices.Shuffle();
      Debug.LogFormat("[Secret Santa #{0}] Final prices are ${1}, ${2}, ${3}, ${4}, ${5}, ${6}.", ModuleId, FinalPrices[0], FinalPrices[1], FinalPrices[2], FinalPrices[3], FinalPrices[4], FinalPrices[5]);
      for (int i = 0; i < 12; i += 2) {
         PriceTags[i].text = "$" + FinalPrices[i / 2].ToString();
         PriceTags[i + 1].text = "$" + FinalPrices[i / 2].ToString();
      }

      for (int i = 0; i < 6; i++) {
         if ((FinalPrices[i] > GiftPrices[4] && FinalPrices[i] < GiftPrices[5]) || (FinalPrices[i] < GiftPrices[1] && FinalPrices[i] > GiftPrices[0])) {
            Solution = i;
            return;
         }
      }
   }

   #region Misc Methods

   void BubbleSort (int[] arr) {
      bool HasSwapped = false;
      int temp = 0;
      for (int i = 0; i < arr.Length - 1; i++) {
         if (arr[i] > arr[i + 1]) {
            temp = arr[i + 1];
            arr[i + 1] = arr[i];
            arr[i] = temp;
            HasSwapped = true;
         }
      }
      if (HasSwapped) {
         BubbleSort(arr);
      }
   }

   int RevDig (int input) {
      return input / 10 + input % 10 * 10;
   }

   int Avg (int input) {
      return (50 + input) / 2;
   }

   int DigRoot (int input) {
      return (input - 1) % 9 + 1;
   }

   #endregion

#pragma warning disable 414
   private readonly string TwitchHelpMessage = @"Use !{0} TL to press that gift. Use !{0} submit to submit your selection.";
#pragma warning restore 414

   IEnumerator ProcessTwitchCommand (string Command) {
      Command = Command.Trim().ToUpperInvariant();
      string[] Positions = { "ML", "TL", "TR", "MR", "BR", "BL" };
      yield return null;
      if (!Positions.Contains(Command) && Command != "SUBMIT") {
         yield return "sendtochaterror I don't understand!";
      }
      else {
         if (Positions.Contains(Command)) {
            Gifts[Array.IndexOf(Positions, Command)].OnInteract();
         }
         else {
            Sumbit.OnInteract();
         }
      }
   }

   IEnumerator TwitchHandleForcedSolve () {
      string[] Positions = { "ML", "TL", "TR", "MR", "BR", "BL" };
      if (!Selected) {
         yield return ProcessTwitchCommand(Positions[Solution]);
      }
      if (Array.IndexOf(Gifts, GiftThatWillSound) != Solution) {
         yield return ProcessTwitchCommand(Positions[Solution]);
      }
      Sumbit.OnInteract();
   }
}
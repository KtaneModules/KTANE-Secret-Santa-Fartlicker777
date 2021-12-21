using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using KModkit;

public class SecretSanta : MonoBehaviour {

   public KMBombInfo Bomb;
   public KMAudio Audio;

   public KMSelectable[] Gifts;
   public KMSelectable Sumbit;
   public GameObject[] ResidualHighlights;


   int[] GiftChoice = new int[10];
   int[] GiftColorsToNumbers = { 12, 15, 22, 25, 18, 7};
   string[] PresentColorNames = { "red", "orange", "yellow", "green", "blue", "purple"};
   string[] GiftNames = "Handball (10),Wine Glass,5L of Soda,Foreign Coins,Kickball,Live Chicken,Walkie Talkie,Cookbook,Shoebill,Pipe Bomb,Tortured Soul,Gold Marbles,Toy Piano,Marimba,Discord Nitro".Split(',');

   bool Activated;

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

   }

   void GiftPress (KMSelectable Gift) {
      for (int i = 0; i < 6; i++) {
         if (Gift == Gifts[i]) {
            Audio.PlaySoundAtTransform(GiftNames[GiftChoice[i]], this.transform);
            foreach (GameObject H in ResidualHighlights) {
               H.SetActive(false);
            }
            ResidualHighlights[i].SetActive(true);
         }
      }
   }

   void Start () {
      Calculate();
   }

   void Calculate () {

      ShuffleData ThisShuffle = new ShuffleData();
      GiftData ThisData = new GiftData();
      DisplayGifts ThisDisplay = new DisplayGifts();

      int[] GiftChoice = ThisShuffle.Shuf[0].GetGiftChoice();
      for (int i = 0; i < 6; i++) {
         Debug.LogFormat("[Secret Santa #{0}] Gift {1} is colored {2}.", ModuleId, i + 1, PresentColorNames[ThisShuffle.Shuf[0].GetGiftColors()[i]]);
         int temp = ThisData.Gift[GiftChoice[i]].GetRibbon();
         Debug.LogFormat("[Secret Santa #{0}] Its ribbon is colored {1}.", ModuleId, new string[] { "gold", "white", "bronze", "silver"}[temp]);
         Debug.LogFormat("[Secret Santa #{0}] Its internal gift is {1}.", ModuleId, GiftNames[GiftChoice[i]]);
         if (temp == 0) {
            temp = -3;
         }
         else if (temp == 2) {   //Gets the direction to move in the table. Numbers are converted to actual directions here.
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
               break;         //Table for the value modifiers
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

         if ((val < 45 && val - T >= 10) || (val >= 45 && val + T > 99)) { //Last table where you add/subtract T
            val -= T;
         }
         else {
            val += T;
         }

         Debug.LogFormat("[Secret Santa #{0}] Its final value is {1}.", ModuleId, val);
         ThisData.Gift[GiftChoice[i]].SetValue(val);
      }
      this.GiftChoice = ThisShuffle.Shuf[0].GetGiftChoice();
      ThisDisplay.ColorRibbonsAndGifts(ThisShuffle.Shuf[0].GetGiftColors(), ThisData);
   }

   #region Table 2 Methods

   int RevDig (int input) {
      return input / 10 + input % 10;
   }

   int Avg (int input) {
      return (50 + input) / 2;
   }

   int DigRoot (int input) {
      return (input - 1) % 9 + 1;
   }

   #endregion
}

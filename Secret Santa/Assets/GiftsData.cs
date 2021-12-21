using System.Collections.Generic;
using Rnd = UnityEngine.Random;
using System.Linq;

public class GiftsClass {
   private string name { get; set; }
   private int price { get; set; }
   private int[] restrictions { get; set; }
   private int giftID;
   static int curID;

   private int ribbon;
   private int value;

   public GiftsClass (string name, int price, int[] restrictions) {
      this.name = name;
      this.price = price;
      this.restrictions = restrictions;
      value = price;
      SetRibbonAndID();
   }

   public GiftsClass (string name, int price) {
      this.name = name;
      this.price = price;
      value = price;
      this.restrictions = new int[] { };
      SetRibbonAndID();
   }

   private void SetRibbonAndID () {
      giftID = curID;
      curID++;
      do {
         ribbon = Rnd.Range(0, 4);
      } while (restrictions.Contains(ribbon));
   }

   public int GetRibbon () {
      return ribbon;
   }

   public int GetValue () {
      return value;
   }

   public void SetValue (int val) {
      value = val;
   }
}

public class GiftData {
   public readonly List<GiftsClass> Gift = new List<GiftsClass> {
   new GiftsClass ("Handball (10)", 10, new int[] {0, 3}),
   new GiftsClass ("Wine Glass", 15, new int[] {0}),
   new GiftsClass ("5L of Soda", 18, new int[] { 0, 1 }),

   new GiftsClass ("Foreign Coins", 22, new int[] {3}),
   new GiftsClass ("Kickball", 29),
   new GiftsClass ("Live Chicken", 31, new int[] {1}),

   new GiftsClass ("Walkie Talkie", 35, new int[] {3}),
   new GiftsClass ("Cookbook", 42),
   new GiftsClass ("Shoebill", 50, new int[] {1}),

   new GiftsClass ("Pipe Bomb", 55, new int[] {3}),
   new GiftsClass ("Tortured Soul", 59),
   new GiftsClass ("Gold Marbles", 73, new int[] { 0, 3}),

   new GiftsClass ("Toy Piano", 79, new int[] {2, 3}),
   new GiftsClass ("Marimba", 80, new int[] {2}),
   new GiftsClass ("Discord Nitro", 95, new int[] {1, 2}),
   };
}


using System.Collections.Generic;

public class Shuffles {
   private int[] GiftChoice = new int[15];
   private int[] GiftColors = new int[6];

   public Shuffles() {
      ShuffleGifts();
   }

   public void ShuffleGifts () {
      for (int i = 0; i < 15; i++) {
         if (i < 6) {
            GiftColors[i] = i;
         }
         GiftChoice[i] = i;
      }
      GiftColors.Shuffle();
      GiftChoice.Shuffle();
   }

   public int[] GetGiftColors () {
      return GiftColors;
   }

   public int[] GetGiftChoice () {
      return GiftChoice;
   }
}

public class ShuffleData {
   public readonly List<Shuffles> Shuf = new List<Shuffles> {
      new Shuffles()
   };
}
/*using UnityEngine;
using System.Collections.Generic;
using Rnd = UnityEngine.Random;

public class DisplayGifts : MonoBehaviour {

   public GameObject[] WholePresent;

   // Use this for initialization
   public void RotateGifts () {
      foreach (GameObject Gift in WholePresent) {
         Gift.transform.Rotate(0, Rnd.Range(0, 90f), 0);
      }
   }

   // Threw a bunch of errors for no fucking reason, so I moved it to the main script
   /*public void ColorRibbonsAndGifts (ShuffleData ThisShuffle, GiftData ThisData) {
      for (int i = 0; i < 6; i++) {
         Presents[i].GetComponent<MeshRenderer>().material = PColors[ThisShuffle.Shuf[0].GetGiftColors()[i]];
         Ribbons[i].GetComponent<MeshRenderer>().material = RColors[ThisData.Gift[ThisShuffle.Shuf[0].GetGiftChoice()[i]].GetRibbon()];
      }
   }
}

public class CreateDisplayer {
   public readonly List<DisplayGifts> Why = new List<DisplayGifts> {
      new DisplayGifts()
   };
}*/
using UnityEngine;
using Rnd = UnityEngine.Random;

public class DisplayGifts : MonoBehaviour {

   public GameObject[] Presents;
   public GameObject[] Ribbons;
   public GameObject[] WholePresent;
   public Material[] PColors;
   public Material[] RColors;

   void Start () {
      RotateGifts();
   }

   // Use this for initialization
   public void RotateGifts () {
      foreach (GameObject Gift in WholePresent) {
         Gift.transform.Rotate(0, Rnd.Range(0, 90f), 0);
      }
   }

   // Update is called once per frame
   public void ColorRibbonsAndGifts (int[] GColors, GiftData ThisData) {
      for (int i = 0; i < 6; i++) {
         Presents[i].GetComponent<MeshRenderer>().material = PColors[GColors[i]];
         Ribbons[i].GetComponent<MeshRenderer>().material = RColors[ThisData.Gift[GColors[i]].GetRibbon()];
      }
   }
}

using System.Collections;
using UnityEngine;

public class IsRotating : MonoBehaviour {

   public KMSelectable Mod;
   static bool IsRot = false;

   void Awake () {
      StartCoroutine(UpdateRotating());
   }

   public bool GetIsRot () {
      return IsRot;
   }

   IEnumerator UpdateRotating () {
      while (true) {
         Quaternion OldPos = Mod.transform.rotation;
         yield return new WaitForSecondsRealtime(.01f);
         if (Mathf.Abs(OldPos.eulerAngles.x - Mod.transform.rotation.eulerAngles.x) > 90f || Mathf.Abs(OldPos.eulerAngles.y - Mod.transform.rotation.eulerAngles.y) > 90f || Mathf.Abs(OldPos.eulerAngles.z - Mod.transform.rotation.eulerAngles.z) > 90f) {
            IsRot = true;
         }
         else {
            IsRot = false;
         }
      }
   }
}

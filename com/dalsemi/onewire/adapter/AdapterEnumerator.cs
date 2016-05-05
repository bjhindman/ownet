using System;
using System.Collections;
namespace com.dalsemi.onewire.adapter
{
   /// <summary>
   /// Presents a class that can enumerate through DSPortAdapters
   /// </summary>
   public class AdapterEnumerator : IEnumerator
   {
      private System.Collections.ArrayList adapterList;
      private int currentIndex;

      public AdapterEnumerator(System.Collections.ArrayList adapters)
      {
         //
         // TODO: Add constructor logic here
         //
         adapterList = adapters;
         currentIndex = -1;
      }
      public void Reset()
      {
         currentIndex = -1;
      }
      public bool MoveNext()
      {
         currentIndex++;
         if (currentIndex >= adapterList.Count)
         {
            return false;
         }
         else
         {
            return true;
         }
      }
      public object Current
      {
         get
         {
            return adapterList[currentIndex];
         }
      }
   }
}

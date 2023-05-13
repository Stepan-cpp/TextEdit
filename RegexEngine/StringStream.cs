using System.Runtime.InteropServices;

namespace RegexEngine;

public class StringStream
{
   private LinkedList<char> data = new();

   public StringStream(string str)
   {
      foreach (var t in str)
         data.AddLast(t);
   }

   public char Read()
   {
      char c = data.First.Value;
      data.RemoveFirst();
      return c;
   }

   public string Read(int count)
   {
      string s = Seek(count);
      ConfirmRead(count);
      return s;
   }
   
   public string Seek(int count)
   {
      if (count < 0)
         throw new ArgumentException($"{nameof(count)} was less than zero");
      
      string str = String.Empty;
      var node = data.First;
      for (int i = 0; i < count; i++)
      {
         str += node.Value;
         node = node.Next;
         if (node == null)
            break;
      }

      return str;
   }

   public void ConfirmRead(int count)
   {
      if (count < 0)
         throw new ArgumentException($"{nameof(count)} was less than zero");
      for (int i = 0; i < count; i++)
         data.RemoveFirst();
   }

   public long Length => data.Count;
}
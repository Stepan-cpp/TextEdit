namespace RegexEngine;

public class CountModifier
{
   private Predicate<int> matchPattern;

   public bool DoesCountMatch(int count)
   {
      return matchPattern(count);
   }

   private CountModifier(Predicate<int> match)
   {
      matchPattern = match;
   }

   public static CountModifier ReadModifier(StringStream str)
   {
      string opening = str.Read(1);
      switch (opening)
      {
         case "*":
            return new CountModifier(c => true);
         case "+":
            return new CountModifier(c => c > 0);
         case "?":
            return new CountModifier(c => c < 1);
         case "[":
            string s = String.Empty;
            char read;
            while ((read = str.Read()) != ']')
               s += read;
            string[] args = s.Substring(1, s.Length - 2).Split(';');
            return new CountModifier(c => c >= int.Parse(args[0]) && c <= int.Parse(args[1]));
      }

      throw new ArgumentException($"Unexpected modifier key: {opening}");
   }
}
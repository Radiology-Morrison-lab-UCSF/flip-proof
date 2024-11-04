namespace FlipProof.Base;

public static class BoxExtensionMethods
{
   public static void ForEachPosition(this Box<int> box, Action<int, int, int> action)
   {
      for (int x = box.Origin.X; x < box.FarCorner.X; x++)
      {
         for (int y = box.Origin.Y; y < box.FarCorner.Y; y++)
         {
            for (int z = box.Origin.Z; z < box.FarCorner.Z; z++)
               action.Invoke(x, y, z);
         }
      }
   }
}

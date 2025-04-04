namespace FlipProof.Base.Tests;

[TestClass]
public sealed class ArrayExtensionsTests
{
   [TestMethod]
   public void ArraySwitch_Sbyte()
   {
      sbyte[] array = new sbyte[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => a[0],
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((sbyte)111, o);
   }
   
   [TestMethod]
   public void ArraySwitch_byte()
   {
      byte[] array = new byte[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => a[0],
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((byte)111, o);
   }   

   [TestMethod]
   public void ArraySwitch_Int16()
   {
      Int16[] array = new Int16[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => a[0],
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((Int16)111, o);
   }
   [TestMethod]
   public void ArraySwitch_Int32()
   {
      Int32[] array = new Int32[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => a[0],
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((Int32)111, o);
   }
   [TestMethod]
   public void ArraySwitch_Int64()
   {
      Int64[] array = new Int64[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => a[0],
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((Int64)111, o);
   }
   [TestMethod]
   public void ArraySwitch_UInt16()
   {
      UInt16[] array = new UInt16[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => a[0],
         a => ob,
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((UInt16)111, o);
   }
   [TestMethod]
   public void ArraySwitch_UInt32()
   {
      UInt32[] array = new UInt32[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => a[0],
         a => ob,
         a => ob,
         a => ob
         );

      Assert.AreEqual((UInt32)111, o);
   }
   [TestMethod]
   public void ArraySwitch_UInt64()
   {
      UInt64[] array = new UInt64[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => a[0],
         a => ob,
         a => ob
         );

      Assert.AreEqual((UInt64)111, o);
   }
   [TestMethod]
   public void ArraySwitch_Float()
   {
      float[] array = new float[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => a[0],
         a => ob
         );

      Assert.AreEqual((float)111, o);
   }
   [TestMethod]
   public void ArraySwitch_Double()
   {
      double[] array = new double[10];
      array[0] = 111;
      object ob = new();
      object o = array.ArraySwitch(a => 0,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => ob,
         a => a[0]
         );

      Assert.AreEqual(111d, o);
   }

}

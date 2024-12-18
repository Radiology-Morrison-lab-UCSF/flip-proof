#pragma expandtemplate typeToReplace=DoubleTensor
#pragma expandtemplate Int8Tensor UInt8Tensor Int16Tensor Int32Tensor Int64Tensor FloatTensor BoolTensor ComplexTensor
#pragma expandtemplate typeToReplace=double
#pragma expandtemplate Int8 UInt8 Int16 Int32 Int64 float bool Complex
#pragma expandtemplate typeToReplace=Float64
#pragma expandtemplate Int8 Byte Int16 Int32 Int64 Float32 Bool ComplexFloat64


using static TorchSharp.torch;
using TorchSharp;
using Int8 = System.SByte;
using UInt8 = System.Byte;
using System.Numerics;

namespace FlipProof.Torch;

public partial class DoubleTensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(double value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(double val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(double[] arr) => torch.tensor(arr);

}

#region TEMPLATE EXPANSION
public partial class Int8Tensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(Int8 value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Int8 val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Int8[] arr) => torch.tensor(arr);

}

public partial class UInt8Tensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(UInt8 value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(UInt8 val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(UInt8[] arr) => torch.tensor(arr);

}

public partial class Int16Tensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(Int16 value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Int16 val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Int16[] arr) => torch.tensor(arr);

}

public partial class Int32Tensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(Int32 value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Int32 val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Int32[] arr) => torch.tensor(arr);

}

public partial class Int64Tensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(Int64 value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Int64 val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Int64[] arr) => torch.tensor(arr);

}

public partial class FloatTensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(float value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(float val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(float[] arr) => torch.tensor(arr);

}

public partial class BoolTensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(bool value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(bool val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(bool[] arr) => torch.tensor(arr);

}

public partial class ComplexTensor
{
   /// <summary>
   /// Sets the value at the indices provided in place
   /// </summary>
   /// <param name="value">New value</param>
   /// <param name="indices">Where to set</param>
   protected override void Set(Complex value, params long[] indices) => Storage[indices] = value;

   /// <summary>
   /// Creates a <see cref="Tensor"/> containing a single value
   /// </summary>
   /// <param name="val">The value</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(Complex val) => torch.tensor(val);

   /// <summary>
   /// Creates a 1D <see cref="Tensor"/>
   /// </summary>
   /// <param name="arr">Data copied into the tensor</param>
   /// <returns>A new <see cref="Tensor"/></returns>
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(Complex[] arr) => torch.tensor(arr);

}

#endregion TEMPLATE EXPANSION

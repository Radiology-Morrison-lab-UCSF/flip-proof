#pragma expandtemplate typeToReplace=FloatTensor
#pragma expandtemplate Int8Tensor UInt8Tensor Int16Tensor
#pragma expandtemplate typeToReplace=float
#pragma expandtemplate Int8 UInt8 Int16
#pragma expandtemplate typeToReplace=Float32
#pragma expandtemplate Int8 Byte Int16


using static TorchSharp.torch;
using TorchSharp.Modules;
using TorchSharp;
using Int8 = System.SByte;
using UInt8 = System.Byte;
using System.Numerics;
using static Tensorboard.TensorShapeProto.Types;
using System.Diagnostics.CodeAnalysis;
using FlipProof.Base;

namespace FlipProof.Torch;

public partial class FloatTensor
{
   /// <summary>
   /// Fourier transform, returning single precision complex
   /// </summary>
   /// <param name="dimensions"></param>
   /// <returns></returns>
   public new Complex32Tensor FFTN(long[]? dimensions = null) => base.FFTN(dimensions);

}

#region TEMPLATE EXPANSION
public partial class Int8Tensor
{
   /// <summary>
   /// Fourier transform, returning single precision complex
   /// </summary>
   /// <param name="dimensions"></param>
   /// <returns></returns>
   public new Complex32Tensor FFTN(long[]? dimensions = null) => base.FFTN(dimensions);

}

public partial class UInt8Tensor
{
   /// <summary>
   /// Fourier transform, returning single precision complex
   /// </summary>
   /// <param name="dimensions"></param>
   /// <returns></returns>
   public new Complex32Tensor FFTN(long[]? dimensions = null) => base.FFTN(dimensions);

}

public partial class Int16Tensor
{
   /// <summary>
   /// Fourier transform, returning single precision complex
   /// </summary>
   /// <param name="dimensions"></param>
   /// <returns></returns>
   public new Complex32Tensor FFTN(long[]? dimensions = null) => base.FFTN(dimensions);

}

#endregion TEMPLATE EXPANSION

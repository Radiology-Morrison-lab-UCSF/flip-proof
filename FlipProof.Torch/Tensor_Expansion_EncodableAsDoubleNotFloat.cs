#pragma expandtemplate typeToReplace=DoubleTensor
#pragma expandtemplate Int32Tensor Int64Tensor
#pragma expandtemplate typeToReplace=double
#pragma expandtemplate Int32 Int64
#pragma expandtemplate typeToReplace=Float64
#pragma expandtemplate Int32 Int64


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

public partial class DoubleTensor
{

}

#region TEMPLATE EXPANSION
public partial class Int32Tensor
{

}

public partial class Int64Tensor
{

}

#endregion TEMPLATE EXPANSION

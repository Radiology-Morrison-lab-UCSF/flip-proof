using static TorchSharp.torch;
using TorchSharp;
using TorchSharp.Modules;

namespace FlipProof.Torch;

public class FloatTensor : SimpleNumericTensor<float, FloatTensor>
{

   public FloatTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Float32))
   {
   }

   public FloatTensor(Tensor t) : base(t) { }

   public override ScalarType DType => ScalarType.Float32;


   protected override void Set(float value, params long[] indices) => _storage[indices] = value;


   protected override FloatTensor CreateSameSizeBlank() => new(_storage.shape);

   protected override FloatTensor CreateFromTensorSub(Tensor t) => new(t);


   public override Tensor ScalarToTensor(float arr) => torch.tensor(arr);
   public override Tensor ArrayToTensor(float[] arr) => torch.tensor(arr);

   protected override float ToScalar(Tensor t) => t.ToSingle();

   #region Operators
   // Operators remove ambiguity between float and double operations specified in the base class
   public static DoubleTensor operator +(FloatTensor left, DoubleTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator +(DoubleTensor left, FloatTensor right) => new(left.Storage + right.Storage);
   public static DoubleTensor operator -(FloatTensor left, DoubleTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator -(DoubleTensor left, FloatTensor right) => new(left.Storage - right.Storage);
   public static DoubleTensor operator *(FloatTensor left, DoubleTensor right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator *(DoubleTensor left, FloatTensor right) => new(left.Storage * right.Storage);
   public static DoubleTensor operator /(FloatTensor left, DoubleTensor right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(DoubleTensor left, FloatTensor right) => new(left.Storage / right.Storage);
   #endregion
}

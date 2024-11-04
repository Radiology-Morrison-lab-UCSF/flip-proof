using static TorchSharp.torch;
using TorchSharp;
using TorchSharp.Modules;

namespace FlipProof.Torch;

public class DoubleTensor : SimpleNumericTensor<double, DoubleTensor>
{

   public DoubleTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Float64))
   {
   }

   public DoubleTensor(Tensor t) : base(t) { }

   public override ScalarType DType => ScalarType.Float64;


   protected override void Set(double value, params long[] indices) => _storage[indices] = value;


   protected override DoubleTensor CreateSameSizeBlank() => new(_storage.shape);

   protected override DoubleTensor CreateFromTensorSub(Tensor t) => new(t);


   public override Tensor ScalarToTensor(double arr) => torch.tensor(arr);
   public override Tensor ArrayToTensor(double[] arr) => torch.tensor(arr);

   protected override double ToScalar(Tensor t) => t.ToDouble();

   public static DoubleTensor operator *(DoubleTensor left, double right) => new(left.Storage.multiply(right));
   public static DoubleTensor operator *(double left, DoubleTensor right) => new(right.Storage.multiply(left));
   public static DoubleTensor operator /(DoubleTensor left, DoubleTensor right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(DoubleTensor left, SimpleNumericTensor<double, DoubleTensor> right) => new(left.Storage / right.Storage);
   public static DoubleTensor operator /(SimpleNumericTensor<double, DoubleTensor> left, DoubleTensor right) => new(left.Storage / right.Storage);

}

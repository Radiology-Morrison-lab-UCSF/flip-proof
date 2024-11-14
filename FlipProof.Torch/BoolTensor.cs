using static TorchSharp.torch;
using TorchSharp;

namespace FlipProof.Torch;

public class BoolTensor : Tensor<bool>
{

   public BoolTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Bool))
   {
   }

   [CLSCompliant(false)]
   public BoolTensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Bool;


   protected override void Set(bool value, params long[] indices) => _storage[indices] = value;

   protected override Tensor<bool> CreateSameSizeBlank() => new BoolTensor(_storage.shape);

   [CLSCompliant(false)]
   protected override Tensor<bool> CreateFromTensor_Sub(Tensor t) => new BoolTensor(t);


   [CLSCompliant(false)]
   public override Tensor ScalarToTensor(bool arr) => torch.tensor(arr);
   [CLSCompliant(false)]
   public override Tensor ArrayToTensor(bool[] arr) => torch.tensor(arr);

   [CLSCompliant(false)]
   protected override bool ToScalar(Tensor t) => t.ToBoolean();

   /// <summary>
   /// True if all elements are true
   /// </summary>
   /// <returns></returns>
   public bool All() => _storage.all().ToBoolean();

}

using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;

namespace FlipProof.Torch;

public sealed class BoolTensor : Tensor<bool>
{

   [SetsRequiredMembers]
   public BoolTensor(long[] dimSizes) : base(torch.zeros(dimSizes, ScalarType.Bool))
   {
   }

   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public BoolTensor(Tensor t) : base(t) { }

   [CLSCompliant(false)]
   public override ScalarType DType => ScalarType.Bool;


   protected override void Set(bool value, params long[] indices) => Storage[indices] = value;


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
   public bool All() => Storage.all().ToBoolean();

   

}

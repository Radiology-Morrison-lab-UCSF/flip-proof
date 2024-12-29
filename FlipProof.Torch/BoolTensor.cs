using static TorchSharp.torch;
using TorchSharp;
using System.Diagnostics.CodeAnalysis;
using TorchSharp.Modules;

namespace FlipProof.Torch;

public sealed partial class BoolTensor : Tensor<bool,BoolTensor>
{
   /// <summary>
   /// True if all elements are true
   /// </summary>
   /// <returns></returns>
   public bool All() => Storage.all().ToBoolean();

   [CLSCompliant(false)]
   protected override bool ToScalar(Tensor t) => t.ToBoolean();

   public override void FillWithRandom()
   {
      using torch.Tensor rand = torch.randint(0, 2, Storage.shape, DType);
      using Tensor asB = rand == 1;
      Storage.copy_(asB);
   }
}

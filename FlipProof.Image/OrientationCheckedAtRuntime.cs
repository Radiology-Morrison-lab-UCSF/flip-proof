using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Image;

[System.AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor, Inherited = true, AllowMultiple = false)]
public sealed class OrientationCheckedAtRuntime : Attribute
{
   const string DefaultMessage = "Causes a Runtime Orientation Check";
   public OrientationCheckedAtRuntime(string? alternativeOption = null)
   {
      Message = alternativeOption is null ? DefaultMessage : (DefaultMessage + ". " + alternativeOption);
   }

   public string Message
   {
      get; 
   }

   public override bool IsDefaultAttribute() => Message == DefaultMessage;

   public override string ToString() => Message;
}
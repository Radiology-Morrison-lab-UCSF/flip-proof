using FlipProof.Image.Nifti;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.ImageTests.Nifti;
[TestClass]
public class NiftiReaderTests
{
   [TestMethod]
   public void ReadNifti()
   {
      NiftiReader nr = new(@"C:\Users\lreid\data\fieldmap\_Psilocybin_Parkinson_Bra_20240507164537_4.nii");
      bool result = nr.TryRead(out string msg, out NiftiFile_Base? nf);
      Assert.IsTrue(result);
      Assert.IsNotNull(nf);
   }

}

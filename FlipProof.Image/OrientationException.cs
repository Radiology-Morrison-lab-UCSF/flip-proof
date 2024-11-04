using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipProof.Image;
public class OrientationException(string Message) : Exception(message:Message)
{
}

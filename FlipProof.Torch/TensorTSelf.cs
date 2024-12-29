using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TorchSharp.torch;
using TorchSharp;
using FlipProof.Base;

namespace FlipProof.Torch;
public abstract class Tensor<T, TSelf> : Tensor<T>
   where T : struct
   where TSelf : Tensor<T, TSelf>
{
   [CLSCompliant(false)]
   [SetsRequiredMembers]
   public Tensor(torch.Tensor t) : base(t)
   {
   }
   /// <summary>
   /// Creates a new <see cref="TSelf"/> by applying a torch operation
   /// </summary>
   /// <param name="func">Must return a new object</param>
   /// <param name="doNotCast">when true, if the function returns the wrong type, an exception is thrown. When false, if the function returns the wrong type, the result is cast to <see cref="T"/> </param>
   /// <returns></returns>
   internal TSelf CreateFromTrustedOperation(Func<Tensor, Tensor> func)
   {
      Tensor t = func(Storage);
      System.Diagnostics.Debug.Assert(!object.ReferenceEquals(t, Storage), "Functions must return a new tensor to avoid sharing");
      return CreateFromTensor(func(Storage));
   }
   
   /// <summary>
   /// Creates a new Tensor
   /// </summary>
   /// <param name="input">The tensor to wrap</param>
   /// <param name="allowCast">If <paramref name="input"/> is the wrong type, this allows casting rather than throwing an <see cref="ArgumentException"/></param>
   /// <exception cref="ArgumentException">Bad input type</exception>
   [CLSCompliant(false)]
   public new TSelf CreateFromTensor(Tensor input, bool allowCast = false)
   {
      if (input.dtype != DType)
      {
         if (!allowCast)
         {
            throw new ArgumentException("Input tensor is wrong data type");
         }
         input = input.to_type(DType);
      }
      return CreateFromTensorSub(input);
   }

   [CLSCompliant(false)]
   protected sealed override Tensor<T> CreateFromTensor_Sub(Tensor t) => CreateFromTensor(t);



   /// <summary>
   /// Create a new <typeparamref name="TSelf"/> from this input tensor
   /// </summary>
   /// <param name="t">Guaranteed not null and of the correct type</param>
   /// <returns></returns>
   [CLSCompliant(false)]
   protected virtual TSelf CreateFromTensorSub(Tensor t) => throw new NotImplementedException("Must be implemented by deriving class");

   [CLSCompliant(false)]
   protected TSelf Create(Tensor other, Func<Tensor, Tensor, Tensor> operation) => CreateFromTensor(operation(this.Storage, other));


   /// <summary>
   /// Concatenates all along the provided dimension
   /// </summary>
   /// <exception cref="ArgumentException">Empty collection provided</exception>
   public static TSelf Concat(IReadOnlyList<TSelf> other, int dimension)
   {
      if (other.Count == 0)
      {
         throw new ArgumentException("No tensors provided");
      }
      return other[0].CreateFromTensor(torch.stack(other.Select(a => a.Storage), dimension));
   }


   /// <summary>
   /// Pads or crops a 3D tensor, returning a tensor in a different space
   /// </summary>
   /// <param name="newBounds">The region of voxels to keep, in Voxel space. If within the image, cropping occurs. If outside, padding occurs</param>
   /// <returns>A padded and/or cropped image</returns>
   public TSelf Pad(Box<long> newBounds)
   {
      if (NDims != 3)
      {
         throw new InvalidOperationException($"3D box cannot pad {NDims}D tensor");
      }
      var mySize = Storage.shape;
      return Pad(
         -newBounds.Origin.X, newBounds.FarCorner.X - mySize[0],
         -newBounds.Origin.Y, newBounds.FarCorner.Y - mySize[1],
         -newBounds.Origin.Z, newBounds.FarCorner.Z - mySize[2]
         );
   }
   /// <summary>
   /// Pads or crops a 4D tensor, returning a tensor in a different space
   /// </summary>
   /// <param name="newBounds">The region of voxels to keep, in Voxel space. If within the image, cropping occurs. If outside, padding occurs</param>
   /// <returns>A padded and/or cropped image</returns>
   public TSelf Pad(Box4D<long> newBounds)
   {
      if (NDims != 4)
      {
         throw new InvalidOperationException($"4D box cannot pad {NDims}D tensor");
      }
      new Box4D<long>(new(), new(Storage.shape)).CalcPadding(newBounds, out long xB4, out long xAfter, out long yB4, out long yAfter, out long zB4, out long zAfter, out long volB4, out long volAfter);

      return Pad(xB4, xAfter, yB4, yAfter, zB4, zAfter,volB4, volAfter);
   }
   /// <summary>
   /// Performs the inverse operation of <see cref="Pad(Box{long})"/>
   /// </summary>
   /// <param name="currentBounds">The region of voxels requested in <see cref="Pad(Box{long})"/></param>
   /// <param name="origSize">The original size of the unpadded tensor</param>
   /// <returns>A padded and/or cropped tensor</returns>
   public TSelf Unpad(Box<long> currentBounds, XYZ<long> origSize) => Pad(new Box<long>(currentBounds.Origin.Negate(), origSize));

   /// <summary>
   /// Performs the inverse operation of <see cref="Pad(Box{long})"/>
   /// </summary>
   /// <param name="currentBounds">The region of voxels requested in <see cref="Pad(Box{long})"/></param>
   /// <param name="origSize">The original size of the unpadded tensor</param>
   /// <returns>A padded and/or cropped tensor</returns>
   public TSelf Unpad(Box4D<long> currentBounds, XYZA<long> origSize) => Pad(new Box4D<long>(currentBounds.Origin.Negate(), origSize));

   /// <summary>
   /// Pads the tensor so it is centered in the result
   /// </summary>
   /// <param name="padBy">Amount to pad before and after data in each dimension. Provide twice as many values as their are dimensions ordered LDim0, RDim0, LDim1, RDim1, etc</param>
   /// <remarks>This does not match behaviour of torch.pad, which has arguments in a sort of reverse order</remarks>
   /// <returns>A new, padded, tensor</returns>
   public TSelf Pad(params long[] padBy)
   {
      if (padBy.Length != NDims * 2)
      {
         throw new ArgumentException($"Expected {NDims * 2} values");
      }

      // Torch orders things in reverse DimN L, DimN R, Dim N-1 L, Dim N-1 R, etc
      long[] padTorchOrder = padBy.Reverse().ToArray();

      for (int i = 0; i < padTorchOrder.Length; i += 2)
      {
         (padTorchOrder[i], padTorchOrder[i + 1]) = (padTorchOrder[i + 1], padTorchOrder[i]);
      }

      return CreateFromTensor(torch.nn.functional.pad(Storage, padTorchOrder, PaddingModes.Constant));
   }


   /// <summary>
   /// Fills this tensor with random values
   /// </summary>
   public abstract void FillWithRandom();

   /// <summary>
   /// Returns a copy of this tensor
   /// </summary>
   /// <returns></returns>
   public abstract TSelf DeepClone();

}
# Flip Proof

Flip Proof is an Orientation and Voxel-Type Safe Medical Imaging Framework for .NET languages including C#, Python (via 
IronPython), F#, C++/CLI, and Visual Basic. It is intended to support medical image processing for high-stakes settings 
such as surgery.

'Voxel-Type safety' refers to an inability to accidentally mix images of incompatible types (such as dividing by a binary
mask). 'Orientation safety' refers to an inability to accidentaly mix images of different orientations, such as 
multiplying two images together that are not registered. These are largely enforced at *compile time*. This prevents 
the majority of coding errors which can take hours to discover, or remain hidden, in other frameworks. See below for more
information.

Image processing is conducted, under the hood, using the highly optimised libraries employed by PyTorch, with 
Orientation and Voxel-Type safety enforced by .NET wrappers.

## Understanding Compile-Time Orientation Safety

In Flip Proof, an `Image` is made up of an array of voxels (as a variable) and a physical space it occupies and is 
aligned to. Both the voxel type (`TVoxel`) and the space (`TSpace`) form part of its generic type definition: 
`Image<TVoxel, TSpace>`.

For example, an image whose voxels are integers and is aligned to the MNI152 template would be of type
`Image<int, MNI152>`. 

Just as all `Image<int, ...>` are guaranteed to hold integers, all images with the same `TSpace` are guaranteed to be 
aligned. That is, they are guaranteed to have the same:
1. Orientation, resolution, and origin (a 3x4 matrix),
1. Data-ordering (always left, anterior, superior, volume), and 
1. Number of voxels in each dimension (e.g 172 x 196 x 64 x 2)

You should name your spaces tangible orientations, like `NativeCT` or `MNI152`. Note that the space does _not_ define 
the kind of image contained. For example, a PET image aligned and resliced to a patient's CT should be in `NativeCT` 
space:

```csharp
Image<NativeCT, float> reslicedPET = ...
```

In Flip Proof, attempts to create two images of the same `TSpace` but mismatched orientations results in either a 
run-time or compile-time error, depending on the attempt made. 

How this works is best understood by exploring the two ways images can be created: from raw information, or by 
performing image operations.

### Creating Images From Raw Information

Images can be created by combining raw voxel values with orientation information. For example:
1. Reading a NIfTI file
1. Manually specifying voxel and orientation values
1. Casting between spaces (e.g. post-registration)

Although not typically dangerous _per se_, these operations become unsafe when conducted twice, with the assumption 
that the two resulting images will be aligned. For example, if one reads a T1w MR Image "T1.nii", and a mask called 
"T1w-mask.nii", there is no guarantee that the second image is aligned with the first: the second could readily be
a raw CT image that was renamed by accident, or an image aligned to the wrong T1w MR image.

In Flip Proof, all image creation that involved raw information is run-time checked. When the first image of a given 
`TSpace` is constructed, there is typically no known orientation to compare against, and so this operation always passes.
Subsequent image creation operations of this type, however, are checked against this first image. If they do not align
an exception is thrown, preventing `Image` construction.

This guarantees that all `Image` objects of a given `TSpace` have matching orientations.

All operations that create images from raw data carry a compiler warning that it is run-time, not compile time, checked.
You can suppress these warnings by suppressing `obselete` warnings with compiler flags, if you wish.

### Creating Images Through Operations

Most images are not created by disk reads, but by performing mathematical operations on images. For example, an image
might be multiplied by another, have a value subtracted, or be thresholded into a mask. 

In Flip Proof, operations involving two images can only take place if such images have the same `TSpace` (are aligned).
This is checked at _compile time_, rather than at run time. For example, the following will compile because the compiler
knows it is safe:

```csharp
Image<double, MNI152> Add(Image<double, MNI152> im1, Image<double, MNI152> im2)
{
    return im1 + im2;
}
```

Internally, this add operation is adding voxel values to one another, and wrapping the result in a new 
`Image<double, MNI152>` object. As all images of the same `TSpace` are guaranteed to share the same space there is no 
need for a run-time orientation check.

The following code will not compile, because the `TSpace`s are mixed:

```csharp
Image<double, MNI152> Add(Image<double, NativeCT> im1, Image<double, MNI152> im2)
{
    return im1 + im2; // compile time error
}
```

Flip Proof does not support operations that could readily jeopardise image orientations, such as image flipping. Unlike
some frameworks, image voxels are mutable, but image orientations are not.


## Understanding Type Safety

Flip Proof is fully type safe, which means that it is impossible to write impossible operations. For example, while in
vanilla python you might be able to write faulty code such as

```python
def add_image(im1, im2):
    return im1 + im2

add_image(read_image("t1.nii"), "banana")
```

this would not compile when using Flip Proof. Likewise, some frameworks will allow you to attempt to apply operations 
that are impossible through weak static typing, only for run-time checks to cause an exception because the voxel data 
are of the wrong type. This comes about due to methods and parameters accepting `Image` objects without requiring that 
they contain any particular type of voxel data. 

In Flip Proof, `Image` classes include both the space (see above) and voxel type (e.g. `double` or `bool`) they contain.
Resultantly, all methods are compile-time checked for correct usage. Note that, due to limitations of C#, there are 
concrete classes for each type that allow operators to work. For example `ImageDouble<TSpace>` derives from
`Image<double,TSpace>` to allow certain operators to be defined.

For example, this method will only accept masks and attempts to use it with non-boolean images will prevent compilation:

```csharp
int CountTrueVoxels<TSpace>(Image<bool,TSpace> image) => ...

Image<double, TSpace> im = ...
CountTrueVoxels(im); // Compilation error

```

Likewise, operators are compile-time checked:

```csharp
ImageBool<T1> myMask = ...
ImageDouble<T1> myImage = ...
ImageDouble<T1> mySecondImage = ...

var divided = myImage / mySecondImage; // works
var masked = myImage * myMask; // works. Double * bool = masked doubles
var unmasked = myImage / myMask; // compilation error - cannot divide ImageDouble<T1> by ImageBool<T1>
```




## Getting Started

Start by defining one or more space you need to use, by inheriting from `ISpace` and implementing its required members.
For example:

```csharp
public sealed class NativeCT : ISpace
{
   public static object LockObj { get; } = new object();

   static ImageHeader? ISpace.Orientation { get; set; }
}
```

You can now create images via NIfTI reads:
```csharp
ImageFloat<NativeCT> myIm = Nifti.NiftiReader.ReadToFloat<NativeCT>(@"C:\path\to\image.nii.gz");
ImageBool<NativeCT> myMask = Nifti.NiftiReader.ReadToBool<NativeCT>(@"C:\path\to\mask.nii.gz");
```

...and interact with them naturally:

```csharp
ImageFloat<NativeCT> maskedIm = myIm * myMask;
```

You can create generic methods that are space invariant using normal generics:

```csharp
Image<float, TSpace> AbsDifference<TSpace>(ImageFloat<TSpace> im1, ImageFloat<TSpace> im2) where TSpace:ISpace
{
    return (im1 - im2).Abs();
}
```


## Memory Management

Images that are not disposed are cleaned up automatically by the GC in time. However, when working with temporary images 
that may be large, consider disposing them to free memory eagerly. For example:

```csharp
Image<float, TSpace> AbsDifference<TSpace>(ImageFloat<TSpace> im1, ImageFloat<TSpace> im2) where TSpace:ISpace
{
    using var diff = im1 - im2;
    return diff.Abs();
}
```

Avoid `using` Torch dispose scopes in your code as these will corrupt non-disposed images that you have not disposed of.


## Disclaimer

### Subject to change

Flip Proof is currently in Alpha and all public interfaces are subject to change without notice. 

### Not a registered medical device

Flip Proof and its associated code is not a registered medical device and has not undergone third-party testing or verification of any kind. 

Efforts have been made to ensure safety and correctness of outputs. These can be checked by running the associated unit tests and checking coverage. However, like all frameworks, bugs and limitations will exist and so this framework should be used with caution. Responsibility is on you to verify your pipelines work as expected.

Finally, Flip Proof is designed to prevent common hidden coding errors that can present danger in clinical scenarios, or cause unreliabilty in derived products. While reasonably watertight, it is not 'hack proof' and is designed to be used in good faith. Attempts to subvert its 'safety rails' (for example, using reflection to access private members) will endanger patient safety and suggest this is not the framework for you.
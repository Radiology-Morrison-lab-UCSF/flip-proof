# Flip Proof

Flip Proof is an Orientation and Type Safe Medical Imaging Framework for .NET languages including C#, Python (via IronPython), F#, C++/CLI, and Visual Basic. It is intended to support medical image processing for high-stakes settings such as surgery.

'Type safety' refers to an inability to accidentally mix images of incompatible types (such as dividing by a binary mask). 
'Orientation safety' refers to an inability to accidentaly mix images of different orientations, such as multiplying two images together that are not registered.

Both Type and Orientation safety are enforced at *compile time* via use of Generics. This prevents the majority of coding errors which can take hours to discover, or remain hidden, in other frameworks.

Image processing is conducted, under the hood, using the highly optimised libraries employed by PyTorch, with orientation and type safety enforced by .NET wrappers.


## Disclaimer

### Subject to change

Flip Proof is currently in Alpha and all public interfaces are subject to change without notice. 

### Not a registered medical device

Flip Proof and its associated code is not a registered medical device and has not undergone third-party testing or verification of any kind. 

Efforts have been made to ensure safety and correctness of outputs. These can be checked by running the associated unit tests and checking coverage. However, like all frameworks, bugs and limitations will exist and so this framework should be used with caution. Responsibility is on you to verify your pipelines work as expected.

Finally, Flip Proof is designed to prevent common hidden coding errors that can present danger in clinical scenarios, or cause unreliabilty in derived products. While reasonably watertight, it is not 'hack proof' and is designed to be used in good faith. Attempts to subvert its 'safety rails' (for example, using reflection to access private members) will endanger patient safety and suggest this is not the framework for you.
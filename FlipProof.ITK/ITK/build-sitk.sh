#!/bin/bash
set -e

# Get the operating system name
os_name=$(uname -s)

# There are no binaries available for macOS on a non-intel chip so we have to build
# them ourselves. Yes, they release "anycpu" but it's actually for intel only
# because the itk library is build in c++
if [ -e current-build ]; then
  rm current-build
fi
if [ "$os_name" == "Darwin" ]; then

    file1=$(pwd)/"macOS/SimpleITKCSharpManaged.dll"
    file2=$(pwd)/"macOS/libSimpleITKCSharpNative.dylib"

    # Check if both files exist
    if [ -e "$file1" ] && [ -e "$file2" ]; then
        echo "SimpleITK found for macOS"
    else
        echo Building SimpleITK for macOS. This will takes some time

        here=$(pwd)
        if [ -e macOS/temp ]; then
          # Data already presumably cloned
          cd macOS/temp
        else
          mkdir -p macOS/temp
  
          cd macOS/temp
          git clone --recursive https://github.com/SimpleITK/SimpleITK.git
          
        fi
        
        if [ -e build ]; then
          # Assume built but not copied
          cd build
        else
          brew install mono
          mkdir build
          cd build
          cmake -DWRAP_CSHARP=ON -DWRAP_RUBY=OFF ../SimpleITK/SuperBuild
          make -j 8
        fi
        
        
        cd SimpleITK-build/Wrapping/CSharp/CSharpBinaries
        cp SimpleITKCSharpManaged.dll "$file1"
        cp libSimpleITKCSharpNative.dylib "$file2"

        xattr -d com.apple.quarantine "$file2"
        
        cd "$here"
        rm -rf macOS/temp
        echo "---Simple ITK Built for Mac---"
    fi
    
    ln -s macOS current-build
    
else
    echo "Using Linux sitk Binary"
    ln -s SimpleITK-2.3.1-CSharp-linux current-build
fi


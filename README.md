# composite sketch recognition

Matching facial photographs to composite sketches.

The software retreive the photos closest to the composite sketch given in input.

Steps:
+ Use Haar-like features to detect face and facial landmarks.
+ Compute HOG and Sift descriptors of hair, brow, eyes, nose, mouth.
+ Use euclidean distance of descriptors to sort photos.

# Quick start

> The first time you run a search it would take much time because the program has to build descriptors.
> Descriptors will be saved as _descriptors.bin_, in the _bin_ directory.

1) Populate the _database_ directory.

    + If you want to use your database:
    
      Please check the paths and file extentions specified as constants in _ImageRetreivalSystem.cs_

    + If you want to use the suggested database:

        The file _database-link.txt_ contains links to suggested database. The sub-directories of _UoM-SGFS-v2_ have txt files containing the list of used files.
        
        Please, note that only frontal face images without duplicates have been kept, so, rely on images list.

        Dir _Sketches/Set A_ and _Sketches/Set B_ contains only selected images from **UoM-SGFS**.

        Dir _Photos/Images_ contains only selected images from **FERET**.

        Dir _Photos/Others_ contains selected images from **CUFS** and **PRIP-HDC**.

        

2) Run with Visual Studio

    Required libraries in _bin_ are already linked in project files.
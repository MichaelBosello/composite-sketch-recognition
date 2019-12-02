# composite sketch recognition

Matching facial photographs to composite sketches.

The software retreive the photos closest to the composite sketch given in input.

Steps:
+ Use Haar-like features to detect face and facial landmarks.
+ Compute HOG and Sift descriptors of hair, brow, eyes, nose, mouth.
+ Use euclidean distance of descriptors to sort photos.

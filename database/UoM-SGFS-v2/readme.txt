-------------------------------------------------------------
-------------------------------------------------------------
UoM-SGFS Database (http://wp.me/P6CDe8-4q, http://goo.gl/KYeQxt)
-------------------------------------------------------------
-------------------------------------------------------------

The University of Malta Software-Generated Face Sketch (UoM-SGFS) database described and used in [1] contains 1200 viewed software-generated composite sketches of 600 subjects created using the EFIT-V software. This version is an extension of the original UoM-SGFS database [2] which contained 300 subjects.

This archive contains 3 folders, as elaborated shortly:
- Photos, containing the filenames of the photos used from the Color FERET database and the faducial points
- Sketches, containing the EFIT-V-generated sketches and the faducial points
- Additional Data, containing meta-data about the subjects and images used.

As described in the paper [1], there are two sets of sketches (two sketches per subject, for a total of 1200 sketches): 
- Set A, containing the sketches generated using EFIT-V [3]
- Set B, containing sketches in Set A that were modified using image editing software to make them closer in appearance to the true photographs.

Sketches intentionally include several deformations, destortions and inaccuracies to mimic sketches obtained in real-life forensic investigations. Therefore, Set A is arguably a better reflection of real-life sketches due to the greater amount of imperfections than sketches in Set B.

The EFIT-V operator was trained by a qualified forensic scientist from the Malta Police Force [4] so as to ensure that practices adopted in real-life were also used in the creation of the UoM-SGFS database.

Photos have been obtained from the Color FERET database [5]; hence, only the names of files are provided. The pictures were chosen such that subjects are in a frontal pose and with a neutral expression, similar to the approach adopted for the CUFSF database [6].

Faducial points (of the eye and mouth centres) are also included for both photos and sketches. Faducial points of sketches in Set A are very similar to the faducial points of sketches in Set B.

Additional data is also included in both .mat (MATLAB) and .xlsx (Microsoft Excel) formats, containing:
- subject ID's, 
- file names of photos used from the Color FERET database, 
- gender of subjects as provided in the Color FERET database, 
- race of subjects as provided in the Color FERET database.

-------------------------------------------------------------
-------------------------------------------------------------
NOTE: By using the contents in this zip file, you agree not to disseminate any files contained to other persons or entities (i.e. it is for personal, non-commercial use only), and you must reference the below papers in any work utilising the provided data:

C. Galea and R. A. Farrugia, "Matching Software-Generated Sketches to Face Photos with a Very Deep CNN, Morphed Faces, and Transfer Learning," IEEE Transactions on Information Forensics and Security, vol. 13, no. 6, pp. 1421-1431, Jun. 2018.

BibTeX:
@article{Gal18,
author={Galea, C. and Farrugia, R. A.},
title={Matching Software-Generated Sketches to Face Photos with a Very Deep CNN, Morphed Faces, and Transfer Learning},
journal={IEEE Transactions on Information Forensics and Security},
year={2018},
volume={13},
number={6},
pages={1421-1431},
month={June},
}

C. Galea and R.A. Farrugia, "A Large-Scale Software-Generated Face Composite Sketch Database," in Proceedings of the 15th International Conference of the Biometrics Special Interest Group (BIOSIG), Sep. 2016.

BibTeX:
@incollection{Gal16,
author={Galea, C. and Farrugia, R. A.},
title={A Large-Scale Software-Generated Face Composite Sketch Database},
booktitle={Proceedings of the 15th International Conference of the Biometrics Special Interest Group (BIOSIG)},
year={2016},
month={September},
}

-------------------------------------------------------------
-------------------------------------------------------------

Further details may be found in [1], [2] and on the databse websites, http://wp.me/P6CDe8-4q and http://goo.gl/KYeQxt .

If you have any queries, please contact: 
Christian Galea, christian.galea.09@um.edu.mt
or
Reuben A. Farrugia, reuben.farrugia@um.edu.mt

-------------------------------------------------------------
-------------------------------------------------------------

References:

[1] C. Galea and R. A. Farrugia, "Matching Software-Generated Sketches to Face Photos with a Very Deep CNN, Morphed Faces, and Transfer Learning," IEEE Transactions on Information Forensics and Security, accepted for publication.

[2] C. Galea and R. A. Farrugia, "A Large-Scale Software-Generated Face Composite Sketch Database," in Proceedings of the 15th International Conference of the Biometrics Special Interest Group (BIOSIG), Sep. 2016.

[3] VisionMetric, About EFIT-V. [Online]. Available: http://www.visionmetric.com/products/about-efit-v/

[4] Malta Police Force, Forensic Science Laboratory (FSL). [Online]. Available: http://www.pulizija.gov.mt/en-us/forensicscience.aspx

[5] ColorFERET database, available at: http://www.nist.gov/itl/iad/ig/colorferet.cfm

[6] CUFSF database, available at: http://mmlab.ie.cuhk.edu.hk/archive/cufsf/

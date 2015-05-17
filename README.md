# ED-UA
Elite Dangerous unknown artifact sound pattern analysis program

Imports label exports from Audacity to help identify patterns in sound recording of the unknonw artefact in Elite Dangerous.

- Imports required in tab delimited format as example below (This is the default format of a label export from audacity.)
- Elements can be in any order and are sorted within the application after import

Supported tags
--------------
Audio elements: include timestamp data
px  - identifies a purr, where x is a single digit numeric identifier, typically 1-4
pxq - identifies a purr as above, including a tag indiciating the sample is reduced amplitude
c   - identifies a chitter
hx  - identifies a howl, where x is either 1 or 2.

Metadata: no timestamp recorded.
a:x - identifies the author of the sequence, where x is a string of any length 
d:x - A description of the recording where x is a string of any length         
s:  - Identifies the system the recording was made in                          
t:  - Identifies the data and time of the recording in dd/mm/yy hh:mm format   

Example import file:

0.000000	0.000000	A: DigitalScream
0.000000	0.000000	D: UA_1
9.742372	14.214849	h1
14.214849	18.929621	c
41.675930	41.675930	p1q
45.142930	45.142930	p4
48.817930	48.817930	p1
53.047930	53.047930	p4
56.791930	56.791930	p1
60.813930	60.813930	p4
64.627930	64.627930	p4
72.393930	72.393930	p4q
75.999930	75.999930	p1
80.159930	80.159930	p4
83.973930	83.973930	p4
87.648930	87.648930	p1
91.739930	91.739930	p1
95.692930	95.692930	p4
103.666930	103.666930	p1
107.687930	107.687930	p4

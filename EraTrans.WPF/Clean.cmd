if not exist DLL\ md DLL\
if not exist Tool\ md Tool\
del *.pdb
move EncodingConversion.* Tool\
move readme.txt Tool\
move *.dll DLL\
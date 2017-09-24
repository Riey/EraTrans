
if not exist DLL\ md DLL\
move *.dll DLL\
move *.pdb DLL\

if not exist Tool\ md Tool\
move EncodingConversion.* Tool\
move readme.txt Tool\

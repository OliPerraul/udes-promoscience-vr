#!/usr/bin/python

import os

src = '/usr/bin/python'
dst = '/tmp/python'

rootdir = 'C:/Users/sid/Desktop/test'
dest = 'C:/Users/sid/Desktop/test'

for subdir, dirs, files in os.walk(rootdir):
    for file in files:
        
        fullname = os.path.join(subdir, file)

        # This creates a symbolic link on python in tmp directory
        os.symlink(fullname, dest)


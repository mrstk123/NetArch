#!/bin/bash

# Find and delete all .keep files in the current directory and subdirectories
find . -type f -name ".keep" -delete

echo "Cleaned all .keep files."

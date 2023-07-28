#!/bin/bash
<<<<<<< HEAD
cp -r /home/coder/project/workspace/nunit/test/WebAppTest /home/coder/project/workspace/dotnetapp;
cd /home/coder/project/workspace/dotnetapp;
dotnet test
=======
if [ -d "/home/coder/project/workspace/dotnetapp/" ]
then
    echo "project folder present"
    # checking for src folder
    if [ -d "/home/coder/project/workspace/dotnetapp/" ]
    then
        cp -r /home/coder/project/workspace/nunit/test/TestProject /home/coder/project/workspace/dotnetapp/;
    cd /home/coder/project/workspace/dotnetapp/TestProject || exit;
     dotnet clean;    
     dotnet test;
    else
        echo "test_case1 FAILED";
        echo "test_case2 FAILED";
	    echo "test_case3 FAILED";
    fi
else   
    echo "test_case1 FAILED";
    echo "test_case2 FAILED";
    echo "test_case3 FAILED";
fi
>>>>>>> 70bd1c282c644dabd8dbdb34e79df8891d95d9f8

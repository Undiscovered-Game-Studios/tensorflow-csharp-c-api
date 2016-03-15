rm wrap/*.cs
swig -csharp -c++ -features directors -outdir ./wrap -o wrapcxx/tensor_c_api_wrap.cc -I. -I../tensorflow tensor_c_api.i
mcs -target:library -out:bin/tensor_c_api.dll wrap/*.cs
g++ -c -fpic -std=c++11 callback.cc deallocator.cc wrapcxx/tensor_c_api_wrap.cc -I. -I../tensorflow
g++ -shared callback.o deallocator.o tensor_c_api_wrap.o -L../tensorflow/bazel-bin/tensorflow -ltensorflow -o bin/libtensorflow_c_api.so


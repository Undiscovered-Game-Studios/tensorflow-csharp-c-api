# tensorflow-csharp-c-api
Port of the tensorflow c api to csharp

Adaptation of this :
https://medium.com/jim-fleming/loading-tensorflow-graphs-via-host-languages-be10fd81876f
to CSharp using SWIG

Status :
Example Graph Successfully Runs
Still a few TODOs before fully fonctional


Prerequisites:
tensorflow (so you should already have swig and g++)

Install instructions (ubuntu 14.04) :
Clone tensorflow-csharp-c-api repository
cd to tensorflow-csharp-c-api repository
look and adapt build.sh so that its path to tensorflow are correct
run build.sh

The generated libraries should be in the bin folder

Add reference to the generated dll
Build csharp project

Create symlinks or copy the libtensorflow_c_api.so and libtensorflow.so alongside the binary exe so that it can load the dll

Look at example project for usage


Alternatively you could use Bazel by copying it into the tensorflow/tensorflow directory
generate wrapper with swig
compile all cs into a dll
then bazel build :all to build the shared library





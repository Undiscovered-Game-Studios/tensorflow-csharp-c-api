
%module(directors="1") tensorflow_c_api
%{
#include "tensorflow/core/public/tensor_c_api.h"
#include "deallocator.h"
#include "callback.h"
%}

%include "carrays.i"

%include <arrays_csharp.i>

CSHARP_ARRAYS(char *, string)
%typemap(imtype, inattributes="[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, ArraySubType=UnmanagedType.LPStr)]") char *INPUT[] "string[]"

%apply char *INPUT[]  { char ** }


%apply long long INPUT[]  { long long* }

%array_functions(TF_Tensor*, tensorArray);

//TODO:use a cleaner csharparray interface
//this is the carrays interface from swig
//It is not as clean as we would like
//But I tried using a managed array approach using CSHARP_ARRAYS, it compiles but crash upon marshalling 
//Because SWIGTYPE_p_TF_Tensor doesn't have a Stuct Layout attribute because it is a generated wrapper by swig
//We would like to marshall it as an array containing the pointers : SWIGTYPE_p_TF_Tensor.getCPtr()

//CSHARP_ARRAYS(TF_Tensor *, SWIGTYPE_p_TF_Tensor)
//%typemap(imtype, inattributes="[In, MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0, ArraySubType=UnmanagedType.ByValArray)]") TF_Tensor *INPUT[] "SWIGTYPE_p_TF_Tensor[]"
//%apply TF_Tensor *INPUT[]  { TF_Tensor** }

%feature("director") Callback;


//Copy pasted from the web
%define %cs_marshal_intptr(TYPE, ARGNAME...)
        %typemap(ctype)  TYPE ARGNAME "void*"
        %typemap(imtype) TYPE ARGNAME "IntPtr"
        %typemap(cstype) TYPE ARGNAME "IntPtr"
        %typemap(in)     TYPE ARGNAME %{ $1 = ($1_ltype)$input; /* IntPtr */ %}
        %typemap(csin)   TYPE ARGNAME "$csinput"
       
        %typemap(out)    TYPE ARGNAME %{ $result = $1; %}
        %typemap(csout, excode=SWIGEXCODE) TYPE ARGNAME {
                IntPtr cPtr = $imcall;$excode
                return cPtr;
        }
        %typemap(csvarout, excode=SWIGEXCODE2) TYPE ARGNAME %{
                get {
                        IntPtr cPtr = $imcall;$excode
                        return cPtr;
                }
        %}

        %typemap(ctype)  TYPE& ARGNAME "void**"
        %typemap(imtype) TYPE& ARGNAME "ref IntPtr"
        %typemap(cstype) TYPE& ARGNAME  "ref IntPtr"
        %typemap(in)     TYPE& ARGNAME %{ $1 = ($1_ltype)$input; %}
        %typemap(csin)   TYPE& ARGNAME "ref $csinput"
%enddef 

%cs_marshal_intptr(void*);

%include "tensorflow/core/public/tensor_c_api.h"
%include "deallocator.h"
%include "callback.h"

//tensorflow/tensor_c_api/



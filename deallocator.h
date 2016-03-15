#ifndef DEALLOCATOR_H
#define DEALLOCATOR_H

#include "tensorflow/core/public/tensor_c_api.h"
#include "callback.h"

extern TF_Tensor* TF_NewTensorAllocated(TF_DataType dt, long long* dims, int num_dims);

extern TF_Tensor* TF_NewTensorCB(TF_DataType dt, long long* dims, int num_dims,
                               void* data, size_t len,
                               Callback* cb);

#endif

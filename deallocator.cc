#include "tensorflow/core/public/tensor_c_api.h"
#include <vector>
//#include "tensorflow/core/framework/tensor.h"
#include "tensorflow/tensor_c_api/deallocator.h"
#include <cstdlib>
#include <cstdint>

static void Deallocator(void* data, size_t, void* arg) {
	//std::cout<< "Deallocator free" << std::endl;
	std::free(data);
  //tensorflow::cpu_allocator()->DeallocateRaw(data);
  *reinterpret_cast<bool*>(arg) = true;
}

TF_Tensor* TF_NewTensorAllocated(TF_DataType dt, long long* dims, int num_dims) 
{
int nbElem = 1;
for( int i = 0 ; i < num_dims ; i++)
	nbElem *= dims[i];
unsigned int len = 0;
void* data = NULL;
switch( dt )
{
  case TF_FLOAT :
		len = nbElem * sizeof(float);
		data = std::malloc( len );
	break;
  case TF_DOUBLE :
		len = nbElem * sizeof(double);
		data = std::malloc( len );		
	break;
  case TF_INT32 :
		len = nbElem * sizeof(double);
		data = std::malloc( len );	
	break;  
  case TF_UINT8 :
		len = nbElem * sizeof(uint8_t);
		data = std::malloc( len );	
	break;
  case TF_INT16 :
		len = nbElem * sizeof(int16_t);
		data = std::malloc( len );	
	break;
  case TF_INT8 :
		len = nbElem * sizeof(int8_t);
		data = std::malloc( len );	
	break;
  case TF_STRING :
		//TODO
	break;
  case TF_COMPLEX :
		//TODO
	break;
  case TF_INT64 :
		len = nbElem * sizeof(int64_t);
		data = std::malloc( len );	
	break;
  case TF_BOOL :
		len = nbElem * sizeof(bool);
		data = std::malloc( len );
	break;  
	case TF_QINT8 :

	break;
  case TF_QUINT8 :

	break;
  case TF_QINT32 :

	break;
  case TF_BFLOAT16 :

	break;
  case TF_QINT16 :

	break;
  case TF_QUINT16 :

	break;
  case TF_UINT16 :
  	len = nbElem * sizeof(uint16_t);
		data = std::malloc( len );	
	break;
}

/*
float* values =
      reinterpret_cast<float*>(tensorflow::cpu_allocator()->AllocateRaw(
          EIGEN_MAX_ALIGN_BYTES, 10 * sizeof(float)));
  tensorflow::int64 dims2[] = {10};
*/

  bool deallocator_called = false;
  TF_Tensor* t = TF_NewTensor(dt, dims,num_dims, data, len,
                              &Deallocator, &deallocator_called);
	
	return t;
}

void callbackDeallocator(void* data, size_t len,
                                                   void* arg)
{
	//std::cout << "callbackDeallocator " << arg << std::endl;
	((Callback*)arg)->run();
}

// memory managed by something like numpy.
TF_Tensor* TF_NewTensorCB(TF_DataType dt, long long* dims, int num_dims,
                               void* data, size_t len,
                               Callback* cb)
{
return TF_NewTensor(dt,dims,num_dims,data,len, callbackDeallocator,cb); 
}

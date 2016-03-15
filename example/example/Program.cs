using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace example
{
    //To create a callback which will be called in csharp upon deallocation
    //Just inherit from Callback
    public class CSharpCallback : Callback
    {
        public CSharpCallback()
            : base()
        {
        }

        public override void run()
        {
            Console.WriteLine("CSharpCallback.run()");
            //In this simple callback we don't release memory
        }
    }

    class MainClass
    {
        public static void Main(string[] args)
        {
            var options = tensorflow_c_api.TF_NewSessionOptions();

            var status = tensorflow_c_api.TF_NewStatus();
            var session = tensorflow_c_api.TF_NewSession(options, status);
            if (tensorflow_c_api.TF_GetCode(status) == TF_Code.TF_OK)
                Console.WriteLine("OK");
            else
                Console.WriteLine("Not OK");

            byte[] data = File.ReadAllBytes("../../models/graph.pb");

            IntPtr ptr = Marshal.AllocHGlobal(data.Length);
            Marshal.Copy(data,0,ptr,data.Length);


           
            var tens = new float[10];
            for (int i = 0; i < tens.Length; i++)
                tens[i] = (float)(5+i);
            
            IntPtr ptrTensor = Marshal.AllocHGlobal(tens.Length*sizeof(float) );
            Marshal.Copy(tens,0,ptrTensor,tens.Length);

            try
            {
                
                var dims = new long[]{tens.Length};
                //This callbacks are called back upon destruction of the tensor so that we can handle release the memory in csharp
                //You should make sure this callback object doesn't get garbage collected before it is called
                var cb = new CSharpCallback();

                //We can use two tensors pointing to the same array
                var TFtens = tensorflow_c_api.TF_NewTensorCB(TF_DataType.TF_FLOAT, dims, 1,ptrTensor,(uint)(tens.Length*sizeof(float)), cb);

                //Alternatively we can use the library to allocate and release memory directly
                //But this mean additional copies
                var TFtens2 = tensorflow_c_api.TF_NewTensorAllocated(TF_DataType.TF_FLOAT,dims,1);
                //The tensor data is allocated but not yet initialized
                var tens2IntPtr = tensorflow_c_api.TF_TensorData( TFtens2 );
                Marshal.Copy(tens,0,tens2IntPtr,tens.Length);

                //We construct the graph from the binary proto file
                tensorflow_c_api.TF_ExtendGraph( session,ptr,(uint)(data.Length),status );

                if (tensorflow_c_api.TF_GetCode(status) == TF_Code.TF_OK)
                {
                    Console.WriteLine("Graph Creation OK");
                }
                else
                {
                    Console.WriteLine("Graph Creation Not OK");
                    return;
                }
                    
                //We create an array of tensors 
                //This array will have to be deleted with
                //tensorflow_c_api.delete_tensorArray
                var inputs = tensorflow_c_api.new_tensorArray(2);
                tensorflow_c_api.tensorArray_setitem(inputs, 0, TFtens);
                //Note that we have two different tensors as input
                //using tensorflow_c_api.tensorArray_setitem(inputs, 1, TFtens);
                //would result in an error upon TF_RUN which delete its inputs
                tensorflow_c_api.tensorArray_setitem(inputs, 1, TFtens2);
                var outputs = tensorflow_c_api.new_tensorArray(1);

                //We run the "i" OpDef to initialize the load the shared variables into the variables nodes
                //This OpDef was created in python with tf.initialize_variables(tf.all_variables(),name = 'i')

                tensorflow_c_api.TF_Run( session, new string[0],inputs,0,new string[0],outputs,0,new string[]{"i"},1,status);
                if (tensorflow_c_api.TF_GetCode(status) == TF_Code.TF_OK)
                    Console.WriteLine("Init Run OK");
                else
                {
                    Console.WriteLine("Init Run Not OK");
                    Console.WriteLine( tensorflow_c_api.TF_Message( status) );
                    return;
                }

                //We run the graph which uses the previously initialized variables 
                tensorflow_c_api.TF_Run( session, new string[0]{} ,inputs,0,new string[]{"output"},outputs,1,new string[0],0,status);
                if (tensorflow_c_api.TF_GetCode(status) == TF_Code.TF_OK)
                    Console.WriteLine("Run using initialized variable OK");
                else
                {
                    Console.WriteLine("Run using initialized variable Not OK");
                    Console.WriteLine( tensorflow_c_api.TF_Message( status) );
                    return;
                }

                {
                    var outtensor = tensorflow_c_api.tensorArray_getitem(outputs,0);
                    var resu = tensorflow_c_api.TF_TensorData( outtensor );
                    var resuf = new float[10];
                    Marshal.Copy(resu,resuf,0,10);
                    Console.WriteLine("The result from the execution of the graph is : resu[3] : {0}",resuf[3]);
                    //We release the tensor which now is under our responsibility
                    tensorflow_c_api.TF_DeleteTensor( outtensor );
                }
                    
                //We run the graph feeding it some inputs
                //It will call TF_DeleteTensor on the inputs
                tensorflow_c_api.TF_Run( session, new string[2]{ "a","b" } ,inputs,2,new string[]{"output"},outputs,1,new string[0],0,status);
                if (tensorflow_c_api.TF_GetCode(status) == TF_Code.TF_OK)
                    Console.WriteLine("Run feeding input variables OK");
                else
                {
                    Console.WriteLine("Run feeding input variables Not OK");
                    Console.WriteLine( tensorflow_c_api.TF_Message( status) );
                    return;
                }

                {
                    var outtensor = tensorflow_c_api.tensorArray_getitem(outputs,0);
                    var resu = tensorflow_c_api.TF_TensorData( outtensor );
                    var resuf = new float[10];
                    Marshal.Copy(resu,resuf,0,10);
                    Console.WriteLine("The result from the execution of the graph is : resu[3] : {0}",resuf[3]);
                    tensorflow_c_api.TF_DeleteTensor( outtensor );
                }

                //We delete the tensors
                tensorflow_c_api.delete_tensorArray(inputs);
                tensorflow_c_api.delete_tensorArray(outputs);

            }
            finally
            {
                //We release The binary protobuf of the graph
                Marshal.FreeHGlobal(ptr);
                //We release Our tensor data
                Marshal.FreeHGlobal(ptrTensor);
            }



           

        }
    }
}

cc_binary(
    name = "libtensorflow_c_api.so",
    srcs = ["wrapcxx/tensor_c_api_wrap.cc",
						"wrapcxx/tensor_c_api_wrap.h",
						"deallocator.cc",
						"deallocator.h",
						"callback.cc",
						"callback.h"],
		linkshared = 1,
    deps = [
        "//tensorflow/core:tensorflow",
    ],
)


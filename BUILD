cc_binary(
    name = "libtensorflow_c_api.so",
    srcs = ["tensor_c_api_wrap.cc",
						"tensor_c_api_wrap.h",
						"deallocator.cc",
						"deallocator.h",
						"callback.cc",
						"callback.h"],
		linkshared = 1,
    deps = [
        "//tensorflow/core:tensorflow",
    ],
)


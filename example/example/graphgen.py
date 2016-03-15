import tensorflow as tf
import numpy as np

with tf.Session() as sess:
    a = tf.Variable(np.arange(10).astype(np.float32), name="a")
    b = tf.Variable(np.arange(10).astype(np.float32), name="b")
    #d = tf.Variable(np.arange(10000000), name='d')
    #a = tf.constant(5.0)
    #b = tf.constant(6.0)
    c = tf.mul(a, b, name="output")
    sess.run( tf.initialize_variables(tf.all_variables(),name = 'i'))
		
    print a.eval() # 5.0
    print b.eval() # 6.0
    print c.eval() # 30.0
    
    tf.train.write_graph(sess.graph_def, 'models', 'graph.pb', as_text=False)

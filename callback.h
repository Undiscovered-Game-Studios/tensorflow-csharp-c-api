#ifndef _CALLBACK_H
#define _CALLBACK_H
#include <cstdio>
#include <iostream>

class Callback {
public:
	virtual ~Callback(); 
	virtual void run();
};


#endif


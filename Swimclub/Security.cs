using System;
using System.Collections.Generic;
using System.Text;

namespace Swimclub
{
	public static class Security
	{
		//FLOW FOR HASHING A PASSWORD
		//HASH THE PLAINTEXT PASSWORD WITH THE USERNAME
		//HASH(USERNAME, PASSWORD) = pass1
		//THAT CAN BE USED FOR SENDING OVER THE NETWORK.
		//FOR DATABASE STORAGE HOWEVER, THE pass1 IS NOT ENOUGH
		//THE SALT IS BASED ON THE pass1
	}
}

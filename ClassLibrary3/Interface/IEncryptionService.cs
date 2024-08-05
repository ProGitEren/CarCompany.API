using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface
{
    public interface IEncryptionService
    {
       string Encrypt(string plainText);   
        
       string Decrypt(string cipherText);
        
    }
}

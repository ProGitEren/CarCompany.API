using Microsoft.AspNetCore.DataProtection.XmlEncryption;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class TransferAddress
    {
        public string Name { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int ZipCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EvidencijaPacijenata.Models
{
    public static class EncryptPass
    {
        public static string strmsg = String.Empty;

        public static string EncryptFunc(string Lozinka) {
            byte[] encode = new byte[Lozinka.Length];
            encode = Encoding.UTF8.GetBytes(Lozinka);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        } 
    }
}
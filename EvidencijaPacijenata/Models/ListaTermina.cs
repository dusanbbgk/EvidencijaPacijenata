using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EvidencijaPacijenata.Models
{
    public class ListaTermina
    {
        private List<string> lista;
        public ListaTermina() {
            lista = new List<string>();
        }

        public List<string> Lista { get => lista; set => lista = value; }
    }
}
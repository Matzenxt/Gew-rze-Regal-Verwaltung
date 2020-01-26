using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gewürze_Regal_Verwaltung.Common
{
    public class Verpackung
    {
        private string Bezeichnung;
        private int Witdh;

        public Verpackung(string bezeichnung, int witdh)
        {
            this.Bezeichnung = bezeichnung;
            this.Witdh = witdh;
        }

        public string getBezeichnung()
        {
            return this.Bezeichnung;
        }

        public int getWitdh()
        {
            return this.Witdh;
        }
    }
}

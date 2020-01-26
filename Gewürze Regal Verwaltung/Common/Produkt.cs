using Gewürze_Regal_Verwaltung.Common;
using Gewürze_Regal_Verwaltung.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gewürze_Regal_Verwaltung
{
    class Produkt
    {
        private string Name;
        private int Weight;
        private Kategorie.Kategorien kategorie;
        private Verpackung Verpackung;

        public Produkt(string name, int weight, Kategorie.Kategorien kategorie, Verpackung verpackung)
        {
            this.Name = name;
            this.Weight = weight;
            this.kategorie = kategorie;
            this.Verpackung = verpackung;
        }

        public string getName()
        {
            return this.Name;
        }

        public int getWeight()
        {
            return this.Weight;
        }

        public Verpackung GetVerpackung()
        {
            return this.Verpackung;
        }

        public Kategorie.Kategorien GetKategorie()
        {
            return this.kategorie;
        }

        public override bool Equals(object obj)
        {
            var item = obj as Produkt;

            if (item == null)
            {
                return false;
            }

            return this.Name.Equals(item.Name) && this.Verpackung.getBezeichnung().Equals(item.GetVerpackung().getBezeichnung());
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
    }
}

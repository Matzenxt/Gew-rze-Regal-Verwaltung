using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gewürze_Regal_Verwaltung.Common
{
    class Regal
    {
        private string bezeichnung;
        private int witdh;
        private int rowCount;
        private List<Produkt> allProdukts;
        private List<Produkt>[] produktsPerRow;
        private List<Produkt> produktOverFlow;

        public Regal(string bezeichnung, int witdh, int rowCount)
        {
            this.bezeichnung = bezeichnung;
            this.witdh = witdh;
            this.rowCount = rowCount;
            this.produktsPerRow = new List<Produkt>[this.rowCount];

            for(int i = 0; i < rowCount; i++)
            {
                produktsPerRow[i] = new List<Produkt>();
            }

            this.produktOverFlow = new List<Produkt>();
        }

        public void fillRegal()
        {
            this.produktOverFlow = new List<Produkt>();

            int currentWitdh = 0;
            int rowIndex = 0;

            foreach (Produkt produkt in allProdukts) {
                if (rowIndex >= rowCount)
                {
                    produktOverFlow.Add(produkt);
                }
                else
                {
                    if ((currentWitdh + produkt.GetVerpackung().getWitdh()) <= witdh)
                    {
                        produktsPerRow[rowIndex].Add(produkt);
                        currentWitdh += produkt.GetVerpackung().getWitdh();
                    }
                    else
                    {
                        rowIndex++;
                        if(rowIndex >= rowCount)
                        {
                            produktOverFlow.Add(produkt);
                        }
                        else
                        {
                            produktsPerRow[rowIndex].Add(produkt);
                            currentWitdh = produkt.GetVerpackung().getWitdh();
                        }
                    }
                }
            }
        }

        public void printRows()
        {
            int currentRow = 0;

            foreach(List<Produkt> row in produktsPerRow)
            {
                Console.WriteLine("Reihe: " + (currentRow + 1));
                
                foreach(Produkt produkt in row)
                {
                    Console.Write(produkt.getName() + ", " + produkt.GetVerpackung().getBezeichnung() +  " | ");
                }

                Console.WriteLine();
                currentRow++;
            }
        }

        public void printOverFlow()
        {
            Console.WriteLine("Produkte die nicht in das Regal passen:");

            foreach(Produkt produkt in produktOverFlow)
            {
                Console.WriteLine(" - " + produkt.getName() + ", " + produkt.GetVerpackung().getBezeichnung());
            }
        }

        public int calcSpaceNeeded()
        {
            int spaceNeeded = 0;

            foreach(Produkt produkt in produktOverFlow)
            {
                spaceNeeded += produkt.GetVerpackung().getWitdh();
            }

            return spaceNeeded;
        }

        public int clacBödenNeeded()
        {
            int currentWidth = 0;
            int anzahlBöden = 0;

            if(produktOverFlow.Count > 0)
            {
                anzahlBöden = 1;
            }

            foreach(Produkt produkt in produktOverFlow)
            {
                if(currentWidth + produkt.GetVerpackung().getWitdh() < witdh)
                {
                    currentWidth = currentWidth + produkt.GetVerpackung().getWitdh();
                }
                else
                {
                    anzahlBöden++;
                    currentWidth = produkt.GetVerpackung().getWitdh();
                }
            }

            return anzahlBöden;
        }

        public List<Produkt> getProduktOverFlow()
        {
            return this.produktOverFlow;
        }

        public List<Produkt> getProdukte()
        {
            return allProdukts;
        }

        public List<Produkt>[] getProduktsPerRow()
        {
            return produktsPerRow;
        }

        public void readFromExcelFile(string path)
        {
            allProdukts = new List<Produkt>();

            var csvRows = System.IO.File.ReadAllLines(path, Encoding.Default).ToList();
            csvRows.RemoveAt(0);

            for(int offset = 0; offset < csvRows.Count()-2; offset = offset + 2)
            {
                Verpackung verpackung;
                string[] content = csvRows[offset].Split(';');
                string contentVerpackung = csvRows[offset + 1].Split(';')[1];

                Console.WriteLine("Anzahl: " + content[0] + " Name: " + content[1]);
                Console.WriteLine("Verpackung: " + contentVerpackung);

                if (contentVerpackung.Contains("kl. Glas"))
                {
                    verpackung = new Verpackung("Kleines Glas", 48);
                }
                else if (contentVerpackung.Contains("Glas") && !contentVerpackung.Contains("kl."))
                {
                    verpackung = new Verpackung("Glas", 70);
                }
                else if (contentVerpackung.Contains("Beutel"))
                {
                    verpackung = new Verpackung("Beutel", 180);
                }
                else if (contentVerpackung.Contains("Tüte"))
                {
                    verpackung = new Verpackung("Tüte", 180);
                }
                else if (contentVerpackung.Contains("Röhrchen"))
                {
                    verpackung = new Verpackung("Röhrchen", 18);
                }
                else if (contentVerpackung.Contains("Standbodenbeutel"))
                {
                    verpackung = new Verpackung("Standbodenbeutel", 180);
                }
                else
                {
                    verpackung = new Verpackung("FEHLER!!!", 9999);
                    Console.WriteLine("Fehler: Verpackungs Zuordnung!!!");
                    Console.WriteLine(" -> " + contentVerpackung);
                }

                allProdukts.Add(new Produkt(content[1], 0, Common.Types.Kategorie.Kategorien.Chili, verpackung));
            }

            List<Produkt> temp = allProdukts.Distinct().ToList();

            allProdukts = temp;
        }
    }
}

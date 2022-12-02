using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using mikroszimulacio.Entities;

namespace mikroszimulacio
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        public Form1()
        {
            InitializeComponent();

            Population = Load_Population("C:\\Temp\\nép-teszt.csv");
            BirthProbabilities = Load_Birth_Probabilities("C:\\Temp\\születés.csv");
            DeathProbabilities = Load_Death_Probabilities("C:\\Temp\\halál");
        }

        private List<Person> Load_Population(string filePath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');

                    var person = new Person()
                    {
                        BirthYear = int.Parse(sor[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), sor[1]),
                        NbrOfChildren = int.Parse(sor[2])
                    };

                    population.Add(person);
                }
            }

            return population;
        }



        private List<BirthProbability> Load_Birth_Probabilities(string filePath)
        {
            List<BirthProbability> birtprobabilities = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');

                    var BirthProb = new BirthProbability()
                    {
                        Age = int.Parse(sor[0]),
                        Child_count = int.Parse(sor[1]),
                        Probability = decimal.Parse(sor[2])
                    };

                    birtprobabilities.Add(BirthProb);

                }
            }

            return birtprobabilities;
        }



        private List<DeathProbability> Load_Death_Probabilities(string filePath)
        {
            List<DeathProbability> deathprobabilities = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(filePath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var sor = sr.ReadLine().Split(';');

                    var DeathProb = new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), sor[0]),
                        Age = int.Parse(sor[1]),
                        Probability = decimal.Parse(sor[2])
                    };

                    deathprobabilities.Add(DeathProb);
                }
            }

            return deathprobabilities;
        }
    }
}

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
        Random rnd = new Random(69);


        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();

        public Form1()
        {
            InitializeComponent();

            Population = Load_Population("C:\\Temp\\nép-teszt.csv");
            BirthProbabilities = Load_Birth_Probabilities("C:\\Temp\\születés.csv");
            DeathProbabilities = Load_Death_Probabilities("C:\\Temp\\halál.csv");


            for (int year = 2005; year < 2024; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {

                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();

                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();

                Console.WriteLine(string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));
            }
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
                        Probability = double.Parse(sor[2])
                    };

                    deathprobabilities.Add(DeathProb);
                }
            }

            return deathprobabilities;
        }



        private void SimStep(int year, Person person)
        {
            if (!person.IsAlive) return;

            byte age = (byte)(year - person.BirthYear);

            double pDeath = (from x in DeathProbabilities
                              where x.Gender == person.Gender && x.Age == age
                              select x.Probability).FirstOrDefault();

            if(rnd.NextDouble() <= pDeath)
            {
                person.IsAlive = true;
            }

            if (person.IsAlive && person.Gender == Gender.Female)
            {

                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.Probability).FirstOrDefault();

                if (rnd.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rnd.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }
    }
}

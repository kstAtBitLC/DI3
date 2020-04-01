using System;
using System.Collections.Generic;

namespace DependencyInjection_3 {

    #region Main

    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello Arche!");

            Arche a = new Arche(new Alien() { Name = "Ford Prefect" }, Inventory.berechtigte);

            // neue Passagiere
            UnTier ut = new UnTier() { Name = "Khan" };
            a.AddMitreisenden(ut);

            Mensch m1 = new Mensch() { Name = "Donald" };
            a.AddMitreisenden(m1);

            Alien a1 = new Alien() {Name = "Donald"};
            a.AddMitreisenden(a1);

            a.ZeigeAlleMitreisenden();
            System.Console.ReadLine();
        }
    }
    #endregion


    #region Interfaces    


    interface IVerantwortlicher {
        bool BerechtigungPruefen(IBerechtigung berechtigter);
    }

    interface IBerechtigung {
        void Eintreten(Arche arche);
        string ZeigeInfo();
    }


    #endregion


    #region Inventory
    static class Inventory {
        public static List<IBerechtigung> berechtigte = new List<IBerechtigung>();
        public static List<string> noGo = new List<string>() ;

        static Inventory() {
            berechtigte.Add(new Mensch() { Name = "Jim" });
            berechtigte.Add(new Mensch() { Name = "Pille" });
            berechtigte.Add(new Mensch() { Name = "Scotty" });
            berechtigte.Add(new Mensch() { Name = "Ohura" });
            berechtigte.Add(new Alien() { Name = "Spock" });
            berechtigte.Add(new Tier() { Tierart = "Pottwal" });
            berechtigte.Add(new Tier() { Tierart = "Buckelwal" });
            berechtigte.Add(new Mensch() { Name = "Pinocchio" });
            berechtigte.Add(new Tier() { Tierart = "Stubenfliege" });

            // nicht Berechtigte
            noGo.Add("Donald");
            noGo.Add("Hanni");
            noGo.Add("Nanni");
            noGo.Add("Fliwatüt");
            noGo.Add("Der Imperator");
        }
    }
    #endregion


    #region  Arche
    class Arche {
        public string Name { get; set; } = "Noahs Arche";
        public List<IBerechtigung> Berechtigte;
        public List<string> NoGo { get; set; } = Inventory.noGo;
        public IVerantwortlicher Verantwortlicher;

        // Constructor
        public Arche(IVerantwortlicher verantwortlicher, List<IBerechtigung> berechtigte) {
            Verantwortlicher = verantwortlicher;
            Berechtigte = berechtigte;
        }

        public void AddMitreisenden(IBerechtigung passagier) {
            passagier.Eintreten(this);

            if (Verantwortlicher.BerechtigungPruefen(passagier)) {
                Berechtigte.Add((IBerechtigung)passagier);
            }
            else {
                System.Console.WriteLine($"Ein Berechtigter muß berechtigt draußen bleiben! ");
            }
        }


        public void AddMitreisenden(UnTier passagier) {
            Alien a = (Alien)Verantwortlicher;
            System.Console.WriteLine($"{a.Name} sagt: {passagier.Name} muß draußen bleiben! ");
        }

        public void ZeigeAlleMitreisenden() {
            foreach (var berechtigter in Berechtigte) {
                System.Console.WriteLine(berechtigter.ZeigeInfo());
            }
        }

    }
    #endregion


    #region TierUndUntier

    class Tier : IBerechtigung {
        public string Tierart { get; set; }
        public IVerantwortlicher Verantwortlicher { get; set; }
        public Arche Zufluchtsort { get; set; }


        public void Eintreten(Arche arche) {
            Zufluchtsort = arche;
        }

        public string ZeigeInfo() {
            return this.Tierart;
        }

    }

    class UnTier {
        public string Name { get; set; }
        public IVerantwortlicher Verantwortlicher { get; set; }

        public void FahrscheinZeigen(IVerantwortlicher iv) {
            Verantwortlicher = iv;
        }

        public void Eintreten(Arche arche) {

        }

        public string ZeigeInfo() {
            return this.Name;
        }

    }
    #endregion


    #region  Mensch und Alien

    class Mensch : IBerechtigung {

        public string Name { get; set; }
        public IVerantwortlicher Verantwortlicher { get; set; }
        public Arche Zufluchtsort { get; set; }

        public void Eintreten(Arche arche) {
            Zufluchtsort = arche;
        }

        public string ZeigeInfo() {
            return this.Name;
        }
    }




    class Alien :  IBerechtigung, IVerantwortlicher {

        public string Name { get; set; }
        public new IVerantwortlicher Verantwortlicher { get; set; }
        public  Arche Zufluchtsort { get; set; }

        public new void Eintreten(Arche arche) {
            Zufluchtsort = arche;
        }

        public bool BerechtigungPruefen(IBerechtigung berechtigter) {
            //string name=null;
            Mensch m=null;
            bool der = true;

            if (berechtigter is Mensch) {
                m = (Mensch)berechtigter;

                if (m.Zufluchtsort.NoGo.Contains(m.Name)) {
                    der = false;
                } 
            }
            return der;
        }

        public string ZeigeInfo() {
            return this.Name;
        }
    }
    #endregion
}
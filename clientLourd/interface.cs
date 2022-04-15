using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;


namespace clientLourd
{
    public static class Interface
    {
        //Ca c'est le menu principal 
        public static int MenuPrincipal()
        {
            Console.Clear();
            Console.WriteLine("...................Bienvenue au salon............");
            Console.WriteLine("1 : Ajouter un participant ");
            Console.WriteLine("2 : Rechercher un participant");
            Console.WriteLine("3 : Quitter");
            Console.WriteLine("");
            Console.Write("Votre choix : - ");
            try
            {
                String LeChoix = Console.ReadLine();
                return int.Parse(LeChoix);
            }
            catch
            {
                return 0; //Erreur de Saisie
            }
        }
        //Ca c'est la methode pour quand tu choisis une action 
        public static void TraiterChoix(int LeChoix, DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            switch (LeChoix)
            {
                case 0:
                    Console.WriteLine("Les choix possibles sont 1, 2 ou 3. Appuyez sur une touche pour continuer");
                    Console.ReadKey();
                    break;

                case 1:
                    Console.WriteLine("Vous souhaitez ajouter un participant. Appuyez sur une touche pour continuer");
                    Console.ReadKey();
                    AjouterParticipant(DataBaseConnection, TheReader);
                    break;

                case 2:
                    Console.WriteLine("Vous souhaitez rechercher un participant. Appuyez sur une touche pour continuer");
                    Console.ReadKey();
                    RechercherParticipant(DataBaseConnection, TheReader);
                    break;

                case 3:
                    Console.WriteLine("Au revoir.....");
                    break;
            }
        }
        //Ca c'est la methode pour entrer les données du participant que tu veux creer
        public static void AjouterParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            Participant UnParticipant = new Participant(); ;
            String NomParticipant, PrenomParticipant, EmailParticipant;
            Console.Clear();
            Console.WriteLine(".....................................................");
            Console.Write("...................Nom du participant: ");
            NomParticipant = Console.ReadLine();
            Console.Write("...................Prenom du participant: ");
            PrenomParticipant = Console.ReadLine();
            Console.Write("...................Email du participant: ");
            EmailParticipant = Console.ReadLine();
            Console.WriteLine("...................Voulez-vous enregistrer ce participant (O/N) ?");
            String Reponse = "";
            do
                try
                {
                    Reponse = Console.ReadLine();
                    Reponse = Reponse.ToUpper();//On convertit en majuscule
                    if (Reponse == "O")
                        //Ici on effectue l'enregistrement dans la BDD
                        UnParticipant.Init(NomParticipant, PrenomParticipant, EmailParticipant);
                    UnParticipant.Save(DataBaseConnection, TheReader);
                    Console.WriteLine("Le participant est enregistré!");
                    System.Threading.Thread.Sleep(5000);//On patiente deux secondes
                }
                catch
                {
                    Console.WriteLine("Choix incorrect");
                }
            while ((Reponse != "o") && (Reponse != "O") && (Reponse != "n") && (Reponse != "N"));
        }
        //Ca c'est la methode pour rechercher un participant dans la BDD
        public static void RechercherParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {

            String PrenomParticipant;
            Console.Clear();
            Console.WriteLine(".....................................................");
            Console.Write("...................Prenom du participant: ");
            PrenomParticipant = Console.ReadLine();
            string query = "SELECT ID, Nom, Prenom, Email FROM participants WHERE Prenom = ?prenom";
            query = Tools.PrepareLigne(query, "?prenom", Tools.PrepareChamp(PrenomParticipant, "Chaine"));
            var cmd = new MySqlCommand(query, DataBaseConnection.Connection);
            List<Participant> LesParticipants = new List<Participant>();
            TheReader = cmd.ExecuteReader();
            while (TheReader.Read())
            {
                Participant leParticipant = new Participant
                {
                    IDParticipant = (int)TheReader["ID"],
                    NomParticipant = (string)TheReader["Nom"],
                    PrenomParticipant = (string)TheReader["Prenom"],
                    EmailParticipant = (string)TheReader["Email"]
                };
                LesParticipants.Add(leParticipant);
            }
            if (LesParticipants.Count > 0)
            {
                Console.WriteLine("-----------------------Voici le résultat: ");
                foreach (Participant leparticipant in LesParticipants)
                    Console.WriteLine("|" + leparticipant.IDParticipant.ToString() + "|" + leparticipant.NomParticipant + "|" + leparticipant.PrenomParticipant + "|" + leparticipant.EmailParticipant + "|");
            }
            else
                Console.WriteLine("Il n'y a pas de résultat...");
            Console.WriteLine("Appuyer sur une touche pour retourner au menu principal");
            Console.ReadKey();
            TheReader.Close();
        }
        
            
        }
    }

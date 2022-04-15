using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace clientLourd
{
    //Creation de la classe 
    public class Participant
    {
        public int IDParticipant { get; set; }
        public string NomParticipant { get; set; }
        public string PrenomParticipant { get; set; }
        public string EmailParticipant { get; set; }

        //Creation de la methode Save pour sauvegarder le participant
        public void Save(DBConnection DataBaseConnection, MySqlDataReader TheReader)

        {
            //Si le participant existe deja il fait la mis à jour 
            if (IDParticipant > 0)
                UpdateParticipant(DataBaseConnection, TheReader);
            //Sinon il crée un nouveau participant
            else
                AddParticipant(DataBaseConnection, TheReader);
        }
        //Ca c'est la methode pour initialiser les données du participant
        public void Init(string NomParticipant, string PrenomParticipant, string EmailParticipant)
        {
            this.NomParticipant = NomParticipant;
            this.PrenomParticipant = PrenomParticipant;
            this.EmailParticipant = EmailParticipant;
        }
        //Ca c'est la methode pour mettre a jour le participant
        private void UpdateParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            try
            {
                String sqlString = "UPDATE participants SET Nom = ?nom, Prenom = ?prenom, Email = ?email WHERE ID = ?id";
                sqlString = Tools.PrepareLigne(sqlString, "?id", Tools.PrepareChamp(IDParticipant.ToString(), "Nombre"));
                sqlString = Tools.PrepareLigne(sqlString, "?nom", Tools.PrepareChamp(NomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?prenom", Tools.PrepareChamp(PrenomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?email", Tools.PrepareChamp(EmailParticipant, "Chaine"));
                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }

        }
        //Ca c'est pour donner un id automatiquement car dans l'id est en auto-increment dans la base
        private int GiveNewID(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            int NewCode_c = 0;
            try
            {
                String sqlString = "SELECT MAX(ID) FROM participants;";
                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                TheReader = cmd.ExecuteReader();

                while (TheReader.Read())
                { NewCode_c = TheReader.GetInt32(0); }
                TheReader.Close();
                NewCode_c++;
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }
            return NewCode_c;
        }
        //Ca c'est la methode pour ajouter le participant
        private void AddParticipant(DBConnection DataBaseConnection, MySqlDataReader TheReader)
        {
            try
            {
                IDParticipant = GiveNewID(DataBaseConnection, TheReader);
                String sqlString = "INSERT INTO participants(ID,Nom,Prenom,Email) VALUES(?id,?nom,?prenom,?email)";
                
                sqlString = Tools.PrepareLigne(sqlString, "?id", Tools.PrepareChamp(IDParticipant.ToString(), "Nombre"));
                sqlString = Tools.PrepareLigne(sqlString, "?nom", Tools.PrepareChamp(NomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?prenom", Tools.PrepareChamp(PrenomParticipant, "Chaine"));
                sqlString = Tools.PrepareLigne(sqlString, "?email", Tools.PrepareChamp(EmailParticipant, "Chaine"));
                var cmd = new MySqlCommand(sqlString, DataBaseConnection.Connection);
                cmd.ExecuteNonQuery();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                Console.Write("Erreur N° " + ex.Number + " : " + ex.Message);
            }

        }
       



    }
}

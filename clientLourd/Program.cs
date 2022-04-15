using System;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace clientLourd

{
    

   
      class Program
    {
        public static void Main(string[] args)
        {
            DBConnection dBConnection = new DBConnection();
         
            //La tu mets le nom de ton serveur SQL (normalement c'est localhost)
            dBConnection.Server = "localhost";
            //La tu mets le nom de ta BDD
            dBConnection.DatabaseName = "clientLourd";
            //La tu mets le nom du user (normalement c'est root aussi)
            dBConnection.UserName = "root";
            //La tu mets le mot de passe de ton SQL (si y'a pas de mot de passe tu laisse comme ca)
            dBConnection.Password = " ";

            if (dBConnection.IsConnect())
            {
                //Parcours Classique d'un curseur, adressage des colonnes par leur position ordinale dans la requête
                string query = "select ID, Nom, Prenom, Email from participants;";
                var cmd = new MySqlCommand(query, dBConnection.Connection);
                var reader = cmd.ExecuteReader();//Remplissage du curseur
                reader.Close();
                
                //La tu appelles la classe Interface
                int ChoixUtilisateur;
                do
                {
                    ChoixUtilisateur = Interface.MenuPrincipal();
                    Interface.TraiterChoix(ChoixUtilisateur, dBConnection, reader);
                } while (ChoixUtilisateur != 3);
            }
        }
    
      }
}

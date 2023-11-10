using Data.DataBase;
using Entidades;
using System;
using System.Data;
using System.Text.RegularExpressions;
namespace Servicios
{
    public class S_Persona
    {
        PersonasDAO PersonasData = new PersonasDAO();

        public DataTable ObtenerTodasLasPersonas(int tipo)
        {
            return PersonasData.ObtenerTodasLasPersonas(tipo);
        }
        public int ModificarPersona(Persona persona)
        {
            return PersonasData.ModificarPersona(persona);
        }
        public int InsertarPersona(Persona persona)
        {
            return PersonasData.InsertarPersona(persona);
        }
        public bool EliminarPersona(Persona persona)
        {
            return PersonasData.EliminarPersona(persona);
        }

        public DataTable BusquedaFiltrada(int tipo, string apellido)
        {
            return PersonasData.BusquedaFiltrada(tipo, apellido);
        }
        public static bool ValidaEmail(string email)
        {            
           return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");            
        }
        public static bool ValidaEntero(string entero)
        {
            if (int.TryParse(entero, out int dev)) { return true;} else return false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Helpers
{
    public class Validaciones
    {
        //Constructor.
        public Validaciones()
        {

        }

        //Metodos
        public bool ValidarTipoDato(string TipoDato, string Dato, int Longitud = 0)
        {
            //Declaración de variables usadas
            DateTime ResultDateTime;           

            //1. Tipo de dato bit.                                          
            if (TipoDato == "bit")
            {
                //Validamos si es un 0, 1, true, false.
                if (Dato == "1" || Dato == "0" || Dato.ToLower() == "true" || Dato.ToLower() == "false") // || Dato == ""
                {
                    return true;
                }

            }
            //Tipos de datos Fechas.
            else if (TipoDato == "datetime")
            {
                var IsDate = DateTime.TryParse(Dato.ToString(), out ResultDateTime);
                if (IsDoubleOrInt(Dato.ToString()))
                {
                    IsDate = false;
                }
                if (Dato == "") { IsDate = true; }
                if (IsDate == true)
                {
                    if (Dato != "")
                    {
                        //"'" + ResultDateTime.ToString("yyyy-MM-dd") + "', ";
                        //ResultDateTime.ToString("yyyy-MM-dd");
                        return true;
                    }
                }               
            }
            //Tipos de datos int y bigint.
            else if (TipoDato == "int")
            {
                //Validamos si es un numero entero.
                var IsNumber = IsNumeric(Dato);
                if (Dato == "") { IsNumber = true; }
                if (IsNumber == true)
                {
                    if (Dato != "")
                    {
                        return true;
                    }
                }
                //Tipo de dato bigint.
                else
                {
                    //Validamos si es un numero entero bigint.
                    IsNumber = IsLong(Dato);
                    if (Dato == "") { IsNumber = true; }
                    if (IsNumber == true)
                    {
                        if (Dato != "")
                        {
                            return true;
                        }
                    }                    
                }
            }
            //Tipo de dato decimal, numeric.
            else if (TipoDato == "numeric")
            {
                //Validamos si es un numero double.
                var IsNumber = IsNumericOrDecimal(Dato);
                if (Dato == "") { IsNumber = true; }
                if (IsNumber == true)
                {
                    if (Dato != "")
                    {                       
                        var Valid = 0;
                        var IndexOf = Dato.LastIndexOf('.');
                        if (IndexOf > 0)
                        {
                            Valid = Dato.Substring(0, Dato.LastIndexOf('.')).Length;
                        }
                        if (Valid < 19)
                        {
                            return true;
                        }
                    }
                }

                //Validamos si es un numero float (double).
                IsNumber = IsDouble(Dato);
                if (Dato == "") { IsNumber = true; }
                if (IsNumber == true)
                {
                    if (Dato != "")
                    {
                        return true;
                    }
                }
            }
            //Tipo de dato varchar(255).
            else if (TipoDato == "varchar")
            {
                //validamos longitud
                if (Longitud != 0)
                {
                    if(Dato.Length <= Longitud)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /* BASICOS */

        //Para validar que un string sea un numero (int).
        public Boolean IsNumeric(string valor)
        {
            int result;
            return int.TryParse(valor, out result);
        }

        //Para validar que un string sea un numero grande (Bigint/long)
        public Boolean IsLong(string valor)
        {
            long result;
            return long.TryParse(valor, out result);
        }

        //Para validar que un string sea un numero (Double o Float en bases de datos).
        public Boolean IsDouble(string valor)
        {
            Double result;
            return Double.TryParse(valor, out result);
        }

        //Para validar que un string sea un numero Decimal o Numeric de bases de datos.
        public Boolean IsNumericOrDecimal(string valor)
        {
            Decimal result;
            return Decimal.TryParse(valor, out result);
        }

        //Para validar que un string sea un numero (Double o Integer)
        public Boolean IsDoubleOrInt(string valor)
        {
            Double resultDouble;
            int ResultInt;
            Boolean Valid = false;
            if (Double.TryParse(valor, out resultDouble) == true || int.TryParse(valor, out ResultInt) == true)
            {
                Valid = true;
            }

            return Valid;
        }

        //Para validar que un string sea un numero int o bigint de bases de datos.
        public Boolean IsIntOrBigInt(string valor)
        {
            int ResultInt;
            long ResultBigInt;
            Boolean Valid = false;

            if (int.TryParse(valor, out ResultInt) == true || long.TryParse(valor, out ResultBigInt) == true)
            {
                Valid = true;
            }

            return Valid;
        }
    }
}

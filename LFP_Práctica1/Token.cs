using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFP_Práctica1
{

    enum Tipo
    {

        TOKEN_MENORQ,
        TOKEN_MAYORQ,
        TOKEN_CERRARETIQUETA,
        TOKEN_NUMERO,
        TOKEN_IDENTIFICADOR,
        TOKEN_RESERVADA,
        TOKEN_COLOR

    }


    class Token
    {
        private Tipo tipoToken;
        private string lexema;
        private int fila;
        private int columna;
        private int id;





        internal Tipo TipoToken
        {
            get
            {
                return tipoToken;
            }

            set
            {
                tipoToken = value;
            }
        }

        public string Lexema
        {
            get
            {
                return lexema;
            }

            set
            {
                lexema = value;
            }
        }

        public int Fila
        {
            get
            {
                return fila;
            }

            set
            {
                fila = value;
            }
        }

        public int Columna
        {
            get
            {
                return columna;
            }

            set
            {
                columna = value;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

       

        public Token() {


        }


        public Token(string lexema, int fila, int columna, int id, Tipo tipoToken)
        {
            Lexema = lexema;
            Fila = fila;
            Columna = columna;
            Id = id;
            TipoToken = tipoToken;
           
        }
        


       

    }


   



}

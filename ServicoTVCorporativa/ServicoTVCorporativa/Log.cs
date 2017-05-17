using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicoTVCorporativa
{
    class Log
    {
        public static void Add(string texto)
        {
            string nomeArquivo = System.Configuration.ConfigurationSettings.AppSettings["log"] + "Log_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".txt";
            if (!(File.Exists(nomeArquivo)))
            {
                FileStream arquivo = new FileStream(nomeArquivo, FileMode.Create);
                arquivo.Close();
            }
            StreamWriter registro = new StreamWriter(nomeArquivo, true);
            registro.WriteLine(texto);
            registro.Close();
        }

        public static void Add(string texto, string caminhoArquivo)
        {
            StreamWriter registro = new StreamWriter(caminhoArquivo, true);
            registro.Write(texto);
            registro.Close();
        }

        public static void Clear(string arquivo)
        {
            StreamWriter registro = new StreamWriter(arquivo);
            registro.Write("");
            registro.Close();
        }
    }
}

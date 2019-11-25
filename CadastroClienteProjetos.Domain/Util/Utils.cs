using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CadastroClienteProjetos.Domain.Util
{
    public static class Utils
    {
        /// <summary>
        /// Este snippet fornece duas funções para substituir apenas a primeira ocorrência de uma string dentro de uma string maior. Usando o método Replace irá substituir todas as ocorrências encontradas.
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Find"></param>
        /// <param name="Replace"></param>
        /// <returns></returns>
        public static string ReplaceFirstOccurrence(string Source, string Find, string Replace)
        {
            var Place = Source.IndexOf(Find);
            var result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        /// <summary>
        /// Este snippet fornece duas funções para substituir apenas a última ocorrência de uma string dentro de uma string maior. Usando o método Replace irá substituir todas as ocorrências encontradas.
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Find"></param>
        /// <param name="Replace"></param>
        /// <returns></returns>
        public static string ReplaceLastOccurrence(string Source, string Find, string Replace)
        {
            var Place = Source.LastIndexOf(Find);
            var result = Source.Remove(Place, Find.Length).Insert(Place, Replace);
            return result;
        }

        /// <summary>
        /// RETORNA STRING COM NOMES DO MEIO ABREVIADOS
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static string ObterNomeAbreviado(string nome)
        {
            if (nome.Contains("  "))
                nome = nome.Replace("  ", " ");

            string[] Listanomes = nome.Split(' ');
            var nomeAbreviado = "";
            var contador = 1;
            var total = Listanomes.Count();
            var builder = new StringBuilder();
            builder.Append(nomeAbreviado);

            foreach (string item in Listanomes)
            {
                if (contador == 1) // PRIMEIRO NOME
                    builder.Append(item + " ");
                else if (contador == total) // ULTIMO NOME
                    builder.Append(item);
                else // NOMES DO MEIO
                    builder.Append(item.Substring(0, 1) + " ");

                contador++;
            }

            nomeAbreviado = builder.ToString();
            return nomeAbreviado;
        }

        /// <summary>
        /// Transforma os dados na Url em uma Url amigavel para ganhar em Seo
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ToSeoUrl(this string url)
        {
            // make the url lowercase
            var encodedUrl = (url ?? "").ToLower();

            // replace & with and
            encodedUrl = Regex.Replace(encodedUrl, @"\&+", "and");

            // remove characters
            encodedUrl = encodedUrl.Replace("'", "");

            // remove invalid characters
            encodedUrl = Regex.Replace(encodedUrl, @"[^a-z0-9]", "-");

            // remove duplicates
            encodedUrl = Regex.Replace(encodedUrl, @"-+", "-");

            // trim leading & trailing characters
            encodedUrl = encodedUrl.Trim('-');

            return encodedUrl;
        }

        /// <summary>
        /// Deixa a primeira letra da parava em Maiúscula
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string UpperCaseFirst(this string title)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(title))
                return string.Empty;

            // Return char and concat substring.
            return char.ToUpper(title[0]) + title.Substring(1);
        }

        /// <summary>
        /// REMOVE CARACTERES ESPECIAIS (^~´Ç etc..)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoverCharsEspeciais(string text)
        {
            var sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }

            return sbReturn.ToString();
        }

        /// <summary>
        /// REMOVE MASCARAS (/-,._\)
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoverMascaras(string text)
        {
            return text.Replace("/", "").Replace(@"\", "").Replace(".", "").Replace(",", "").Replace("-", "").Replace("(", "").Replace(")", "").Replace("_", "").Replace("'", "\"");
        }

        /// <summary>
        /// Verifica se é nulo ou vazio e manda para a base de dados
        /// </summary>
        /// <param name="that"></param>
        /// <returns></returns>
        public static string IsNullToEmpty(this string that)
        {
            return string.IsNullOrEmpty(that) ? "" : that;
        }

        /// <summary>
        /// Lê a quantidade de bytes total e apresenta o tamanho em sua classificação amigavel
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string TamanhoAmigavel(long bytes)
        {
            if (bytes < 0) throw new ArgumentException(nameof(bytes));

            double humano;
            string sufixo;

            if (bytes >= 1152921504606846976L) // Exabyte (1024^6)
            {
                humano = bytes >> 50;
                sufixo = "EB";
            }
            else if (bytes >= 1125899906842624L) // Petabyte (1024^5)
            {
                humano = bytes >> 40;
                sufixo = "PB";
            }
            else if (bytes >= 1099511627776L) // Terabyte (1024^4)
            {
                humano = bytes >> 30;
                sufixo = "TB";
            }
            else if (bytes >= 1073741824) // Gigabyte (1024^3)
            {
                humano = bytes >> 20;
                sufixo = "GB";
            }
            else if (bytes >= 1048576) // Megabyte (1024^2)
            {
                humano = bytes >> 10;
                sufixo = "MB";
            }
            else if (bytes >= 1024) // Kilobyte (1024^1)
            {
                humano = bytes;
                sufixo = "KB";
            }
            else return bytes.ToString("0 B"); // Byte

            humano /= 1024;
            return humano.ToString("0.## ") + sufixo;
        }

        /// <summary>
        /// Redimenciona uma imagem com os parametros que deseja customizar
        /// </summary>
        /// <param name="current"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static Image Resize(this Image current, int maxWidth, int maxHeight)
        {
            int width, height;

            #region [reckon size]

            if (current.Width > current.Height)
            {
                width = maxWidth;
                height = Convert.ToInt32(current.Height * maxHeight / (double)current.Width);
            }
            else
            {
                width = Convert.ToInt32(current.Width * maxWidth / (double)current.Height);
                height = maxHeight;
            }

            #endregion

            #region [get resized bitmap]

            var canvas = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(canvas))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(current, 0, 0, width, height);
            }
            return canvas;

            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Image current)
        {
            using (var stream = new MemoryStream())
            {
                current.Save(stream, current.RawFormat);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Retorna o nome do valor informado, transformado em palavras a quantidade em reais
        /// </summary>
        /// <param name="valor"></param>
        /// <returns></returns>
        public static string toExtenso(decimal valor)
        {
            if ((valor <= 0M) | (valor >= 1000000000000000M))
                return "Valor n\x00e3o suportado pelo sistema.";

            var str = valor.ToString("000000000000000.00");
            var str2 = string.Empty;
            for (int i = 0; i <= 15; i += 3)
            {
                str2 = str2 + escreva_parte(Convert.ToDecimal(str.Substring(i, 3)));
                if ((i == 0) & (str2 != string.Empty))
                {
                    if (Convert.ToInt32(str.Substring(0, 3)) == 1)
                    {
                        str2 = str2 + " TRILH\x00c3O" + ((Convert.ToDecimal(str.Substring(3, 12)) > 0M) ? " E " : string.Empty);
                    }
                    else if (Convert.ToInt32(str.Substring(0, 3)) > 1)
                    {
                        str2 = str2 + " TRILH\x00d5ES" + ((Convert.ToDecimal(str.Substring(3, 12)) > 0M) ? " E " : string.Empty);
                    }
                }
                else if ((i == 3) & (str2 != string.Empty))
                {
                    if (Convert.ToInt32(str.Substring(3, 3)) == 1)
                    {
                        str2 = str2 + " BILH\x00c3O" + ((Convert.ToDecimal(str.Substring(6, 9)) > 0M) ? " E " : string.Empty);
                    }
                    else if (Convert.ToInt32(str.Substring(3, 3)) > 1)
                    {
                        str2 = str2 + " BILH\x00d5ES" + ((Convert.ToDecimal(str.Substring(6, 9)) > 0M) ? " E " : string.Empty);
                    }
                }
                else if ((i == 6) & (str2 != string.Empty))
                {
                    if (Convert.ToInt32(str.Substring(6, 3)) == 1)
                    {
                        str2 = str2 + " MILH\x00c3O" + ((Convert.ToDecimal(str.Substring(9, 6)) > 0M) ? " E " : string.Empty);
                    }
                    else if (Convert.ToInt32(str.Substring(6, 3)) > 1)
                    {
                        str2 = str2 + " MILH\x00d5ES" + ((Convert.ToDecimal(str.Substring(9, 6)) > 0M) ? " E " : string.Empty);
                    }
                }
                else if (((i == 9) & (str2 != string.Empty)) && (Convert.ToInt32(str.Substring(9, 3)) > 0))
                {
                    str2 = str2 + " MIL" + ((Convert.ToDecimal(str.Substring(12, 3)) > 0M) ? " E " : string.Empty);
                }
                if (i == 12)
                {
                    if (str2.Length > 8)
                    {
                        if ((str2.Substring(str2.Length - 6, 6) == "BILH\x00c3O") | (str2.Substring(str2.Length - 6, 6) == "MILH\x00c3O"))
                        {
                            str2 = str2 + " DE";
                        }
                        else if (((str2.Substring(str2.Length - 7, 7) == "BILH\x00d5ES") | (str2.Substring(str2.Length - 7, 7) == "MILH\x00d5ES")) | (str2.Substring(str2.Length - 8, 7) == "TRILH\x00d5ES"))
                        {
                            str2 = str2 + " DE";
                        }
                        else if (str2.Substring(str2.Length - 8, 8) == "TRILH\x00d5ES")
                        {
                            str2 = str2 + " DE";
                        }
                    }
                    if (Convert.ToInt64(str.Substring(0, 15)) == 1L)
                    {
                        str2 = str2 + " REAL";
                    }
                    else if (Convert.ToInt64(str.Substring(0, 15)) > 1L)
                    {
                        str2 = str2 + " REAIS";
                    }
                    if ((Convert.ToInt32(str.Substring(0x10, 2)) > 0) && (str2 != string.Empty))
                    {
                        str2 = str2 + " E ";
                    }
                }
                if (i == 15)
                {
                    if (Convert.ToInt32(str.Substring(0x10, 2)) == 1)
                    {
                        str2 = str2 + " CENTAVO";
                    }
                    else if (Convert.ToInt32(str.Substring(0x10, 2)) > 1)
                    {
                        str2 = str2 + " CENTAVOS";
                    }
                }
            }
            return str2;
        }

        private static string escreva_parte(decimal valor)
        {
            if (valor <= 0M)
                return string.Empty;

            var str = string.Empty;

            if ((valor > 0M) & (valor < 1M))
                valor *= 100M;

            var str2 = valor.ToString("000");
            var num = Convert.ToInt32(str2.Substring(0, 1));
            var num2 = Convert.ToInt32(str2.Substring(1, 1));
            var num3 = Convert.ToInt32(str2.Substring(2, 1));
            if (num == 1)
            {
                str = str + (((num2 + num3) == 0) ? "CEM" : "CENTO");
            }
            else if (num == 2)
            {
                str = str + "DUZENTOS";
            }
            else if (num == 3)
            {
                str = str + "TREZENTOS";
            }
            else if (num == 4)
            {
                str = str + "QUATROCENTOS";
            }
            else if (num == 5)
            {
                str = str + "QUINHENTOS";
            }
            else if (num == 6)
            {
                str = str + "SEISCENTOS";
            }
            else if (num == 7)
            {
                str = str + "SETECENTOS";
            }
            else if (num == 8)
            {
                str = str + "OITOCENTOS";
            }
            else if (num == 9)
            {
                str = str + "NOVECENTOS";
            }
            if (num2 == 1)
            {
                if (num3 == 0)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "DEZ";
                }
                else if (num3 == 1)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "ONZE";
                }
                else if (num3 == 2)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "DOZE";
                }
                else if (num3 == 3)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "TREZE";
                }
                else if (num3 == 4)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "QUATORZE";
                }
                else if (num3 == 5)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "QUINZE";
                }
                else if (num3 == 6)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "DEZESSEIS";
                }
                else if (num3 == 7)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "DEZESSETE";
                }
                else if (num3 == 8)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "DEZOITO";
                }
                else if (num3 == 9)
                {
                    str = str + ((num > 0) ? " E " : string.Empty) + "DEZENOVE";
                }
            }
            else if (num2 == 2)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "VINTE";
            }
            else if (num2 == 3)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "TRINTA";
            }
            else if (num2 == 4)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "QUARENTA";
            }
            else if (num2 == 5)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "CINQUENTA";
            }
            else if (num2 == 6)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "SESSENTA";
            }
            else if (num2 == 7)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "SETENTA";
            }
            else if (num2 == 8)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "OITENTA";
            }
            else if (num2 == 9)
            {
                str = str + ((num > 0) ? " E " : string.Empty) + "NOVENTA";
            }
            if (((str2.Substring(1, 1) != "1") & (num3 != 0)) & (str != string.Empty))
            {
                str = str + " E ";
            }
            if (str2.Substring(1, 1) != "1")
            {
                if (num3 == 1)
                {
                    str = str + "UM";
                }
                else
                {
                    if (num3 == 2)
                    {
                        return (str + "DOIS");
                    }
                    if (num3 == 3)
                    {
                        return (str + "TR\x00caS");
                    }
                    if (num3 == 4)
                    {
                        return (str + "QUATRO");
                    }
                    if (num3 == 5)
                    {
                        return (str + "CINCO");
                    }
                    if (num3 == 6)
                    {
                        return (str + "SEIS");
                    }
                    if (num3 == 7)
                    {
                        return (str + "SETE");
                    }
                    if (num3 == 8)
                    {
                        str = str + "OITO";
                    }
                    else if (num3 == 9)
                    {
                        str = str + "NOVE";
                    }
                }
            }
            return str;
        }
    }
}
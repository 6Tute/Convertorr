// convertir la parte fraccionaria de un numero decimal a otras bases
// la parte entera se divide sucesivamente por la base
// la parte fraccionaria se multiplica sucesivamente por la base


using System;

class ConversorNumeros
{
    // Parte entera de la conversión
    static string DecimalEnteroABase(int decimalNum, int baseDestino) // numeo entrero decimal y la base que queremos convertir
    {
        if (decimalNum == 0) return "0";   // si el numero es 0 devolvemos 0
        string resultado = "";
        char[] hexChars = "0123456789ABCDEF".ToCharArray();
        while (decimalNum > 0)                 // se divide el numero repetidamente entre la base y se guarda el resto
        {                                      
            int resto = decimalNum % baseDestino;  // el resto corresponde a la nueva base
            resultado = hexChars[resto] + resultado;   // si es mayor que 9 hexchars lo representa como a,b,c ...
            decimalNum /= baseDestino;
        }
        return resultado;
    }

    // Parte fraccionaria de la conversión
    static string FraccionDecimalABase(double fraccion, int baseDestino, int precision = 6) // 6 digitos max
    {
        string resultado = "";
        char[] hexChars = "0123456789ABCDEF".ToCharArray();
        int contador = 0;
        while (fraccion > 0 && contador < precision) // se repite hasta que llege a 6 (que es lo maximo establecido)
        {
            fraccion *= baseDestino;     // multiplica por la base
            int parteEntera = (int)fraccion;   // la parte entera de la multi va ser el siguiente digito en la base del destino
            resultado += hexChars[parteEntera]; // se anade el resultado 
            fraccion -= parteEntera;  // aca queda solo la parte restante al restarle  la fraccion
            contador++;
        }
        if (resultado == "") resultado = "0"; // en caso de que no devuelva digito te devuelve 0
        return resultado;
    }

    // Función completa: decimal → base
    static string DecimalABase(double numero, int baseDestino)
    {
        int parteEntera = (int)numero;
        double parteFraccionaria = numero - parteEntera;  // separo en parte entera y fraccionaria

        string resultadoEntero = DecimalEnteroABase(parteEntera, baseDestino);   // convertimos las partes usando las funciones de arriba
        string resultadoFraccion = FraccionDecimalABase(parteFraccionaria, baseDestino);

        return resultadoEntero + "." + resultadoFraccion; // y se convinan el restulado con un punto para tener el numero final
    }

    static void Main()
    {
        Console.WriteLine("=== CONVERSOR DE NUMEROS ===");

        while (true)
        {
            Console.WriteLine("\nIngresar un número!:");
            string numero = Console.ReadLine();
            double numeroDecimal;

            while (!double.TryParse(numero, out numeroDecimal))
            {
                Console.WriteLine("Número inválido. Ingrese un número racional o entero:");// x si ingresan mal numero
                numero = Console.ReadLine();
            }

            Console.WriteLine("Elegir sistema de origen !(decimal, binario, octal, hexadecimal):");
            string origen = Console.ReadLine().ToLower();

            Console.WriteLine("Elegir sistema a convertir !(decimal, binario, octal, hexadecimal):");
            string destino = Console.ReadLine().ToLower();

            // Convertir a decimal si es necesario
            switch (origen)
            {
                case "decimal":
                    break; // si ya esta en decimal break
                case "binario":
                    numeroDecimal = BinarioADecimal(numero);
                    break;
                case "octal":
                    numeroDecimal = OctalADecimal(numero);
                    break;
                case "hexadecimal":
                    numeroDecimal = HexADecimal(numero);
                    break;
                default:
                    Console.WriteLine("Sistema de origen no válido"); // si se escribe mal devuelve este msj
                    continue;
            }

            // Convertir a sistema destino
            string resultado = "";
            switch (destino)
            {
                case "decimal":
                    resultado = numeroDecimal.ToString();
                    break;
                case "binario":
                    resultado = DecimalABase(numeroDecimal, 2);  // bin
                    break;
                case "octal":
                    resultado = DecimalABase(numeroDecimal, 8);  // octa
                    break;
                case "hexadecimal":
                    resultado = DecimalABase(numeroDecimal, 16); // hexa
                    break;
                default:
                    Console.WriteLine("Sistema de destino no válido"); // si se escribe mal devuelve este msj
                    continue;
            }

            Console.WriteLine("Resultado: " + resultado);

            Console.WriteLine("\n¿Desea hacer otra conversión? (s/n)");
            string respuesta = Console.ReadLine().ToLower();
            if (respuesta != "s") break;           //  aca permite seguir con el programa 
        }

        Console.WriteLine(
            "Creado por: Guzman Avril y Garcia Matias, " +
            "Gracias por usar nuestro conversor, " +
            " PRESIONE CUALQUIER TECLA PARA SALIR!"+
            "");
        Console.ReadLine(); // esto es para que no se cierre  de golpe la consola
    }

    // convertir a decimal desde otros sistemas
    static double BinarioADecimal(string binario)   // aca se divide la cadena del entero y fraccionaria
    {
        double resultado = 0;
        string[] partes = binario.Split('.');
        // Parte entera
        string parteEntera = partes[0];
        int potencia = 1;
        for (int i = parteEntera.Length - 1; i >= 0; i--)
            if (parteEntera[i] == '1')
                resultado += potencia;                // en la entera sumamos 2^ para cada digito
        potencia *= 2;

        // Parte fraccionaria
        if (partes.Length > 1)
        {
            string fraccion = partes[1];
            double pot = 0.5;
            foreach (char c in fraccion)           
            {
                if (c == '1') resultado += pot;        // en la fraccionaria sumamos 1/2^ para cada digito despues del punto
                pot /= 2;
            }
        }
        return resultado;
    }

    static double OctalADecimal(string octal)
    {
        double resultado = 0;
        string[] partes = octal.Split('.');
        // Parte entera
        string parteEntera = partes[0];
        int potencia = 1;
        for (int i = parteEntera.Length - 1; i >= 0; i--)  // en la entera sumamos 8^ para cada digito
        {
            resultado += (parteEntera[i] - '0') * potencia;
            potencia *= 8;
        }
        // Parte fraccionaria
        if (partes.Length > 1)
        {
            string fraccion = partes[1];
            double pot = 1.0 / 8;
            foreach (char c in fraccion)
            {
                resultado += (c - '0') * pot;       // en la fraccionaria sumamos 1/8^ para cada digito despues del punto
                pot /= 8;
            }
        }
        return resultado;
    }

    static double HexADecimal(string hex)
    {
        double resultado = 0;
        string[] partes = hex.Split('.');
        string parteEntera = partes[0];                     //  convierte las letras A-F a 10-15
        int potencia = 1;
        for (int i = parteEntera.Length - 1; i >= 0; i--) 
        {
            char c = parteEntera[i];
            int valor = (c >= '0' && c <= '9') ? c - '0' : 10 + (char.ToUpper(c) - 'A'); // aca le va agregando el valor 
            resultado += valor * potencia;                                              // cada vez que sube el caracter
            potencia *= 16;
        }
        // Parte fraccionaria
        if (partes.Length > 1)
        {
            string fraccion = partes[1];  // se suman los digitos y se dividen x base^posicion
            double pot = 1.0 / 16;
            foreach (char c in fraccion)
            {
                int valor = (c >= '0' && c <= '9') ? c - '0' : 10 + (char.ToUpper(c) - 'A');
                resultado += valor * pot;
                pot /= 16;
            }
        }
        return resultado;
    }
}

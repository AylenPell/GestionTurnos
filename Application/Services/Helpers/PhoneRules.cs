using System.Collections.Generic;

namespace Application.Services.Helpers
{
    public static class PhoneRules
    {
        // Código de área -> cantidad esperada de dígitos en el número local
        public static readonly Dictionary<string, int> AreaCodes = new()
        {
            // --- 2 dígitos (8 dígitos locales)
            { "11", 8 }, // Ciudad Autónoma de Buenos Aires y alrededores

            // --- 3 dígitos (7 dígitos locales)
            { "220", 7 }, // Merlo, Moreno
            { "221", 7 }, // La Plata
            { "223", 7 }, // Mar del Plata
            { "230", 7 }, // Pilar
            { "236", 7 }, // Junín
            { "237", 7 }, // Morón
            { "249", 7 }, // Tandil
            { "260", 7 }, // San Rafael
            { "261", 7 }, // Mendoza
            { "263", 7 }, // San Martín (Mendoza)
            { "264", 7 }, // San Juan
            { "265", 7 }, // Villa Mercedes (San Luis)
            { "266", 7 }, // San Luis
            { "280", 7 }, // Puerto Madryn
            { "291", 7 }, // Bahía Blanca
            { "292", 7 }, // Tres Arroyos
            { "297", 7 }, // Comodoro Rivadavia
            { "298", 7 }, // General Roca (Río Negro)
            { "299", 7 }, // Neuquén
            { "336", 7 }, // San Nicolás
            { "341", 7 }, // Rosario
            { "342", 7 }, // Santa Fe
            { "343", 7 }, // Paraná
            { "345", 7 }, // Concordia
            { "348", 7 }, // Zárate / Campana
            { "351", 7 }, // Córdoba
            { "353", 7 }, // Villa María
            { "358", 7 }, // Río Cuarto
            { "362", 7 }, // Resistencia (Chaco)
            { "370", 7 }, // Formosa
            { "376", 7 }, // Posadas
            { "379", 7 }, // Corrientes
            { "380", 7 }, // La Rioja
            { "381", 7 }, // Tucumán
            { "383", 7 }, // Catamarca
            { "385", 7 }, // Santiago del Estero
            { "387", 7 }, // Salta
            { "388", 7 }, // San Salvador de Jujuy

            // --- 4 dígitos (6 dígitos locales)
            { "2202", 6 }, // Cañuelas
            { "2221", 6 }, // San Vicente
            { "2223", 6 }, // Chascomús
            { "2224", 6 }, // Lobos
            { "2225", 6 }, // Brandsen
            { "2226", 6 }, // Monte
            { "2227", 6 }, // General Belgrano
            { "2229", 6 }, // General Paz
            { "2241", 6 }, // Castelli
            { "2242", 6 }, // Dolores
            { "2243", 6 }, // Lezama
            { "2244", 6 }, // General Guido
            { "2245", 6 }, // Ayacucho
            { "2246", 6 }, // Rauch
            { "2247", 6 }, // Balcarce
            { "2254", 6 }, // Villa Gesell
            { "2255", 6 }, // Pinamar
            { "2256", 6 }, // San Clemente del Tuyú
            { "2257", 6 }, // Santa Teresita
            { "2258", 6 }, // Mar de Ajó
            { "2261", 6 }, // Necochea
            { "2262", 6 }, // Quequén
            { "2264", 6 }, // Lobería
            { "2265", 6 }, // Benito Juárez
            { "2266", 6 }, // González Chaves
            { "2271", 6 }, // Saladillo
            { "2272", 6 }, // Roque Pérez
            { "2273", 6 }, // 25 de Mayo
            { "2274", 6 }, // Bragado
            { "2275", 6 }, // Chivilcoy
            { "2281", 6 }, // Olavarría
            { "2283", 6 }, // Azul
            { "2284", 6 }, // Tapalqué
            { "2286", 6 }, // General Lamadrid
            { "2287", 6 }, // Laprida
            { "2289", 6 }, // Daireaux
            { "2291", 6 }, // Coronel Suárez
            { "2292", 6 }, // Pigüé
            { "2296", 6 }, // Salliqueló
            { "2297", 6 }, // Tres Lomas
            { "2302", 6 }, // Pehuajó
            { "2303", 6 }, // Carlos Casares
            { "2314", 6 }, // 9 de Julio
            { "2316", 6 }, // Lincoln
            { "2317", 6 }, // General Villegas
            { "2320", 6 }, // Trenque Lauquen
            { "2321", 6 }, // América
            { "2323", 6 }, // Rivadavia (BA)
            { "2324", 6 }, // Pellegrini
            { "2331", 6 }, // Chacabuco
            { "2333", 6 }, // Rojas
            { "2334", 6 }, // Colón (BA)
            { "2335", 6 }, // Pergamino
            { "2336", 6 }, // Salto
            { "2337", 6 }, // Arrecifes
            { "2338", 6 }, // Capitán Sarmiento
            { "2342", 6 }, // San Antonio de Areco
            { "2343", 6 }, // Mercedes
            { "2344", 6 }, // Luján
            { "2345", 6 }, // General Rodríguez
            { "2352", 6 }, // San Andrés de Giles
            { "2353", 6 }, // Carmen de Areco
            { "2358", 6 }, // Bolívar

            // --- Patagonia y NOA ejemplos
            { "2901", 6 }, // Ushuaia
            { "2902", 6 }, // Río Grande
            { "2903", 6 }, // Tolhuin
            { "2920", 6 }, // Carmen de Patagones
            { "2921", 6 }, // Viedma
            { "294", 7 },  // Bariloche
            { "2966", 6 }, // Río Gallegos
            { "2964", 6 }, // El Calafate
            { "2962", 6 }, // Puerto Deseado
            { "2965", 6 }  // Río Turbio
        };
    }
}

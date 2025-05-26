using System;
using System.IO;
using System.Linq;
using System.Text;

class VigenereCipher
{
    public static string Encrypt(string plaintext, string keyword)
    {
        StringBuilder encryptedText = new StringBuilder();
        int keywordLength = keyword.Length;

        for (int i = 0; i < plaintext.Length; i++)
        {
            char p = plaintext[i];
            char k = keyword[i % keywordLength];
            int shift = k - 'A';
            encryptedText.Append((char)((p + shift - 'A') % 26 + 'A'));
        }

        return encryptedText.ToString();
    }

    public static string Decrypt(string ciphertext, string keyword)
    {
        StringBuilder decryptedText = new StringBuilder();
        int keywordLength = keyword.Length;

        for (int i = 0; i < ciphertext.Length; i++)
        {
            char c = ciphertext[i];
            char k = keyword[i % keywordLength];
            int shift = k - 'A';
            decryptedText.Append((char)((c - shift - 'A' + 26) % 26 + 'A'));
        }

        return decryptedText.ToString();
    }
}

class Program
{
    static readonly string BasePath = @"C:\Users\bhoma\programming\С#\lab1-2\CipherApp\vigenereCipher";

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Виберіть режим:");
            Console.WriteLine("1 - Шифрування (Віженер)");
            Console.WriteLine("2 - Дешифрування (Віженер)");
            Console.WriteLine("3 - Вихід");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    EncryptMode();
                    break;
                case "2":
                    DecryptMode();
                    break;
                case "3":
                    Console.WriteLine("Вихід з програми.");
                    return;
                default:
                    Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                    break;
            }

            Console.WriteLine("Натисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
        }
    }

    static void EncryptMode()
    {
        string plaintext = ReadText("Введіть текст для шифрування:");
        if (string.IsNullOrEmpty(plaintext)) return;

        string keyword = ReadKeyword("Введіть ключ для шифрування:");
        if (string.IsNullOrEmpty(keyword)) return;

        string encryptedText = VigenereCipher.Encrypt(plaintext, keyword);

        Console.WriteLine("Оберіть, як зберегти результат:");
        Console.WriteLine("1 - Зберегти у наявний файл");
        Console.WriteLine("2 - Зберегти у новий файл");

        string saveChoice = Console.ReadLine();
        string filePath = "";

        switch (saveChoice)
        {
            case "1":
                filePath = GetFilePathFromUser("Введіть назву наявного файлу (з .txt):");
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Файл не знайдено.");
                    return;
                }
                File.WriteAllText(filePath, encryptedText);
                break;

            case "2":
                Console.WriteLine("Введіть назву нового файлу (без розширення):");
                string fileName = Console.ReadLine();
                filePath = Path.Combine(BasePath, $"{fileName}.txt");
                File.WriteAllText(filePath, encryptedText);
                break;

            default:
                Console.WriteLine("Невідома команда.");
                return;
        }

        Console.WriteLine("Зашифрований текст: " + encryptedText);
        Console.WriteLine($"Результат збережено у файл '{filePath}'.");
    }

    static void DecryptMode()
    {
        string filePath = GetFilePathFromUser("Введіть назву файлу для дешифрування (з .txt):");

        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не знайдено.");
            return;
        }

        string ciphertext = File.ReadAllText(filePath);

        if (string.IsNullOrEmpty(ciphertext))
        {
            Console.WriteLine("Файл порожній.");
            return;
        }

        string keyword = ReadKeyword("Введіть ключ для дешифрування:");
        if (string.IsNullOrEmpty(keyword)) return;

        string decryptedText = VigenereCipher.Decrypt(ciphertext, keyword);
        Console.WriteLine("Розшифрований текст: " + decryptedText);
    }

    static string ReadText(string prompt)
    {
        string text = "";
        bool validInput = false;

        while (!validInput)
        {
            Console.WriteLine(prompt);
            text = Console.ReadLine().ToUpper();
            validInput = !string.IsNullOrEmpty(text) && text.All(c => char.IsLetter(c));
            if (!validInput)
            {
                Console.WriteLine("Текст повинен містити лише літери. Спробуйте ще раз.");
            }
        }

        return text;
    }

    static string ReadKeyword(string prompt)
    {
        string keyword = "";
        bool validInput = false;

        while (!validInput)
        {
            Console.WriteLine(prompt);
            keyword = Console.ReadLine().ToUpper();
            validInput = !string.IsNullOrEmpty(keyword) && keyword.All(c => char.IsLetter(c));
            if (!validInput)
            {
                Console.WriteLine("Ключ повинен містити лише літери. Спробуйте ще раз.");
            }
        }

        return keyword;
    }

    static string GetFilePathFromUser(string prompt)
    {
        Console.WriteLine(prompt);
        string fileName = Console.ReadLine();
        return Path.Combine(BasePath, fileName);
    }
}

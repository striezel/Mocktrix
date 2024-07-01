/*
    This file is part of Mocktrix.
    Copyright (C) 2024  Dirk Stolle

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace Mocktrix
{
    public class Program
    {
        /// <summary>
        /// Prints the help text to the standard output.
        /// </summary>
        private static void ShowHelp()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var name = asm.GetName().Name;
            Console.WriteLine(name + " [OPTIONS]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -? | --help     - Shows this help message and quits.");
            Console.WriteLine("  -v | --version  - Shows the version of the program and quits.");
            Console.WriteLine("  --conf file.xml - Loads the configuration from the given XML file.");
        }


        /// <summary>
        /// Prints the version information to the standard output.
        /// </summary>
        private static void ShowVersion()
        {
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            var name = asm.GetName().Name;
            var ver = asm.GetName().Version;
            Console.WriteLine(name + ", version " + ver?.ToString(3));
            Console.WriteLine();
            Console.WriteLine("Copyright (C) 2024  Dirk Stolle");
            Console.WriteLine("License GPLv3+: GNU GPL version 3 or later <https://gnu.org/licenses/gpl.html>");
            Console.WriteLine("This is free software: you are free to change and redistribute it under the");
            Console.WriteLine("terms of the GNU General Public License version 3 or any later version.");
            Console.WriteLine("There is NO WARRANTY, to the extent permitted by law.");
        }


        /// <summary>
        /// Parses and handles command-line arguments of the program.
        /// </summary>
        /// <param name="args">the array of arguments</param>
        /// <returns>Returns a Tuple where the first element is an exit code and
        /// the second element is a boolean value indicating whether to exit the
        /// program after parsing.
        ///
        /// The return code element is zero, if parsing was successful.
        /// The return code element is non-zero value, if an error occurred.</returns>
        public static Tuple<int, bool> ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--conf")
                {
                    if (i + 1 >= args.Length)
                    {
                        Console.Error.WriteLine("Error: Parameter " + args[i]
                            + " must be followed by a file path for the configuration file!");
                        return new Tuple<int, bool>(1, true);
                    }
                    if (!Configuration.ConfigurationManager.Current.LoadFromFile(args[i + 1]))
                    {
                        Console.Error.WriteLine("Failed to load configuration file!");
                        return new Tuple<int, bool>(1, true);
                    }
                    Console.WriteLine("Information: Configuration was loaded from file "
                        + args[i + 1] + ".");
                    // Skip next argument, that is the file name.
                    ++i;
                }
                else if ((args[i] == "--help") || (args[i] == "/?") || (args[i] == "-?"))
                {
                    ShowHelp();
                    return new Tuple<int, bool>(0, true);
                }
                else if ((args[i] == "--version") || (args[i] == "-v"))
                {
                    ShowVersion();
                    return new Tuple<int, bool>(0, true);
                }
                else
                {
                    Console.Error.WriteLine("Error: " + args[i]
                        + " is not a valid command line option!");
                    return new Tuple<int, bool>(1, true);
                }
            }

            return new Tuple<int, bool>(0, false);
        }

        public static int Main(string[] args)
        {
            (int rc, bool exit) = ParseArguments(args);
            if (rc != 0 || exit)
            {
                return rc;
            }

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();

            client.r0_6_1.All.AddEndpoints(app);

            MockData.Add();

            app.Run();

            return 0;
        }
    }
}

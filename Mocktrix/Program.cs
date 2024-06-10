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
        /// Parses and handles command-line arguments of the program.
        /// </summary>
        /// <param name="args">the array of arguments</param>
        /// <returns>Returns zero, if parsing was successful.
        /// Returns a non-zero value, if an error occurred.</returns>
        public static int ParseArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--conf")
                {
                    if (i + 1 >= args.Length)
                    {
                        Console.Error.WriteLine("Error: Parameter " + args[i]
                            + " must be followed by a file path for the configuration file!");
                        return 1;
                    }
                    if (!Configuration.ConfigurationManager.Current.LoadFromFile(args[i+1]))
                    {
                        Console.Error.WriteLine("Failed to load configuration file!");
                        return 1;
                    }
                    Console.WriteLine("Information: Configuration was loaded from file "
                        + args[i + 1] + ".");
                    // Skip next argument, that is the file name.
                    ++i;
                }
                else
                {
                    Console.Error.WriteLine("Error: " + args[i]
                        + " is not a valid command line option!");
                    return 1;
                }
            }

            return 0;
        }

        public static int Main(string[] args)
        {
            int rc = ParseArguments(args);
            if (rc != 0)
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

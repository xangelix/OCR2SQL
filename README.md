# OCR2SQL

Scans through returned mail and creates a database including the addresses and reasons for the return.

[Download setup from the releases page](https://github.com/Matrix-Guy/OCR2SQL/releases)

This likely has limited compatibility with other purposes than what I have used it for so feel free to submit an issue report and increased compatibility may be added.

## For Feature Suggestions and Bug Reports

Please use the [issues tracker on this github repository](https://github.com/Matrix-Guy/OCR2SQL/issues)!

## Current Features
* User gives scanned documents, the return address zip code, and a name of the database to be written.
* Scanned documents are converted into text one by one, allowing the user to see each scan and the estimated properties collected along with the raw detected text for easy fixing of matching errors.
* These are then written to a .sql file (SQL Script) which has all the matching data in it.

### Current Detected Properties 
1. Name
2. Street
3. City
4. State
5. ZIP Code
6. Reason for return

## Upcoming Features
* Better variable names
* Many more code comments
* Better detection of address patterns
* Loading screen instead of not responding
* OCR scan status in task bar

## Prerequisites

* Windows 7 SP1, Windows 8.1, or Windows 10 Anniversary Update OS
* .NET Framework 4.7.1

## Installing and Deployment

[Download setup from the releases page](https://github.com/Matrix-Guy/OCR2SQL/releases)

WIP: Step by step guide to using

or

1. Clone the repository.
2. Load the .sln in a C# compiler/IDE such as [Visual Stuidio](https://www.visualstudio.com/vs/) or [MonoDevelop](http://www.monodevelop.com).
3. Build the solution.
4. Run "OCR to SQL.exe".
5. Done!

Most editing you will want to do if any will take place in [Form1.cs](https://github.com/Matrix-Guy/OCR2SQL/blob/master/OCR%20to%20SQL/OCR%20to%20SQL/Form1.cs) and [Form2.cs](https://github.com/Matrix-Guy/OCR2SQL/blob/master/OCR%20to%20SQL/OCR%20to%20SQL/Form2.cs).

Happy forking!

## Built With

* [C#](https://en.wikipedia.org/wiki/C_Sharp_(programming_language)) - The .Net Framework language
* [Tesseract NuGet](https://github.com/charlesw/tesseract) - The NuGet version of tesseract
* [Tesseract](https://github.com/tesseract-ocr/) - The official tesseract

## Contributing

Submit an issue tracker or a pull request and someone will get back to you.

## Authors

* **Cody Neiman** - *AKA* - [Matrix-Guy](https://github.com/Matrix-Guy)

Any future collaborators will be added here.

## License

This project is licensed under the GNU GENERAL PUBLIC LICENSE (Version 3, 29 June 2007) - see the [LICENSE](LICENSE) file for details.
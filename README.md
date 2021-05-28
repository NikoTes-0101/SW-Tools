# SW-Tools
Tools for translate text of Splush Wave games. It supports also conversion to Translator++. Font modify is not supported.<br><br>
This tool atm works with DAT files (files package), and DID_MES_'XX' files (text for main dialogues).<br><br>
Remain to add support for exe file text, database text and image de/compilation. 
# How to de/compile DAT?
1. For decompile, in CMD, write: <b>SW_Tools.exe -decompile_dat</b>, then write the file name of your DAT, and then the output folder path (it will create also a folder using filename to extract the files there.)
2. For compile, write: <b>SW_Tools.exe -compile_dat</b>, then write the path to the folder with all extracted files, and then, the output dat file name. This DON'T search on subdirectories, just grab the files on the same directory.
# How to de/compile text?
1. For decompile, write: <b>SW_Tools.exe -decompile_text</b>, then the input DID_MES_'XX' file to extract, and after it, the output folder path (it will create also a folder using filename to extract the files there). If you want to export for Translator++, add <b>-csv_exp</b> after output folder, and it will generate a folder called csv inside output folder.
2. For compile, write: <b>SW_Tools.exe -compile_text</b>, then write input folder path with all files, and then, the output file name of your text package (should be the same as the used in decompilation). If you want to import text from Translator++, add the <b>folder with exported csv texts</b> of Translator++. The tool will automatically inject the translation on the text files, formatting the text.
# How to import/export CSVs from Translator++?
I mostly recommend use this due that the game text format need to be full-width japanese text chars. ASCII chars are not allowed to use in game, and my tool will do the conversion of ASCII chars to full-width japanese ones.
1. Create new project in Translator++ as Spreadsheet. Select the csv folder and use default options.
2. Translate the text you want. Be aware to use only the Initial column for translation. This don't support multiple translations on import process. Also don't add line breaks to any text, since the game has a auto line break system for text display.
3. After you do the changes, do right-click on the files you want to export, then select X selected > Export into... > RMTrans Patch. Select your export folder.
4. Use the command above for import your exported translation folder.
When you translate dialogues in the game, you'll find that easily you will lack space to insert text on the same dialogue. Fortunately, you can insert new dialogues, so don't worry about the need of translate all the text on the same dialogue.<br><br>
To insert new lines using Translator++, add a new row, and in original text column (where it should be the japanese text fields), write: <b><insert\_XX></b> , where XX is the number of the dialogue you want to insert text between.<br><br>
For example, if you write <insert\_7> it will insert text AFTER the 7 dialogue entry in your translation table when import the translation using my tool.
# Extra tools
I added some extra tools for FGoNyo, since I like that game, and it has an interesting addition where you can unlock characters from their other games if you own the game, like Jotaro, Asuna or Misaka. The tools I did are basically to unlock those guest characters in the gacha list of your savedata.<br><br>
Also, there is a known issue, that if you unlock the characters with my unlocker, then you get some of those games, they will appear twice in your gacha list. For that reason, I did a little tool that clean all your gacha list from repeated entries.<br><br>
For unlock characters, write: SW_Tools.exe -fgn_sp_unlock, then write your savedata file as input, and then, the new file name of your save for output.<br><br>
For clean repeated entries, write: SW_Tools.exe -fgn_cleangacha, then write the name of your savedata. It'll overwrite the file with the cleaned data.
# TODO List
· Image de/compile. Quite lost on how the game compress the images, so not sure if I'll be able to do it. Quite necessary since is the only way to edit the font for translate in other languages that are not english.<br><br>
· DID_PARA_ database text de/compile. This basically store text and other binary data related to common game mechanics, such as name or description of units, skills or whatever.<br> Depends on the game, the database structure change, but they can be identified using file header name.<br><br>
· Create an exe file text de/compile. Most likely it will be a separated tool, and I have more or less the idea on how to do it, so will happen on a near future.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Splush_Wave_Tools
{
    class DAT_process
    {
        public static void Compile(string folder_input, string file_out)
        {
            string[] files = Directory.GetFiles(folder_input);
            List<long> pointers = new List<long>();

            using (BinaryWriter writer = new BinaryWriter(File.Open(file_out, FileMode.Create)))
            {
                //Write DAT header

                writer.Write(System.Text.Encoding.ASCII.GetBytes("CLS_FILELINK"));
                writer.Write(0);
                writer.Write(files.Length);
                writer.Write(0);
                writer.Write(64);
                writer.Write((long)0);
                writer.Write((long)0);
                writer.Write((long)0);
                writer.Write((long)0);
                writer.Write(0);

                //Write each file info
                for(int x = 0; x < files.Length; ++x)
                {
                    Console.WriteLine("Processing file: "+Path.GetFileName(files[x]));
                    writer.BaseStream.Seek(64+64*x, SeekOrigin.Begin);
                    string namefile = Path.GetFileName(files[x]);
                    if (namefile.Length > 40)
                    {
                        Console.WriteLine("This file name is too long. 40 chars is the max size, and your current name is " + namefile.Length);
                        Console.WriteLine("Program stop working.");
                        return;
                    }
                    else
                    {
                        writer.Write(System.Text.Encoding.ASCII.GetBytes(namefile));
                        while(namefile.Length < 40) {
                            namefile += " ";
                            writer.Write((byte)0);
                        }
                    }
                    writer.BaseStream.Seek(104 + 64 * x, SeekOrigin.Begin);
                    if (Path.GetFileName(files[x]).StartsWith("DID_")) writer.Write(268435456); //Data file identifier
                    else if (Path.GetFileName(files[x]).StartsWith("CID_")) writer.Write(268566528); //Image/Texture identifier
                    else if (Path.GetFileName(files[x]).StartsWith("VID_")) writer.Write(268959744); //Audio identifier
                    else //Just in case an unsupported identifier appear, or a misplaced file is in the folder
                    {
                        Console.WriteLine("Not compatible format in file name!\nFormats need to start with 'CID_'(for images), 'VID_'(for audio), or 'DID_'(for data)\nProgram stop working.");
                        return;
                    }
                    pointers.Add(writer.BaseStream.Position);
                    writer.Write(0); //Reserved for later pointer
                    long length = new FileInfo(files[x]).Length;
                    writer.Write(length);
                    writer.Write((long)0);
                    writer.Write(0);
                }

                //Adding file data
                for (int x = 0; x < files.Length; ++x)
                {
                    Console.WriteLine("Including file: " + Path.GetFileName(files[x]) + " on DAT.");
                    writer.BaseStream.Seek(0, SeekOrigin.End);
                    int savepointer = (int)writer.BaseStream.Position;
                    writer.BaseStream.Seek(pointers[x], SeekOrigin.Begin);
                    writer.Write(savepointer);
                    byte[] filedata = File.ReadAllBytes(files[x]);
                    writer.BaseStream.Seek(0, SeekOrigin.End);
                    writer.Write(filedata);
                }
            }
        }
        public static void Decompile(string file_input, string folder_out)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //A fix to japanese text input issues on CMD

            //Use filename as output folder
            string foldername = folder_out + Path.GetFileNameWithoutExtension(file_input);
            Directory.CreateDirectory(foldername);

            using (BinaryReader reader = new BinaryReader(File.Open(file_input, FileMode.Open)))
            {
                List<string> filename_list = new List<string>();
                List<int> offsets = new List<int>();
                List<int> size = new List<int>();

                reader.BaseStream.Seek(16, SeekOrigin.Begin);
                int files = reader.ReadInt32();

                reader.BaseStream.Seek(24, SeekOrigin.Begin);
                long start_pos = reader.ReadInt32();

                //Just go for filename, offset, and size for a raw decompilation
                for (int x = 0; x < files; ++x)
                {
                    reader.BaseStream.Seek(start_pos + 64 * x, SeekOrigin.Begin);

                    string name = System.Text.Encoding.GetEncoding("shift-jis").GetString(reader.ReadBytes(40)).Replace("\0", "");
                    filename_list.Add(name);
                    reader.BaseStream.Seek(4, SeekOrigin.Current);
                    offsets.Add(reader.ReadInt32());
                    size.Add(reader.ReadInt32());
                }
                for (int x = 0; x < files; ++x)
                {
                    Console.WriteLine("Extracting file: " + filename_list[x]);
                    reader.BaseStream.Seek(offsets[x], SeekOrigin.Begin);
                    byte[] data = reader.ReadBytes(size[x]);
                    File.WriteAllBytes(folder_out+"\\"+foldername+"\\"+filename_list[x], data);
                }
            }
        }
    }
}
